using System;
using System.Collections.Generic;
using System.Drawing;
using System.Net.Http;
using System.Net.Http.Headers;
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
                Console.WriteLine("Get (A)ll Todo, (C)reate a New Item, (O)ne Todo or (Q)uit");
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
                    case "C":
                        Console.WriteLine("Enter the description of your new todo: ");
                        var text = Console.ReadLine();

                        var newItem = new Item { Text = text };

                        await AddOneItem(token, newItem);

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
            static async Task AddOneItem(string token, Item newItem)
            {
                // throw new NotImplementedException();
                var client = new HttpClient();
                var url = $"https://one-list-api.herokuapp.com/items?access_token={token}";

                //we are going the other way around. we are turning an object into a string
                var jsonBody = JsonSerializer.Serialize(newItem);

                //we turn the object into a StringContent object and indicate we are using JSON
                //ensuring there is a media type header of application/json

                var jsonBodyAsContent = new StringContent(jsonBody);
                // jsonBodyAsContent.Headers.ContentType.MediaType = "application/json";
                jsonBodyAsContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                //we ask our client to Send the POST async request to the URL and supply the JSON body
                var response = await client.PostAsync(url, jsonBodyAsContent);

                //Get the response as a stream
                var responseJSON = await response.Content.ReadAsStreamAsync();

                //Supply that stream of data to Deserialize that will interpret it as a single item
                var item = await JsonSerializer.DeserializeAsync<Item>(responseJSON); //turning this into an sync

                //make a table to output our new item
                var table = new ConsoleTable("ID".Pastel(Color.Red), "Description".Pastel(Color.Red), "Created At".Pastel(Color.Red), "Updated At".Pastel(Color.Red), "Completed".Pastel(Color.Red));
                table.AddRow(item.Id, item.Text, item.CreatedAt, item.UpdatedAt, item.CompletedStatus);

                //write the table to the console
                table.Write(Format.Minimal);
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
