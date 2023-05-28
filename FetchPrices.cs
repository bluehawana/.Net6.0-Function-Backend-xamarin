using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.SignalRService;
using Microsoft.Extensions.Logging;

namespace Fetch.Function
{
    public static class FetchPrices
    {
        [FunctionName("FetchPrices")]
        public async static Task Run(
            [TimerTrigger("0 */5 * * * *")] TimerInfo myTimer,
            [SignalR(HubName = "CoinPrices")] IAsyncCollector<SignalRMessage> signalRMessages,
            [CosmosDB(
                databaseName: "CoinPricesDB",
                collectionName: "CoinPrices",
                ConnectionStringSetting = "CosmosDBConnection")] IAsyncCollector<Coin> coinsOut,
            ILogger log)
        {
            log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");

            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Add("User-Agent", "CoinsTracker/1.0");
                var coinData = await httpClient.GetAsync("https://api.coingecko.com/api/v3/coins/markets?vs_currency=usd&order=market_cap_desc&per_page=100&page=1&sparkline=false");
                log.LogInformation($"API call status: {coinData.StatusCode}");
                var body = await coinData.Content.ReadAsStringAsync();
                Coin[] prices = JsonSerializer.Deserialize<Coin[]>(body);
                log.LogInformation($"Number of coins fetched: {prices.Length}");

                if (prices is not null)
                {
                    foreach (var coin in prices)
                    {
                        // Log the coin id
                        log.LogInformation(coin.id);

                        // Add the coin price to Cosmos DB
                        await coinsOut.AddAsync(coin);
                    }

                    // Send the prices to SignalR
                    await signalRMessages.AddAsync(
                        new SignalRMessage
                        {
                            Target = "updated",
                            Arguments = new[] { prices }
                        });
                }
            }
        }
    }

    // Define this class based on the structure of the data returned by the CoinGecko API.
    public class Coin
    {
        // Properties for the coin data (e.g., symbol, name, price, etc.)
        public string id { get; set; }
        public string symbol { get; set; }
        public string image { get; set; }
        public double current_price { get; set; }
        public double market_cap_rank { get; set; }
        public double price_change_percentage_24h { get; set; }
    }
}
