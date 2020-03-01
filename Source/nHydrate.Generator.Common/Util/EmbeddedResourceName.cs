namespace nHydrate.Generator.Common.Util
{
    public class EmbeddedResourceName
    {
        public EmbeddedResourceName()
        {
        }

        public EmbeddedResourceName(string resourceName)
        {
            var splitResourceName = resourceName.Split('.');
            for (var ii = 0; ii < splitResourceName.Length - 2; ii++)
            {
                AsmLocation += splitResourceName[ii];
                if (ii < splitResourceName.Length - 3)
                {
                    AsmLocation += ".";
                }
            }

            FullName = resourceName;
            FileName = splitResourceName[splitResourceName.Length - 2] + "." + splitResourceName[splitResourceName.Length - 1];
        }

        public string FullName { get; set; } = string.Empty;

        public string FileName { get; set; } = string.Empty;

        public string AsmLocation { get; set; } = string.Empty;

    }
}