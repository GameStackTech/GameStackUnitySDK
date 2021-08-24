using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using GameStackUnitySDK.Model.Auth.Entity;
using GameStackUnitySDK.Model.Auth.Input;
using GameStackUnitySDK.Model.Auth.Output;
using GameStackUnitySDK.Model.Errors;
using GameStackUnitySDK.Model.Leaderboard.Entity;
using GameStackUnitySDK.Model.Leaderboard.Input;
using GameStackUnitySDK.Model.Leaderboard.Output;
using GameStackUnitySDK.Singleton;
using UnityEngine;
using UnityEngine.Events;

namespace GameStackUnitySDK
{
    public class GameStackClient : PersistentLazySingleton<GameStackClient>
    {
        private Session _session;
        private Token _token;
        private DateTime tokenExpires;
        private ReaderWriterLockSlim _credentialsLock = new ReaderWriterLockSlim();

        /// <summary>
        /// SendRequest executes an HTTP request in a coroutine.
        /// </summary>
        /// <param name="verb">The HTTP verb to use for the request.</param>
        /// <param name="headers">
        /// The HTTP headers to use for the request.
        /// </param>
        /// <param name="url">The URL the request will be sent to.</param>
        /// <param name="body">The body of the HTTP request.</param>
        /// <param name="callbackOnSuccess">
        /// A callback that will be called if the request was successful.
        /// </param>
        /// <param name="callbackOnFail">
        /// A callback that will be called if the request failed.
        /// </param>
        /// <typeparam name="T">
        /// The element type of the success response object that the response
        /// body will be converted to.
        /// </typeparam>
        private void SendRequest<T>(string verb,
            Dictionary<string, string> headers, string url, string body,
            UnityAction<T> callbackOnSuccess,
            UnityAction<Error> callbackOnFail)
        {
            SendRequestCoroutine(verb, headers, url, body, callbackOnSuccess,
                callbackOnFail);
        }

        /// <summary>
        /// SendRequestCoroutine executes an HTTP request in a coroutine that
        /// is returned to the caller.
        /// </summary>
        /// <param name="verb">The HTTP verb to use for the request.</param>
        /// <param name="headers">
        /// The HTTP headers to use for the request.
        /// </param>
        /// <param name="url">The URL the request will be sent to.</param>
        /// <param name="body">The body of the HTTP request.</param>
        /// <param name="callbackOnSuccess">
        /// A callback that will be called if the request was successful.
        /// </param>
        /// <param name="callbackOnFail">
        /// A callback that will be called if the request failed.
        /// </param>
        /// <typeparam name="T">
        /// The element type of the success response object that the response
        /// body will be converted to.
        /// </typeparam>
        private Coroutine SendRequestCoroutine<T>(string verb,
            Dictionary<string, string> headers, string url, string body,
            UnityAction<T> callbackOnSuccess,
            UnityAction<Error> callbackOnFail)
        {
            return StartCoroutine(GameStackRESTClientUtil.RequestCoroutine(
                headers, url, body, verb, callbackOnSuccess, callbackOnFail));
        }

        /// <summary>
        /// SetCredentials assigns the session and token (if supplied) to
        /// the currently valid session and token credentials using a write lock
        /// to allow async access.
        /// </summary>
        /// <param name="session">
        /// The optional session that will be set as the valid session.
        /// </param>
        /// <param name="token">
        /// The token that will be set as the valid token.
        /// </param>
        private void SetCredentials(Session session, Token token)
        {
            _credentialsLock.EnterWriteLock();
            try
            {
                if (_session != null)
                {
                    _session = session;
                }

                _token = token;
                tokenExpires = DateTime.UtcNow.AddSeconds(_token.expires_in);
            }
            finally
            {
                _credentialsLock.ExitWriteLock();
            }
        }

