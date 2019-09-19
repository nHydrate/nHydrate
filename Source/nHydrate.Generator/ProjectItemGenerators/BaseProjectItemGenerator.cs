#region Copyright (c) 2006-2019 nHydrate.org, All Rights Reserved
// -------------------------------------------------------------------------- *
//                           NHYDRATE.ORG                                     *
//              Copyright (c) 2006-2019 All Rights reserved                   *
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
using System.Linq;
using nHydrate.Generator.Common.EventArgs;
using nHydrate.Generator.Common.GeneratorFramework;
using nHydrate.Generator.Models;

namespace nHydrate.Generator.ProjectItemGenerators
{
    public abstract class BaseProjectItemGenerator : IProjectItemGenerator
    {
        #region Class Member

        protected ModelRoot _model;

        #endregion

        public event System.EventHandler GenerationStarted;
        public event ProjectItemGenerationCompleteEventHandler GenerationComplete;
        public event ProjectItemGeneratedEventHandler ProjectItemGenerated;
        public event ProjectItemDeletedEventHandler ProjectItemDeleted;
        public event ProjectItemExistsEventHandler ProjectItemExists;
        public event ProjectItemGeneratedErrorEventHandler ProjectItemGenerationError;

        public abstract string LocalNamespaceExtension { get; }

