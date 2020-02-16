using System.Collections;
using System.Collections.Generic;
using nHydrate.Generator.Common.EventArgs;

namespace nHydrate.Generator
{
    public delegate void MessageDelegate(object sender, MessageEventArgs e);

    public class MessageCollection : CollectionBase, System.Collections.Generic.IEnumerable<nHydrate.Generator.Message>
    {
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
            AfterMessageRemove?.Invoke(sender, e);
        }

        #endregion

        #region IList Members

        public Message this[int index]
        {
            get { return (Message) this.InnerList[index]; }
            set { this.InnerList[index] = value; }
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

        public Message Add(string text, INHydrateModelObjectController controller)
        {
            var newItem = new Message(text, controller);
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