        /// <summary>
        /// RefreshToken calls the token refresh API and will attempt to refresh
        /// the access token using the current refresh token.
        /// </summary>
        /// <param name="refreshToken">
        /// The refresh token to use for the refresh request.
        /// </param>
        /// <param name="session">The current session.</param>
        private IEnumerator RefreshToken(string refreshToken, Session session)
        {
            UnityAction<RefreshTokenOutput> cbSuccess = (output) => {
                SetCredentials(null, output.token);
            };
            UnityAction<Error> cbFail = (output) => { };

            yield return SendRequestCoroutine(HttpMethod.Post.Method,
                GameStackRESTClientUtil.GenerateHeaders(),
                GameStackClientConfig.GetGameStackAuthURL("players/refresh"),
                JsonUtility.ToJson(new RefreshTokenInput(refreshToken,
                    session)), cbSuccess, cbFail);
        }

        /// <summary>
        /// GetToken will return the authentication token if it is cached and
        /// the token is valid (I.E. before the tokens expiry time). If the
        /// token is not valid we attempt to refresh the token to get a valid
        /// token.
        /// </summary>
        /// <exception cref="RequiresAuthenticationException">
        /// Throws RequiresAuthenticationException if there is no logged in
        /// user or the user session is expired.
        /// </exception>
        private Token GetToken()
        {
            _credentialsLock.EnterReadLock();
            try
            {
                var now = DateTime.UtcNow;

                // User has to log in and authenticate before they get a token.
                if (_session == null || _token == null)
                {
                    throw new RequiresAuthenticationException(
                        "user is not logged in, please call the Login API");
                }
                else // If the session expired we need the user to authenticate.
                {
                    var sessionExpiresIn = DateTime.Parse(
                        _session.session_expires_in);
                    if (now >= sessionExpiresIn) {
                        throw new RequiresAuthenticationException(
                            "user is not logged in, please call the Login API"
                        );
                    }
                }

                // If the token is past its expiration we need to get a new one.
                if (now >= tokenExpires)
                {
                    RefreshToken(_token.refresh_token, _session);
                }

                return _token;
            }
            finally
            {
                _credentialsLock.ExitReadLock();
            }
        }

        /// <summary>
        /// CreateGameStackUser creates a new GameStack user.
        /// </summary>
        /// <param name="username">The alias the user wants.</param>
        /// <param name="email">The users email address.</param>
        /// <param name="password">The users password.</param>
        /// <param name="name">The users real name.</param>
        /// <param name="callbackOnSuccess">
        /// A callback that will be called if the request was successful.
        /// </param>
        /// <param name="callbackOnFail">
        /// A callback that will be called if the request failed.
        /// </param>
        public void CreateGameStackUser(string username, string email,
            string password, string name, UnityAction<string> callbackOnSuccess,
            UnityAction<Error> callbackOnFail)
        {
            var encodedPassword = GameStackRESTClientUtil.EncodePassword(
                password);
            SendRequest(HttpMethod.Post.Method,
                GameStackRESTClientUtil.GenerateHeaders(),
                GameStackClientConfig.GetGameStackAuthURL("players/signup"),
                JsonUtility.ToJson(new CreateGameStackUserInput(username, email,
                    encodedPassword, name)), callbackOnSuccess, callbackOnFail);
        }

        /// <summary>
        /// Login authenticates and authorzies a player with GameStack.
        /// </summary>
        /// <param name="username">The players username.</param>
        /// <param name="password">The players password.</param>
        /// <param name="callbackOnSuccess">
        /// A callback that will be called if the request was successful.
        /// </param>
        /// <param name="callbackOnFail">
        /// A callback that will be called if the request failed.
        /// </param>
        public void LoginPlayer(string username, string password,
            UnityAction<LoginPlayerOutput> callbackOnSuccess,
            UnityAction<Error> callbackOnFail)
        {
            UnityAction<LoginPlayerOutput> cbSuccess = (output) => {
                SetCredentials(output.session, output.token);
                callbackOnSuccess(output);
            };
            var encodedPassword = GameStackRESTClientUtil.EncodePassword(
                password);

            SendRequest(HttpMethod.Post.Method,
                GameStackRESTClientUtil.GenerateHeaders(),
                GameStackClientConfig.GetGameStackAuthURL("players/login"),
                JsonUtility.ToJson(new LoginPlayerInput(username, encodedPassword)),
                cbSuccess, callbackOnFail);
        }

