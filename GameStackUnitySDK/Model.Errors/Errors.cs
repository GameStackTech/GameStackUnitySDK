using System;

namespace GameStackUnitySDK.Model.Errors
{
    /// <summary>
    /// Error continas information about a client error.
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
    }
}
