#region Copyright (c) 2006-2020 nHydrate.org, All Rights Reserved
// -------------------------------------------------------------------------- *
//                           NHYDRATE.ORG                                     *
//              Copyright (c) 2006-2020 All Rights reserved                   *
//                                                                            *
//                                                                            *
// Permission is hereby granted, free of charge, to any person obtaining a    *
// copy of this software and associated documentation files (the "Software"), *
// to deal in the Software without restriction, including without limitation  *
// the rights to use, copy, modify, merge, publish, distribute, sublicense,   *
// and/or sell copies of the Software, and to permit persons to whom the      *
// Software is furnished to do so, subject to the following conditions:       *
//                                                                            *
// The above copyright notice and this permission notice shall be included    *
// in all copies or substantial portions of the Software.                     *
//                                                                            *
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,            *
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES            *
// OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.  *
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY       *
// CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,       *
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE          *
// SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.                     *
// -------------------------------------------------------------------------- *
#endregion
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using nHydrate.Dsl;
using nHydrate.DslPackage.Objects;

namespace nHydrate.DslPackage.Forms
{
	public partial class ModuleMappings : Form
	{
		private nHydrate.Dsl.nHydrateModel _model = null;
		private Dictionary<IModuleLink, TreeNode> _nodeCache = new Dictionary<IModuleLink, TreeNode>();
		private Dictionary<EntityHasEntities, bool> _relationEnforcement = new Dictionary<EntityHasEntities, bool>();
		private bool _isLoading = false;

		public ModuleMappings()
		{
			InitializeComponent();

			tvwEntity.AfterCheck += new TreeViewEventHandler(TreeViewAfterCheck);
			tvwEntity.MouseUp += new MouseEventHandler(tvwEntity_MouseUp);
			tvwView.AfterCheck += new TreeViewEventHandler(TreeViewAfterCheck);
			tvwStoredProc.AfterCheck += new TreeViewEventHandler(TreeViewAfterCheck);
			tvwFunction.AfterCheck += new TreeViewEventHandler(TreeViewAfterCheck);
			this.FormClosing += new FormClosingEventHandler(ModuleMappings_FormClosing);
		}

		public ModuleMappings(nHydrate.Dsl.nHydrateModel model)
			: this()
		{
			_model = model;

			#region Load Tree
			foreach (var item in model.Entities.OrderBy(x => x.Name))
			{
				var n = new TreeNode() { Text = item.Name, Tag = item };
				tvwEntity.Nodes.Add(n);
				_nodeCache.Add(item, n);
				n.ImageIndex = 0;

				//Add fields
				foreach (var field in item.Fields.OrderBy(x => x.Name))
				{
					var n2 = new TreeNode() { Text = field.Name, Tag = field };
					n.Nodes.Add(n2);
					_nodeCache.Add(field, n2);
					n2.ImageIndex = 1;
				}

				//Add relations
				foreach (var relation in item.RelationshipList)
				{
					var n2 = new TreeNode() { Text = relation.DisplayName, Tag = relation };
					n.Nodes.Add(n2);
					_nodeCache.Add(relation, n2);
					n2.ImageIndex = 5;
				}

				//Add indexes (no need to include as they will always be in there)
				foreach (var index in item.IndexList.Where(x => x.IndexType != IndexTypeConstants.PrimaryKey))
				{
					var n2 = new TreeNode() { Text = index.ToString(), Tag = index };
					n.Nodes.Add(n2);
					_nodeCache.Add(index, n2);
					n2.ImageIndex = 6;
				}
			
			}

			foreach (var item in model.Views.OrderBy(x => x.Name))
			{
				var n = new TreeNode() { Text = item.Name, Tag = item };
				tvwView.Nodes.Add(n);
				_nodeCache.Add(item, n);
				n.ImageIndex = 2;
			}

			foreach (var item in model.StoredProcedures.OrderBy(x => x.Name))
			{
				var n = new TreeNode() { Text = item.Name, Tag = item };
				tvwStoredProc.Nodes.Add(n);
				_nodeCache.Add(item, n);
				n.ImageIndex = 3;
			}

			foreach (var item in model.Functions.OrderBy(x => x.Name))
			{
				var n = new TreeNode() { Text = item.Name, Tag = item };
				tvwFunction.Nodes.Add(n);
				_nodeCache.Add(item, n);
				n.ImageIndex = 4;
			}

			_nodeCache.Keys.ToList().ForEach(x => _nodeCache[x].SelectedImageIndex = _nodeCache[x].ImageIndex);

			#endregion

			#region Load Modules
			cboModule.Items.Add("(Select One)");
			model.Modules.ForEach(x => cboModule.Items.Add(x.Name));
			cboModule.SelectedIndex = 0;
			cboModule.Enabled = true;
			#endregion

		}