        /// <summary>
        /// LogoutPlayer ends the players current session and invalidates their
        /// access tokens.
        /// </summary>
        /// <param name="callbackOnSuccess">
        /// A callback that will be called if the request was successful.
        /// </param>
        /// <param name="callbackOnFail">
        /// A callback that will be called if the request failed.
        /// </param>
        public void LogoutPlayer(UnityAction<string> callbackOnSuccess,
            UnityAction<Error> callbackOnFail)
        {
            UnityAction<string> cbSuccess = (output) => {
                _credentialsLock.EnterWriteLock();
                _session = null;
                _token = null;
                _credentialsLock.ExitWriteLock();

                callbackOnSuccess(output);
            };

            SendRequest(HttpMethod.Delete.Method,
                GameStackRESTClientUtil.GenerateHeaders(),
                GameStackClientConfig.GetGameStackAuthURL("logout"),
                JsonUtility.ToJson(new LogoutInput(_token.access_token,
                    _token.refresh_token)), cbSuccess, callbackOnFail);
        }

        /// <summary>
        /// CreateApplicationUser creates a new user for an application.
        /// </summary>
        /// <param name="applicationID">
        /// The GameStack application ID to create the user under.
        /// </param>
        /// <param name="alias">
        /// The alias that will be assigned to the user.
        /// </param>
        /// <param name="callbackOnSuccess">
        /// A callback that will be called if the request was successful.
        /// </param>
        /// <param name="callbackOnFail">
        /// A callback that will be called if the request failed.
        /// </param>
        public void CreateApplicationUser(string applicationID, string alias,
            UnityAction<string> callbackOnSuccess,
            UnityAction<Error> callbackOnFail)
        {
            var path = $"app/{applicationID}/user";

            var input = new CreateApplicationUserInput();
            input.alias = alias;

            try
            {
                SendRequest(WebRequestMethods.Http.Put,
                    GameStackRESTClientUtil.GenerateHeaders(GetToken()),
                        GameStackClientConfig.GetGameStackLeaderboardURL(path),
                        JsonUtility.ToJson(input), callbackOnSuccess,
                    callbackOnFail);
            }
            catch (RequiresAuthenticationException ex)
            {
                callbackOnFail(new Error((long)HttpStatusCode.Unauthorized,
                    ex.Message));
            }
        }

        /// <summary>
        /// GetLeaderboardStats retrieves statistics from a specific
        /// leaderboard.
        /// </summary>
        /// <param name="applicationID">
        /// The GameStack application ID the leaderboard to get stats from
        /// belongs to.
        /// </param>
        /// <param name="leaderboardID">
        /// The GameStack leaderboard ID to get stats from.
        /// </param>
        /// <param name="dimensions">
        /// An optional list of dimension names to get stats for. If not
        /// specified all the leaderboards stat dimensions will be returned.
        /// </param>
        /// <param name="filters">
        /// An optional list of filters that control how a result set is
        /// filtered.
        /// </param>
        /// <param name="pagination">
        /// An optional pagination configuration that controls how result sets
        /// are paginated.
        /// </param>
        public void GetLeaderboardStats(string applicationID,
            string leaderboardID, string[] dimensions, Filter[] filters,
            Sort sort, Pagination pagination,
            UnityAction<GetLeaderboardStatsOutput> callbackOnSuccess,
            UnityAction<Error> callbackOnFail)
        {
            var path = $"app/{applicationID}/leaderboard/{leaderboardID}/stats";

            var input = new GetLeaderboardStatsInput();
            input.dimensions = dimensions;
            input.filters = filters;
            input.sort = sort;
            input.pagination = pagination;

            try
            {
                SendRequest(WebRequestMethods.Http.Post,
                    GameStackRESTClientUtil.GenerateHeaders(GetToken()),
                        GameStackClientConfig.GetGameStackLeaderboardURL(path),
                        JsonUtility.ToJson(input), callbackOnSuccess,
                    callbackOnFail);
            }
            catch (RequiresAuthenticationException ex)
            {
                callbackOnFail(new Error((long)HttpStatusCode.Unauthorized,
                    ex.Message));
            }
        }

