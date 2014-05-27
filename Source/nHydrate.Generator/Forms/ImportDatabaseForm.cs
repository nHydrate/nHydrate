#region Copyright (c) 2006-2012 nHydrate.org, All Rights Reserved
//--------------------------------------------------------------------- *
//                          NHYDRATE.ORG                                *
//             Copyright (c) 2006-2012 All Rights reserved              *
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
//THIRD PARTY WITHOUT THE EXPRESS WRITTEN CONSENT OF THE NHYDRATE GROUP *
//                                                                      *
//UNDER NO CIRCUMSTANCES MAY THE SOURCE CODE BE USED IN WHOLE OR IN     *
//PART, AS THE BASIS FOR CREATING A PRODUCT THAT PROVIDES THE SAME, OR  *
//SUBSTANTIALLY THE SAME, FUNCTIONALITY AS THIS PRODUCT                 *
//                                                                      *
//THE REGISTERED DEVELOPER ACKNOWLEDGES THAT THIS SOURCE CODE           *
//CONTAINS VALUABLE AND PROPRIETARY TRADE SECRETS OF NHYDRATE,          *
//THE REGISTERED DEVELOPER AGREES TO EXPEND EVERY EFFORT TO             *
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
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using nHydrate.Generator.Models;

namespace nHydrate.Generator.Forms
{
	public partial class ImportDatabaseForm : Form
	{
		#region Class Members

		private enum ActionConstants
		{
			Add,
			Refresh,
			Delete,
		}

		private bool _addOnly = false;

		#endregion

		#region Constructor

		public ImportDatabaseForm()
		{
			InitializeComponent();

			tvwAdd.AfterCheck += new TreeViewEventHandler(treeView1_AfterCheck);
			wizard1.BeforeSwitchPages += new nHydrate.Wizard.Wizard.BeforeSwitchPagesEventHandler(wizard1_BeforeSwitchPages);
			wizard1.AfterSwitchPages += new nHydrate.Wizard.Wizard.AfterSwitchPagesEventHandler(wizard1_AfterSwitchPages);
			wizard1.Finish += new EventHandler(wizard1_Finish);
			wizard1.FinishEnabled = false;
			chkSettingPK.CheckedChanged += new EventHandler(chkSettingPK_CheckedChanged);
			cmdTestConnection.Click += new EventHandler(cmdTestConnection_Click);

			this.Settings = new ImportModelSettings();
			this.Settings.OverridePrimaryKey = chkSettingPK.Checked;

			this.SelectedTables = new List<string>();
			this.SelectedViews = new List<string>();

			DatabaseConnectionControl1.LoadSettings();
		}

		#endregion

		#region Methods

		public nHydrate.Generator.Models.Database CurrentDatabase { get; set; }
		public nHydrate.Generator.Models.Database NewDatabase { get; private set; }

		public void Populate()
		{
			this.PopulateAllTrees();
		}

		internal ImportModelSettings Settings { get; private set; }
		internal SqlSchemaToModel.ImportReturnConstants Status { get; private set; }

		internal string GetConnectionString()
		{
			return DatabaseConnectionControl1.ImportOptions.ConnectionString;
		}

		/// <summary>
		/// Make this an add dialog and remove the tabs for delete and refresh
		/// </summary>
		public void ForceAddOnly()
		{
			pnlMain.Controls.Clear();
			pnlMain.Controls.Add(tvwAdd);
			tvwAdd.Dock = DockStyle.Fill;
			_addOnly = true;
		}

		private void PopulateAllTrees()
		{
			this.tvwAdd.Nodes.Clear();
			this.PopulateAddTree();
			this.PopulateRefreshTree();
			this.PopulateDeleteTree();
		}
	
		private void PopulateAddTree()
		{
			try
			{
				this.tvwAdd.Nodes.Clear();

				var tableParentNode = this.tvwAdd.Nodes[this.tvwAdd.Nodes.Add(new TreeNode("Entities"))];
				tableParentNode.Checked = true;
				tableParentNode.ImageIndex = 0;

				//var relationshipParentNode = this.treeView1.Nodes[this.treeView1.Nodes.Add(new TreeNode("Relations"))];

				var viewParentNode = this.tvwAdd.Nodes[this.tvwAdd.Nodes.Add(new TreeNode("Views"))];
				viewParentNode.Checked = true;
				viewParentNode.ImageIndex = 1;

				//TreeNode storedProcedureParentNode = this.treeView1.Nodes[this.treeView1.Nodes.Add(new TreeNode("Stored Procedures"))];
				
				this.PopulateDataSchema(tableParentNode);
				//this.PopulateRelationships(relationshipParentNode);
				this.PopulateViews(viewParentNode);
				//this.PopulateStoredProcedures(storedProcedureParentNode);

				foreach (TreeNode node in this.tvwAdd.Nodes)
					this.RemoveAllNodes(node.Nodes, DataTreeItem.DataTreeItemStateConstants.Added);

			}
			catch (Exception ex)
			{
				throw;
			}
		}

