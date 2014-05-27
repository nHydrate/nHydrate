#region Copyright (c) 2006-2009 Widgetsphere LLC, All Rights Reserved
//--------------------------------------------------------------------- *
//                          Widgetsphere  LLC                           *
//             Copyright (c) 2006-2009 All Rights reserved              *
//                                                                      *
//                                                                      *
//This file and its contents are protected by United States and         *
//International copyright laws.  Unauthorized reproduction and/or       *
//distribution of all or any portion of the code contained herein       *
//is strictly prohibited and will result in severe civil and criminal   *
//penalties.  Any violations of this copyright will be prosecuted       *
//to the fullest extent possible under law.                             *
//                                                                      *
//THE SOURCE CODE CONTAINED HEREIN AND IN RELATED FILES IS PROVIDED     *
//TO THE REGISTERED DEVELOPER FOR THE PURPOSES OF EDUCATION AND         *
//TROUBLESHOOTING. UNDER NO CIRCUMSTANCES MAY ANY PORTION OF THE SOURCE *
//CODE BE DISTRIBUTED, DISCLOSED OR OTHERWISE MADE AVAILABLE TO ANY     *
//THIRD PARTY WITHOUT THE EXPRESS WRITTEN CONSENT OF WIDGETSPHERE LLC   *
//                                                                      *
//UNDER NO CIRCUMSTANCES MAY THE SOURCE CODE BE USED IN WHOLE OR IN     *
//PART, AS THE BASIS FOR CREATING A PRODUCT THAT PROVIDES THE SAME, OR  *
//SUBSTANTIALLY THE SAME, FUNCTIONALITY AS ANY WIDGETSPHERE PRODUCT.    *
//                                                                      *
//THE REGISTERED DEVELOPER ACKNOWLEDGES THAT THIS SOURCE CODE           *
//CONTAINS VALUABLE AND PROPRIETARY TRADE SECRETS OF WIDGETSPHERE,      *
//INC.  THE REGISTERED DEVELOPER AGREES TO EXPEND EVERY EFFORT TO       *
//INSURE ITS CONFIDENTIALITY.                                           *
//                                                                      *
//THE END USER LICENSE AGREEMENT (EULA) ACCOMPANYING THE PRODUCT        *
//PERMITS THE REGISTERED DEVELOPER TO REDISTRIBUTE THE PRODUCT IN       *
//EXECUTABLE FORM ONLY IN SUPPORT OF APPLICATIONS WRITTEN USING         *
//THE PRODUCT.  IT DOES NOT PROVIDE ANY RIGHTS REGARDING THE            *
//SOURCE CODE CONTAINED HEREIN.                                         *
//                                                                      *
//THIS COPYRIGHT NOTICE MAY NOT BE REMOVED FROM THIS FILE.              *
//--------------------------------------------------------------------- *
#endregion
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Widgetsphere.Generator.Models;

namespace Widgetsphere.Generator.ProjectItemGenerators.SubDomain
{
	class SubDomainTemplate : BaseClassTemplate
	{
		private StringBuilder sb = new StringBuilder();

		#region Constructors
		public SubDomainTemplate(ModelRoot model)
		{
			_model = model;
		}
		#endregion

		#region BaseClassTemplate overrides

		public override string FileContent
		{
			get
			{
				GenerateContent();
				return sb.ToString();
			}
		}

		public override string FileName
		{
			get
			{
				return string.Format("SubDomain.cs");
			}
		}

		#endregion

