namespace nHydrate.Generator.Common.Util
{
  public class FileStateInfo
  {
    #region Class Members

    private nHydrate.Generator.Common.Util.EnvDTEHelper.FileStateConstants _fileState = EnvDTEHelper.FileStateConstants.Success;
    private string _fileName = string.Empty;

    #endregion

    #region Constructor

    public FileStateInfo()
    {
    }

    public FileStateInfo(nHydrate.Generator.Common.Util.EnvDTEHelper.FileStateConstants fileState, string fileName)
    {
      _fileName = fileName;
      _fileState = fileState;
    }

    #endregion

    #region Property Impelementations

    public nHydrate.Generator.Common.Util.EnvDTEHelper.FileStateConstants FileState
    {
      get { return _fileState; }
      set { _fileState = value; }
    }

    public string FileName
    {
      get { return _fileName; }
      set { _fileName = value; }
    }

    #endregion

  }
}