		private void PopulateRefreshTree()
		{
			try
			{
				this.tvwRefresh.Nodes.Clear();

				var tableParentNode = this.tvwRefresh.Nodes[this.tvwRefresh.Nodes.Add(new TreeNode("Entities"))];
				tableParentNode.Checked = true;
				tableParentNode.ImageIndex = 0;

				//var relationshipParentNode = this.treeView1.Nodes[this.treeView1.Nodes.Add(new TreeNode("Relations"))];

				var viewParentNode = this.tvwRefresh.Nodes[this.tvwRefresh.Nodes.Add(new TreeNode("Views"))];
				viewParentNode.Checked = true;
				viewParentNode.ImageIndex = 1;

				//TreeNode storedProcedureParentNode = this.treeView1.Nodes[this.treeView1.Nodes.Add(new TreeNode("Stored Procedures"))];

				this.PopulateDataSchema(tableParentNode);
				//this.PopulateRelationships(relationshipParentNode);
				this.PopulateViews(viewParentNode);
				//this.PopulateStoredProcedures(storedProcedureParentNode);

				foreach (TreeNode node in this.tvwRefresh.Nodes)
					this.RemoveAllNodes(node.Nodes, DataTreeItem.DataTreeItemStateConstants.Modified);

			}
			catch (Exception ex)
			{
				throw;
			}
		}

		private void PopulateDeleteTree()
		{
			try
			{
				this.tvwDelete.Nodes.Clear();

				var tableParentNode = this.tvwDelete.Nodes[this.tvwDelete.Nodes.Add(new TreeNode("Entities"))];
				tableParentNode.Checked = true;
				tableParentNode.ImageIndex = 0;

				//var relationshipParentNode = this.treeView1.Nodes[this.treeView1.Nodes.Add(new TreeNode("Relations"))];

				var viewParentNode = this.tvwDelete.Nodes[this.tvwDelete.Nodes.Add(new TreeNode("Views"))];
				viewParentNode.Checked = true;
				viewParentNode.ImageIndex = 1;

				//TreeNode storedProcedureParentNode = this.treeView1.Nodes[this.treeView1.Nodes.Add(new TreeNode("Stored Procedures"))];

				this.PopulateDataSchema(tableParentNode);
				//this.PopulateRelationships(relationshipParentNode);
				this.PopulateViews(viewParentNode);
				//this.PopulateStoredProcedures(storedProcedureParentNode);

				foreach (TreeNode node in this.tvwDelete.Nodes)
					this.RemoveAllNodes(node.Nodes, DataTreeItem.DataTreeItemStateConstants.Delete);

			}
			catch (Exception ex)
			{
				throw;
			}
		}

		public int GetChangeCount()
		{
			return CountChanges(tvwAdd.Nodes);
		}

