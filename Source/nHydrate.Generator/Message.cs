namespace nHydrate.Generator.Common.GeneratorFramework
{
	public enum MessageTypeConstants
	{
		Warning,
		Error
	}

	public class Message
	{
		#region Class Members

		protected MessageTypeConstants _messageType = MessageTypeConstants.Error;
		protected string _text = string.Empty;
		protected INHydrateModelObjectController _controller = null;

		#endregion

		#region Constructor

		public Message(MessageTypeConstants messageType, string text, INHydrateModelObjectController controller)
		{
			_messageType = messageType;
			_text = text;
			_controller = controller;
		}

		#endregion

		#region Property Implementations

		public MessageTypeConstants MessageType
		{
			get { return _messageType; }
		}

		public string Text
		{
			get { return _text; }
		}

		public INHydrateModelObjectController Controller
		{
			get { return _controller; }
		}

		#endregion

	}
}
