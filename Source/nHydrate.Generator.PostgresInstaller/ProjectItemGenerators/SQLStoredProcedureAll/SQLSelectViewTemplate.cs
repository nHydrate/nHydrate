#pragma warning disable 0168
using System;
using System.Linq;
using System.Text;
using nHydrate.Generator.Models;

namespace nHydrate.Generator.PostgresInstaller.ProjectItemGenerators.SQLStoredProcedureAll
{
    class SQLSelectViewTemplate : ISQLGenerate
    {
        private ModelRoot _model;
        private CustomView _currentView;

        #region Constructors
        public SQLSelectViewTemplate(ModelRoot model, CustomView currentView)
        {
            _model = model;
            _currentView = currentView;
        }
        #endregion

        #region GenerateContent
        public void GenerateContent(StringBuilder sb)
        {
            try
            {
                this.AppendFullTemplate(sb);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        #endregion

        private void AppendFullTemplate(StringBuilder sb)
        {
            try
            {
                sb.Append(SQLEmit.GetSqlCreateView(_currentView, true));
            }
            catch (Exception ex)
            {
                throw;
            }
        }

    }
}