		private void PopulateDataSchema(TreeNode parentNode)
		{
			try
			{
				var tables = new SortedList<string, DataTreeItem>();

				//Current Tables
				foreach (Table t in this.CurrentDatabase.Tables)
				{
					var dti = new DataTreeItem(t.Name);
					if (this.NewDatabase.Tables.Contains(t.Name))
						dti.Name = this.NewDatabase.Tables[t.Name].Name;

					//Check for deleted status
					if (!this.NewDatabase.Tables.Contains(t.Name))
						dti.State = DataTreeItem.DataTreeItemStateConstants.Delete;
					else if (this.NewDatabase.Tables[t.Name].CorePropertiesHash != t.CorePropertiesHash)
						dti.State = DataTreeItem.DataTreeItemStateConstants.Modified;
					else if (this.NewDatabase.Tables[t.Name].GetColumns().GetCorePropertiesHash() != t.GetColumns().GetCorePropertiesHash())
						dti.State = DataTreeItem.DataTreeItemStateConstants.Modified;

					tables.Add(t.Name.ToLower(), dti);
				}

				//Added Tables
				foreach (Table t in this.NewDatabase.Tables)
				{
					if (!this.CurrentDatabase.Tables.Contains(t.Name))
						tables.Add(t.Name.ToLower(), new DataTreeItem(t.Name, DataTreeItem.DataTreeItemStateConstants.Added));
				}

				//Now add to tree
				foreach (var tableKey in tables.Keys)
				{
					var tableNode = new TreeNode(tables[tableKey].Name);
					tableNode.Checked = true;
					tableNode.ImageIndex = 3;
					tableNode.Tag = tables[tableKey];
					parentNode.Nodes.Add(tableNode);

					var oldTable = this.CurrentDatabase.Tables[tableKey];
					var newTable = this.NewDatabase.Tables[tableKey];
					if (oldTable == null) oldTable = new Table(this.CurrentDatabase.Root);
					if (newTable == null) newTable = new Table(this.NewDatabase.Root);

					//Create list of all columns (new and old)
					var columns = new SortedList<string, DataTreeItem>();
					foreach (Reference r in oldTable.Columns)
					{
						var column = (Column)r.Object;
						var dti = new DataTreeItem(column.Name);

						//Check for deleted status
						if (this.NewDatabase.Tables.Contains(tables[oldTable.Name.ToLower()].Name))
						{
							var statusColumn = this.NewDatabase.Tables[tables[oldTable.Name.ToLower()].Name].GetColumns().FirstOrDefault(x => x.Name == column.Name);
							if (statusColumn == null)
								dti.State = DataTreeItem.DataTreeItemStateConstants.Delete;
							else if (column.CorePropertiesHash != statusColumn.CorePropertiesHash)
								dti.State = DataTreeItem.DataTreeItemStateConstants.Modified;
						}

						columns.Add(column.Name.ToLower(), dti);

					}

					#region Added Columns
					//if (oldTable.Name == string.Empty)
					//{
					//  foreach (Column column in this.NewDatabase.Tables[tableKey].GetColumns())
					//  {
					//    columns.Add(column.Name.ToLower(), new DataTreeItem(column.Name, DataTreeItem.DataTreeItemStateConstants.Added));
					//  }
					//}
					//else
					//{
					//  if (this.NewDatabase.Tables.Contains(tables[oldTable.Name.ToLower()].Name))
					//  {
					//    foreach (Column column in this.NewDatabase.Tables[tables[oldTable.Name.ToLower()].Name].GetColumns())
					//    {
					//      Column statusColumn = this.CurrentDatabase.Tables[tables[oldTable.Name.ToLower()].Name].GetColumns().FirstOrDefault(x => x.Name == column.Name);
					//      if (statusColumn == null)
					//        columns.Add(column.Name.ToLower(), new DataTreeItem(column.Name, DataTreeItem.DataTreeItemStateConstants.Added));
					//    }
					//  }
					//}

					////Now load columns into tree
					//if (this.NewDatabase.Tables.Contains(oldTable.Name))
					//{
					//  foreach (string columnKey in columns.Keys)
					//  {
					//    var columnNode = new TreeNode(columns[columnKey].Name);
					//    if (columns[columnKey].State == DataTreeItem.DataTreeItemStateConstants.Added)
					//      columnNode.ForeColor = Color.Green;
					//    if (columns[columnKey].State == DataTreeItem.DataTreeItemStateConstants.Delete)
					//      columnNode.ForeColor = Color.Red;
					//    if (columns[columnKey].State == DataTreeItem.DataTreeItemStateConstants.Modified)
					//      columnNode.ForeColor = Color.Blue;

					//    columnNode.Tag = columns[columnKey];
					//    tableNode.Nodes.Add(columnNode);
					//  }
					//}
					#endregion

				}

				//this.BrandNodes(parentNode.Nodes);

			}
			catch (Exception ex)
			{
				throw;
			}
		}

		#region PopulateRelationships
		//private void PopulateRelationships(TreeNode parentNode)
		//{
		//  try
		//  {
		//    Random rnd = new Random();
		//    SortedList<string, DataTreeItem> relationList = new SortedList<string, DataTreeItem>();

		//    //Current Tables
		//    foreach (Table t in this.CurrentDatabase.Tables)
		//    {
		//      //Remove invalid links. Something is very wrong
		//      RelationCollection relationCollection = t.GetRelations();
		//      for (int ii = relationCollection.Count - 1; ii >= 0; ii--)
		//      {
		//        Relation relationshipItem = relationCollection[ii];
		//        int errorCount = 0;
		//        foreach (ColumnRelationship relationshipLinkItem in relationshipItem.ColumnRelationships)
		//        {
		//          if ((relationshipLinkItem.ChildColumnRef == null) || (relationshipLinkItem.ParentColumnRef == null))
		//            errorCount++;
		//        }
		//        if (errorCount > 0) t.Relationships.RemoveAt(ii);
		//      }
		//      //Remove Errors

