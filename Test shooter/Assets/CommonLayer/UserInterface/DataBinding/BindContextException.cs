using System;

namespace CommonLayer.UserInterface.DataBinding
{
    public class BindContextException : Exception
    {
        public BindContextException() { }

        public BindContextException(string msg) : base(msg) { }
    }
}