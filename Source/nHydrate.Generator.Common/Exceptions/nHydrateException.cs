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
        public nHydrateException(string message)
            : base(message)
        {
        }

        /// <summary />
        public nHydrateException(string message, System.Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary />
        public nHydrateException(string errorCode, string message)
            : base(message)
        {
            this.ErrorCode = errorCode;
        }

        /// <summary />
        public nHydrateException(string errorCode, params object[] arguments)
        {
            this.ErrorCode = errorCode;
            //this.arguments = arguments;

            this.Arguments = new string[arguments.Length];

            for (var length = 0; length < arguments.Length; ++length)
            {
                this.Arguments[length] = (string)arguments[length];
            }

        }

        /// <summary />
        public nHydrateException(string errorCode, string message, System.Exception innerException)
            : base(message, innerException)
        {
            this.ErrorCode = errorCode;
        }

        /// <summary />
        public nHydrateException(SerializationInfo serializationInfo, StreamingContext context)
            : base(serializationInfo, context)
        {
            this.ErrorCode = (string)serializationInfo.GetValue("errorCode", typeof(string));
        }

    }
}
