using System;

namespace GameStackUnitySDK
{
    public class RequiresAuthenticationException : Exception
    {
        public RequiresAuthenticationException()
        {
        }

        public RequiresAuthenticationException(string message)
            : base(message)
        {
        }

        public RequiresAuthenticationException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }

    public class UnauthorizedException : Exception
    {
        public UnauthorizedException()
        {
        }

        public UnauthorizedException(string message)
            : base(message)
        {
        }

        public UnauthorizedException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }

    public class UnknownHTTPVerbException : Exception
    {
        public UnknownHTTPVerbException()
        {
        }

        public UnknownHTTPVerbException(string message)
            : base(message)
        {
        }

        public UnknownHTTPVerbException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
