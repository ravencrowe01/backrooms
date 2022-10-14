using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Exceptions {
    public class IDConflictException : Exception{
        public IDConflictException (string message) : base (message) { }
        public IDConflictException (string message, Exception innerException) : base (message, innerException) { }
        public IDConflictException (SerializationInfo info, StreamingContext context) : base (info, context) { }
    }
}
