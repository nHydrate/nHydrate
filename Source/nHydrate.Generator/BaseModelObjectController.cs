#region Copyright (c) 2006-2014 nHydrate.org, All Rights Reserved
// -------------------------------------------------------------------------- *
//                           NHYDRATE.ORG                                     *
//              Copyright (c) 2006-2014 All Rights reserved                   *
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
using System.Windows.Forms;

namespace nHydrate.Generator.Common.GeneratorFramework
{
	public abstract class BaseModelObjectController : INHydrateModelObjectController
	{
		protected ModelObjectUserInterface _userInterface = null;
		protected INHydrateModelObject _object = null;
		protected ModelObjectTreeNode _node = null;
		protected bool _isEnabled = true;

		#region Constructor

		public BaseModelObjectController(INHydrateModelObject modelObject)
		{
			if (modelObject == null)
				throw new Exception("The model object cannot be null!");

			_object = modelObject;
			if (_object != null)
				((BaseModelObject)this.Object).PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(ObjectPropertyChanged);

			this.IsPopupUI = false;
			modelObject.Controller = this;
			this.HeaderImage = ImageHelper.GetImage(ImageConstants.Default);
		}

		#endregion

		#region Events

		public event ItemChanagedEventHandler ItemChanged;
		public void OnItemChanged(object sender, System.EventArgs e)
		{
			if (this.ItemChanged != null)
			{
				this.ItemChanged(sender, e);
			}
		}

		public event PropertyValueChangedEventHandler PropertyValueChanged;
		protected virtual void OnPropertyValueChanged(PropertyValueChangedEventArgs e)
		{
			if (this.PropertyValueChanged != null)
			{
				this.PropertyValueChanged(this, e);
			}
		}

		#endregion

		#region Property Implementations

		protected ModelObjectUserInterface UserInterface
		{
			get { return _userInterface; }
			set { _userInterface = value; }
		}

		public virtual bool IsEnabled
		{
			get { return _isEnabled; }
		}

		/// <summary>
		/// Determines if this UI should be on a dialog box
		/// </summary>
		public virtual bool IsPopupUI { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public virtual string HeaderText { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public virtual string HeaderDescription { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public virtual System.Drawing.Bitmap HeaderImage { get; set; }

		#endregion

		#region IDisposable Members

		public void Dispose()
		{
			UIControl.Dispose();
		}

		#endregion

		#region UIControl

		public virtual ModelObjectUserInterface UIControl
		{
			get
			{
				if (this._userInterface == null)
				{
					var pg = new nHydrate.Generator.Common.GeneratorFramework.PropertyGrid();
					pg.SelectedObject = this.Object;
					pg.Dock = System.Windows.Forms.DockStyle.Fill;
					pg.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(pg_PropertyValueChanged);
					this._userInterface = pg;
				}
				this._userInterface.Enabled = this.IsEnabled;
				return this._userInterface;
			}
		}

		private void pg_PropertyValueChanged(object s, System.Windows.Forms.PropertyValueChangedEventArgs e)
		{
			this.OnPropertyValueChanged(e);
		}

		#endregion

		#region Verify

		public virtual MessageCollection Verify()
		{
			try
			{
				var retval = new MessageCollection();
				foreach (ModelObjectTreeNode node in this.Node.Nodes)
					retval.AddRange(((BaseModelObjectController)node.Controller).Verify());
				return retval;
			}
			catch (Exception ex)
			{
				throw;
			}
		}

		#endregion

		#region Event Handlers

		protected virtual void ObjectPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			this.OnItemChanged(this, new System.EventArgs());
		}

		#endregion

		public virtual INHydrateModelObject Object
		{
			get { return _object; }
			set { _object = value; }
		}

		public abstract ModelObjectTreeNode Node { get; }
		public abstract MenuCommand[] GetMenuCommands();
		public abstract bool DeleteObject();
		public abstract void Refresh();

	}
}
