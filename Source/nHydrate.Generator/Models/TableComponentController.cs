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
using System.Windows.Forms;
using nHydrate.Generator.Common;
using nHydrate.Generator.Common.GeneratorFramework;
using nHydrate.Generator.Forms;

namespace nHydrate.Generator.Models
{
	public class TableComponentController : BaseModelObjectController
	{
		#region Member Variables

		#endregion

		#region Constructor

		protected internal TableComponentController(INHydrateModelObject modelObject)
			: base(modelObject)
		{
			this.HeaderText = "Component";
			this.HeaderDescription = "Defines settings for the selected component";
			this.HeaderImage = ImageHelper.GetImage(ImageConstants.Component);
		}

		#endregion

		#region BaseModelObjectController Members

		public override ModelObjectTreeNode Node
		{
			get
			{
				if(_node == null)
				{
					_node = new TableComponentNode(this);
				}
				return _node;
			}
		}

		public override MenuCommand[] GetMenuCommands()
		{
			var menuEdit = new DefaultMenuCommand();
			menuEdit.Text = "Edit";
			menuEdit.Click += new EventHandler(EditMenuClick);

			var menuSep1 = new DefaultMenuCommand();
			menuSep1.Text = "-";

			var menuDelete = new DefaultMenuCommand();
			menuDelete.Text = "Delete";
			menuDelete.Click += new EventHandler(DeleteMenuClick);

			return new MenuCommand[] { menuEdit, menuSep1, menuDelete };
		}

		public override MessageCollection Verify()
		{
			try
			{
				var retval = new MessageCollection();
				retval.AddRange(base.Verify());

				var tableComponent = (TableComponent)this.Object;

				if (tableComponent.Generated)
				{

					#region Table Component must have PK for parent table
					var pkCount = 0;
					foreach (Reference reference in tableComponent.Columns)
					{
						var column = (Column)reference.Object;
						if (tableComponent.Parent.PrimaryKeyColumns.Contains(column))
							pkCount++;
					}

					if (pkCount != tableComponent.Parent.PrimaryKeyColumns.Count)
					{
						retval.Add(MessageTypeConstants.Error, String.Format(ValidationHelper.ErrorTextComponentMustHaveTablePK, tableComponent.Name), this);
					}
					#endregion

					#region Check valid name
					if (!ValidationHelper.ValidCodeIdentifier(tableComponent.PascalName))
						retval.Add(MessageTypeConstants.Error, string.Format(ValidationHelper.ErrorTextInvalidIdentifier, tableComponent.Parent.Name), this);
					#endregion

					#region Check that object does not have same name as project

					if (tableComponent.PascalName == ((ModelRoot)tableComponent.Parent.Root).ProjectName)
					{
						retval.Add(MessageTypeConstants.Error, string.Format(ValidationHelper.ErrorTextComponentProjectSameName, tableComponent.Parent.PascalName), this);
					}

					#endregion

					#region Check for classes that will confict with generated classes

					var classExtensions = new List<string>();
					classExtensions.Add("collection");
					classExtensions.Add("enumerator");
					classExtensions.Add("query");
					//classExtensions.Add("search");
					classExtensions.Add("pagingfielditem");
					classExtensions.Add("paging");
					classExtensions.Add("primarykey");
					classExtensions.Add("selectall");
					classExtensions.Add("pagedselect");
					classExtensions.Add("selectbypks");
					classExtensions.Add("selectbycreateddaterange");
					classExtensions.Add("selectbymodifieddaterange");
					classExtensions.Add("selectbysearch");
					classExtensions.Add("beforechangeeventargs");
					classExtensions.Add("afterchangeeventargs");

					foreach (var ending in classExtensions)
					{
						if (tableComponent.PascalName.ToLower().EndsWith(ending))
						{
							retval.Add(MessageTypeConstants.Error, string.Format(ValidationHelper.ErrorTextNameConfictsWithGeneratedCode, tableComponent.Parent.Name), this);
						}
					}

					#endregion

				}

				return retval;

			}
			catch (Exception ex)
			{
				throw;
			}

		}

		public override bool DeleteObject()
		{
			if (MessageBox.Show("Do you wish to delete this component?", "Delete?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
			 {
				((TableComponent)this.Object).Parent.ComponentList.Remove((TableComponent)this.Object);
				this.Node.Remove();
				this.Object.Root.Dirty = true;
				this.Refresh();
				return true;
			}
			return false;
		}

		public override void Refresh()
		{
		}

		#endregion

		#region Menu Handlers

		private void DeleteMenuClick(object sender, System.EventArgs e)
		{
			this.DeleteObject();
		}

		private void EditMenuClick(object sender, System.EventArgs e)
		{
			var F = new TableComponentColumnForm((TableComponent)this.Object);
			if (F.ShowDialog() == DialogResult.OK)
			{
				this.Object.Root.Dirty = true;
				this.OnItemChanged(this, new System.EventArgs());
				this.Refresh();
			}

		}

		#endregion

	}
}