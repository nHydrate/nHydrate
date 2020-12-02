namespace nHydrate.Generator.Common.Util
{
    public class FileStateInfo
    {
        public FileStateInfo()
        {
        }

        public FileStateInfo(FileStateConstants fileState, string fileName)
        {
            FileName = fileName;
            FileState = fileState;
        }

        public FileStateConstants FileState { get; set; } = FileStateConstants.Success;

        public string FileName { get; set; } = string.Empty;
    }
}
