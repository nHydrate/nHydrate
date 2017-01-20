#region Copyright (c) 2006-2017 nHydrate.org, All Rights Reserved
// -------------------------------------------------------------------------- *
//                           NHYDRATE.ORG                                     *
//              Copyright (c) 2006-2017 All Rights reserved                   *
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

namespace nHydrate.DslPackage.Forms
{
    public partial class RefactorPreviewChangeVarcharForm : Form
    {
        private Microsoft.VisualStudio.Modeling.Store _store = null;
        private nHydrateModel _model = null;
        private List<Microsoft.VisualStudio.Modeling.ModelElement> _list = null;

        public RefactorPreviewChangeVarcharForm()
        {
            InitializeComponent();
            cboDirection.Items.Add("Varchar -> NVarchar");
            cboDirection.Items.Add("NVarchar -> Varchar");
            cboDirection.SelectedIndex = 0;
            cboDirection.SelectedIndexChanged += cboDirection_SelectedIndexChanged;
        }

        private void cboDirection_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.Populate();
        }

        private DataTypeConstants SourceDatatype
        {
            get { return ((cboDirection.SelectedIndex == 0) ? DataTypeConstants.VarChar : DataTypeConstants.NVarChar); }
        }

        private DataTypeConstants TargetDatatype
        {
            get { return ((cboDirection.SelectedIndex == 1) ? DataTypeConstants.VarChar : DataTypeConstants.NVarChar); }
        }

        public RefactorPreviewChangeVarcharForm(Microsoft.VisualStudio.Modeling.Store store, nHydrateModel model, List<Microsoft.VisualStudio.Modeling.ModelElement> list)
            : this()
        {
            _store = store;
            _model = model;
            _list = list;

            //If the list is null then this use the whole model
            if (_list == null)
            {
                _list = new List<Microsoft.VisualStudio.Modeling.ModelElement>();
                _list.AddRange(_model.Entities);
                _list.AddRange(_model.Views);
                _list.AddRange(_model.StoredProcedures);
                _list.AddRange(_model.Functions);
            }

            this.Populate();
        }

