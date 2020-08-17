using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace nHydrate.ModelManagement
{
    public static class FileManagement
    {
        private const string FOLDER_ET = "_Entities";
        private const string FOLDER_VW = "_Views";

        public static DiskModel Load(string rootFolder, string modelName)
        {
            var results = new DiskModel();
            LoadEntities(rootFolder, results);
            LoadViews(rootFolder, results);
            return results;
        }

        private static void LoadEntities(string rootFolder, DiskModel results)
        {
            var folder = Path.Combine(rootFolder, FOLDER_ET);
            if (!Directory.Exists(folder)) return;

            //Load Entities
            var fList = Directory.GetFiles(folder, "*.configuration.xml");
            foreach (var f in fList)
            {
                results.Entities.Add(GetObject<Entity.configuration>(f));
            }

            //Load Indexes
            fList = Directory.GetFiles(folder, "*.indexes.xml");
            foreach (var f in fList)
            {
                results.Indexes.Add(GetObject<nHydrate.ModelManagement.Index.configuration>(f));
            }

            //Load Relations
            fList = Directory.GetFiles(folder, "*.relations.xml");
            foreach (var f in fList)
            {
                results.Relations.Add(GetObject<Relation.configuration>(f));
            }

            //Load Static Data
            fList = Directory.GetFiles(folder, "*.staticdata.xml");
            foreach (var f in fList)
            {
                results.StaticData.Add(GetObject<StaticData.configuration>(f));
            }
        }

        private static void LoadViews(string rootFolder, DiskModel results)
        {
            var folder = Path.Combine(rootFolder, FOLDER_VW);

            //Load Views
            if (Directory.Exists(folder))
            {
                var fList = Directory.GetFiles(folder, "*.configuration.xml");
                foreach (var f in fList)
                {
                    results.Views.Add(GetObject<nHydrate.ModelManagement.View.configuration>(f));
                }
            }

        }

        private static T GetObject<T> (string fileName)
        {
            var reader =    new System.Xml.Serialization.XmlSerializer(typeof(T));
            using (var file = new System.IO.StreamReader(fileName))
            {
                return  (T)reader.Deserialize(file);
            }

        }

        public static void Save(string rootFolder, DiskModel model)
        {
            SaveEntities(rootFolder, model);
            SaveViews(rootFolder, model);
        }

        private static void SaveEntities(string rootFolder, DiskModel model)
        {
            var folder = Path.Combine(rootFolder, FOLDER_ET);
            if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);

            //Save Entities
            foreach (var obj in model.Entities)
            {
                var f = Path.Combine(folder, obj.name + ".configuration.xml");
                SaveObject(obj, f);
            }

            //Save Indexes
            foreach (var obj in model.Indexes)
            {
                var entity = model.Entities.FirstOrDefault(x => x.id == obj.id);
                var f = Path.Combine(folder, entity.name + ".indexes.xml");
                SaveObject(obj, f);
            }

            //Save Relations
            foreach (var obj in model.Relations)
            {
                var entity = model.Entities.FirstOrDefault(x => x.id == obj.id);
                var f = Path.Combine(folder, entity.name + ".relations.xml");
                SaveObject(obj, f);
            }

            //Save Static Data
            foreach (var obj in model.StaticData)
            {
                var entity = model.Entities.FirstOrDefault(x => x.id == obj.id);
                var f = Path.Combine(folder, entity.name + ".staticdata.xml");
                SaveObject(obj, f);
            }

        }

        private static void SaveViews(string rootFolder, DiskModel model)
        {
            var folder = Path.Combine(rootFolder, FOLDER_VW);
            if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);

            //Save Views
            foreach (var obj in model.Views)
            {
                var f = Path.Combine(folder, obj.name + ".configuration.xml");
                SaveObject(obj, f);
            }

        }

        private static void SaveObject<T>(T obj, string fileName)
        {
            var ns = new XmlSerializerNamespaces();
            ns.Add("", "");
            var writer = new System.Xml.Serialization.XmlSerializer(typeof(T));
            using (var wfile = new System.IO.StreamWriter(fileName))
            {
                writer.Serialize(wfile, obj, ns);
            }
        }

    }
}
