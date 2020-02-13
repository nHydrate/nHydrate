using nHydrate.Generator.Common.GeneratorFramework;

namespace nHydrate.Generator.Common.EventArgs
{
	public class MessageCollectionEventArgs : System.EventArgs
	{
		protected MessageCollection _messageCollection = null;

		public MessageCollectionEventArgs(MessageCollection messageCollection)
		{
			_messageCollection = messageCollection;
		}

		public MessageCollection MessageCollection
		{
			get { return _messageCollection; }
		}

	}
}