        /// <summary>
        /// PutLeaderboardStats adds statistics to a specific leaderboard.
        /// </summary>
        /// <param name="applicationID">
        /// The GameStack application ID the leaderboard to add stats to belongs
        /// to.
        /// </param>
        /// <param name="leaderboardID">
        /// The GameStack leaderboard ID to add stats to.
        /// </param>
        /// <param name="instanceID">
        /// The GameStack play instance ID that these stats apply to.
        /// </param>
        /// <param name="dimensions">
        /// An list of stat dimensions that will be added to the leaderboard.
        /// </param>
        /// <param name="callbackOnSuccess">
        /// A callback that will be called if the request was successful.
        /// </param>
        /// <param name="callbackOnFail">
        /// A callback that will be called if the request failed.
        /// </param>
        public void PutLeaderboardStats(string applicationID,
            string leaderboardID, string instanceID, Dimension[] dimensions,
            UnityAction<PutLeaderboardStatsOutput> callbackOnSuccess,
            UnityAction<Error> callbackOnFail)
        {
            var path = $"app/{applicationID}/instance/{instanceID}/leaderboard/{leaderboardID}/stats";

            var input = new PutLeaderboardStatsInput();
            input.dimensions = dimensions;

            try
            {
                SendRequest(WebRequestMethods.Http.Put,
                    GameStackRESTClientUtil.GenerateHeaders(GetToken()),
                        GameStackClientConfig.GetGameStackLeaderboardURL(path),
                        JsonUtility.ToJson(input), callbackOnSuccess,
                    callbackOnFail);
            }
            catch (RequiresAuthenticationException ex)
            {
                callbackOnFail(new Error((long)HttpStatusCode.Unauthorized,
                    ex.Message));
            }
        }

        /// <summary>
        /// CreateInstance creates a play instance associated with a specific
        /// application.
        /// </summary>
        /// <param name="applicationID">
        /// The GameStack application ID this play instance is valid for.
        /// </param>
        /// <param name="callbackOnSuccess">
        /// A callback that will be called if the request was successful.
        /// </param>
        /// <param name="callbackOnFail">
        /// A callback that will be called if the request failed.
        /// </param>
        public void CreateInstance(string applicationID,
            UnityAction<CreateInstanceOutput> callbackOnSuccess,
            UnityAction<Error> callbackOnFail)
        {
            var path = $"app/{applicationID}/instance";

            try
            {
                SendRequest(WebRequestMethods.Http.Put,
                    GameStackRESTClientUtil.GenerateHeaders(GetToken()),
                        GameStackClientConfig.GetGameStackLeaderboardURL(path),
                        string.Empty, callbackOnSuccess, callbackOnFail);
            }
            catch (RequiresAuthenticationException ex)
            {
                callbackOnFail(new Error((long)HttpStatusCode.Unauthorized,
                    ex.Message));
            }
        }

        /// <summary>
        /// IsLoggedIn returns true if there is a currently logged in user and
        /// returns false if no user is logged in.
        /// </summary>
        public bool IsLoggedIn()
        {
            _credentialsLock.EnterReadLock();
            try
            {
                if (_session == null || _token == null)
                {
                    return false;
                }

                var now = DateTime.UtcNow;
                var sessionExpiresIn = DateTime.Parse(
                    _session.session_expires_in);
                return (now < sessionExpiresIn) && (now < tokenExpires);
            }
            finally
            {
                _credentialsLock.ExitReadLock();
            }
        }
    }
}