		//      //If the table exists in the new graph...
		//      foreach (Relation relation in relationCollection)
		//      {
		//        Table t2 = this.NewDatabase.Tables[t.Name];
		//        DataTreeItem dti = new DataTreeItem(relation.RoleName);
		//        dti.Name = relation.ToLongString();
		//        if (t2 == null)
		//        {
		//          //The table was removed so the relationship was removed
		//          dti.State = DataTreeItem.DataTreeItemStateConstants.Delete;
		//        }
		//        else
		//        {
		//          //If the name changed then it was modified
		//          Relation[] r2List = t2.GetRelations().GetFromMatch(relation);
		//          if (r2List.Length == 0)
		//          {
		//            dti.State = DataTreeItem.DataTreeItemStateConstants.Delete;
		//          }
		//          else if (r2List.Length == 1)
		//          {
		//            if (r2List[0].RoleName != relation.RoleName)
		//              dti.State = DataTreeItem.DataTreeItemStateConstants.Modified;
		//          }
		//          else if (r2List.Length > 1)
		//          {
		//            //There are multiple relationships for this table/column combination
		//            bool found = false;
		//            foreach (Relation r2 in r2List)
		//            {
		//              #region OLD CODE
		//              //if (r2.RoleName == relation.RoleName) found = true;
		//              //string parentTableName1 = ((Table)r2.ParentTableRef.Object).Name;
		//              //string parentTableName2 = ((Table)relation.ParentTableRef.Object).Name;

		//              //SortedDictionary<string, ColumnRelationship> list1 = new SortedDictionary<string,ColumnRelationship>();
		//              //foreach (ColumnRelationship cr in r2.ColumnRelationships)
		//              //{
		//              //  list1.Add(((Column)cr.ChildColumnRef.Object).Name, cr);
		//              //}

		//              //SortedDictionary<string, ColumnRelationship> list2 = new SortedDictionary<string, ColumnRelationship>();
		//              //foreach (ColumnRelationship cr in relation.ColumnRelationships)
		//              //{
		//              //  list2.Add(((Column)cr.ChildColumnRef.Object).Name, cr);
		//              //}

		//              //string parentColName1 = string.Empty;
		//              //foreach (string key in list1.Keys)
		//              //{
		//              //  parentColName1 += key;
		//              //}

		//              //string parentColName2 = string.Empty;
		//              //foreach (string key in list2.Keys)
		//              //{
		//              //  parentColName2 += key;
		//              //}

		//              ////string parentCol
		//              //if ((parentTableName1 == parentTableName2) && (parentColName1 == parentColName2))
		//              //  found = true;
		//              #endregion

		//              if (r2.Equals(relation)) found = true;

		//            }
		//            if (!found)
		//              dti.State = DataTreeItem.DataTreeItemStateConstants.Modified;
		//          }

		//        }

		//        //If there is a duplicate then add a FAKE ROLE to it
		//        if (relationList.ContainsKey(relation.ToLongString().ToLower()))
		//        {
		//          relation.RoleName = "ROLE" + rnd.Next(10000, 100000);
		//          relationList.Add(relation.ToLongString().ToLower(), dti);
		//        }
		//        else
		//        {
		//          relationList.Add(relation.ToLongString().ToLower(), dti);
		//        }

		//      }
		//    }

		//    //Added Relationships
		//    foreach (Table t in this.NewDatabase.Tables)
		//    {
		//      foreach (Reference reference in t.Relationships)
		//      {
		//        Relation r = (Relation)reference.Object;
		//        string relationName = r.ToLongString();
		//        if (this.CurrentDatabase.Tables.Contains(t.Name))
		//        {
		//          //If the old model has this table
		//          Table t2 = this.CurrentDatabase.Tables[t.Name];
		//          if ((t2 != null) && (t2.GetRelations().GetFromMatch(r).Length == 0))
		//            relationList.Add(relationName.ToLower(), new DataTreeItem(relationName, DataTreeItem.DataTreeItemStateConstants.Added));
		//        }
		//        else
		//        {
		//          //Table and Relation not in old model
		//          if (!relationList.ContainsKey(relationName.ToLower()))
		//          {
		//            relationList.Add(relationName.ToLower(), new DataTreeItem(relationName, DataTreeItem.DataTreeItemStateConstants.Added));
		//          }
		//          else
		//          {
		//            int loop = 1;
		//            while (relationList.ContainsKey((relationName + "_RELATION" + loop).ToLower()))
		//            {
		//              loop++;
		//            }
		//            relationName += "_RELATION" + loop;
		//            relationList.Add(relationName.ToLower(), new DataTreeItem(relationName, DataTreeItem.DataTreeItemStateConstants.Added));
		//          }
		//        }
		//      }
		//    }

		//    //Now add to tree
		//    foreach (string relationKey in relationList.Keys)
		//    {
		//      var relationNode = new TreeNode(relationList[relationKey].Name);
		//      relationNode.Tag = relationList[relationKey];
		//      if (relationList[relationKey].State == DataTreeItem.DataTreeItemStateConstants.Added)
		//        relationNode.ForeColor = Color.Green;
		//      if (relationList[relationKey].State == DataTreeItem.DataTreeItemStateConstants.Delete)
		//        relationNode.ForeColor = Color.Red;
		//      if (relationList[relationKey].State == DataTreeItem.DataTreeItemStateConstants.Modified)
		//        relationNode.ForeColor = Color.Blue;

