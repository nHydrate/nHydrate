namespace nHydrate.Generator.Common.Util
{
    public class EmbeddedResourceName
    {
        #region members

        private string _asmLocation = string.Empty;
        #endregion

        #region construction
        public EmbeddedResourceName()
        {
        }

        public EmbeddedResourceName(string resourceName)
        {
            var splitResourceName = resourceName.Split('.');
            for(var ii = 0; ii < splitResourceName.Length -2; ii++)
            {
                _asmLocation += splitResourceName[ii];
                if (ii < splitResourceName.Length - 3)
                {
                    _asmLocation += ".";
                }
            }
            FullName = resourceName;
            FileName = splitResourceName[splitResourceName.Length - 2] + "." + splitResourceName[splitResourceName.Length - 1];
        }
        #endregion

        #region properties
        public string FullName { get; set; } = string.Empty;

        public string FileName { get; set; } = string.Empty;

        public string AsmLocation
        {
            get { return _asmLocation; }
            set { _asmLocation = value; }
        }
        #endregion
    }
}

