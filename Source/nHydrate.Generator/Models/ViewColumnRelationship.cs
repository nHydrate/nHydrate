#pragma warning disable 0168
using System;
using System.Xml;
using nHydrate.Generator.Common.GeneratorFramework;
using nHydrate.Generator.Common.Util;

namespace nHydrate.Generator.Models
{
	public class ViewColumnRelationship : BaseModelObject
	{
		public ViewColumnRelationship(INHydrateModelObject root)
			: base(root)
		{
		}

		public Reference ParentColumnRef { get; set; }

        public Reference ChildColumnRef { get; set; }

        public Column ParentColumn
		{
			get
            {
                return ParentColumnRef?.Object as Column;
            }
		}

		public CustomViewColumn ChildColumn
		{
			get
            {
                return ChildColumnRef?.Object as CustomViewColumn;
            }
		}

		public override void XmlAppend(XmlNode node)
		{
			try
			{
				var oDoc = node.OwnerDocument;

				var parentColumnRefNode = oDoc.CreateElement("pt");
				this.ParentColumnRef.XmlAppend(parentColumnRefNode);
				node.AppendChild(parentColumnRefNode);

				var childColumnRefNode = oDoc.CreateElement("ct");
				this.ChildColumnRef.XmlAppend(childColumnRefNode);
				node.AppendChild(childColumnRefNode);
			}
			catch (Exception ex)
			{
				throw;
			}
		}

		public override void XmlLoad(XmlNode node)
		{
			try
			{
                this.Key = XmlHelper.GetAttributeValue(node, "key", string.Empty);
				var parentColumnRefNode = node.SelectSingleNode("parentColumnRef"); //deprecated, use "pt"
				if (parentColumnRefNode == null) parentColumnRefNode = node.SelectSingleNode("pt");
				this.ParentColumnRef = new Reference(this.Root);
				this.ParentColumnRef.XmlLoad(parentColumnRefNode);

				var childColumnRefNode = node.SelectSingleNode("childColumnRef"); //deprecated, use "ct"
				if (childColumnRefNode == null) childColumnRefNode = node.SelectSingleNode("ct");
				this.ChildColumnRef = new Reference(this.Root);
				this.ChildColumnRef.XmlLoad(childColumnRefNode);
			}
			catch (Exception ex)
			{
				throw;
			}
		}

	}
}
