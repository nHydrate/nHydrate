using nHydrate.Generator.Common.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace nHydrate.DslPackage.Objects
{
    public static class ModelStatsFile
    {
        public static void Log(string modelKey, int eCount, int fCount)
        {
            var modelerVersion = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;

            var model = new ServerObjects.GenStatModel
            {
                MachineKey = SecurityHelper.GetMachineID(),
                ModelKey = modelKey,
                EntityCount = eCount,
                FieldCount = fCount,
                Timestamp = DateTime.UtcNow.Ticks,
            };

            //Cache file
            var line = $"{model.MachineKey},{model.ModelKey},{model.EntityCount},{model.FieldCount},{model.Timestamp}";

            File.AppendAllLines(FileName, new[] { line });
            //if (Generator.Common.GeneratorFramework.AddinAppData.Instance.AllowStats)
            {
                try
                {
                    Task.Run(() => { VersionHelper.LogStats(model); });
                } catch (Exception ex)
                {
                    //Do Nothing
                }
            }
        }

        private static string FileName
        {
            get
            {
                var fileName = Assembly.GetExecutingAssembly().Location;
                var fi = new System.IO.FileInfo(fileName);
                if (fi.Exists)
                {
                    //Get file name
                    fileName = System.IO.Path.Combine(fi.DirectoryName, "genstats.txt");
                    return fileName;
                }
                else return "";
            }
        }

    }
}
