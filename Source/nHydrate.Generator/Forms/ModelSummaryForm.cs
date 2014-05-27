using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using nHydrate.Generator.Models;

namespace nHydrate.Generator.Forms
{
	public partial class ModelSummaryForm : Form
	{
		public ModelSummaryForm()
		{
			InitializeComponent();
		}

		public ModelSummaryForm(ModelRoot root)
			: this()
		{
			try
			{
				var sb = new StringBuilder();

				var allColumns = new List<Column>();
				foreach (Table table in root.Database.Tables)
				{
					allColumns.AddRange(table.GetColumnsFullHierarchy(true));
				}

				//Views
				var allViews = new List<CustomView>();
				allViews.AddRange(root.Database.CustomViews);

				var allViewColumns = new List<CustomViewColumn>();
				foreach (var item in allViews)
				{
					allViewColumns.AddRange(item.GetColumns());
				}

				//Stored Procedures
				var allStoredProcedures = new List<CustomStoredProcedure>();
				allStoredProcedures.AddRange(root.Database.CustomStoredProcedures);

				var allStoredProcedureColumns = new List<CustomStoredProcedureColumn>();
				foreach (var item in allStoredProcedures)
				{
					allStoredProcedureColumns.AddRange(item.GetColumns());
				}

				var allStoredProcedureParameters = new List<Parameter>();
				foreach (var item in allStoredProcedures)
				{
					allStoredProcedureParameters.AddRange(item.GetParameters());
				}

				sb.AppendLine("Model Objects (Total/Generated)");
				sb.AppendLine("tables: " + root.Database.Tables.Count() + " / " + root.Database.Tables.Count(x => x.Generated));
				sb.AppendLine("\tColumns: " + allColumns.Count() + " / " + allColumns.Count(x => x.Generated && ((Table)x.ParentTableRef.Object).Generated));
				sb.AppendLine("\tComponents: " + root.Database.Tables.GetAllComponents().Count() + " / " + root.Database.Tables.GetAllComponents().Count(x => x.Generated && x.Parent.Generated));
				sb.AppendLine();
				//sb.AppendLine("Views: " + allViews.Count() + " / " + allViews.Count(x => x.Generated));
				//sb.AppendLine("\tColumns: " + allViewColumns.Count() + " / " + allViewColumns.Count(x => x.Generated && ((CustomView)x.ParentViewRef.Object).Generated));
				//sb.AppendLine();
				//sb.AppendLine("Stored Procedures: " + allStoredProcedures.Count() + " / " + allStoredProcedures.Count(x => x.Generated));
				//sb.AppendLine("\tColumns: " + allStoredProcedureColumns.Count() + " / " + allStoredProcedureColumns.Count(x => x.Generated && ((CustomStoredProcedure)(x.ParentRef).Object).Generated));
				//sb.AppendLine("\tParameters: " + allStoredProcedureParameters.Count() + " / " + allStoredProcedureParameters.Count(x => x.Generated && ((CustomStoredProcedure)(x.ParentTableRef).Object).Generated));
				//sb.AppendLine();
				sb.AppendLine("Generated Table List:");
				foreach (var item in root.Database.Tables.Where(x => x.Generated))
				{
					sb.AppendLine("\t" + item.DatabaseName + " has " + item.GetColumns().Count(x => x.Generated) + " columns and " + item.ComponentList.Count(x => x.Generated) + " components");
				}

				//sb.AppendLine();
				//sb.AppendLine("Generated View List:");
				//foreach (CustomView item in root.Database.CustomViews.Where(x => x.Generated))
				//{
				//  sb.AppendLine("\t" + item.DatabaseName + " has " + item.GetColumns().Count(x => x.Generated) + " columns");
				//}
				//sb.AppendLine();
				//sb.AppendLine("Generated Stored Procedure List:");
				//foreach (CustomStoredProcedure item in root.Database.CustomStoredProcedures.Where(x => x.Generated))
				//{
				//  sb.AppendLine("\t" + item.DatabaseName + " has " + item.GetColumns().Count(x => x.Generated) + " columns and " + item.GetParameters().Count(x => x.Generated) + " parameters");
				//}
				sb.AppendLine();
				//sb.AppendLine("Upon generation there will be X entities generated.");

				txtDetails.Text = sb.ToString();
			}
			catch (Exception ex)
			{
				throw;
			}
		}

		private void cmdClose_Click(object sender, EventArgs e)
		{
			this.DialogResult = DialogResult.Cancel;
			this.Close();
		}

	}
}