        public virtual void Initialize(IModelObject model)
        {
            try
            {
                _model = (ModelRoot)model;

                var hasMetaData = false;
                foreach (var table in (from x in _model.Database.Tables where x.Generated orderby x.Name select x))
                {
                    if (table.CreateMetaData)
                        hasMetaData = true;
                }

                Table projectItemDataType = null;
                if (hasMetaData)
                {
                    #region Create the PROPERTY_ITEM_DATA_TYPE table
                    projectItemDataType = _model.Database.Tables.Add("PROPERTY_ITEM_DATA_TYPE");
                    projectItemDataType.IsMetaDataMaster = true;

                    Column column = null;
                    column = _model.Database.Columns.Add("property_item_data_type_id");
                    column.ParentTableRef = projectItemDataType.CreateRef();
                    column.DataType = System.Data.SqlDbType.Int;
                    column.PrimaryKey = true;
                    column.Identity = IdentityTypeConstants.None;
                    column.AllowNull = false;
                    projectItemDataType.Columns.Add(column.CreateRef());

                    column = _model.Database.Columns.Add("name");
                    column.ParentTableRef = projectItemDataType.CreateRef();
                    column.DataType = System.Data.SqlDbType.VarChar;
                    column.Length = 50;
                    column.AllowNull = false;
                    projectItemDataType.Columns.Add(column.CreateRef());
                    #endregion
                }

                foreach (var table in (from x in _model.Database.Tables where x.Generated orderby x.Name select x))
                {
                    if (table.CreateMetaData)
                    {
                        Column column = null;

                        #region Create the PROPERTY_ITEM_DEFINE table
                        var projectItemDefineTable = _model.Database.Tables.Add(table.DatabaseName + "_PROPERTY_ITEM_DEFINE");
                        projectItemDefineTable.IsMetaDataDefinition = true;

                        column = _model.Database.Columns.Add("property_item_define_id");
                        column.ParentTableRef = projectItemDefineTable.CreateRef();
                        column.DataType = System.Data.SqlDbType.Int;
                        column.PrimaryKey = true;
                        column.Identity = IdentityTypeConstants.Database;
                        column.AllowNull = false;
                        projectItemDefineTable.Columns.Add(column.CreateRef());

                        column = _model.Database.Columns.Add("name");
                        column.ParentTableRef = projectItemDefineTable.CreateRef();
                        column.DataType = System.Data.SqlDbType.VarChar;
                        column.Length = 50;
                        column.AllowNull = false;
                        column.UIVisible = true;
                        column.SortOrder = 0;
                        column.FriendlyName = "Name";
                        projectItemDefineTable.Columns.Add(column.CreateRef());

                        column = _model.Database.Columns.Add("property_item_data_type");
                        column.EnumType = "PropertyBagDataTypeConstants";
                        column.ParentTableRef = projectItemDefineTable.CreateRef();
                        column.DataType = System.Data.SqlDbType.Int;
                        column.UIVisible = true;
                        column.SortOrder = 1;
                        column.FriendlyName = "Data type";
                        column.AllowNull = false;
                        projectItemDefineTable.Columns.Add(column.CreateRef());

                        //RELATIONRELATIONRELATIONRELATIONRELATIONRELATIONRELATIONRELATIONRELATIONRELATION
                        //Add a relation to from the datatype table to this one
                        var relation2 = _model.Database.Relations.Add();
                        relation2.ParentTableRef = projectItemDataType.CreateRef();
                        relation2.ChildTableRef = projectItemDefineTable.CreateRef();
                        var relationship = new ColumnRelationship((INHydrateModelObject)relation2.Root);
                        relationship.ParentColumnRef = ((Column)projectItemDataType.Columns[0].Object).CreateRef();
                        relationship.ChildColumnRef = column.CreateRef();
                        relation2.ColumnRelationships.Add(relationship);
                        projectItemDataType.Relationships.Add(relation2.CreateRef());
                        //RELATIONRELATIONRELATIONRELATIONRELATIONRELATIONRELATIONRELATIONRELATIONRELATION            

                        column = _model.Database.Columns.Add("group");
                        column.ParentTableRef = projectItemDefineTable.CreateRef();
                        column.DataType = System.Data.SqlDbType.VarChar;
                        column.Length = 50;
                        column.AllowNull = true;
                        column.UIVisible = true;
                        column.SortOrder = 2;
                        column.FriendlyName = "Group";
                        projectItemDefineTable.Columns.Add(column.CreateRef());

                        column = _model.Database.Columns.Add("sort_index");
                        column.ParentTableRef = projectItemDefineTable.CreateRef();
                        column.DataType = System.Data.SqlDbType.Int;
                        column.AllowNull = false;
                        column.UIVisible = true;
                        column.SortOrder = 3;
                        column.FriendlyName = "Sort order";
                        projectItemDefineTable.Columns.Add(column.CreateRef());

                        column = _model.Database.Columns.Add("minimum_value");
                        column.ParentTableRef = projectItemDefineTable.CreateRef();
                        column.DataType = System.Data.SqlDbType.VarChar;
                        column.Length = 50;
                        column.UIVisible = true;
                        column.SortOrder = 4;
                        column.FriendlyName = "Minimum value";
                        projectItemDefineTable.Columns.Add(column.CreateRef());

                        column = _model.Database.Columns.Add("maximum_value");
                        column.ParentTableRef = projectItemDefineTable.CreateRef();
                        column.DataType = System.Data.SqlDbType.VarChar;
                        column.Length = 50;
                        column.UIVisible = true;
                        column.SortOrder = 5;
                        column.FriendlyName = "Maximum value";
                        projectItemDefineTable.Columns.Add(column.CreateRef());

                        column = _model.Database.Columns.Add("max_length");
                        column.ParentTableRef = projectItemDefineTable.CreateRef();
                        column.DataType = System.Data.SqlDbType.Int;
                        column.UIVisible = true;
                        column.SortOrder = 6;
                        column.FriendlyName = "Maximum Length";
                        projectItemDefineTable.Columns.Add(column.CreateRef());


                        column = _model.Database.Columns.Add("is_required");
                        column.ParentTableRef = projectItemDefineTable.CreateRef();
                        column.DataType = System.Data.SqlDbType.Bit;
                        column.AllowNull = false;
                        column.UIVisible = true;
                        column.SortOrder = 7;
                        column.FriendlyName = "Required";
                        projectItemDefineTable.Columns.Add(column.CreateRef());

                        #endregion

                        #region Create the PROPERTY_ITEM table
                        var projectItemValueTable = _model.Database.Tables.Add(table.DatabaseName + "_PROPERTY_ITEM");
                        projectItemValueTable.IsMetaData = true;

                        column = _model.Database.Columns.Add("property_item_id");
                        column.ParentTableRef = projectItemValueTable.CreateRef();
                        column.DataType = System.Data.SqlDbType.Int;
                        column.PrimaryKey = true;
                        column.AllowNull = false;
                        column.Identity = IdentityTypeConstants.Database;
                        projectItemValueTable.Columns.Add(column.CreateRef());

                        column = _model.Database.Columns.Add("property_item_define_id");
                        column.ParentTableRef = projectItemValueTable.CreateRef();
                        column.DataType = System.Data.SqlDbType.Int;
                        column.AllowNull = false;
                        projectItemValueTable.Columns.Add(column.CreateRef());

                        column = _model.Database.Columns.Add("item_value");
                        column.ParentTableRef = projectItemValueTable.CreateRef();
                        column.DataType = System.Data.SqlDbType.VarChar;
                        column.Length = 1024;
                        column.AllowNull = false;
                        projectItemValueTable.Columns.Add(column.CreateRef());

                        //Create all primary keys
                        foreach (var pkColumn in table.PrimaryKeyColumns.OrderBy(x => x.Name))
                        {
                            column = _model.Database.Columns.Add(pkColumn.DatabaseName);
                            column.ParentTableRef = projectItemValueTable.CreateRef();
                            column.DataType = pkColumn.DataType;
                            column.Length = pkColumn.Length;
                            column.AllowNull = false;
                            projectItemValueTable.Columns.Add(column.CreateRef());
                        }

                        //Add relationship between Definition and Value table
                        if (true)
                        {
                            var relation = new Relation(this._model);
                            relation.ParentTableRef = projectItemDefineTable.CreateRef();
                            relation.ChildTableRef = projectItemValueTable.CreateRef();
                            var colRel = new ColumnRelationship(_model);
                            colRel.ParentColumnRef = projectItemDefineTable.Columns["property_item_define_id"];
                            colRel.ChildColumnRef = projectItemValueTable.Columns["property_item_define_id"];
                            relation.ColumnRelationships.Add(colRel);
                            relation.RoleName = string.Empty;
                            _model.Database.Relations.Add(relation);
                            projectItemDefineTable.Relationships.Add(relation.CreateRef());
                        }

                        //Add relationship between Value table and primary table
                        if (true)
                        {
                            var relation = new Relation(this._model);
                            relation.ParentTableRef = table.CreateRef();
                            relation.ChildTableRef = projectItemValueTable.CreateRef();
                            foreach (var pkColumn in table.PrimaryKeyColumns.OrderBy(x => x.Name))
                            {
                                var colRel = new ColumnRelationship(_model);
                                colRel.ParentColumnRef = table.Columns[pkColumn.DatabaseName];
                                colRel.ChildColumnRef = projectItemValueTable.Columns[pkColumn.DatabaseName];
                                relation.RoleName = string.Empty;
                                relation.ColumnRelationships.Add(colRel);
                                table.Relationships.Add(relation.CreateRef());
                            }
                            _model.Database.Relations.Add(relation);
                        }

                        #endregion
                    }
                }

                //_model = (ModelRoot)model;

            }
            catch (Exception ex)
            {
                throw;
            }

        }