		#region ReLoad

		private void ReloadModule()
		{
			try
			{
				_relationEnforcement = new Dictionary<EntityHasEntities,bool>();
				_isLoading = true;
				if (cboModule.SelectedIndex == 0)
				{
					_nodeCache.Keys.ToList().ForEach(x => _nodeCache[x].Checked = false);
					tvwEntity.Enabled = false;
					tvwView.Enabled = false;
					tvwStoredProc.Enabled = false;
					tvwFunction.Enabled = false;
				}
				else
				{
					var module = _model.Modules.FirstOrDefault(x => x.Name == (string)cboModule.SelectedItem);

					foreach (var item in _model.Entities)
					{
						_nodeCache[item].Checked = item.Modules.Contains(module);
						foreach (var field in item.Fields)
						{
							_nodeCache[field].Checked = field.Modules.Contains(module);
						}

						foreach (IModuleLink relation in item.RelationshipList)
						{
							_nodeCache[relation].Checked = relation.Modules.Contains(module);

							var r = relation as EntityHasEntities;
							var relationModule = _model.RelationModules.FirstOrDefault(x => x.RelationID == r.Id && x.ModuleId == module.Id);
							if (relationModule == null)
								_relationEnforcement.Add(r, false);
							else
								_relationEnforcement.Add(r, relationModule.IsEnforced);
						}

						foreach (IModuleLink index in item.IndexList)
						{
							if (_nodeCache.ContainsKey(index))
								_nodeCache[index].Checked = index.Modules.Contains(module);
						}

					}

					_model.Views.ForEach(x => _nodeCache[x].Checked = x.Modules.Contains(module));
					_model.StoredProcedures.ForEach(x => _nodeCache[x].Checked = x.Modules.Contains(module));
					_model.Functions.ForEach(x => _nodeCache[x].Checked = x.Modules.Contains(module));

					tvwEntity.Enabled = true;
					tvwView.Enabled = true;
					tvwStoredProc.Enabled = true;
					tvwFunction.Enabled = true;
					cmdSave.Enabled = false;
					cmdCancel.Enabled = false;

				}

			}
			catch (Exception ex)
			{
				throw;
			}
			finally
			{
				_isLoading = false;
			}

			cmdCheck.Enabled = (cboModule.SelectedIndex > 0);
			cmdUncheck.Enabled = (cboModule.SelectedIndex > 0);

		}

		#endregion

