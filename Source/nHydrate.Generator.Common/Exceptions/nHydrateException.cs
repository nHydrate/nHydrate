using System;
using System.Runtime.Serialization;

namespace nHydrate.Generator.Common.Exceptions
{
    public class nHydrateException : System.Exception
    {
        /// <summary />
        public string ErrorCode = null;
        public string[] Arguments = null;

        /// <summary />
        public nHydrateException()
            : base()
        {
        }

        /// <summary />
        public nHydrateException(string Message)
            : base(Message)
        {
        }

        /// <summary />
        public nHydrateException(string Message, System.Exception InnerException)
            : base(Message, InnerException)
        {
        }

        /// <summary />
        public nHydrateException(string ErrorCode, string Message)
            : base(Message)
        {
            this.ErrorCode = ErrorCode;
        }

        /// <summary />
        public nHydrateException(string ErrorCode, params object[] Arguments)
        {
            this.ErrorCode = ErrorCode;
            //this.arguments = arguments;

            this.Arguments = new string[Arguments.Length];

            for (var length = 0; length < Arguments.Length; ++length)
            {
                this.Arguments[length] = (string)Arguments[length];
            }

        }

        /// <summary />
        public nHydrateException(string ErrorCode, string Message, System.Exception InnerException)
            : base(Message, InnerException)
        {
            this.ErrorCode = ErrorCode;
        }

        /// <summary />
        public nHydrateException(SerializationInfo SerializationInfo, StreamingContext Context)
            : base(SerializationInfo, Context)
        {
            this.ErrorCode = (string)SerializationInfo.GetValue("errorCode", typeof(string));
        }

    }
}
