#pragma warning disable 0168
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using nHydrate.Generator.Models;
using nHydrate.Generator.Common.GeneratorFramework;

namespace nHydrate.Generator.PostgresInstaller.ProjectItemGenerators.DatabaseSchema
{
	public class SQLSelectTenantViewTemplate : BaseDbScriptTemplate
	{
		private Table _table;
		private StringBuilder _grantSB = null;

		#region Constructors

		public SQLSelectTenantViewTemplate(ModelRoot model, Table table, StringBuilder grantSB)
			: base(model)
		{
			_table = table;
			_grantSB = grantSB;
		}

		#endregion

		#region BaseClassTemplate overrides

		public override string FileContent
		{
			get
			{
				var sb = new StringBuilder();
				this.GenerateContent(sb);
				return sb.ToString();
			}
		}

		public override string FileName => $"{_model.TenantPrefix + "_" + _table.DatabaseName}.sql";

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
				sb.Append(SQLEmit.GetSqlTenantView(_model, _table, _grantSB));
			}
			catch (Exception ex)
			{
				throw;
			}
		}

	}
}