        protected virtual void OnGenerationStart(object sender, System.EventArgs e)
        {
            if (this.GenerationStarted != null)
            {
                this.GenerationStarted(sender, e);
            }
        }

        protected virtual void OnProjectItemGenerated(object sender, ProjectItemGeneratedEventArgs pigArgs)
        {
            if (this.ProjectItemGenerated != null)
            {
                this.ProjectItemGenerated(sender, pigArgs);
            }
        }

        protected virtual void OnProjectItemDeleted(object sender, ProjectItemDeletedEventArgs pigArgs)
        {
            if (this.ProjectItemDeleted != null)
            {
                this.ProjectItemDeleted(sender, pigArgs);
            }
        }

        protected virtual void OnGenerationComplete(object sender, ProjectItemGenerationCompleteEventArgs args)
        {
            if (this.GenerationComplete != null)
            {
                this.GenerationComplete(sender, args);
            }
        }

        protected virtual void OnProjectItemExists(object sender, ProjectItemExistsEventArgs args)
        {
            if (this.ProjectItemExists != null)
            {
                this.ProjectItemExists(sender, args);
            }
        }

        protected virtual void OnProjectItemGeneratedError(object sender, ProjectItemGeneratedErrorEventArgs args)
        {
            if (this.ProjectItemGenerationError != null)
            {
                this.ProjectItemGenerationError(sender, args);
            }
        }

        public virtual string DefaultNamespace
        {
            get { return nHydrateGeneratorProject.DomainProjectName(_model); }
        }

        public virtual string GetLocalNamespace()
        {
            if (string.IsNullOrEmpty(this.LocalNamespaceExtension))
                return this.DefaultNamespace;
            else
                return this.DefaultNamespace + "." + this.LocalNamespaceExtension;
        }

        public virtual string ProjectName
        {
            get { return this.GetLocalNamespace(); }
        }

        public abstract void Generate();
        public abstract int FileCount { get; }

    }
}
