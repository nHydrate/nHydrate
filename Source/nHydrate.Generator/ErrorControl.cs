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
using System.Drawing;
using System.Windows.Forms;
using nHydrate.Generator.Common.GeneratorFramework;

namespace nHydrate.Generator.Common.Forms
{
	public partial class ErrorControl : ListView
	{
		public ErrorControl()
		{
			InitializeComponent();

			var typeList = new System.Type[] { typeof(MessageTypeConstants), typeof(int), typeof(string), typeof(string) };
			this.ListViewItemSorter = new ListViewComparer(this, typeList);

			this.Columns.Clear();
			this.Columns.Add("", 20, HorizontalAlignment.Left);
			this.Columns.Add("#", 24, HorizontalAlignment.Right);
			this.Columns.Add("Description", 300, HorizontalAlignment.Left);
			this.Columns.Add("Object Type", 300, HorizontalAlignment.Left);

			this.Sorting = SortOrder.Ascending;
			this.SmallImageList = this._imageList;
			this.View = View.Details;
			this.HideSelection = false;
			this.FullRowSelect = true;

			_imageList.Images.Clear();
			_imageList.Images.Add(GetIcon("warning.png"));
			_imageList.Images.Add(GetIcon("Error.ico"));

		}

		#region Class Members

		protected nHydrate.Generator.Common.GeneratorFramework.Message _message = null;
		private readonly ImageList _imageList = new ImageList();

		#endregion

		#region Property Implementations

		public nHydrate.Generator.Common.GeneratorFramework.Message SelectedMessage
		{
			get { return _message; }
			set { _message = value; }
		}

		#endregion

		#region Methods

		public void ClearMessages()
		{
			this.Items.Clear();
		}

		public void AddMessages(MessageCollection messageCollection)
		{
			foreach (var message in messageCollection)
			{
				this.AddMessage(message);
			}
		}

		public void AddMessage(nHydrate.Generator.Common.GeneratorFramework.Message message)
		{
			var newItem = new ListViewItem();
			newItem.Tag = message;

			//Image
			if (message.MessageType == MessageTypeConstants.Warning)
				newItem.ImageIndex = 0;
			else if (message.MessageType == MessageTypeConstants.Error)
				newItem.ImageIndex = 1;

			//Error Number
			newItem.SubItems.Add((this.Items.Count + 1).ToString());

			//Message
			newItem.SubItems.Add(message.Text);

			//Type Column
			if(message.Controller != null)
			{
				var typeName = message.Controller.Object.GetType().ToString();
				var typeNameArr = typeName.Split('.');
				typeName = typeNameArr[typeNameArr.Length - 1];
				newItem.SubItems.Add(typeName);
			}
			else
			{
				newItem.SubItems.Add(string.Empty);
			}

			//Add to the list
			this.Items.Add(newItem);
		}

		private System.Drawing.Image GetIcon(string iconName)
		{
			return new Bitmap(GetProjectFileAsStream(iconName));
		}

		private System.IO.Stream GetProjectFileAsStream(string iconName)
		{
			try
			{
				var asbly = System.Reflection.Assembly.GetExecutingAssembly();
				var name = asbly.GetName().Name;
				name = name.Replace("2008", string.Empty);
				var stream = asbly.GetManifestResourceStream(name + ".Images." + iconName);
				var sr = new System.IO.StreamReader(stream, System.Text.Encoding.UTF8);
				return sr.BaseStream;
			}
			catch (Exception ex)
			{
				throw;
			}
		}

		#endregion

	}
}