		#region GenerateContent
		private void GenerateContent()
		{
			try
			{
				ValidationHelper.AppendCopyrightInCode(sb, _model);
				this.AppendUsingStatements();
				sb.AppendLine("//Add the namespaces so they will always exist even if the objects do not");
				sb.AppendLine("namespace " + DefaultNamespace + ".Business.Views");
				sb.AppendLine("{");
				sb.AppendLine("	internal class X { }");
				sb.AppendLine("}");
				sb.AppendLine("namespace " + DefaultNamespace + ".Domain.Views");
				sb.AppendLine("{");
				sb.AppendLine("	internal class X { }");
				sb.AppendLine("}");
				sb.AppendLine("namespace " + DefaultNamespace + ".Business.Objects");
				sb.AppendLine("{");
				sb.AppendLine("	internal class X { }");
				sb.AppendLine("}");
				sb.AppendLine("namespace " + DefaultNamespace + ".Business.Objects.Components");
				sb.AppendLine("{");
				sb.AppendLine("	internal class X { }");
				sb.AppendLine("}");
				sb.AppendLine("namespace " + DefaultNamespace + ".Business.Objects.Composites");
				sb.AppendLine("{");
				sb.AppendLine("	internal class X { }");
				sb.AppendLine("}");
				sb.AppendLine("namespace " + DefaultNamespace + ".Domain.Objects");
				sb.AppendLine("{");
				sb.AppendLine("	internal class X { }");
				sb.AppendLine("}");
				sb.AppendLine("namespace " + DefaultNamespace + ".Domain.Objects.Components");
				sb.AppendLine("{");
				sb.AppendLine("	internal class X { }");
				sb.AppendLine("}");
				sb.AppendLine("namespace " + DefaultNamespace + ".Business.Rules");
				sb.AppendLine("{");
				sb.AppendLine("	internal class X { }");
				sb.AppendLine("}");				
				sb.AppendLine("namespace " + DefaultNamespace + ".Business.StoredProcedures");
				sb.AppendLine("{");
				sb.AppendLine("	internal class X { }");
				sb.AppendLine("}");
				sb.AppendLine();
				sb.AppendLine("namespace " + DefaultNamespace + ".Business.SelectCommands");
				sb.AppendLine("{");
				sb.AppendLine("	internal class X { }");
				sb.AppendLine("}");
				sb.AppendLine();
				sb.AppendLine("namespace " + DefaultNamespace + ".Domain.StoredProcedures");
				sb.AppendLine("{");
				sb.AppendLine("	internal class X { }");
				sb.AppendLine("}");
				sb.AppendLine("namespace " + DefaultNamespace + ".Business");
				sb.AppendLine("{");
				this.AppendClass();
				sb.AppendLine("}");
			}
			catch (Exception ex)
			{
				throw;
			}
		}

		#endregion

		#region namespace / objects

		public void AppendUsingStatements()
		{
			sb.AppendLine("using System;");
			sb.AppendLine("using System.Runtime.Serialization;");
			sb.AppendLine("using System.Data;");
			sb.AppendLine("using Widgetsphere.Core.DataAccess;");
			sb.AppendLine("using " + DefaultNamespace + ".Business.Objects;");
			sb.AppendLine("using " + DefaultNamespace + ".Business.Objects.Components;");
			sb.AppendLine("using " + DefaultNamespace + ".Business.Views;");
			sb.AppendLine("using " + DefaultNamespace + ".Business.StoredProcedures;");
			sb.AppendLine("using " + DefaultNamespace + ".Domain.Objects;");
			sb.AppendLine("using " + DefaultNamespace + ".Domain.Views;");
			sb.AppendLine("using " + DefaultNamespace + ".Domain.StoredProcedures;");
			sb.AppendLine();
		}

