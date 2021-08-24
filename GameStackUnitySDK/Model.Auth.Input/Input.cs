using System;
using GameStackUnitySDK.Model.Auth.Entity;

namespace GameStackUnitySDK.Model.Auth.Input
{
    [Serializable]
    public class LoginPlayerInput
    {
        /// <summary>
        /// username is the username that will be used for authentication.
        /// </summary>
        public string username;
        /// <summary>
        /// password is the password that will be used for authentication.
        /// </summary>
        public string password;

        public LoginPlayerInput()
        {
            username = null;
            password = null;
        }

        public LoginPlayerInput(string username, string password)
        {
            this.username = username;
            this.password = password;
        }
    }

    [Serializable]
    public class LogoutInput
    {
        /// <summary>
        /// access_token is the active access token to invalidate.
        /// </summary>
        public string access_token;
        /// <summary>
        /// refresh_token is the active refresh token to invalidate.
        /// </summary>
        public string refresh_token;

        public LogoutInput()
        {
            access_token = null;
            refresh_token = null;
        }

        public LogoutInput(string accessToken, string refreshToken)
        {
            access_token = accessToken;
            refresh_token = refreshToken;
        }
    }

    [Serializable]
    public class CreateGameStackUserInput
    {
        /// <summary>
        /// username is the players human readable alias.
        /// </summary>
        public string username;
        /// <summary>
        /// email is the entities contact email address.
        /// </summary>
        public string email;
        /// <summary>
        /// password is the password for the users account.
        /// </summary>
        public string password;
        // <summary>
        /// name is the players real world name.
        /// </summary>
        public string name;

        public CreateGameStackUserInput()
        {
            username = null;
            email = null;
            password = null;
            name = null;
        }

        public CreateGameStackUserInput(string username, string email,
            string password, string name)
        {
            this.username = username;
            this.email = email;
            this.password = password;
            this.name = name;
        }
    }

    [Serializable]
    public class RefreshTokenInput
    {
        /// <summary>
        /// refresh_token is the refresh token to use to get a new access token.
        /// </summary>
        public string refresh_token;
        /// <summary>
        /// session is a users current session.
        /// </summary>
        public Session session;

        public RefreshTokenInput()
        {
            refresh_token = null;
            session = null;
        }

        public RefreshTokenInput(string refreshToken, Session session)
        {
            refresh_token = refreshToken;
            this.session = session;
        }
    }
}
