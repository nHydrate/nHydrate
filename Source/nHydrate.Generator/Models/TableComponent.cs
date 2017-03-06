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
using System.Linq;
using System.Xml;
using nHydrate.Generator.Common.GeneratorFramework;
using nHydrate.Generator.Common.Util;

namespace nHydrate.Generator.Models
{
	//[Designer(typeof(nHydrate.Generator.Design.Designers.TableDesigner))]
	//[DesignTimeVisible(true)]
	public class TableComponent : BaseModelObject, ICodeFacadeObject
	{
		#region Member Variables

		protected const bool _def_generated = true;
		protected const string _def_codeFacade = "";

		protected Table _parent = null;
		protected string _name = string.Empty;
		protected string _description = string.Empty;
		protected string _codeFacade = _def_codeFacade;
		protected ReferenceCollection _columns = null;
		protected bool _generated = _def_generated;

		#endregion

		#region Constructor

		public TableComponent(INHydrateModelObject root, Table parent)
			: base(root)
		{
			_parent = parent;
			_columns = new ReferenceCollection(this.Root, this, ReferenceType.Column);

			_columns.ObjectPlural = "Fields";
			_columns.ObjectSingular = "Field";
			_columns.ImageIndex = ImageHelper.GetImageIndex(TreeIconConstants.FolderClose);
			_columns.SelectedImageIndex = ImageHelper.GetImageIndex(TreeIconConstants.FolderOpen);

		}

		#endregion

		#region Property Implementations

		[Browsable(false)]
		public Table Parent
		{
			get { return _parent; }
		}

		[
		Browsable(true),
		Description("Determines if this item is used in the generation."),
		Category("Data"),
		DefaultValue(_def_generated),
		]
		public bool Generated
		{
			get { return _generated; }
			set
			{
				_generated = value;
				this.OnPropertyChanged(this, new PropertyChangedEventArgs("Generated"));
			}
		}

		[Browsable(false)]
		public string CamelName
		{
			//get { return StringHelper.DatabaseNameToCamelCase(this.Name); }
			get { return StringHelper.DatabaseNameToCamelCase(this.PascalName); }
		}

		[Browsable(false)]
		public string PascalName
		{
			get
			{
				if ((!string.IsNullOrEmpty(this.CodeFacade)) && (((ModelRoot)this.Root).TransformNames))
					return StringHelper.DatabaseNameToPascalCase(this.CodeFacade);
				else if ((this.CodeFacade == "") && (((ModelRoot)this.Root).TransformNames))
					return StringHelper.DatabaseNameToPascalCase(this.Name);
				if ((!string.IsNullOrEmpty(this.CodeFacade)) && !(((ModelRoot)this.Root).TransformNames))
					return this.CodeFacade;
				else if ((this.CodeFacade == "") && !(((ModelRoot)this.Root).TransformNames))
					return this.Name;
				return this.Name; //Default
			}
		}

		[Browsable(false)]
		public string DatabaseName
		{
			get { return this.Name; }
		}

		[
		Browsable(true),
		Description("Determines the name of this component."),
		Category("Design"),
		DefaultValue("")
		]
		public string Name
		{
			get { return _name; }
			set
			{
				_name = value;
				this.OnPropertyChanged(this, new PropertyChangedEventArgs("Name"));
			}
		}

		[
		Browsable(false),
		Description("Determines the columns that are associated with this table."),
		Category("Data"),
		//TypeConverter(typeof(nHydrate.Generator.Design.Converters.ColumnReferenceCollectionConverter)),
		//Editor(typeof(nHydrate.Generator.Design.Editors.ColumnReferenceCollectionEditor), typeof(System.Drawing.Design.UITypeEditor))
		]
		public ReferenceCollection Columns
		{
			get { return _columns; }
		}

		[
		Browsable(true),
		Description("Determines the description of this table."),
		Category("Data"),
		DefaultValue(""),
		]
		public string Description
		{
			get { return _description; }
			set
			{
				_description = value;
				this.OnPropertyChanged(this, new PropertyChangedEventArgs("Description"));
			}
		}

