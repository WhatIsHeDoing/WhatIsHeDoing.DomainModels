namespace WhatIsHeDoing.DomainModels
{
    using System;

    public class DomainValueException : Exception
    {
        public DomainValueException()
        {
        }

        public DomainValueException(string message)
            : base(message)
        {
        }
    }
}