		//      parentNode.Nodes.Add(relationNode);
		//    }

		//    this.BrandNodes(parentNode.Nodes);

		//  }
		//  catch (Exception ex)
		//  {
		//    throw;
		//  }
		//}
		#endregion

		#region PopulateStoredProcedures
		//private void PopulateStoredProcedures(TreeNode parentNode)
		//{
		//  try
		//  {
		//    SortedList<string, DataTreeItem> itemCache = new SortedList<string, DataTreeItem>();

		//    //Current Items
		//    foreach (CustomStoredProcedure storedProcedure in this.CurrentDatabase.CustomStoredProcedures)
		//    {
		//      DataTreeItem dti = new DataTreeItem(storedProcedure.Name);
		//      if (this.NewDatabase.CustomStoredProcedures.Contains(storedProcedure.Name))
		//        dti.Name = this.NewDatabase.CustomStoredProcedures[storedProcedure.Name].Name;

		//      //Check for deleted status
		//      if (!this.NewDatabase.CustomStoredProcedures.Contains(storedProcedure.Name))
		//        dti.State = DataTreeItem.DataTreeItemStateConstants.Delete;
		//      else if (this.NewDatabase.CustomStoredProcedures[storedProcedure.Name].Name != storedProcedure.Name)
		//        dti.State = DataTreeItem.DataTreeItemStateConstants.Modified;
		//      else if (this.NewDatabase.CustomStoredProcedures[storedProcedure.Name].GetColumns().GetCorePropertiesHash() != storedProcedure.GetColumns().GetCorePropertiesHash())
		//        dti.State = DataTreeItem.DataTreeItemStateConstants.Modified;

		//      itemCache.Add(storedProcedure.Name.ToLower(), dti);
		//    }

		//    //Added Items
		//    foreach (CustomStoredProcedure t in this.NewDatabase.CustomStoredProcedures)
		//    {
		//      if (!this.CurrentDatabase.CustomStoredProcedures.Contains(t.Name))
		//        itemCache.Add(t.Name.ToLower(), new DataTreeItem(t.Name, DataTreeItem.DataTreeItemStateConstants.Added));
		//    }

		//    //Now add to tree
		//    foreach (string storedProcedureKey in itemCache.Keys)
		//    {
		//      var newNode = new TreeNode(itemCache[storedProcedureKey].Name);
		//      newNode.Tag = itemCache[storedProcedureKey];
		//      if (itemCache[storedProcedureKey].State == DataTreeItem.DataTreeItemStateConstants.Added)
		//        newNode.ForeColor = Color.Green;
		//      if (itemCache[storedProcedureKey].State == DataTreeItem.DataTreeItemStateConstants.Delete)
		//        newNode.ForeColor = Color.Red;
		//      if (itemCache[storedProcedureKey].State == DataTreeItem.DataTreeItemStateConstants.Modified)
		//        newNode.ForeColor = Color.Blue;

		//      parentNode.Nodes.Add(newNode);

		//      CustomStoredProcedure oldStoredProcedure = this.CurrentDatabase.CustomStoredProcedures[storedProcedureKey];
		//      CustomStoredProcedure newStoredProcedure = this.NewDatabase.CustomStoredProcedures[storedProcedureKey];
		//      if (oldStoredProcedure == null) oldStoredProcedure = new CustomStoredProcedure(this.CurrentDatabase.Root);
		//      if (newStoredProcedure == null) newStoredProcedure = new CustomStoredProcedure(this.NewDatabase.Root);

		//      //Create list of all columns (new and old)
		//      SortedList<string, DataTreeItem> columns = new SortedList<string, DataTreeItem>();
		//      foreach (Reference r in oldStoredProcedure.Columns)
		//      {
		//        CustomStoredProcedureColumn column = (CustomStoredProcedureColumn)r.Object;
		//        DataTreeItem dti = new DataTreeItem(column.Name);

		//        //Check for deleted status
		//        if (this.NewDatabase.CustomStoredProcedures.Contains(itemCache[oldStoredProcedure.Name.ToLower()].Name))
		//        {
		//          CustomStoredProcedureColumn statusColumn = this.NewDatabase.CustomStoredProcedures[itemCache[oldStoredProcedure.Name.ToLower()].Name].GetColumns().FirstOrDefault(x => x.Name == column.Name);
		//          if (statusColumn == null)
		//            dti.State = DataTreeItem.DataTreeItemStateConstants.Delete;
		//          else if (column.CorePropertiesHash != statusColumn.CorePropertiesHash)
		//            dti.State = DataTreeItem.DataTreeItemStateConstants.Modified;
		//        }

