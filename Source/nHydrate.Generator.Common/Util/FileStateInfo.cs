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

    public FileStateInfo(nHydrate.Generator.Common.Util.EnvDTEHelper.FileStateConstants fileState, string fileName)
    {
      FileName = fileName;
      FileState = fileState;
    }

    #endregion

    #region Property Impelementations

    public nHydrate.Generator.Common.Util.EnvDTEHelper.FileStateConstants FileState { get; set; } = EnvDTEHelper.FileStateConstants.Success;

    public string FileName { get; set; } = string.Empty;

    #endregion

  }
}

