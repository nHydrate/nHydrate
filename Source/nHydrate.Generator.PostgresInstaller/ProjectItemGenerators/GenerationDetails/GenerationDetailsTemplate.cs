#pragma warning disable 0168
using System;
using System.Linq;
using System.Text;
using nHydrate.Generator.Models;

namespace nHydrate.Generator.PostgresInstaller.ProjectItemGenerators.GenerationDetails
{
    class GenerationDetailsTemplate : BaseDbScriptTemplate
    {
        private StringBuilder sb = new StringBuilder();

        #region Constructors
        public GenerationDetailsTemplate(ModelRoot model)
            : base(model)
        {
        }
        #endregion

        #region BaseClassTemplate overrides
        public override string FileContent
        {
            get
            {
                try
                {
                    GenerateContent();
                    return sb.ToString();

                }
                catch (Exception ex)
                {
                    throw;
                }
            }
        }

        public override string FileName => "GenerationDetails.txt";

        #endregion

        #region GenerateContent
        private void GenerateContent()
        {
            try
            {
                sb.AppendLine("DO NOT MODIFY THIS FILE. IT IS ALWAYS OVERWRITTEN ON GENERATION.");
                sb.AppendLine("Generation Details");
                sb.AppendLine();

                sb.AppendLine($"Version {_model.Version}");
                sb.AppendLine($"Table Count: {_model.Database.Tables.Count(x => x.TypedTable != TypedTableConstants.EnumOnly)}");
                sb.AppendLine($"Tenant Table Count: {_model.Database.Tables.Count(x => x.IsTenant)}");
                sb.AppendLine($"View Count: {_model.Database.CustomViews.Count()}");
                sb.AppendLine();
                sb.AppendLine($"TABLE LIST");
                foreach (var item in _model.Database.Tables.Where(x => x.TypedTable != TypedTableConstants.EnumOnly).OrderBy(x => x.DatabaseName))
                {
                    sb.AppendLine($"{item.DatabaseName}, ColumnCount={item.GetColumns().Count()}, IsTenant={item.IsTenant}");
                    foreach (var column in item.GetColumns().OrderBy(x => x.DatabaseName))
                    {
                        sb.AppendLine($"    {column.GetIntellisenseRemarks()}");
                    }
                }
                sb.AppendLine();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        #endregion
    }
}