		//        columns.Add(column.Name.ToLower(), dti);

		//      }

		//      //Added Columns
		//      if (oldStoredProcedure.Name == string.Empty)
		//      {
		//        foreach (CustomStoredProcedureColumn column in this.NewDatabase.CustomStoredProcedures[storedProcedureKey].GetColumns())
		//        {
		//          columns.Add(column.Name.ToLower(), new DataTreeItem(column.Name, DataTreeItem.DataTreeItemStateConstants.Added));
		//        }
		//      }
		//      else
		//      {
		//        if (this.NewDatabase.CustomStoredProcedures.Contains(itemCache[oldStoredProcedure.Name.ToLower()].Name))
		//        {
		//          foreach (CustomStoredProcedureColumn column in this.NewDatabase.CustomStoredProcedures[itemCache[oldStoredProcedure.Name.ToLower()].Name].GetColumns())
		//          {
		//            CustomStoredProcedureColumn statusColumn = this.CurrentDatabase.CustomStoredProcedures[itemCache[oldStoredProcedure.Name.ToLower()].Name].GetColumns().FirstOrDefault(x => x.Name == column.Name);
		//            if (statusColumn == null)
		//              columns.Add(column.Name.ToLower(), new DataTreeItem(column.Name, DataTreeItem.DataTreeItemStateConstants.Added));
		//          }
		//        }
		//      }

		//      //Now load columns into tree
		//      if (this.NewDatabase.CustomStoredProcedures.Contains(oldStoredProcedure.Name))
		//      {
		//        foreach (string columnKey in columns.Keys)
		//        {
		//          var columnNode = new TreeNode(columns[columnKey].Name);
		//          if (columns[columnKey].State == DataTreeItem.DataTreeItemStateConstants.Added)
		//            columnNode.ForeColor = Color.Green;
		//          if (columns[columnKey].State == DataTreeItem.DataTreeItemStateConstants.Delete)
		//            columnNode.ForeColor = Color.Red;
		//          if (columns[columnKey].State == DataTreeItem.DataTreeItemStateConstants.Modified)
		//            columnNode.ForeColor = Color.Blue;

		//          columnNode.Tag = columns[columnKey];
		//          newNode.Nodes.Add(columnNode);
		//        }
		//      }

		//    }

		//    this.BrandNodes(parentNode.Nodes);

		//  }
		//  catch (Exception ex)
		//  {
		//    throw;
		//  }
		//}
		#endregion

