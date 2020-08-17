using nHydrate.ModelManagement;
using System;

namespace NetCoreTestHarness
{
    class Program
    {
        static void Main(string[] args)
        {
            var q = FileManagement.Load(@"C:\code\nHydrateTestAug\ModelProject\_Model1.model", "");
            FileManagement.Save(@"c:\temp\qwqw", q);


            Console.WriteLine("Hello World!");
        }

        //public static void ReadXML()
        //{
        //    // First write something so that there is something to read ...  
        //    //var b = new Book { title = "Serialization Overview" };
        //    //var writer = new System.Xml.Serialization.XmlSerializer(typeof(Book));
        //    //var wfile = new System.IO.StreamWriter(@"c:\temp\SerializationOverview.xml");
        //    //writer.Serialize(wfile, b);
        //    //wfile.Close();

        //    // Now we can read the serialized book ...  
        //    System.Xml.Serialization.XmlSerializer reader =
        //        new System.Xml.Serialization.XmlSerializer(typeof(Entity.configuration));
        //    System.IO.StreamReader file = new System.IO.StreamReader(
        //        @"C:\code\nHydrateTestAug\ModelProject\_Model1.model\_Entities\Customer.configuration.xml");
        //    var overview = (Entity.configuration)reader.Deserialize(file);
        //    file.Close();

        //}
    }
}
