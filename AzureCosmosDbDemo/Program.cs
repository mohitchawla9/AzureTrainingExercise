using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using System.Configuration;
using System.Threading.Tasks;

namespace AzureCosmosDbDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            Task.Run(async () =>
            {
                var endpoint = ConfigurationManager.AppSettings["DocDbEndpoint"];
                var masterKey = ConfigurationManager.AppSettings["DocDbMasterKey"];
                using (var client = new DocumentClient(new Uri(endpoint), masterKey))
                {
                    // Create new database Object 
                    Console.WriteLine("\r\n>>>>>>>>>>>>>>>> Creating Database <<<<<<<<<<<<<<<<<<<");
                    //Id defines name of the database  
                    var databaseDefinition = new Database { Id = "testDb" };
                    var database = await client.CreateDatabaseIfNotExistsAsync(databaseDefinition);
                    Console.WriteLine("Database testDb created successfully");

                    //Create new database collection  
                    Console.WriteLine("\r\n>>>>>>>>>>>>>>>> Creating Collection <<<<<<<<<<<<<<<<<<<");
                    var collectionDefinition = new DocumentCollection { Id = "testDocumentCollection" };
                    var collection = await client.CreateDocumentCollectionIfNotExistsAsync(UriFactory.CreateDatabaseUri("testDb"),collectionDefinition);
                    Console.WriteLine("Collection testDocumentCollection created successfully");

                    //Insert new Document  
                    Console.WriteLine("\r\n>>>>>>>>>>>>>>>> Creating Document <<<<<<<<<<<<<<<<<<<");
                    dynamic doc1Definition = new
                    {
                        title = "Star War IV ",
                        rank = 600,
                        category = "Sci-fi"
                    };
                    var document1 = await client.CreateDocumentAsync(
                        UriFactory.CreateDocumentCollectionUri("testDb", "testDocumentCollection"),
                        doc1Definition);
                    Console.WriteLine("Document created successfully in DB");

                    Console.WriteLine("\r\n>>>>>>>>>>>> Querying Document <<<<<<<<<<<<<<<<<<<<");
                    var response = client.CreateDocumentQuery(UriFactory.CreateDocumentCollectionUri("testDb", "testDocumentCollection"),
                        "select * from c").ToList();
                    var document = response.First();
                    Console.WriteLine($"Id:{document.id}");
                    Console.WriteLine($"Title:{document.title}");
                    Console.WriteLine($"Rank:{document.rank}");
                    Console.WriteLine($"category:{document.category}");
                    Console.WriteLine("Data Fetched successfully");

                    Console.WriteLine("\r\n>>>>>>>>>>>>>>>> Deleteing Collection <<<<<<<<<<<<<<<<<<<");
                    await client.DeleteDocumentCollectionAsync(UriFactory.CreateDocumentCollectionUri("testDb", "testDocumentCollection"));
                    Console.WriteLine("Data Deleted successfully");

                    Console.ReadKey();
                }

            }).Wait();
        }
    }
}