        private void Populate()
        {
            this.DoShow = false;

            //Create root nodes
            var entityListNode = new TreeNode() { Text = "Entities" };
            var viewListNode = new TreeNode() { Text = "Views" };
            var storedProcedureListNode = new TreeNode() { Text = "Stored Procedures" };
            var functionListNode = new TreeNode() { Text = "Functions" };

            #region Entities
            foreach (var item in _model.Entities.Where(x => _list.Contains(x)))
            {
                var fieldList = item.Fields.Where(x => x.DataType == this.SourceDatatype).ToList();
                if (fieldList.Count > 0)
                {
                    //Add entity node
                    var objectNode = new TreeNode() { Text = item.Name };
                    entityListNode.Nodes.Add(objectNode);

                    //Add fields node
                    var fieldListNode = new TreeNode() { Text = "Fields" };
                    objectNode.Nodes.Add(fieldListNode);

                    foreach (var field in fieldList)
                    {
                        var fieldNode = new TreeNode() { Text = field.Name + " (" + field.DataType.ToString() + ")", Tag = field, Checked = true };
                        fieldListNode.Nodes.Add(fieldNode);
                        this.DoShow = true;
                    }
                }
            }
            #endregion

            #region Views
            foreach (var item in _model.Views.Where(x => _list.Contains(x)))
            {
                var fieldList = item.Fields.Where(x => x.DataType == this.SourceDatatype).ToList();
                if (fieldList.Count > 0)
                {
                    //Add view node
                    var objectNode = new TreeNode() { Text = item.Name };
                    viewListNode.Nodes.Add(objectNode);

                    //Add fields node
                    var fieldListNode = new TreeNode() { Text = "Fields" };
                    objectNode.Nodes.Add(fieldListNode);

                    foreach (var field in fieldList)
                    {
                        var fieldNode = new TreeNode() { Text = field.Name + " (" + field.DataType.ToString() + ")", Tag = field, Checked = true };
                        fieldListNode.Nodes.Add(fieldNode);
                        this.DoShow = true;
                    }
                }
            }
            #endregion

            #region Stored Procedures
            foreach (var item in _model.StoredProcedures.Where(x => _list.Contains(x)))
            {
                var fieldList = item.Fields.Where(x => x.DataType == this.SourceDatatype).ToList();
                var parameterList = item.Parameters.Where(x => x.DataType == this.SourceDatatype).ToList();
                if (fieldList.Count + parameterList.Count > 0)
                {
                    //Add Stored Procedure node
                    var objectNode = new TreeNode() { Text = item.Name };
                    storedProcedureListNode.Nodes.Add(objectNode);

                    if (parameterList.Count > 0)
                    {
                        //Add parameters node
                        var parameterListNode = new TreeNode() { Text = "Parameters" };
                        objectNode.Nodes.Add(parameterListNode);

                        foreach (var parameter in parameterList)
                        {
                            var parameterNode = new TreeNode() { Text = parameter.Name + " (" + parameter.DataType.ToString() + ")", Tag = parameter, Checked = true };
                            parameterListNode.Nodes.Add(parameterNode);
                            this.DoShow = true;
                        }
                    }

                    if (fieldList.Count > 0)
                    {
                        //Add fields node
                        var fieldListNode = new TreeNode() { Text = "Fields" };
                        objectNode.Nodes.Add(fieldListNode);

                        foreach (var field in fieldList)
                        {
                            var fieldNode = new TreeNode() { Text = field.Name + " (" + field.DataType.ToString() + ")", Tag = field, Checked = true };
                            fieldListNode.Nodes.Add(fieldNode);
                            this.DoShow = true;
                        }
                    }

                }
            }
            #endregion

            #region Functions
            foreach (var item in _model.Functions.Where(x => _list.Contains(x)))
            {
                var fieldList = item.Fields.Where(x => x.DataType == this.SourceDatatype).ToList();
                var parameterList = item.Parameters.Where(x => x.DataType == this.SourceDatatype).ToList();
                if (fieldList.Count + parameterList.Count > 0)
                {
                    //Add Function node
                    var objectNode = new TreeNode() { Text = item.Name };
                    functionListNode.Nodes.Add(objectNode);

                    if (parameterList.Count > 0)
                    {
                        //Add parameters node
                        var parameterListNode = new TreeNode() { Text = "Parameters" };
                        objectNode.Nodes.Add(parameterListNode);

                        foreach (var parameter in parameterList)
                        {
                            var parameterNode = new TreeNode() { Text = parameter.Name + " (" + parameter.DataType.ToString() + ")", Tag = parameter, Checked = true };
                            parameterListNode.Nodes.Add(parameterNode);
                            this.DoShow = true;
                        }
                    }

                    if (fieldList.Count > 0)
                    {
                        //Add fields node
                        var fieldListNode = new TreeNode() { Text = "Fields" };
                        objectNode.Nodes.Add(fieldListNode);

                        foreach (var field in fieldList)
                        {
                            var fieldNode = new TreeNode() { Text = field.Name + " (" + field.DataType.ToString() + ")", Tag = field, Checked = true };
                            fieldListNode.Nodes.Add(fieldNode);
                            this.DoShow = true;
                        }
                    }

                }
            }
            #endregion

            //Add the root nodes if need be
            tvwItem.Nodes.Clear();
            if (entityListNode.Nodes.Count > 0) tvwItem.Nodes.Add(entityListNode);
            if (viewListNode.Nodes.Count > 0) tvwItem.Nodes.Add(viewListNode);
            if (storedProcedureListNode.Nodes.Count > 0) tvwItem.Nodes.Add(storedProcedureListNode);
            if (functionListNode.Nodes.Count > 0) tvwItem.Nodes.Add(functionListNode);

            tvwItem.AfterCheck += new TreeViewEventHandler(tvwItem_AfterCheck);
            SetupStatus();
        }

        #region Properties

        public bool DoShow { get; private set; }

        #endregion

        #region Methods

        private void SetupStatus()
        {
            var checkedCount = tvwItem.Nodes.GetAllNodes().Count(x => x.Tag != null && x.Checked);
            var uncheckedCount = tvwItem.Nodes.GetAllNodes().Count(x => x.Tag != null && !x.Checked);

            lblStatus.Text = checkedCount + " checked / " + uncheckedCount + " unchecked";
        }

        #endregion

        #region Event Handlers

