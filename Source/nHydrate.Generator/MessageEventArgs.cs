namespace nHydrate.Generator.Common.EventArgs
{
	public class MessageEventArgs
	{
		#region Class Members

        #endregion

		#region Contructors

		public MessageEventArgs(Message message)
		{
			Message = message;
		}

		#endregion

		#region Property Implementations

		public Message Message { get; } = null;

        #endregion
	}
}
