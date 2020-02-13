using System;

namespace nHydrate.Generator.Common.GeneratorFramework
{
    public interface IGeneratorStore
    {
        void ProcessInstallations();
        void LoadUI();
        void SetDTE(EnvDTE._DTE application);
        event EventHandler OnInstalledGeneratorsChanged;
        event EventHandler OnInstallComplete;
    }
}
