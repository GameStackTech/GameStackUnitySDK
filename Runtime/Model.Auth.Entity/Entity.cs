using System;
using System.Text.RegularExpressions;
using UnityEngine;

namespace GameStack.Model.Auth.Entity
{
    /// <summary>
    /// GameStackCustomClaims defines GameStack specific claims.
    /// </summary>
    [Serializable]
    public class GameStackCustomClaims
    {
        /// <summary>
        /// identity is the claim holders human readable name.
        /// </summary>
        public string identity;

        public override string ToString()
        {
            return typeof(GameStackCustomClaims).FullName + ":: identity: " +
                identity;
        }
    }

    [Serializable]
    public class GameStackJWTPayload
    {
        /// <summary>
        /// aud is the audience (Client ID) that this token applies to.
        /// </summary>
        public string aud;
        /// <summary>
        /// exp is the tokens expriy time.
        /// </summary>
        public long exp;
        /// <summary>
        /// sub is the subjects ID.
        /// </summary>
        public string sub;
        /// <summary>
        /// customClaims contains custom claims specific to GameStack.
        /// </summary>
        public GameStackCustomClaims customClaims;

        /// <summary>
        /// FromJSON produces a new GameStackJWTPayload from a JSON string
        /// representation.
        /// </summary>
        /// <param name="json">
        /// The JSON representation of a GameStackJWTPayload
        /// </param>
        /// <remarks>
        /// Example inputs:
        /// {"aud":"32376043d305cec2a42e67a8e63f237c","exp":1634009397,"sub":"a6ae03c5-7bbc-4416-9c93-eefacc4e48a9","https:\\gamestack.io\jwt\claims":{"identity":"someorg@email.com"}}
        /// </remarks>
        public static GameStackJWTPayload FromJSON(string json)
        {
            if (string.IsNullOrEmpty(json))
            {
                return null;
            }

            // First we deserialize the fields like aud, exp, and sub that will
            // work with the Unity Json utility.
            var output = JsonUtility.FromJson<GameStackJWTPayload>(json);
            // Now we need to strip out the GameStack custom claims since the
            // Unity JSON utility cannot handle generic maps.
            var customClaimsJSON = Regex.Replace(json,
                ",?\\\"(?:aud|exp|sub)\\\":(\"([^\\\"]*)\\\"|[0-9]+),?",
                "");
            customClaimsJSON = customClaimsJSON.Substring(1,
                customClaimsJSON.Length - 2);
            customClaimsJSON = Regex.Replace(customClaimsJSON,
                "\\\"https:\\/\\/gamestack\\.io\\/jwt\\/claims\\\":", "");
            output.customClaims = JsonUtility.FromJson<GameStackCustomClaims>(
                customClaimsJSON);

            return output;
        }

        public override string ToString()
        {
            return typeof(GameStackJWTPayload).FullName + ":: aud: " + aud +
                " exp: " + exp + " sub: " + sub + " customClaims: " +
                customClaims;
        }
    }

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
        /// session_max_age is the maximum age in seconds allowed for the
        /// session.
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
