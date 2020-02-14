#pragma warning disable 0168
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using nHydrate.Dsl;

namespace nHydrate.DslPackage.Forms
{
	public partial class EntityAuditMappings : Form
	{
		private nHydrate.Dsl.nHydrateModel _model = null;
		private Dictionary<Entity, TreeNode> _nodeCache = new Dictionary<Entity, TreeNode>();
		private bool _isLoading = false;

		public EntityAuditMappings()
		{
			InitializeComponent();
		}

		public EntityAuditMappings(nHydrate.Dsl.nHydrateModel model)
			: this()
		{
			_model = model;

			#region Load Tree
			_isLoading = true;
			foreach (var item in model.Entities.OrderBy(x => x.Name))
			{
				var n = new TreeNode() { Text = item.Name, Tag = item };
				tvwItem.Nodes.Add(n);
				_nodeCache.Add(item, n);

				n.Nodes.Add(new TreeNode() { Text = "Create Audit", Tag = "C", Checked = item.AllowCreateAudit });
				n.Nodes.Add(new TreeNode() { Text = "Modify Audit", Tag = "M", Checked = item.AllowModifyAudit });
				n.Nodes.Add(new TreeNode() { Text = "Concurrency", Tag = "T", Checked = item.AllowTimestamp });
				n.Checked = item.AllowCreateAudit || item.AllowModifyAudit || item.AllowTimestamp;
			}
			_isLoading = false;

			#endregion
		}

		private bool _isChecking = false;
		private void tvwItem_AfterCheck(object sender, TreeViewEventArgs e)
		{
			if (_isChecking) return;
			_isChecking = true;
			try
			{
				if (!_isLoading)
				{
					//IF check/uncheck an entity then do the same for all its fields
					if (e.Node.Tag is Entity)
					{
						e.Node.Nodes.ToList<TreeNode>().ForEach(x => x.Checked = e.Node.Checked);
					}

					if (e.Node.Tag is string)
					{
						//If check a field then make sure its parent entity is checked too
						if (e.Node.Checked)
						{
							e.Node.Parent.Checked = true;
						}
						else
						{
							//If NO fields for an entity are checked then uncheck parent entity
							if (e.Node.Parent.Nodes.ToList<TreeNode>().Count(x => x.Checked) == 0)
							{
								e.Node.Parent.Checked = false;
							}
						}
					}
				}

			}
			catch (Exception ex)
			{
				throw;
			}
			finally
			{
				_isChecking = false;
			}
		}

		private void cmdCheck_Click(object sender, EventArgs e)
		{
			_nodeCache.Values.ToList().ForEach(x => x.Checked = true);
		}

		private void cmdUncheck_Click(object sender, EventArgs e)
		{
			_nodeCache.Values.ToList().ForEach(x => x.Checked = false);
		}

		private void cmdExpand_Click(object sender, EventArgs e)
		{
			_nodeCache.Values.ToList().ForEach(x => x.Expand());
		}

		private void cmdCollapse_Click(object sender, EventArgs e)
		{
			_nodeCache.Values.ToList().ForEach(x => x.Collapse());
		}

		private void cmdOK_Click(object sender, EventArgs e)
		{
			try
			{
				using (var transaction = _model.Store.TransactionManager.BeginTransaction(Guid.NewGuid().ToString()))
				{
					foreach (var item in _nodeCache)
					{
						item.Key.AllowCreateAudit = item.Value.Nodes[0].Checked;
						item.Key.AllowModifyAudit = item.Value.Nodes[1].Checked;
						item.Key.AllowTimestamp = item.Value.Nodes[2].Checked;
					}
					transaction.Commit();
				}

			}
			catch (Exception ex)
			{
				throw;
			}

			this.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.Close();
		}

		private void cmdCancel_Click(object sender, EventArgs e)
		{
			this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.Close();
		}

	}
}