        private void cmdApply_Click(object sender, EventArgs e)
        {
            try
            {
                //Change the types
                using (var transaction = _store.TransactionManager.BeginTransaction(Guid.NewGuid().ToString()))
                {
                    var allNodes = tvwItem.Nodes
                        .GetAllNodes()
                        .Where(x => x.Checked && x.Tag != null)
                        .ToList();

                    if (allNodes.Count == 0)
                    {
                        MessageBox.Show("There is nothing selected.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }

                    _model.IsLoading = true;
                    foreach (var node in allNodes)
                    {
                        #region Entity.Field
                        if (node.Tag is nHydrate.Dsl.Field)
                        {
                            var field = node.Tag as nHydrate.Dsl.Field;
                            field.DataType = this.TargetDatatype;
                        }
                        #endregion

                        #region View.Field
                        if (node.Tag is nHydrate.Dsl.ViewField)
                        {
                            var field = node.Tag as nHydrate.Dsl.ViewField;
                            field.DataType = this.TargetDatatype;
                        }
                        #endregion

                        #region StoredProcedure.Field
                        if (node.Tag is nHydrate.Dsl.StoredProcedureField)
                        {
                            var field = node.Tag as nHydrate.Dsl.StoredProcedureField;
                            field.DataType = this.TargetDatatype;
                        }
                        #endregion

                        #region Function.Field
                        if (node.Tag is nHydrate.Dsl.FunctionField)
                        {
                            var field = node.Tag as nHydrate.Dsl.FunctionField;
                            field.DataType = this.TargetDatatype;
                        }
                        #endregion

                        #region StoredProcedure.Parameter
                        if (node.Tag is nHydrate.Dsl.StoredProcedureParameter)
                        {
                            var parameter = node.Tag as nHydrate.Dsl.StoredProcedureParameter;
                            parameter.DataType = this.TargetDatatype;
                        }
                        #endregion

                        #region Function.Parameter
                        if (node.Tag is nHydrate.Dsl.FunctionParameter)
                        {
                            var parameter = node.Tag as nHydrate.Dsl.FunctionParameter;
                            parameter.DataType = this.TargetDatatype;
                        }
                        #endregion

                    }
                    _model.IsLoading = false;
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

        private class DisplayItem
        {
            public Field Field { get; set; }

            public override string ToString()
            {
                return this.Field.Entity.Name + "." + this.Field.Name;
            }
        }

        private void cmdUncheckAll_Click(object sender, EventArgs e)
        {
            tvwItem.Nodes.GetAllNodes().ForEach(x => x.Checked = false);
        }

        private void cmdCheckAllText_Click(object sender, EventArgs e)
        {
            tvwItem.Nodes
                .GetAllNodes()
                .Where(x => x.Tag is nHydrate.Dsl.Field)
                .ToList()
                .ToList()
                .ForEach(z => z.Checked = true);

            tvwItem.Nodes
                .GetAllNodes()
                .Where(x => x.Tag is nHydrate.Dsl.ViewField)
                .ToList()
                .ToList()
                .ForEach(z => z.Checked = true);

            tvwItem.Nodes
                .GetAllNodes()
                .Where(x => x.Tag is nHydrate.Dsl.StoredProcedureField)
                .ToList()
                .ToList()
                .ForEach(z => z.Checked = true);

            tvwItem.Nodes
                .GetAllNodes()
                .Where(x => x.Tag is nHydrate.Dsl.FunctionField)
                .ToList()
                .ToList()
                .ForEach(z => z.Checked = true);

            tvwItem.Nodes
                .GetAllNodes()
                .Where(x => x.Tag is nHydrate.Dsl.StoredProcedureParameter)
                .ToList()
                .ToList()
                .ForEach(z => z.Checked = true);

            tvwItem.Nodes
                .GetAllNodes()
                .Where(x => x.Tag is nHydrate.Dsl.FunctionParameter)
                .ToList()
                .ToList()
                .ForEach(z => z.Checked = true);
        }

        private void cmdExpandAll_Click(object sender, EventArgs e)
        {
            tvwItem.BeginUpdate();
            try
            {
                tvwItem.Nodes
                    .GetAllNodes()
                    .ForEach(x => x.Expand());
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                tvwItem.EndUpdate();
            }
        }

        private void cmdCollapseAll_Click(object sender, EventArgs e)
        {
            tvwItem.BeginUpdate();
            try
            {
                tvwItem.Nodes
                .GetAllNodes()
                .ForEach(x => x.Collapse());
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                tvwItem.EndUpdate();
            }
        }

        private bool _isWorking = false;
        private void tvwItem_AfterCheck(object sender, TreeViewEventArgs e)
        {
            if (_isWorking) return;
            _isWorking = true;

            CheckTree(e.Node.Nodes, e.Node.Checked);

            //if (e.Node.Parent == null)
            //{
            //    e.Node.Nodes.ToList<TreeNode>().ForEach(x => x.Checked = e.Node.Checked);
            //}
            //else
            //{
            //    if (e.Node.Checked)
            //    {
            //        e.Node.Parent.Checked = true;
            //    }
            //    else
            //    {
            //        //If NO fields for an entity are checked then uncheck parent entity
            //        if (e.Node.Parent.Nodes.ToList<TreeNode>().Count(x => x.Checked) == 0)
            //        {
            //            e.Node.Parent.Checked = false;
            //        }
            //    }
            //}

            SetupStatus();
            _isWorking = false;
        }

        #endregion

        private void CheckTree(TreeNodeCollection nodes, bool isChecked)
        {
            foreach (TreeNode node in nodes)
            {
                node.Checked = isChecked;
                CheckTree(node.Nodes, isChecked);
            }
        }

    }
}

