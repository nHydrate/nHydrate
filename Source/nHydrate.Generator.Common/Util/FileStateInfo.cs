namespace nHydrate.Generator.Common.Util
{
    public class FileStateInfo
    {
        public FileStateConstants FileState { get; set; } = FileStateConstants.Success;

        public string FileName { get; set; } = string.Empty;
    }
}
