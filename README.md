# AiTrip


![Diagram of the AiTrip Web App](./diagram.png)

AiTrip is an AI-assisted web application designed to help users discover and register for affordable travel packages and flights.

## Features
- **AI-Assisted Search:** AiTrip uses artificial intelligence to provide users with personalized recommendations for travel destinations based on their preferences.
- **Flight Creation:** AiTrip assists in the creation of new flights by providing detailed descriptions of destinations and generating illustrative images.

## Technology Stack
AiTrip is developed using the latest features of .NET 8 and Blazor, including:
- **Server-Side Rendering (SSR):** AiTrip uses SSR for faster initial page load times and better SEO.
- **Stream Rendering:** This feature allows AiTrip to send HTML to the client as soon as it's ready, improving perceived performance.


AiTrip integrates with various Azure services:
- **Azure Key Vault:** Used for secure storage of application secrets.
- **Azure OpenAI:** Used to create embeddings for AI-assisted search and generate rich descriptions to assist the users.
- **Azure Cosmos DB for MongoDB vCore:** Used to store flight data and embedding vectors.


## Requirements

To execute the project you need to:

### Azure Authentication via Azure CLI
Before you can use AiTrip, you need to authenticate with Azure using the Azure CLI. Follow these steps:
1. **Install Azure CLI:** If you haven't installed the Azure CLI, you can download it from [here](https://docs.microsoft.com/en-us/cli/azure/install-azure-cli).

2. **Login to Azure:** Open a command prompt or terminal and run the following command to login:
```
az login
```

This will open a browser window for you to login to your Azure account. If the browser does not open, you can manually open it and navigate to the URL provided in the command output.
3. **Set Active Subscription:** If you have multiple Azure subscriptions, set the subscription you want to use with the following command:

```
az account set --subscription "<Your-Subscription-Name>"
```


Replace `<Your-Subscription-Name>` with the name of your Azure subscription.
Now, your application is ready to authenticate with Azure via the Azure CLI.

### Create an Azure Key Vault
Create an azure key vault and then configure the key vault name in the **appsettings.json** file:
```
    "Vault": {
        "KeyVaultName": "SmartShop"
    }
```

### Create an Cosmos DB Database
It's required to create an Cosmodb Database, and configure the following in the **appsettings.json** file:

```
    "Database": {
        "ConnectionStringSecretName": "SmartShopConnectionString",
        "DatabaseName": "SmartShop",
        "CollectionName": "Flights"
    }
```

Where ConnectionStringSecretName is a secret in your Azure Key Vault.

The project **SetupProject** gives an example of how to create an index in CosmoDb:

```
var mongoClient = new MongoClient("your_mongodb_connectionstring");
var database = mongoClient.GetDatabase("your_mongodb_database_name");
var collection = database.GetCollection<Flight>("your_mongo_db_collection_name");


BsonDocumentCommand<BsonDocument> command = new BsonDocumentCommand<BsonDocument>(
	BsonDocument.Parse(@"
                        { createIndexes: 'Flights', 
                          indexes: [{ 
                            name: 'vectorSearchIndex', 
                            key: { embedding: 'cosmosSearch' }, 
                            cosmosSearchOptions: { kind: 'vector-ivf', numLists: 5, similarity: 'COS', dimensions: 1536 } 
                          }] 
					}"
));


BsonDocument result = _mongoDatabase.RunCommand(command);
if (result["ok"] != 1)
{
	Console.WriteLine("CreateIndex failed with response: " + result.ToJson());
}
```

Substitute **your_mongodb_connectionstring**, **your_mongodb_database_name** and **your_mongo_db_collection_name** with the correct values.


### Configure your Open Ai Settings
In the **appsettings.json** file configure your Open Ai settings:

```
    "OpenAi": {
        "ProxyUrl": "https://aoai.hacktogether.net",
        "TokenSecretName": "AzureOpenAiSecret",
        "EmbeddingDeployment": "gpt-35-turbo",
        "ChatCompletionDeployment": "gpt-35-turbo",
        "MaxTokens": 2048
    }
```

| Key | Value |
| ProxyUrl | https://aoai.hacktogether.net |
| TokenSecretName | The name of a token registered in Azure Key Vault or an Environment Variable |
| EmbeddingDeployment | Name of deployment for generate embeddings |
| ChatCompletionDeployment | Name of deployment for generate chat completions |
| MaxTokens | Maximum number of tokens for chat completions |