		public void AppendClass()
		{
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// The container for all object collections.");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("	[System.ComponentModel.DesignerCategoryAttribute(\"code\")]");
			sb.AppendLine("	[Serializable()]");
			sb.AppendLine("	public class SubDomain : SubDomainBase");
			sb.AppendLine("	{");
			sb.AppendLine();
			sb.AppendLine("		#region Constructors");
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// The constructor for the subdomain.");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		public SubDomain(string modifier) : base(modifier){}");
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// The constructor for the subdomain.");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		public SubDomain():base(){}");
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// The constructor for the subdomain.");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		public SubDomain(DataSet dataset, string modifier):base(dataset,modifier){}");
			sb.AppendLine();
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// Serialization constructor");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		protected SubDomain(SerializationInfo info, StreamingContext context):base(info,context){}");
			sb.AppendLine("		#endregion ");
			sb.AppendLine();
			sb.AppendLine("		#region SubDomainBase Implementation");
			sb.AppendLine();
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// Deterines if the connection uses the web service or connects directly to the database.");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		[Obsolete(\"This property no longer applies.\")]");
			sb.AppendLine("		protected override bool DirectConnect");
			sb.AppendLine("		{");
			sb.AppendLine("			get");
			sb.AppendLine("			{");
			sb.AppendLine("				return ConfigurationValues.GetInstance().DirectConnect;");
			sb.AppendLine("			}");
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// The web service URL end point.");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		[Obsolete(\"This property no longer applies.\")]");
			sb.AppendLine("		protected override string DataAccessServiceUrl");
			sb.AppendLine("		{");
			sb.AppendLine("			get { return ConfigurationValues.GetInstance().DataAccessServiceUrl; }");
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// Determines if any object in this container needs to be saved.");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		public override bool IsDirty");
			sb.AppendLine("		{");
			sb.AppendLine("			get");
			sb.AppendLine("			{");
			sb.AppendLine("				foreach (DataTable dt in this.Tables)");
			sb.AppendLine("				{");
			sb.AppendLine("					foreach (DataRow dr in dt.Rows)");
			sb.AppendLine("					{");
			sb.AppendLine("						if (dr.RowState != DataRowState.Unchanged)");
			sb.AppendLine("							return true;");
			sb.AppendLine("					}");
			sb.AppendLine("				}");
			sb.AppendLine("				return false;");
			sb.AppendLine("			}");
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// Determines the connection string to the database.");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		public override string ConnectionString");
			sb.AppendLine("		{");
			sb.AppendLine("			get { return ConfigurationValues.GetInstance().ConnectionString; }");
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// Adds all object collections in this assembly to the subdomain.");
			sb.AppendLine("		/// </summary>");			
			sb.AppendLine("		protected override void CreateStronglyTypedTables(System.Data.DataSet ds)");
			sb.AppendLine("		{");
			sb.AppendLine("			this.EnsureParentExistsOn = false;");
			sb.AppendLine();
			foreach (Table table in (from x in _model.Database.Tables where x.Generated orderby x.Name select x))
			{
				sb.AppendLine("			if(ds.Tables.Contains(\"" + table.PascalName + "Collection\"))");
				sb.AppendLine("			{");
				sb.AppendLine("				Domain" + table.PascalName + "Collection tmpCollection = (Domain" + table.PascalName + "Collection)ds.Tables[\"" + table.PascalName + "Collection\"];");
				sb.AppendLine("				this.AddCollection(Collections." + table.PascalName + "Collection);");
				sb.AppendLine("				Domain" + table.PascalName + "Collection permanentCollection = (Domain" + table.PascalName + "Collection)this.GetDomainCollection(Collections." + table.PascalName + "Collection);");
				sb.AppendLine();
				foreach (Relation relation in table.ParentRoleRelations.Where(x => x.IsGenerated))
				{
					if (((Table)relation.ChildTableRef.Object).Generated && relation.ChildTableRef.Object != table)
					{
						sb.AppendLine("				permanentCollection." + relation.PascalRoleName + "" + ((Table)relation.ChildTableRef.Object).PascalName + "Filled = tmpCollection." + relation.PascalRoleName + "" + ((Table)relation.ChildTableRef.Object).PascalName + "Filled;");
					}
				}
				sb.AppendLine("			}");
				sb.AppendLine();
			}
			sb.AppendLine();
			sb.AppendLine("			this.EnsureParentExistsOn = true;");
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		#endregion ");
			sb.AppendLine();
			sb.AppendLine("		#region Contains");
			sb.AppendLine();
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// Returns the number of loaded items in the specified collection.");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		internal bool Contains(Collections collection)");
			sb.AppendLine("		{");
			sb.AppendLine("			string tableName = collection.ToString();");
			sb.AppendLine("			return this.Tables.Contains(tableName);");
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		#endregion ");
			sb.AppendLine();
			sb.AppendLine("		#region GetDomainCollection");
			sb.AppendLine();
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// Returns the specified collection as a strongly-typed object. If the collection is not part of the subdomain, it is added.");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		internal DomainCollectionBase GetDomainCollection(Collections collection)");
			sb.AppendLine("		{");
			sb.AppendLine("			string tableName = collection.ToString();");
			sb.AppendLine("			if(!this.Tables.Contains(tableName))");
			sb.AppendLine("			{");
			sb.AppendLine("				AddCollection(collection);");
			sb.AppendLine("			}");
			sb.AppendLine("			return base[collection.ToString()];");
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		#endregion ");
			sb.AppendLine();
			sb.AppendLine("	#region AddCollection");
			sb.AppendLine();
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// Adds a collection of the specified type to the subdomain.");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		public void AddCollection(Collections collection)");
			sb.AppendLine("		{");
			sb.AppendLine("			bool oldEc = this.EnforceConstraints;");
			sb.AppendLine("			this.EnforceConstraints = false;");
			sb.AppendLine("			switch(collection)");
			sb.AppendLine("			{");

			foreach (Table table in (from x in _model.Database.Tables where x.Generated orderby x.Name select x))
			{
				if (table.Generated)
				{
					sb.AppendLine("				case Collections." + table.PascalName + "Collection: this.AddCollection(new Domain" + table.PascalName + "Collection());break;");
				}

				foreach (TableComponent component in table.ComponentList)
				{
					sb.AppendLine("				case Collections." + component.PascalName + "Collection: this.AddCollection(new Domain" + component.PascalName + "Collection());break;");
				}

			}

			foreach (CustomView view in _model.Database.CustomViews.OrderBy(x => x.Name))
			{
				if (view.Generated)
				{
					sb.AppendLine("				case Collections." + view.PascalName + "Collection: this.AddCollection(new Domain" + view.PascalName + "Collection());break;");
				}
			}

			foreach (CustomStoredProcedure sp in _model.Database.CustomStoredProcedures)
			{
				if (sp.Generated)
				{
					sb.AppendLine("				case Collections." + sp.PascalName + "Collection: this.AddCollection(new Domain" + sp.PascalName + "Collection());break;");
				}
			}

			sb.AppendLine();
			sb.AppendLine("			}");
			sb.AppendLine("			this.EnsureParentRowsExist();");
			sb.AppendLine("			this.EnforceConstraints = oldEc;");
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("	#endregion");
			sb.AppendLine();
			sb.AppendLine("	#region this[]");
			sb.AppendLine();
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// Returns the specified collection in this subdomain.");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		protected override IBusinessCollection this[Enum collection]");
			sb.AppendLine("		{");
			sb.AppendLine("			get");
			sb.AppendLine("			{");
			sb.AppendLine("				return (IBusinessCollection)this[(Collections)collection];");
			sb.AppendLine("			}");
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// Returns the specified collection in this subdomain.");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		public IBusinessCollection this[Collections collection]");
			sb.AppendLine("		{");
			sb.AppendLine("			get");
			sb.AppendLine("			{");
			sb.AppendLine("				IBusinessCollection businessCollection = null;");
			sb.AppendLine("				switch(collection)");
			sb.AppendLine("				{");

			//Load Tables
			foreach (Table table in (from x in _model.Database.Tables where x.Generated orderby x.Name select x))
			{
				if (table.Generated)
				{
					sb.AppendLine("				case Collections." + table.PascalName + "Collection: businessCollection = new " + table.PascalName + "Collection((Domain" + table.PascalName + "Collection)this.GetDomainCollection(collection));break;");
				}

				//Load Components
				foreach (TableComponent component in table.ComponentList)
				{
					sb.AppendLine("				case Collections." + component.PascalName + "Collection: businessCollection = new " + component.PascalName + "Collection((Domain" + component.PascalName + "Collection)this.GetDomainCollection(collection));break;");
				}

			}

			//Load Views
			foreach (CustomView view in _model.Database.CustomViews.OrderBy(x => x.Name))
			{
				if (view.Generated)
				{
					sb.AppendLine("				case Collections." + view.PascalName + "Collection: businessCollection = new " + view.PascalName + "Collection((Domain" + view.PascalName + "Collection)this.GetDomainCollection(collection));break;");
				}
			}

			sb.AppendLine();
			sb.AppendLine();
			sb.AppendLine("				}");
			sb.AppendLine("				return businessCollection;");
			sb.AppendLine("			}");
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("	#endregion");
			sb.AppendLine();
			sb.AppendLine("	}");
			sb.AppendLine();
			sb.AppendLine("	#region Collections Enumeration");
			sb.AppendLine("	 /// <summary>");
			sb.AppendLine("	 /// An enumeration of all tables that can be held by this subdomain");
			sb.AppendLine("	 /// </summary>");
			sb.AppendLine("	 public enum Collections");
			sb.AppendLine("	 {");

			sb.AppendLine("\t\t\t" + "/// <summary>");
			sb.AppendLine("\t\t\t" + "/// Identifies set object set");
			sb.AppendLine("\t\t\t" + "/// </summary>");
			sb.AppendLine("\t\t\t" + "_NotSet = 0,");

			//Tables
			int ii = 1;
			foreach (Table table in (from x in _model.Database.Tables where x.Generated orderby x.Name select x))
			{
				if (table.Generated)
				{
					sb.AppendLine("\t\t\t" + "/// <summary>");
					sb.AppendLine("\t\t\t" + "/// Identifies the " + table.PascalName + " table");
					sb.AppendLine("\t\t\t" + "/// </summary>");
					sb.AppendLine("\t\t\t" + table.PascalName + "Collection = " + ii + ",");
					ii++;
				}				
			}

			//Components			
			foreach (Table table in (from x in _model.Database.Tables where x.Generated orderby x.Name select x))
			{
				if (table.Generated)
				{
					foreach (TableComponent component in table.ComponentList)
					{
						sb.AppendLine("\t\t\t" + "/// <summary>");
						sb.AppendLine("\t\t\t" + "/// Identifies the " + component.PascalName + " table");
						sb.AppendLine("\t\t\t" + "/// </summary>");
						sb.AppendLine("\t\t\t" + component.PascalName + "Collection = " + ii + ",");
						ii++;
					}
				}
			}

			//Views
			foreach (CustomView view in _model.Database.CustomViews.OrderBy(x => x.Name))
			{
				if (view.Generated)
				{
					sb.AppendLine("\t\t\t" + "/// <summary>");
					sb.AppendLine("\t\t\t" + "/// Identifies the " + view.PascalName + " table");
					sb.AppendLine("\t\t\t" + "/// </summary>");
					sb.AppendLine("\t\t\t" + view.PascalName + "Collection = " + ii + ",");
					ii++;
				}				
			}

			//Stored Procedures
			foreach (CustomStoredProcedure storedProcedure in _model.Database.CustomStoredProcedures.OrderBy(x => x.Name))
			{
				if (storedProcedure.Generated)
				{
					sb.AppendLine("\t\t\t" + "/// <summary>");
					sb.AppendLine("\t\t\t" + "/// Identifies the " + storedProcedure.PascalName + " table");
					sb.AppendLine("\t\t\t" + "/// </summary>");
					sb.AppendLine("\t\t\t" + storedProcedure.PascalName + "Collection = " + ii + ",");
					ii++;
				}				
			}

			sb.AppendLine("	}");
			sb.AppendLine("	#endregion");
			sb.AppendLine();
		}
		#endregion

		#region append regions
		#endregion

		#region append member variables
		#endregion

		#region append constructors
		#endregion

		#region append properties
		#endregion

		#region append methods
		#endregion

		#region append operator overloads
		#endregion

	}
}