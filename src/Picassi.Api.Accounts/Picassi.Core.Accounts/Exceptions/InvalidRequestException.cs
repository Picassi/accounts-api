using System;

namespace Picassi.Core.Accounts.Exceptions
{
    public class InvalidRequestException : PicassiException
    {
        public InvalidRequestException(string message) : base(message)
        {
            
        }
    }
}
