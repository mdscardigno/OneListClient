using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace OneListClient
{
    class Program
    {
        static async Task Main(string[] args) //this changed from void to async Task
        {
            Console.WriteLine("Welcome to OneListClient");
            var client = new HttpClient();

            var responseBodyAsString = await client.GetStringAsync("https://one-list-api.herokuapp.com/items?access_token=cohort22");//turning this into an sync
            //this code will happen at the same time our network request is happening
            Console.WriteLine(responseBodyAsString);
        }
    }
}
