using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace OneListClient
{
    class Program
    {
        static async Task Main(string[] args) //this changed from void to async Task
        {
            Console.WriteLine("Welcome to OneListClient");
            var client = new HttpClient();

            //var responseBodyAsString = await client.GetStringAsync("https://one-list-api.herokuapp.com/items?access_token=cohort22");//turning this into an sync
            var responseBodyAsStream = await client.GetStreamAsync("https://one-list-api.herokuapp.com/items?access_token=illustriousvoyage");//turning this into an sync
            //this code will happen at the same time our network request is happening
            //we cannot Console.WriteLine a stream, so we need to deserialize it by feeding it to something called a JSON serializer
            //Console.WriteLine(responseBodyAsString);
            //                                      Describe the Shape of the Data (array in JSON => List, Object => Item class )
            //                                                  v   v
            var items = await JsonSerializer.DeserializeAsync<List<Item>>(responseBodyAsStream);//turning this into an sync
            //Back in the world of List/LINQ/C#
            foreach (var item in items)
            {
                Console.WriteLine($"The task {item.Text} was created on {item.CreatedAt} and has a completion of {item.CompletedStatus} ");
            }
        }
    }
}
