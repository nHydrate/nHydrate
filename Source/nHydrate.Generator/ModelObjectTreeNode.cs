#pragma warning disable 0168
using System;
using System.Collections;
using System.Windows.Forms;

namespace nHydrate.Generator
{
	public abstract class ModelObjectTreeNode : TreeNode
	{

		public abstract void Refresh();

        public INHydrateModelObjectController Controller { get; } = null;

        #region Constructor

		public ModelObjectTreeNode(INHydrateModelObjectController controller)
		{      
			Controller = controller;
			Object = Controller.Object;
			this.Refresh();
		}

		#endregion

		#region Object

        public virtual INHydrateModelObject Object { get; } = null;

        #endregion

		#region RefreshDeep

		public void RefreshDeep()
		{
			this.RefreshDeep(this);
		}

		private void RefreshDeep(ModelObjectTreeNode node)
		{
			try
			{
				if (node == null)
					return;

				node.Refresh();
				foreach (ModelObjectTreeNode child in node.Nodes)
				{
					this.RefreshDeep(child);
					if (DateTime.Now.Millisecond % 6 == 0)
						System.Windows.Forms.Application.DoEvents();
				}
			}
			catch (Exception ex)
			{
				throw;
			}
		}

		#endregion

		#region Sort

		public void Sort()
		{
			try
			{
				//Sort Nodes
				TreeNode selNode = null;
				if (this.TreeView != null) 
					selNode = this.TreeView.SelectedNode;

				var sortedList = new SortedList();
				foreach (TreeNode node in this.Nodes)
				{
					//Ensure key is unique to avoid error
					var text = node.Text.ToLower();
					var key = text;
					var ii = 0;
					while (sortedList.ContainsKey(key))
					{
						key = text + ii.ToString();
						ii++;
					}
					sortedList.Add(key, node);
				}

				//Cache a sorted node list
				var nodeList = new TreeNode[this.Nodes.Count];
				var index = 0;
				foreach (DictionaryEntry di in sortedList)
				{
					nodeList[index] = (TreeNode)di.Value;
					index++;
				}

				//Loop through the current nodes and determine if the sorted list matches
				var needUpdate = false;        
				for(var ii=0;ii<nodeList.Length;ii++)
				{
					if (!nHydrate.Generator.Common.Util.StringHelper.Match(this.Nodes[ii].Text, nodeList[ii].Text, true))
						needUpdate = true;
				}

				//If the nodes list was in the same order as the 
				//new sortedlist then there is no need to reorder
				if (needUpdate)
				{
					//Clear nodes
					this.Nodes.Clear();

					//Re-add them in order
					if (this.TreeView != null)
						this.TreeView.BeginUpdate();

					foreach (DictionaryEntry di in sortedList)
						this.Nodes.Add((TreeNode)di.Value);

					if (this.TreeView != null)
						this.TreeView.EndUpdate();

					//Reselect previously selected node
					if (this.TreeView != null)
						this.TreeView.SelectedNode = selNode;
				}

			}
			catch (Exception ex)
			{        
				throw;
			}
		}

		#endregion

	}
}

