namespace nHydrate.Generator.Common.EventArgs
{
	public class MessageEventArgs
	{
		#region Class Members

		protected Message _message = null;

		#endregion

		#region Contructors

		public MessageEventArgs(Message message)
		{
			_message = message;
		}

		#endregion

		#region Property Implementations

		public Message Message
		{
			get { return _message; }
		}

		#endregion
	}
}
