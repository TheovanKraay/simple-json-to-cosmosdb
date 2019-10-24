using System;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace JSONToCosmos
{
    class Program
    {
        private static readonly string EndpointUri = "https://localhost:8081";
        // The primary key for the Azure Cosmos account.
        private static readonly string PrimaryKey = "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==";
        private CosmosClient cosmosClient;
        // The database we will create
        private Database database;

        // The container we will create.
        private Container container;
        private string databaseId = "database";
        private string containerId = "container";
        static async Task Main(string[] args)
        {
            Console.WriteLine("inserting doc...");
            Program p = new Program();
            await p.createDoc();
        }
        public async Task createDoc()
        {
            try
            {
                // Create a new instance of the Cosmos Client
                cosmosClient = new CosmosClient(EndpointUri, PrimaryKey);
                database = await this.cosmosClient.CreateDatabaseIfNotExistsAsync(databaseId);
                container = await this.database.CreateContainerIfNotExistsAsync(containerId, "/pk");
                var json = "{\"id\": \"2\",\"pk\": \"pk1\",\"Name\": \"theo\"}";
                var doc = JsonConvert.DeserializeObject<JObject>(json);
                //JObject doc = JObject.Parse(json);
                await this.container.CreateItemAsync<JObject>(doc, new PartitionKey(doc.GetValue("pk").ToString()));
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("exception: " + e);
            }
        }
    }
}
