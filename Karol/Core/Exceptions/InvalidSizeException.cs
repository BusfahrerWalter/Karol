using System;
using System.Collections.Generic;
using System.Text;

namespace Karol.Core.Exceptions
{
    public class InvalidSizeException : Exception
    {
        public InvalidSizeException(string msg) : base(msg)
        { }
    }
}