		private void ModuleMappings_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (e.CloseReason == CloseReason.UserClosing)
			{
				//If the module dropdown is not enabled tehn the form is dirty so prompt user
				if (!cboModule.Enabled)
				{
					if (MessageBox.Show("Do you wish to close this screen and ignore changes?", "Ignore Changes?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != System.Windows.Forms.DialogResult.Yes)
					{
						e.Cancel = true;
					}
				}
			}
		}

		private void cboModule_SelectedIndexChanged(object sender, EventArgs e)
		{
			ReloadModule();

			cmdSave.Enabled = false;
			cmdCancel.Enabled = false;
			cboModule.Enabled = true;

		}

		private bool _isChecking = false;
		private void TreeViewAfterCheck(object sender, TreeViewEventArgs e)
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
					
					//Control parent node if child checked
					if (e.Node.Tag is Field || e.Node.Tag is EntityHasEntities)
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

					//Remove relation if field is unchecked.
					if (e.Node.Tag is Field)
					{
						//Unchecked this field so uncheck all relations and indexes that contain it
						if (!e.Node.Checked)
						{
							var field = e.Node.Tag as Field;

							var allRelations = _nodeCache.Keys.Where(x => x is EntityHasEntities).Cast<EntityHasEntities>().ToList();
							foreach (var key in allRelations.Where(x => x.FieldMapList().Count(z => z.GetSourceField(x) == field || z.GetTargetField(x) == field) > 0))
							{
								if (_nodeCache[key].Checked)
								{
									_nodeCache[key].Checked = false;
								}
							}

							var allIndexes = _nodeCache.Keys.Where(x => x is Index).Cast<Index>().ToList();
							foreach (var key in allIndexes.Where(x => x.IndexColumns.Any(z => z.FieldID == field.Id)))
							{
								if (_nodeCache[key].Checked)
								{
									_nodeCache[key].Checked = false;
								}
							}

						}
					}

					//Include fields if a relation is checked
					if (e.Node.Tag is EntityHasEntities)
					{
						if (e.Node.Checked)
						{
							var relation = e.Node.Tag as EntityHasEntities;
							_relationEnforcement[relation] = true;

							//Ensure that both ends of relationship are in this module
							var doPrompt = false;
							foreach (var fieldMap in relation.FieldMapList())
							{
								var sourceField = fieldMap.GetSourceField(relation);
								var targetField = fieldMap.GetTargetField(relation);

								var sourceNode = _nodeCache.Where(x => x.Value.Tag == sourceField).Select(x => x.Value).FirstOrDefault();
								if (sourceNode != null && !sourceNode.Checked)
								{
									sourceNode.Checked = true;
									doPrompt = true;
								}

								var targetNode = _nodeCache.Where(x => x.Value.Tag == targetField).Select(x => x.Value).FirstOrDefault();
								if (targetNode != null && !targetNode.Checked)
								{
									targetNode.Checked = true;
									targetNode.Parent.Checked = true; //Make sure parent entity is checked too
									doPrompt = true;
								}
							}

							//Tell the user that we have done this
							if (doPrompt && !_isLoading)
							{
								MessageBox.Show("Both ends of a relationship must be in the current module when the relationship is in the module. These have been checked automatically.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
							}

						}
					}

					//Index
					if (e.Node.Tag is Index)
					{
						if (e.Node.Checked)
						{
							e.Node.Parent.Checked = true;
							var index = e.Node.Tag as Index;
							var entity = (e.Node.Parent.Tag as Entity);
							var allChildren = e.Node.Parent.Nodes.ToList<TreeNode>();
							var needChecking = allChildren.Where(x => index.FieldList.Contains(x.Tag) && !x.Checked).ToList();
							needChecking.ForEach(x => x.Checked = true);

							if (needChecking.Any() && !_isLoading)
							{
							MessageBox.Show("All fields in an index must be in the current module. These have been checked automatically.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
							}
						}
						else
						{
							//Do Nothing
						}
					}

				}

				cmdSave.Enabled = true;
				cmdCancel.Enabled = true;
				cboModule.Enabled = false;

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

		private void cmdSave_Click(object sender, EventArgs e)
		{
			var uiKey = ProgressHelper.ProgressingStarted("Saving Modules...");
			try
			{
				using (var transaction = _model.Store.TransactionManager.BeginTransaction(Guid.NewGuid().ToString()))
				{
					var module = _model.Modules.FirstOrDefault(x => x.Name == (string)cboModule.SelectedItem);
					foreach (var item in _nodeCache)
					{
						if (item.Value.Checked)
						{
							if (item.Key.Modules.Count(x => x == module) == 0)
							{
								item.Key.AddModule(module);

								//Add PK if entity
								if (item.Key is Entity)
								{
									var entity = item.Key as Entity;
									var pk = entity.Indexes.FirstOrDefault(x => x.IndexType == IndexTypeConstants.PrimaryKey);
									if (pk != null && !_model.IndexModules.Any(x => x.IndexID == pk.Id && x.ModuleId == module.Id))
									{
										_model.IndexModules.Add(new IndexModule(_model.Partition) { IndexID = pk.Id, ModuleId = module.Id });
									}
								}
							}
						}
						else
						{
							item.Key.RemoveModule(module);

							//Remove PK if entity
							if (item.Key is Entity)
							{
								var entity = item.Key as Entity;
								var pk = entity.Indexes.FirstOrDefault(x => x.IndexType == IndexTypeConstants.PrimaryKey);
								if (pk != null)
								{
									_model.IndexModules.Remove(x => (x.IndexID == pk.Id) && (x.ModuleId == module.Id));
								}
							}

						}
					}

					//Now process the enforce bits
					var allRelations = _nodeCache.Keys.Where(x => x is EntityHasEntities).ToList();
					foreach (var key in allRelations)
					{
						if (_nodeCache[key].Checked)
						{
							var relation = _nodeCache[key].Tag as EntityHasEntities;
							var relationModule = _model.RelationModules.FirstOrDefault(x => x.RelationID == relation.Id && x.ModuleId == module.Id);
							if (relationModule == null)
							{
								_model.RelationModules.Add(new RelationModule(_model.Partition)
								{
									RelationID = relation.Id,
									ModuleId = module.Id,
									Included = true,
									IsEnforced = _relationEnforcement[relation]
								});
							}
							else
							{
								relationModule.IsEnforced = _relationEnforcement[relation];
							}
						}

					}

					//Process Indexes
					var allIndexes = _nodeCache.Keys.Where(x => x is Index).ToList();
					foreach (var key in allIndexes)
					{
						if (_nodeCache[key].Checked)
						{
							var index = _nodeCache[key].Tag as Index;
							var indexModule = _model.IndexModules.FirstOrDefault(x => x.IndexID == index.Id && x.ModuleId == module.Id);
							if (indexModule == null)
							{
								_model.IndexModules.Add(new IndexModule(_model.Partition)
								{
									IndexID = index.Id,
									ModuleId = module.Id,
								});
							}
						}

					}

					transaction.Commit();
				}

				cmdSave.Enabled = false;
				cmdCancel.Enabled = false;
				cboModule.Enabled = true;

			}
			catch (Exception ex)
			{
				throw;
			}
			finally
			{
				ProgressHelper.ProgressingComplete(uiKey);
			}
		}

		private void cmdCancel_Click(object sender, EventArgs e)
		{
			ReloadModule();
			cmdSave.Enabled = false;
			cmdCancel.Enabled = false;
			cboModule.Enabled = true;
		}

		private void cmdCheck_Click(object sender, EventArgs e)
		{
			_isLoading = true;
			try
			{
				_nodeCache.Values.ToList().ForEach(x => x.Checked = true);

				cmdSave.Enabled = true;
				cmdCancel.Enabled = true;
				cboModule.Enabled = false;

			}
			catch (Exception ex)
			{
				throw;
			}
			finally
			{
				_isLoading = false;
			}
		}

		private void cmdUncheck_Click(object sender, EventArgs e)
		{
			_isLoading = true;
			try
			{
				_nodeCache.Values.ToList().ForEach(x => x.Checked = false);

				cmdSave.Enabled = true;
				cmdCancel.Enabled = true;
				cboModule.Enabled = false;

			}
			catch (Exception ex)
			{
				throw;
			}
			finally
			{
				_isLoading = false;
			}
		}

		private void cmdExpand_Click(object sender, EventArgs e)
		{
			_nodeCache.Values.ToList().ForEach(x => x.Expand());
		}

		private void cmdCollapse_Click(object sender, EventArgs e)
		{
			_nodeCache.Values.ToList().ForEach(x => x.Collapse());
		}

		private void tvwEntity_MouseUp(object sender, MouseEventArgs e)
		{
			if (e.Button == System.Windows.Forms.MouseButtons.Right)
			{
				var node = tvwEntity.HitTest(e.X, e.Y).Node;
				if (node != null)
				{
					if (node.Tag is EntityHasEntities && node.Checked)
					{
						var relation = node.Tag as EntityHasEntities;
						var module = _model.Modules.FirstOrDefault(x => x.Name == (string)cboModule.SelectedItem);
						var relationModule = _model.RelationModules.FirstOrDefault(x => x.RelationID == relation.Id && x.ModuleId == module.Id);
						var enforced = _relationEnforcement[relation];
						var contextMenu = new ContextMenu();

						var menuEnforce = new MenuItem() { Checked = enforced, Text = "Enforced", Tag = relation };
						menuEnforce.Click += new EventHandler(MenuEnforce_Click);
						contextMenu.MenuItems.Add(menuEnforce);

						var menuNotEnforce = new MenuItem() { Checked = !enforced, Text = "Not Enforced", Tag = relation };
						menuNotEnforce.Click += new EventHandler(MenuNotEnforce_Click);
						contextMenu.MenuItems.Add(menuNotEnforce);

						contextMenu.Show(tvwEntity, new Point(e.X, e.Y));

					}
				}
			}
		}

		private void MenuEnforce_Click(object sender, EventArgs e)
		{
			var menu = sender as MenuItem;
			var relation = menu.Tag as EntityHasEntities;
			_relationEnforcement[relation] = true;

			cmdSave.Enabled = true;
			cmdCancel.Enabled = true;
			cboModule.Enabled = false;
		}

		private void MenuNotEnforce_Click(object sender, EventArgs e)
		{
			var menu = sender as MenuItem;
			var relation = menu.Tag as EntityHasEntities;
			_relationEnforcement[relation] = false;

			cmdSave.Enabled = true;
			cmdCancel.Enabled = true;
			cboModule.Enabled = false;
		}

	}
}

