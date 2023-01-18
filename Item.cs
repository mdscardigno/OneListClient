using System;

namespace OneListClient
{
    public class Item
    {
        public int id { get; set; }//default getter and setter
        public string text { get; set; }
        public bool complete { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
        //note the property names are the same as the JSON keys and match the case

        //what if costumed getter and setter
        public string CompletedStatus
        {//read only, code supported property
            get
            {
                return complete ? "Completed" : "Incomplete";
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