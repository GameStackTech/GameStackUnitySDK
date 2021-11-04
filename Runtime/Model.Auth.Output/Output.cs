using System;
using GameStack.Model.Auth.Entity;

namespace GameStack.Model.Auth.Output
{
    [Serializable]
    public class LoginPlayerOutput
    {
        /// <summary>
        /// session is the users authenticated session.
        /// </summary>
        public Session session;
        /// <summary>
        /// token is the users auth token.
        /// </summary>
        public Token token;

        public LoginPlayerOutput()
        {
            session = null;
            token = null;
        }

        public LoginPlayerOutput(Session session, Token token)
        {
            this.session = session;
            this.token = token;
        }

        public override string ToString()
        {
            return typeof(LoginPlayerOutput).FullName + ":: session: " +
                session + " token: " + token;
        }
    }

    [Serializable]
    public class RefreshTokenOutput
    {
        /// <summary>
        /// token is a new authorization token.
        /// </summary>
        public Token token;

        public RefreshTokenOutput()
        {
            token = null;
        }

        public RefreshTokenOutput(Token token)
        {
            this.token = token;
        }

        public override string ToString()
        {
            return typeof(RefreshTokenOutput).FullName + ":: token: " +
                token.ToString();
        }
    }
}
