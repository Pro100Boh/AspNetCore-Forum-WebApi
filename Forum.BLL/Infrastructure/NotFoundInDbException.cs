using System;
using System.Collections.Generic;
using System.Text;

namespace Forum.BLL.Infrastructure
{
    [Serializable]
    public class NotFoundInDbException : Exception
    {
        public NotFoundInDbException() { }
        public NotFoundInDbException(string message) : base(message) { }
        public NotFoundInDbException(string message, Exception inner) : base(message, inner) { }
        protected NotFoundInDbException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
