using System;

namespace GameStackUnitySDK.Model.Auth.Entity
{
    /// <summary>
    /// Session contains infomration about an authenticated user session.
    /// </summary>
    [Serializable]
    public class Session
    {
        /// <summary>
        /// session_name is the name of the session.
        /// </summary>
        public string session_name;
        /// <summary>
        /// session_value is sessions verification value.
        /// </summary>
        public string session_value;
        /// <summary>
        /// session_path is the path of the session.
        /// </summary>
        public string session_path;
        /// <summary>
        /// session_expires_in is the time that the session will expire in.
        /// </summary>
        public string session_expires_in;
        /// <summary>
        /// session_max_age is the maximum age in seconds allowed for the session.
        /// </summary>
        public long session_max_age;

        public override string ToString()
        {
            return typeof(Session).FullName + ":: session_name: " +
                session_name + " session_value: " + session_value +
                " session_path: " + session_path + " session_expires_in: " +
                session_expires_in + " session_max_age: " +
                session_max_age.ToString();
        }
    }

    /// <summary>
    /// Token contains an access token and information about the token.
    /// </summary>
    [Serializable]
    public class Token
    {
        /// <summary>
        /// access_token is a token that grants the bearer access to protected
        /// resources.
        /// </summary>
        public string access_token;
        /// <summary>
        /// token_type is the type of token (Example: Bearer).
        /// </summary>
        public string token_type;
        /// <summary>
        /// expires_in is the period in seconds that the token will expire in.
        /// </summary>
        public long expires_in;
        /// <summary>
        /// scope is the scope that this token applies to.
        /// </summary>
        public string scope;
        /// <summary>
        /// refresh_token is a token that allows the bearer to refresh their
        /// access token.
        /// </summary>
        public string refresh_token;

        public override string ToString()
        {
            return typeof(Token).FullName + ":: access_token: " + access_token
                + " token_type: " + token_type + " expires_in: "
                + expires_in.ToString() + " scope: " + scope +
                " refresh_token: " + refresh_token;
        }
    }
}
