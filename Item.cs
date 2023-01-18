namespace OneListClient
{
    public class Item
    {
        public int id { get; set; }
        public string text { get; set; }
        public bool complete { get; set; }
        public string created_at { get; set; }
        public string updated_at { get; set; }
        //note the property names are the same as the JSON keys and match the case
    }
}