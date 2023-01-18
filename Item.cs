using System;
using System.Text.Json.Serialization;

namespace OneListClient
{
    public class Item
    {
        [JsonPropertyName("id")] //this is an annotation, it tells the compiler to do something
        public int Id { get; set; }//default getter and setter
        [JsonPropertyName("text")]
        public string Text { get; set; }
        [JsonPropertyName("complete")]
        public bool Complete { get; set; }
        [JsonPropertyName("created_at")]
        public DateTime CreatedAt { get; set; }
        [JsonPropertyName("updated_at")]
        public DateTime UpdatedAt { get; set; }
        //note the property names are the same as the JSON keys and match the case

        //what if costumed getter and setter
        public string CompletedStatus
        {//read only, code supported property
            get
            {
                return Complete ? "Completed" : "Incomplete";
                // if (complete)
                // {
                //     return "Complete";
                // }
                // else
                // {
                //     return "Incomplete";
                // }
            }
        }
        //c# ternary operator: 
        //boolean expression ? value when true : value when false
        //condition ? statement 1 : statement 2
        //int x = 10, y = 100;
        //var result = x > y ? "x is greater than y" : "x is less than y";
        //output is "x is less than y"

    }
}