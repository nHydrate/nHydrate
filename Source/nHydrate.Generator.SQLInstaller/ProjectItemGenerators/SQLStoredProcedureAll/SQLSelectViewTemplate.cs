#pragma warning disable 0168
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using nHydrate.Generator.Models;
using nHydrate.Generator.Common.GeneratorFramework;

namespace nHydrate.Generator.SQLInstaller.ProjectItemGenerators.SQLStoredProcedureAll
{
	internal class SQLSelectViewTemplate : ISQLGenerate
	{
		private ModelRoot _model;
		private CustomView _currentView;
		private StringBuilder _grantSB = null;

		#region Constructors
		public SQLSelectViewTemplate(ModelRoot model, CustomView currentView, StringBuilder grantSB)
		{
			_model = model;
			_currentView = currentView;
			_grantSB = grantSB;
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

		#region string methods

		protected string BuildSelectList()
		{
			var output = new StringBuilder();
			var ii = 0;
			foreach (var column in _currentView.GeneratedColumns.OrderBy(x => x.PascalName))
			{
				ii++;
				output.Append(column.DatabaseName);
				if (ii != _currentView.GeneratedColumns.Count())
				{
					output.Append("," + Environment.NewLine + "\t");
				}
			}
			return output.ToString();
		}

		#endregion

		private void AppendFullTemplate(StringBuilder sb)
		{
			try
			{
				sb.Append(nHydrate.Core.SQLGeneration.SQLEmit.GetSqlCreateView(_currentView, true));

				if (!string.IsNullOrEmpty(_model.Database.GrantExecUser))
				{
					_grantSB.AppendFormat("GRANT ALL ON [" + _currentView.GetSQLSchema() + "].[{0}] TO [{1}]", _currentView.DatabaseName, _model.Database.GrantExecUser).AppendLine();
					_grantSB.AppendLine("--MODELID: " + _currentView.Key);
					_grantSB.AppendLine("GO");
					_grantSB.AppendLine();
				}
			}
			catch (Exception ex)
			{
				throw;
			}
		}

	}
}
