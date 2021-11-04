using System;

namespace GameStack.Model.Errors
{
    /// <summary>
    /// Error contains information about a client error.
    /// </summary>
    [Serializable]
    public class Error
    {
        /// <summary>
        /// Code is the errors status code.
        /// </summary>
        public long Code;
        /// <summary>
        /// Message provides details about the error.
        /// </summary>
        public string Message;

        public Error()
        {
            Code = 0L;
            Message = null;
        }

        public Error(long code, string message)
        {
            Code = code;
            Message = message;
        }

        public override string ToString()
        {
            return typeof(Error).FullName + ":: Code: " +
                Code + " Message: " + Message;
        }
    }

    // ConflictError represents an error where a specified resource
    // is in conflict with an existing resource.
    [Serializable]
    public class ConflictError : Error
    {
        public ConflictError() : base(409L, string.Empty)
        {
        }

        public ConflictError(string message) : base(409L, message)
        {
        }

        public override string ToString()
        {
            return typeof(ConflictError).FullName + ":: Code: " +
                Code + " Message: " + Message;
        }
    }

    // NotFoundError represents an error where a specified resource
    // cannot be found.
    [Serializable]
    public class NotFoundError : Error
    {
        public NotFoundError() : base(403L, string.Empty)
        {
        }

        public NotFoundError(string message) : base(403L, message)
        {
        }

        public override string ToString()
        {
            return typeof(NotFoundError).FullName + ":: Code: " +
                Code + " Message: " + Message;
        }
    }
}