		private void PopulateViews(TreeNode parentNode)
		{
			try
			{
				var itemCache = new SortedList<string, DataTreeItem>();

				//Current Items
				foreach (CustomView view in this.CurrentDatabase.CustomViews)
				{
					var dti = new DataTreeItem(view.Name);
					if (this.NewDatabase.CustomViews.Contains(view.Name))
						dti.Name = this.NewDatabase.CustomViews[view.Name].Name;

					//Check for deleted status
					if (!this.NewDatabase.CustomViews.Contains(view.Name))
						dti.State = DataTreeItem.DataTreeItemStateConstants.Delete;
					else if (this.NewDatabase.CustomViews[view.Name].Name != view.Name)
						dti.State = DataTreeItem.DataTreeItemStateConstants.Modified;
					else if (this.NewDatabase.CustomViews[view.Name].GetColumns().GetCorePropertiesHash() != view.GetColumns().GetCorePropertiesHash())
						dti.State = DataTreeItem.DataTreeItemStateConstants.Modified;

					itemCache.Add(view.Name.ToLower(), dti);
				}

				//Added Items
				foreach (CustomView t in this.NewDatabase.CustomViews)
				{
					if (!this.CurrentDatabase.CustomViews.Contains(t.Name))
						itemCache.Add(t.Name.ToLower(), new DataTreeItem(t.Name, DataTreeItem.DataTreeItemStateConstants.Added));
				}

				//Now add to tree
				foreach (var viewKey in itemCache.Keys)
				{
					var viewNode = new TreeNode(itemCache[viewKey].Name);
					viewNode.Checked = true;
					viewNode.ImageIndex = 4;
					viewNode.Tag = itemCache[viewKey];
					//if (itemCache[viewKey].State == DataTreeItem.DataTreeItemStateConstants.Added)
					//  viewNode.ForeColor = Color.Green;
					//if (itemCache[viewKey].State == DataTreeItem.DataTreeItemStateConstants.Delete)
					//  viewNode.ForeColor = Color.Red;
					//if (itemCache[viewKey].State == DataTreeItem.DataTreeItemStateConstants.Modified)
					//  viewNode.ForeColor = Color.Blue;

					parentNode.Nodes.Add(viewNode);

					var oldView = this.CurrentDatabase.CustomViews[viewKey];
					var newView = this.NewDatabase.CustomViews[viewKey];
					if (oldView == null) oldView = new CustomView(this.CurrentDatabase.Root);
					if (newView == null) newView = new CustomView(this.NewDatabase.Root);

					//Create list of all columns (new and old)
					var columns = new SortedList<string, DataTreeItem>();
					foreach (Reference r in oldView.Columns)
					{
						var column = (CustomViewColumn)r.Object;
						var dti = new DataTreeItem(column.Name);

						//Check for deleted status
						if (this.NewDatabase.CustomViews.Contains(itemCache[oldView.Name.ToLower()].Name))
						{
							var statusColumn = this.NewDatabase.CustomViews[itemCache[oldView.Name.ToLower()].Name].GetColumns().FirstOrDefault(x => x.Name == column.Name);
							if (statusColumn == null)
								dti.State = DataTreeItem.DataTreeItemStateConstants.Delete;
							else if (column.CorePropertiesHash != statusColumn.CorePropertiesHash)
								dti.State = DataTreeItem.DataTreeItemStateConstants.Modified;
						}

						columns.Add(column.Name.ToLower(), dti);

					}

					#region Added Columns
					//if (oldView.Name == string.Empty)
					//{
					//  foreach (CustomViewColumn column in this.NewDatabase.CustomViews[viewKey].GetColumns())
					//  {
					//    columns.Add(column.Name.ToLower(), new DataTreeItem(column.Name, DataTreeItem.DataTreeItemStateConstants.Added));
					//  }
					//}
					//else
					//{
					//  if (this.NewDatabase.CustomViews.Contains(itemCache[oldView.Name.ToLower()].Name))
					//  {
					//    foreach (CustomViewColumn column in this.NewDatabase.CustomViews[itemCache[oldView.Name.ToLower()].Name].GetColumns())
					//    {
					//      CustomViewColumn statusColumn = this.CurrentDatabase.CustomViews[itemCache[oldView.Name.ToLower()].Name].GetColumns().FirstOrDefault(x => x.Name == column.Name);
					//      if (statusColumn == null)
					//        columns.Add(column.Name.ToLower(), new DataTreeItem(column.Name, DataTreeItem.DataTreeItemStateConstants.Added));
					//    }
					//  }
					//}

					////Now load columns into tree
					//if (this.NewDatabase.CustomViews.Contains(oldView.Name))
					//{
					//  foreach (string columnKey in columns.Keys)
					//  {
					//    var columnNode = new TreeNode(columns[columnKey].Name);
					//    if (columns[columnKey].State == DataTreeItem.DataTreeItemStateConstants.Added)
					//      columnNode.ForeColor = Color.Green;
					//    if (columns[columnKey].State == DataTreeItem.DataTreeItemStateConstants.Delete)
					//      columnNode.ForeColor = Color.Red;
					//    if (columns[columnKey].State == DataTreeItem.DataTreeItemStateConstants.Modified)
					//      columnNode.ForeColor = Color.Blue;

					//    columnNode.Tag = columns[columnKey];
					//    viewNode.Nodes.Add(columnNode);
					//  }
					//}
					#endregion

				}

			}
			catch (Exception ex)
			{
				throw;
			}
		}

		private void RemoveAllNodes(TreeNodeCollection nodeList, DataTreeItem.DataTreeItemStateConstants excludeType)
		{
			var delList = new List<TreeNode>();
			foreach (TreeNode node in nodeList)
			{
				if (((DataTreeItem)node.Tag).State != excludeType)
				{
					delList.Add(node);
				}
				RemoveAllNodes(node.Nodes, excludeType);
			}

			foreach (var node in delList)
			{
				this.tvwAdd.Nodes.Remove(node);
			}
		}

		private int CountChanges(TreeNodeCollection nodeList)
		{
			var retval = 0;
			foreach (TreeNode node in nodeList)
			{
				if (node.Tag != null)
				{
					if (((DataTreeItem)node.Tag).State != DataTreeItem.DataTreeItemStateConstants.Unchanged)
						retval++;
				}
				retval += CountChanges(node.Nodes);
			}
			return retval;
		}

		private bool AreChanges()
		{
			if (_addOnly)
			{
				return tvwAdd.GetNodeCount(true) > 2;
			}
			else
			{
				return (tvwAdd.GetNodeCount(true) > 2) ||
					(tvwDelete.GetNodeCount(true) > 2) ||
					(tvwRefresh.GetNodeCount(true) > 2);
			}
		}

		#endregion

		#region Properties

		public List<string> SelectedTables { get; private set; }
		public List<string> SelectedViews { get; private set; }

		#endregion

		#region Event Handlers

		private void treeView1_AfterCheck(object sender, TreeViewEventArgs e)
		{
			foreach (TreeNode node in e.Node.Nodes)
			{
				node.Checked = e.Node.Checked;
			}
		}