		/// <summary>
		/// Returns the realationships that are valid baesd on the column selected from the parent table
		/// </summary>
		[Browsable(false)]
		public List<Relation> AllValidRelationships
		{
			get
			{
				var validRelations = new List<Relation>();
				foreach (Relation relation in this.Parent.AllRelationships)
				{
					var parentTable = ((Table)relation.ParentTableRef.Object);
					var childTable = ((Table)relation.ChildTableRef.Object);
					if (parentTable.Generated && childTable.Generated && !parentTable.AssociativeTable && !childTable.AssociativeTable)
					{
						var columnCount = 0;
						if ((parentTable == this.Parent) && (childTable == this.Parent))
						{
							//Self-reference
							foreach (ColumnRelationship columnRelationship in relation.ColumnRelationships)
							{
								var parentColumn = (Column)columnRelationship.ParentColumnRef.Object;
								var childColumn = (Column)columnRelationship.ChildColumnRef.Object;
								if (this.Columns.Contains(parentColumn.Id) && this.Columns.Contains(childColumn.Id))
								{
									columnCount++;
								}
							}
						}
						else if (parentTable == this.Parent)
						{
							foreach (ColumnRelationship columnRelationship in relation.ColumnRelationships)
							{
								var column = (Column)columnRelationship.ParentColumnRef.Object;
								if (this.Columns.Contains(column.Id))
								{
									columnCount++;
								}
							}
						}
						else
						{
							foreach (ColumnRelationship columnRelationship in relation.ColumnRelationships)
							{
								var column = (Column)columnRelationship.ChildColumnRef.Object;
								if (this.Columns.Contains(column.Id))
								{
									columnCount++;
								}
							}
						}

						if (columnCount == relation.ColumnRelationships.Count)
							validRelations.Add(relation);

					}
				}
				return validRelations;
			}
		}

		[Browsable(false)]
		public IEnumerable<Column> GeneratedColumns
		{
			get
			{
				return this.GetColumns()
					.Where(x => x.Generated)
					.OrderBy(x => x.Name);
			}
		}

		#endregion

		#region Methods

		/// <summary>
		/// Returns the column for this table only (not hierarchy)
		/// </summary>
		/// <returns></returns>
		public IEnumerable<Column> GetColumns()
		{
			try
			{
				var retval = new ColumnCollection(this.Root);
				foreach (Reference r in this.Columns)
				{
					retval.Add((Column)r.Object);
				}
				return retval.OrderBy(x => x.Name);
			}
			catch (Exception ex)
			{
				throw;
			}
		}

		public string GetSQLSchema()
		{
			if (string.IsNullOrEmpty(this.Parent.DBSchema)) return "dbo";
			return this.Parent.DBSchema;
		}

		#endregion

		#region IXMLable Members

		public override void XmlAppend(XmlNode node)
		{
			try
			{
				var oDoc = node.OwnerDocument;

				XmlHelper.AddAttribute(node, "key", this.Key);
				XmlHelper.AddAttribute(node, "name", this.Name);

				if (this.CodeFacade != _def_codeFacade)
					XmlHelper.AddAttribute(node, "codeFacade", this.CodeFacade);

				var columnsNode = oDoc.CreateElement("columns");
				this.Columns.XmlAppend(columnsNode);
				node.AppendChild(columnsNode);

			}
			catch(Exception ex)
			{
				throw;
			}

		}

		public override void XmlLoad(XmlNode node)
		{
			try
			{
				var columnsNode = node.SelectSingleNode("columns");
				if (columnsNode != null)
					this.Columns.XmlLoad(columnsNode);

				this.Name = XmlHelper.GetAttributeValue(node, "name", string.Empty);
				this.CodeFacade = XmlHelper.GetAttributeValue(node, "codeFacade", _def_codeFacade);

				this.Dirty = false;
			}
			catch(Exception ex)
			{
				throw;
			}
		}

		#endregion

		#region ICodeFacadeObject Members

		[
		Browsable(true),
		Description("Determines the object name used in the API. If this property is blank the 'Name' property is used in the API. This property can be used to mask the database identifier."),
		Category("Design"),
		DefaultValue(_def_codeFacade),
		]
		public string CodeFacade
		{
			get { return _codeFacade; }
			set
			{
				_codeFacade = value;
				this.OnPropertyChanged(this, new PropertyChangedEventArgs("codeFacade"));
			}
		}

		public string GetCodeFacade()
		{
			if (this.CodeFacade == "")
				return this.Name;
			else
				return this.CodeFacade;
		}

		#endregion

		#region Helpers

		public override string ToString()
		{
			var retval = this.Name;
			return retval;
		}

		#endregion

	}
}
