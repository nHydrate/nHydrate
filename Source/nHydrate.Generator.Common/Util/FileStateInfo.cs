namespace nHydrate.Generator.Common.Util
{
    public class FileStateInfo
    {
        #region Class Members

        #endregion

        #region Constructor

        public FileStateInfo()
        {
        }

        public FileStateInfo(FileStateConstants fileState, string fileName)
        {
            FileName = fileName;
            FileState = fileState;
        }

        #endregion

        #region Property Impelementations

        public FileStateConstants FileState { get; set; } = FileStateConstants.Success;

        public string FileName { get; set; } = string.Empty;

        #endregion

    }
}

