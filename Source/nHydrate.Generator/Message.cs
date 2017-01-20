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
