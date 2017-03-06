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
using System.Collections;
using System.Collections.Generic;
using nHydrate.Generator.Common.EventArgs;

namespace nHydrate.Generator.Common.GeneratorFramework
{
	public delegate void MessageDelegate(object sender, MessageEventArgs e);
	public delegate void VerifyDelegate(object sender, MessageCollectionEventArgs e);

	public class MessageCollection : CollectionBase, System.Collections.Generic.IEnumerable<nHydrate.Generator.Common.GeneratorFramework.Message>
	{
		#region Member Variables

		#endregion

		#region Constructor

		#endregion

		#region Events

		public event MessageDelegate AfterMessageAdd;
		public event MessageDelegate AfterMessageRemove;

		protected void OnAfterMessageAdd(object sender, MessageEventArgs e)
		{
			if (this.AfterMessageAdd != null)
				this.AfterMessageAdd(sender, e);
		}

		protected void OnAfterMessageRemove(object sender, MessageEventArgs e)
		{
			if (this.AfterMessageRemove != null)
				this.AfterMessageRemove(sender, e);
		}

		#endregion

		#region Property Implementations

		#endregion

		#region IList Members

		public Message this[int index]
		{
			get
			{
				return (Message)this.InnerList[index];
			}
			set
			{
				this.InnerList[index] = value;
			}
		}

		public new void RemoveAt(int index)
		{
			var element = this[index];
			this.InnerList.RemoveAt(index);
			this.OnAfterMessageRemove(this, new nHydrate.Generator.Common.EventArgs.MessageEventArgs(element));
		}

		public void Insert(int index, Message value)
		{
			this.InnerList.Insert(index, value);
		}

		public void Remove(Message value)
		{
			this.InnerList.Remove(value);
			this.OnAfterMessageRemove(this, new nHydrate.Generator.Common.EventArgs.MessageEventArgs(value));
		}

		public bool Contains(Message value)
		{
			return this.InnerList.Contains(value);
		}

		public int IndexOf(Message value)
		{
			return this.InnerList.IndexOf(value);
		}

		internal int Add(Message value)
		{
			var retval = this.InnerList.Add(value);
			this.OnAfterMessageAdd(this, new nHydrate.Generator.Common.EventArgs.MessageEventArgs(value));
			return retval;
		}

		public Message Add(MessageTypeConstants messageType, string text, INHydrateModelObjectController controller)
		{
			var newItem = new Message(messageType, text, controller);
			this.Add(newItem);
			return newItem;
		}

		public void AddRange(ICollection list)
		{
			this.InnerList.AddRange(list);

			foreach (Message message in list)
				this.OnAfterMessageAdd(this, new nHydrate.Generator.Common.EventArgs.MessageEventArgs(message));
		}

		#endregion

		#region IEnumerable<Message> Members

		public new System.Collections.Generic.IEnumerator<Message> GetEnumerator()
		{
			var retval = new List<Message>();
			foreach (Message item in this.InnerList)
			{
				retval.Add(item);
			}
			return retval.GetEnumerator();
		}

		#endregion
	}
}
