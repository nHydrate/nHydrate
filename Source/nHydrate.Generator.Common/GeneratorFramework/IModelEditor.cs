namespace nHydrate.Generator.Common.GeneratorFramework
{
    public interface IModelEditor
    {
        void GenerateAll();
        string OutsideEditorFileChangeMessage { get; }
        bool IsDirty { get; set; }
        string FileName { get; set; }
        void Import();
        void Verify();
        LoadResultConstants LoadFile(string fileName);
        void SaveFile(string fileName);
    }
}
