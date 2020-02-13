#pragma warning disable 0168
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using nHydrate.Generator.Common;
using nHydrate.Generator.Common.GeneratorFramework;
using nHydrate.Generator.Models;

namespace nHydrate.Generator.ModelUI
{
    public partial class GenerateSettings : Form
    {
        #region Class Members

        private List<Type> _wizardTypeList = null;
        private List<Type> _generatorTypeList = null;
        private readonly Dictionary<string, Type> _generatorMap = new Dictionary<string, Type>();
        private readonly IGenerator _generator = null;
        private List<string> _moduleList = null;
        private List<Type> _invisibleList = new List<Type>();

        #endregion

        #region Constructors

        public GenerateSettings()
        {
            InitializeComponent();

            chkModule.ItemCheck += new ItemCheckEventHandler(chkModule_ItemCheck);
            this.tvwProjects.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.tvwProjects_AfterCheck);
            this.tvwProjects.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tvwProjects_AfterSelect);
            wizard1.Cancel += new System.ComponentModel.CancelEventHandler(wizard1_Cancel);
            wizard1.Finish += new EventHandler(wizard1_Finish);
            wizard1.BeforeSwitchPages += new Wizard.Wizard.BeforeSwitchPagesEventHandler(wizard1_BeforeSwitchPages);
            wizard1.AfterSwitchPages += new Wizard.Wizard.AfterSwitchPagesEventHandler(wizard1_AfterSwitchPages);
        }

        public GenerateSettings(IGenerator generator, List<Type> generatorTypeList, List<Type> wizardTypeList, List<string> moduleList)
            : this()
        {
            try
            {
                _generator = generator;
                _moduleList = moduleList;
                _generatorTypeList = generatorTypeList;
                _wizardTypeList = wizardTypeList;

                var globalCacheFile = new GlobalCacheFile();
                var cacheFile = new ModelCacheFile(_generator);
                foreach (var v in generatorTypeList)
                {
                    var attribute = v.GetCustomAttributes(typeof(GeneratorProjectAttribute), true).Cast<GeneratorProjectAttribute>().FirstOrDefault();
                    if (!globalCacheFile.ExcludeList.Contains(v.FullName))
                    {
                        _generatorMap.Add(attribute.Name, v);
                    }
                }
                this.LoadGenerators(true);

                //Add modules
                foreach (var s in moduleList.OrderBy(x => x))
                {
                    chkModule.Items.Add(s);
                    if (cacheFile.GeneratedModuleList.Count(x => x.ToLower() == s.ToLower()) > 0)
                        chkModule.SetItemChecked(chkModule.Items.Count - 1, true);
                }

                if (moduleList.Count == 0)
                {
                    wizard1.WizardPages.Remove(pageModules);
                }

                wizard1.FinishEnabled = (moduleList.Count == 0);

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        #endregion

        #region Properties

        public List<Type> ExcludeList
        {
            get
            {
                var retval = new List<Type>();

                foreach (var key in _generatorMap.Keys)
                {
                    retval.Add(_generatorMap[key]);
                }

                foreach (TreeNode n in tvwProjects.Nodes)
                {
                    if (n.Checked)
                        retval.Remove(_generatorMap[n.Text]);
                }

                //Add all invisible generators to list
                _invisibleList.ForEach(x => retval.Add(x));

                return retval;
            }
        }

        public List<string> SelectedModules
        {
            get
            {
                var retval = new List<string>();
                foreach (string item in chkModule.CheckedItems)
                {
                    retval.Add(item);
                }
                return retval;
            }
        }

        #endregion

        #region Methods

        private void LoadGenerators(bool isMain)
        {
            try
            {
                var globalCacheFile = new GlobalCacheFile();
                var cacheFile = new ModelCacheFile(_generator);
                tvwProjects.Nodes.Clear();

                foreach (var key in _generatorMap.Keys)
                {
                    var sysType = _generatorMap[key];
                    if (!globalCacheFile.ExcludeList.Contains(sysType.FullName))
                    {
                        var attribute = sysType.GetCustomAttributes(typeof(GeneratorProjectAttribute), true).Cast<GeneratorProjectAttribute>().FirstOrDefault();
                        if ((isMain && attribute.IsMain) || !isMain)
                        {
                            var typeName = string.Empty;
                            if (attribute.CurrentType != null) typeName = attribute.CurrentType.ToString();
                            var node = tvwProjects.Nodes.Add(typeName, attribute.Name);
                            node.Tag = attribute;

                            if (_wizardTypeList != null)
                            {
                                node.Checked = _wizardTypeList.Contains(sysType);
                            }
                            else
                            {
                                node.Checked = !cacheFile.ExcludeList.Contains(sysType.FullName);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private bool IsModulesChecked()
        {
            return (chkModule.CheckedItems.Count > 0);
        }

        private bool IsTemplatesChecked()
        {
            var checkedCount = 0;
            foreach (TreeNode n in tvwProjects.Nodes)
            {
                if (n.Checked) checkedCount++;
            }

            return (checkedCount > 0);
        }

        #endregion

        #region Event Handlers

        private void wizard1_Finish(object sender, EventArgs e)
        {
            if (chkModule.Items.Count > 0)
            {
                if (chkModule.CheckedItems.Count == 0)
                {
                    MessageBox.Show("You must select one or more modules for generation.", "Check Modules", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
            }

            var cacheFile = new ModelCacheFile(_generator);
            cacheFile.ExcludeList.Clear();
            foreach (var t in this.ExcludeList)
            {
                cacheFile.ExcludeList.Add(t.FullName);
            }

            cacheFile.GeneratedModuleList.Clear();
            foreach (string s in chkModule.CheckedItems)
            {
                cacheFile.GeneratedModuleList.Add(s);
            }

            cacheFile.Save();

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void wizard1_Cancel(object sender, System.ComponentModel.CancelEventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void tvwProjects_AfterSelect(object sender, TreeViewEventArgs e)
        {
            var nodeList = new List<TreeNode>();
            foreach (TreeNode itemNode in tvwProjects.Nodes)
                nodeList.Add(itemNode);

            lstDependency.Items.Clear();
            if (tvwProjects.SelectedNode == null) return;

            //Now setup the dependencies
            var attribute = tvwProjects.SelectedNode.Tag as GeneratorProjectAttribute;
            txtDescription.Text = attribute.Description;
            if (attribute.DependencyList != null)
            {
                foreach (var dependencyName in attribute.DependencyList)
                {
                    var dependencyNode = nodeList.Where(x => x.Name == dependencyName).FirstOrDefault();
                    if (dependencyNode != null)
                    {
                        lstDependency.Items.Add(dependencyNode.Text);
                    }
                }
            }

            //TODO: add property grid and can see this config object
            //may not need anymore as was being used to set database type and this might not even be needed for EF core
            //propertyGrid1.SelectedObject = _generator.Model.ModelConfigurations[attribute.CurrentType.Name];
        }

        private void tvwProjects_AfterCheck(object sender, TreeViewEventArgs e)
        {
            if (e.Node.Checked)
            {
                var nodeList = new List<TreeNode>();
                foreach (TreeNode itemNode in tvwProjects.Nodes)
                    nodeList.Add(itemNode);

                //Now check the dependencies
                var attribute = e.Node.Tag as GeneratorProjectAttribute;
                txtDescription.Text = attribute.Description;
                if (attribute.DependencyList != null)
                {
                    foreach (var name in attribute.DependencyList)
                    {
                        var dependencyNode = nodeList.Where(x => x.Name == name).FirstOrDefault();
                        if (dependencyNode != null)
                        {
                            dependencyNode.Checked = true;
                        }
                    }
                }

                if (attribute.ExclusionList != null)
                {
                    foreach (var name in attribute.ExclusionList)
                    {
                        var dependencyNode = nodeList.Where(x => (x.Tag as nHydrate.Generator.Common.GeneratorFramework.GeneratorProjectAttribute).CurrentType.ToString() == name).FirstOrDefault();
                        if (dependencyNode != null)
                        {
                            dependencyNode.Checked = false;
                        }
                    }
                }
            
            }
        }

        private void wizard1_AfterSwitchPages(object sender, Wizard.Wizard.AfterSwitchPagesEventArgs e)
        {
            if (_moduleList.Count == 0)
            {
                wizard1.FinishEnabled = true;
            }
            else
            {
                var checkedCount = 0;
                foreach (TreeNode n in tvwProjects.Nodes)
                {
                    if (n.Checked) checkedCount++;
                }

                wizard1.FinishEnabled =
                    (checkedCount > 0) &&
                    (chkModule.CheckedItems.Count > 0);
            }
        }

        private void wizard1_BeforeSwitchPages(object sender, Wizard.Wizard.BeforeSwitchPagesEventArgs e)
        {
            if (e.OldIndex < e.NewIndex)
            {
                if (wizard1.WizardPages[e.OldIndex] == pageTemplates)
                {
                    if (_moduleList.Count > 0)
                    {
                        if (!this.IsTemplatesChecked())
                        {
                            e.Cancel = true;
                            MessageBox.Show("You must choose items to generate.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                else if (wizard1.WizardPages[e.OldIndex] == pageModules)
                {
                    if (_moduleList.Count > 0)
                    {
                        if (!this.IsModulesChecked())
                        {
                            e.Cancel = true;
                            MessageBox.Show("You must choose one or more modules.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }

            }
        }

        private void chkModule_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            wizard1.FinishEnabled = (IsModulesChecked() && IsTemplatesChecked());
        }

        #endregion

        private void linkShowAll_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            LoadGenerators(false);
            linkShowAll.Visible = false;
        }

    }
}