		private void wizard1_BeforeSwitchPages(object sender, nHydrate.Wizard.Wizard.BeforeSwitchPagesEventArgs e)
		{
			if (e.OldIndex == 0)
			{
				this.Cursor = Cursors.WaitCursor;
				try
				{
					DatabaseConnectionControl1.PersistSettings();
					var connectionString = DatabaseConnectionControl1.ImportOptions.GetConnectionString();

					if (!SqlSchemaToModel.IsValidConnectionString(connectionString))
					{
						this.Cursor = Cursors.Default;
						e.Cancel = true;
						MessageBox.Show("This not a valid connection string!", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
						return;
					}

					//Setup new model
					var project = new nHydrateGeneratorProject();
					SqlSchemaToModel.SetupNewProject(project, connectionString);
					this.NewDatabase = (project.Model as ModelRoot).Database;
					SqlSchemaToModel.GetProjectFromSqlSchema(project, connectionString, false, chkInheritance.Checked);

					//Load the tree
					this.Populate();

					if (!this.AreChanges())
					{
						this.Cursor = Cursors.Default;
						e.Cancel = true;
						MessageBox.Show("This model is up-to-date. There are no changes to refresh.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
					}

				}
				catch (Exception)
				{
					throw;
				}
				finally
				{
					this.Cursor = Cursors.Default;
				}
			}

		}

		private void wizard1_AfterSwitchPages(object sender, nHydrate.Wizard.Wizard.AfterSwitchPagesEventArgs e)
		{
			wizard1.FinishEnabled = (wizard1.SelectedIndex == wizard1.WizardPages.Count - 1);
		}

		private void wizard1_Finish(object sender, EventArgs e)
		{
			//Tables
			foreach (TreeNode node in tvwAdd.Nodes[0].Nodes)
			{
				if (node.Checked)
				{
					var tag = node.Tag as DataTreeItem;
					this.SelectedTables.Add(tag.Name.ToLower());
				}
			}
			foreach (TreeNode node in tvwRefresh.Nodes[0].Nodes)
			{
				if (node.Checked)
				{
					var tag = node.Tag as DataTreeItem;
					this.SelectedTables.Add(tag.Name.ToLower());
				}
			}
			foreach (TreeNode node in tvwDelete.Nodes[0].Nodes)
			{
				if (node.Checked)
				{
					var tag = node.Tag as DataTreeItem;
					this.SelectedTables.Add(tag.Name.ToLower());
				}
			}

			//Views
			foreach (TreeNode node in tvwAdd.Nodes[1].Nodes)
			{
				if (node.Checked)
				{
					var tag = node.Tag as DataTreeItem;
					this.SelectedViews.Add(tag.Name.ToLower());
				}
			}
			foreach (TreeNode node in tvwRefresh.Nodes[1].Nodes)
			{
				if (node.Checked)
				{
					var tag = node.Tag as DataTreeItem;
					this.SelectedViews.Add(tag.Name.ToLower());
				}
			}
			foreach (TreeNode node in tvwDelete.Nodes[1].Nodes)
			{
				if (node.Checked)
				{
					var tag = node.Tag as DataTreeItem;
					this.SelectedViews.Add(tag.Name.ToLower());
				}
			}

			SqlSchemaToModel.ImportModel(
				this.CurrentDatabase.Root as ModelRoot, 
				this.NewDatabase.Root as ModelRoot, 
				this.Settings, 
				this.SelectedTables, 
				this.SelectedViews);

			this.Status = SqlSchemaToModel.ImportReturnConstants.Success;
		}

		private void cmdTestConnection_Click(object sender, EventArgs e)
		{
			DatabaseConnectionControl1.RefreshOptions();
			var connectString = DatabaseConnectionControl1.ImportOptions.GetConnectionString();
			var valid = DatabaseHelper.TestConnectionString(connectString);
			if (valid)
			{
				MessageBox.Show("Connection Succeeded.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
			}
			else
			{
				MessageBox.Show("The information does not describe a valid connection string.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void chkSettingPK_CheckedChanged(object sender, EventArgs e)
		{
			this.Settings.OverridePrimaryKey = chkSettingPK.Checked;
		}

		#endregion

		#region DataTreeItem

		private class DataTreeItem
		{
			public enum DataTreeItemStateConstants
			{
				Unchanged = 0,
				Added = 1,
				Delete = 2,
				Modified = 3,
			}

			public string Name = string.Empty;
			public DataTreeItemStateConstants State = DataTreeItemStateConstants.Unchanged;

			public DataTreeItem(string name)
			{
				this.Name = name;
			}

			public DataTreeItem(string name, DataTreeItemStateConstants state)
				: this(name)
			{
				this.State = state;
			}

		}

		#endregion

	}
}