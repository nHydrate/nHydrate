#pragma warning disable 0168
using System;
using System.Collections;
using System.Drawing;
using System.Windows.Forms;

namespace nHydrate.Generator
{
	internal enum ImageConstants
	{
		Default,
		Model,
		Database,
		Entities,
		Entity,
		Fields,
		Field,
		Views,
		View,
	}

	internal enum TreeIconConstants
	{
		Attribute,
		Column,
		ColumnPrimaryKey,
		ColumnForeignKey,
		Database,
		Domain,
		Table,
		TableType,
		Package,
		Packages,
		Error,
		Relationship,
		Entity,
		TableAssociative,
		Tables,
		FolderClose,
		FolderOpen,
		UI,
		Up,
		Down,
		CustomView,
		CustomViews,
		CustomViewColumn,
		CustomRetrieveRule,
		CustomRetrieveRules,
		TableNonGen,
		ColumnInherit,
		TableDerived,
	}

	internal class ImageHelper
	{
		internal static Hashtable ImageMapper = new Hashtable();
		internal static ImageList ImageList = null;

		private ImageHelper()
		{
		}

		static ImageHelper()
		{
			GetImageList();
		}

		public static Bitmap GetImage(ImageConstants image)
		{
			Bitmap retval = null;
			switch(image)
			{
				case ImageConstants.Default:
					retval = GetImage("Default.png");
					break;
				case ImageConstants.Model:
					retval = GetImage("Model.png");
					break;
				case ImageConstants.Database:
					retval = GetImage("Database.png");
					break;
				case ImageConstants.Entities:
					retval = GetImage("Entities.png");
					break;
				case ImageConstants.Entity:
					retval = GetImage("Entity.png");
					break;
				case ImageConstants.Fields:
					retval = GetImage("Fields.png");
					break;
				case ImageConstants.Field:
					retval = GetImage("Field.png");
					break;
				case ImageConstants.Views:
					retval = GetImage("Default.png");
					break;
				case ImageConstants.View:
					retval = GetImage("Default.png");
					break;
			}
			return retval;
		}

		public static Icon GetIcon(TreeIconConstants image)
		{
			Icon retval = null;
			switch (image)
			{
				case TreeIconConstants.Attribute:
					retval = GetIcon("Attribute.ico");
					break;
				case TreeIconConstants.Column:
					retval = GetIcon("Column.ico");
					break;
				case TreeIconConstants.ColumnInherit:
					retval = GetIcon("ColumnInherit.ico");
					break;
				case TreeIconConstants.ColumnPrimaryKey:
					retval = GetIcon("ColumnPrimaryKey.ico");
					break;
				case TreeIconConstants.ColumnForeignKey:
					retval = GetIcon("AttributeForeignKey.ico");
					break;
				case TreeIconConstants.Database:
					retval = GetIcon("Database.ico");
					break;
				case TreeIconConstants.Domain:
					retval = GetIcon("Domain.ico");
					break;
				case TreeIconConstants.Table:
					retval = GetIcon("Table.ico");
					break;
				case TreeIconConstants.TableType:
					retval = GetIcon("TableType.ico");
					break;
				case TreeIconConstants.Error:
					retval = GetIcon("Error.ico");
					break;
				case TreeIconConstants.Relationship:
					retval = GetIcon("Relationship.ico");
					break;
				case TreeIconConstants.Entity:
					retval = GetIcon("Table.ico");
					break;
				case TreeIconConstants.TableDerived:
					retval = GetIcon("TableDerived.ico");
					break;
				case TreeIconConstants.TableAssociative:
					retval = GetIcon("TableAssociative.ico");
					break;
				case TreeIconConstants.Tables:
					retval = GetIcon("Tables.ico");
					break;
				case TreeIconConstants.FolderClose:
					retval = GetIcon("FolderClose.ico");
					break;
				case TreeIconConstants.FolderOpen:
					retval = GetIcon("FolderOpen.ico");
					break;
				case TreeIconConstants.UI:
					retval = GetIcon("UI.ico");
					break;
				case TreeIconConstants.Up:
					retval = GetIcon("Up.ico");
					break;
				case TreeIconConstants.Down:
					retval = GetIcon("Down.ico");
					break;
				case TreeIconConstants.Package:
					retval = GetIcon("Package.ico");
					break;
				case TreeIconConstants.Packages:
					retval = GetIcon("Packages.ico");
					break;
				case TreeIconConstants.CustomView:
					retval = GetIcon("CustomView.ico");
					break;
				case TreeIconConstants.CustomViews:
					retval = GetIcon("CustomViews.ico");
					break;
				case TreeIconConstants.CustomViewColumn:
					retval = GetIcon("CustomViewColumn.ico");
					break;
				case TreeIconConstants.CustomRetrieveRule:
					retval = GetIcon("CustomRetrieveRule.ico");
					break;
				case TreeIconConstants.CustomRetrieveRules:
					retval = GetIcon("CustomRetrieveRules.ico");
					break;
				case TreeIconConstants.TableNonGen:
					retval = GetIcon("TableNonGen.ico");
					break;
				default:
					throw new Exception("Image not found!");
			}

			return retval;
		}

		internal static int GetImageIndex(TreeIconConstants image)
		{
			return (int)ImageMapper[image];
		}

		internal static ImageList GetImageList()
		{
			if (ImageList == null)
			{
				ImageList = new ImageList();
				ImageList.Images.Clear();
				var a = Enum.GetValues(typeof(TreeIconConstants));
				var index = 0;
				foreach(TreeIconConstants ic in a)
				{
					var icon = GetIcon(ic);
					ImageMapper.Add(ic, index);
					ImageList.Images.Add(icon);
					index++;
				}
			}
			return ImageList;
		}

		internal static Icon GetIcon(string name)
		{
			return new Icon(GetProjectFileAsStream(name));
		}

		internal static Bitmap GetImage(string name)
		{
			return new Bitmap(GetProjectFileAsStream(name));
		}

		private static System.IO.Stream GetProjectFileAsStream(string fileName)
		{
			try
			{
				var asbly = System.Reflection.Assembly.GetExecutingAssembly();
				var stream = asbly.GetManifestResourceStream(asbly.GetName().Name + ".Images." + fileName);
				var sr = new System.IO.StreamReader(stream, System.Text.Encoding.UTF8);
				return sr.BaseStream;
			}
			catch(Exception ex)
			{
				throw;
			}
		}

	}
}

