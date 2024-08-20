namespace LearnCosmosDB.Models
{
    public class Family
    {
        public string id { get; set; }  
        public string familyName { get; set; }
        public string address { get; set; }
        public int children { get; set; }
    }

    public class Child
    {
        public string id { get; set; }
        public string name { get; set; }
        public string family { get; set; }
        public int age { get; set; }
    }

    public class Pet
    {
        public string id { get; set; }
        public string child { get; set; }
        public string name { get; set; }
        public string type { get; set; }
    }

}
