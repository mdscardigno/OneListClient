using System;
using System.Collections.Generic;
using System.Drawing;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using ConsoleTables;
using Pastel;

namespace OneListClient
{
    class Program
    {
        static async Task Main(string[] args) //this changed from void to async Task
        {
            Console.WriteLine("Welcome to OneListClient");
            var token = "";
            if (args.Length == 0)
            {
                Console.WriteLine("What list would you like?: ");
                token = Console.ReadLine();
            }
            else
            {
                token = args[0];
            }


            //add menu
            var keepGoing = true;
            while (keepGoing)
            {
                Console.Clear();
                Console.WriteLine("Get (A)ll Todo, (O)ne Todo or (Q)uit");
                var choice = Console.ReadLine().ToUpper();

                switch (choice)
                {
                    case "A":
                        await ShowAllItemsAsync(token);
                        Console.WriteLine("Press ENTER to continue");
                        Console.ReadLine();
                        break;
                    case "O":
                        Console.WriteLine("Enter the ID of the item you want to see");
                        var id = int.Parse(Console.ReadLine());

                        await GetOneItemAsync(token, id);
                        Console.WriteLine("Press ENTER to continue");
                        Console.ReadLine();
                        break;
                    case "Q":
                        keepGoing = false;
                        break;
                    default:
                        Console.WriteLine("Invalid choice");
                        break;
                }
            }

            static async Task GetOneItemAsync(string token, int id)
            {
                try
                {
                    // throw new NotImplementedException();
                    var client = new HttpClient();

                    var responseBodyAsStream = await client.GetStreamAsync($"https://one-list-api.herokuapp.com/items/{id}?access_token={token}");

                    //                                              Describe the Shape of the Data (Object in JSON => Item)
                    //                                                  v   
                    var item = await JsonSerializer.DeserializeAsync<Item>(responseBodyAsStream);

                    var table = new ConsoleTable("Description".Pastel(Color.Red), "Created At".Pastel(Color.Red), "Completed Status".Pastel(Color.Red));
                    //we do not loop because we have one item
                    table.AddRow(item.Text, item.CreatedAt, item.CompletedStatus);

                    table.Write();
                }
                catch (HttpRequestException)
                {
                    Console.WriteLine("I could not find that item");
                }
                catch (IndexOutOfRangeException)
                {
                    //different error message

                }
            }

            static async Task ShowAllItemsAsync(string token)
            {
                try
                {
                    // throw new NotImplementedException();
                    //var responseBodyAsString = await client.GetStringAsync("https://one-list-api.herokuapp.com/items?access_token=cohort22");//turning this into an sync

                    var client = new HttpClient();
                    //list i am using is illustriousvoyage
                    var responseBodyAsStream = await client.GetStreamAsync($"https://one-list-api.herokuapp.com/items?access_token={token}");

                    //turning this into an sync
                    //this code will happen at the same time our network request is happening
                    //we cannot Console.WriteLine a stream, so we need to deserialize it by feeding it to something called a JSON serializer
                    //Console.WriteLine(responseBodyAsString);
                    //                                      Describe the Shape of the Data (array in JSON => List, Object => Item class )
                    //                                                  v   v
                    var items = await JsonSerializer.DeserializeAsync<List<Item>>(responseBodyAsStream);//turning this into an sync
                                                                                                        //Back in the world of List/LINQ/C#

                    //using Console Tables for fancy output
                    var table = new ConsoleTable("Description".Pastel(Color.Red), "Created At".Pastel(Color.Red), "Completed Status".Pastel(Color.Red));



                    foreach (var item in items)
                    {
                        table.AddRow(item.Text, item.CreatedAt, item.CompletedStatus);
                    }

                    table.Write();
                }
                catch (HttpRequestException)
                {
                    Console.WriteLine("I could not find that list");
                }
            }
        }


    }
}
