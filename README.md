# Coin Fetcher Function

This .Net 6.0 Functions app contains a function that fetches data for 100 coins from the CoinGecko API and saves them into a personal Cosmos DB database. The function runs automatically every 2 minutes to keep the data up to date.

## Function Description

The function is implemented in C# and uses the Azure Functions framework. It utilizes the CoinGecko API to retrieve information about the top 100 coins. The fetched data is then stored in a Cosmos DB database.

## Prerequisites

To use this function, you'll need:

- An Azure subscription with an active Cosmos DB account.
- The connection string for your Cosmos DB account.
- The necessary access rights and permissions to interact with the Cosmos DB account.

## Configuration

Before deploying and running the function, make sure to update the configuration settings with your own values:

- `CosmosDBConnectionString`: Replace with your Cosmos DB connection string.
- `DatabaseName`: Replace with the name of your Cosmos DB database.
- `CollectionName`: Replace with the name of the collection in your Cosmos DB database where you want to store the coin data.

## Deployment

To deploy and run the function:

1. Clone or download this repository to your local machine.
2. Open the solution in Visual Studio or your preferred code editor.
3. Update the configuration settings as described in the "Configuration" section.
4. Build the solution to ensure all dependencies are resolved.
5. Publish the Azure Functions app to your Azure subscription.
6. Configure the function's trigger to run every 2 minutes.
7. Start the function app and monitor the logs to verify successful execution.

## Contributing

Contributions are welcome! If you find any issues or have suggestions for improvements, please open an issue or submit a pull request.

## License

This project is licensed under the [MIT License](LICENSE). Feel free to use and modify the code as per your needs.


