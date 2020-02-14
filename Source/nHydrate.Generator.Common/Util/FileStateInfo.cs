namespace nHydrate.Generator.Common.Util
{
  public class FileStateInfo
  {
    #region Class Members

    private nHydrate.Generator.Common.Util.EnvDTEHelper.FileStateConstants _fileState = EnvDTEHelper.FileStateConstants.Success;

    #endregion

    #region Constructor

    public FileStateInfo()
    {
    }

    public FileStateInfo(nHydrate.Generator.Common.Util.EnvDTEHelper.FileStateConstants fileState, string fileName)
    {
      FileName = fileName;
      _fileState = fileState;
    }

    #endregion

    #region Property Impelementations

    public nHydrate.Generator.Common.Util.EnvDTEHelper.FileStateConstants FileState
    {
      get { return _fileState; }
      set { _fileState = value; }
    }

    public string FileName { get; set; } = string.Empty;

    #endregion

  }
}

