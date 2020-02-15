namespace nHydrate.Generator.Common.EventArgs
{
	public class MessageCollectionEventArgs : System.EventArgs
	{
        public MessageCollectionEventArgs(MessageCollection messageCollection)
		{
			MessageCollection = messageCollection;
		}

		public MessageCollection MessageCollection { get; } = null;
    }
}
