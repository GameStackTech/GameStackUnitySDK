using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using GameStack.Model.Auth.Entity;
using GameStack.Model.Auth.Input;
using GameStack.Model.Auth.Output;
using GameStack.Model.Auth.Props;
using GameStack.Model.Errors;
using GameStack.Model.Leaderboard.Entity;
using GameStack.Model.Leaderboard.Input;
using GameStack.Model.Leaderboard.Output;
using GameStack.Model.Leaderboard.Props;
using GameStack.Singleton;
using GameStack.JWT.Algorithms;
using GameStack.JWT.Builder;
using UnityEngine;
using UnityEngine.Events;

namespace GameStack
{
    public class GameStackClient : PersistentLazySingleton<GameStackClient>
    {
        private const long _notFoundCode = 404L;
        private const long _conflictCode = 409L;

        private Session _session;
        private Token _token;
        private DateTime _tokenExpires;
        private User _applicationUser;

        private ReaderWriterLockSlim _credentialsLock = new ReaderWriterLockSlim();
        private ReaderWriterLockSlim _applicationLock = new ReaderWriterLockSlim();

        #region Networking

        /// <summary>
        /// SendRequest executes an HTTP request in a coroutine.
        /// </summary>
        /// <param name="verb">The HTTP verb to use for the request.</param>
        /// <param name="headers">
        /// The HTTP headers to use for the request.
        /// </param>
        /// <param name="url">The URL the request will be sent to.</param>
        /// <param name="body">The body of the HTTP request.</param>
        /// <param name="jsonUnmarshaler">
        /// Optional unmarshaler that will be used if supplied, otherwise the
        /// Unity Json Utility will be used to unmarshal the response.
        /// </param>
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
            Func<string, T> jsonUnmarshaler, UnityAction<T> callbackOnSuccess,
            UnityAction<Error> callbackOnFail)
        {
            SendRequestCoroutine(verb, headers, url, body, jsonUnmarshaler,
                callbackOnSuccess, callbackOnFail);
        }

        /// <summary>
        /// SendRequest is an overriden variant that does not require a custom
        /// JSON unmarshaler.
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
            UnityAction<T> callbackOnSuccess, UnityAction<Error> callbackOnFail)
        {
            SendRequest(verb, headers, url, body, null, callbackOnSuccess,
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
        /// <returns cref="Coroutine">A coroutine that contains the request.</returns>
        private Coroutine SendRequestCoroutine<T>(string verb,
            Dictionary<string, string> headers, string url, string body,
            Func<string, T> jsonUnmarshaler, UnityAction<T> callbackOnSuccess,
            UnityAction<Error> callbackOnFail)
        {
            return StartCoroutine(GameStackRESTClientUtil.RequestCoroutine(
                headers, url, body, verb, jsonUnmarshaler, callbackOnSuccess,
                callbackOnFail));
        }

        private Coroutine SendRequestCoroutine<T>(string verb,
            Dictionary<string, string> headers, string url, string body,
            UnityAction<T> callbackOnSuccess, UnityAction<Error> callbackOnFail)
        {
            return SendRequestCoroutine(verb, headers, url, body, null,
                callbackOnSuccess, callbackOnFail);
        }

        /// <summary>
        /// SendRequestCoroutine is an override that does not require a body string.
        /// </summary>
        /// <param name="verb">The HTTP verb to use for the request.</param>
        /// <param name="headers">
        /// The HTTP headers to use for the request.
        /// </param>
        /// <param name="url">The URL the request will be sent to.</param>
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
        /// <returns cref="Coroutine">A coroutine that contains the request.</returns>
        private Coroutine SendRequestCoroutine<T>(string verb,
            Dictionary<string, string> headers, string url,
            UnityAction<T> callbackOnSuccess, UnityAction<Error> callbackOnFail)
        {
            return StartCoroutine(GameStackRESTClientUtil.RequestCoroutine(
                headers, url, null, verb, callbackOnSuccess, callbackOnFail));
        }

        #endregion

        #region AuthAPIs

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
                if (session != null)
                {
                    _session = session;
                }

                _token = token;
                _tokenExpires = DateTime.UtcNow.AddSeconds(_token.expires_in);
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
        /// <param name="session">
        /// The players session that was obtained on login.
        /// </param>
        /// <param name="session">The current session.</param>
        private IEnumerator RefreshToken(string refreshToken, Session session)
        {
            UnityAction<RefreshTokenOutput> cbSuccess = (output) =>
            {
                SetCredentials(null, output.token);
            };
            UnityAction<Error> cbFail = (err) => { };

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
        /// <returns cref="Token">The current access token.</returns>
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
                    if (now >= sessionExpiresIn)
                    {
                        throw new RequiresAuthenticationException(
                            "user is not logged in, please call the Login API"
                        );
                    }
                }

                // If the token is past its expiration we need to get a new one.
                if (now >= _tokenExpires)
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
        /// <param name="props" cref="CreateGameStackUserProps">
        /// The parameters for the request.
        /// </param>
        /// <param name="callbackOnSuccess">
        /// A callback that will be called if the request was successful.
        /// </param>
        /// <param name="callbackOnFail">
        /// A callback that will be called if the request failed.
        /// </param>
        public void CreateGameStackUser(CreateGameStackUserProps props,
            UnityAction<string> callbackOnSuccess,
            UnityAction<Error> callbackOnFail)
        {
            SendRequest(HttpMethod.Post.Method,
                GameStackRESTClientUtil.GenerateHeaders(),
                GameStackClientConfig.GetGameStackAuthURL("players/signup"),
                JsonUtility.ToJson(new CreateGameStackUserInput(props.Username,
                    props.Email, props.Password, props.Name)),
                callbackOnSuccess, callbackOnFail);
        }

        /// <summary>
        /// LoginPlayercalls the GameStack player login API.
        /// </summary>
        /// <param name="username">The players username.</param>
        /// <param name="password">The players password.</param>
        private void LoginPlayer(string username, string password,
            UnityAction<LoginPlayerOutput> callbackOnSuccess,
            UnityAction<Error> callbackOnFail)
        {
            UnityAction<LoginPlayerOutput> cbSuccess = (output) =>
            {
                SetCredentials(output.session, output.token);
                callbackOnSuccess(output);
            };

            SendRequestCoroutine(HttpMethod.Post.Method,
                GameStackRESTClientUtil.GenerateHeaders(),
                GameStackClientConfig.GetGameStackAuthURL("players/login"),
                JsonUtility.ToJson(new LoginPlayerInput(username,
                    password)), cbSuccess, callbackOnFail);
        }

        /// <summary>
        /// Login authenticates and authorzies a player with GameStack and detects
        /// if the player is registered as a user of the application.
        /// </summary>
        /// <param name="props" cref="LoginPlayerProps">
        /// The parameters of the call.
        /// </param>
        /// <param name="callbackOnSuccess">
        /// A callback that will be called if the request was successful.
        /// </param>
        /// <param name="callbackOnFail">
        /// A callback that will be called if the request failed.
        ///
        /// If the Error object in the callbackOnFail is a
        /// NotFoundError that means the user is logged into
        /// GameStack but not registerd for the application.
        /// </param>
        public void LoginPlayer(LoginPlayerProps props,
            UnityAction<LoginPlayerOutput> callbackOnSuccess,
            UnityAction<Error> callbackOnFail)
        {
            UnityAction<LoginPlayerOutput> cbLoginSuccess = (loginOutput) =>
            {
                UnityAction<GetUserForApplicationOutput> cbGetUserForApplicationSuccess = (getUserForApplicationOutput) =>
                {
                    callbackOnSuccess(loginOutput);
                };
                UnityAction<Error> cbGetUserForApplicationFail = (err) =>
                {
                    var applicationUserNotFound = _notFoundCode == err.Code;
                    if (applicationUserNotFound && props.AutoCreateApplicationUser)
                    {
                        UnityAction<string> cbCreateApplicationUserSuccess = (getUserForApplicationOutput) =>
                        {
                            callbackOnSuccess(loginOutput);
                        };

                        CreateApplicationUser(new CreateApplicationUserProps(
                            props.ApplicationID, GetGameStackAlias()),
                            cbCreateApplicationUserSuccess, callbackOnFail);
                    }
                    else
                    {
                        callbackOnFail(applicationUserNotFound ?
                            new NotFoundError(err.Message) : err);
                    }
                };
                GetUserForApplication(new GetUserForApplicationProps
                    (props.ApplicationID), cbGetUserForApplicationSuccess,
                    cbGetUserForApplicationFail);
            };

            LoginPlayer(props.Username, props.Password, cbLoginSuccess,
                callbackOnFail);
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
            UnityAction<string> cbSuccess = (output) =>
            {
                _credentialsLock.EnterWriteLock();
                _session = null;
                _token = null;
                _credentialsLock.ExitWriteLock();

                _applicationLock.EnterWriteLock();
                _applicationUser = null;
                _applicationLock.ExitWriteLock();

                callbackOnSuccess(output);
            };

            SendRequest(HttpMethod.Delete.Method,
                GameStackRESTClientUtil.GenerateHeaders(),
                GameStackClientConfig.GetGameStackAuthURL("logout"),
                JsonUtility.ToJson(new LogoutInput(_token.access_token,
                    _token.refresh_token)), cbSuccess, callbackOnFail);
        }

        /// <summary>
        /// GetGameStackAlias returns the logged in players GameStack account
        /// alias.
        /// </summary>
        /// <returns>The players GameStack alias.</returns>
        /// <exception cref="InvalidCastException">
        /// InvalidCastException is thrown when the JWT token cannot be cast to
        /// a valid data model.
        /// </exception>
        public string GetGameStackAlias()
        {
            var alias = "";
            _credentialsLock.EnterReadLock();
            try
            {
                if (_token == null)
                {
                    return alias;
                }

                Func<string, GameStackJWTPayload> jsonUnmarshaler = (data) =>
                {
                    return GameStackJWTPayload.FromJSON(data);
                };

                var payload = JwtBuilder.Create()
                    .WithAlgorithm(new HMACSHA512Algorithm())
                    .Decode<GameStackJWTPayload>(_token.access_token,
                        jsonUnmarshaler);

                alias = payload?.customClaims?.identity;
            }
            finally
            {
                _credentialsLock.ExitReadLock();
            }

            return alias;
        }

        #endregion

        #region LeaderboardAPIs

        /// <summary>
        /// CreateApplicationUser creates a new user for an application.
        /// </summary>
        /// <param name="props" cref="CreateApplicationUserProps">
        /// The parameters of the call.
        /// </param>
        /// <param name="callbackOnSuccess">
        /// A callback that will be called if the request was successful.
        /// </param>
        /// <param name="callbackOnFail">
        /// A callback that will be called if the request failed.
        /// </param>
        public void CreateApplicationUser(CreateApplicationUserProps props,
            UnityAction<string> callbackOnSuccess,
            UnityAction<Error> callbackOnFail)
        {
            var path = $"app/{props.ApplicationID}/user";

            var input = new CreateApplicationUserInput();
            input.alias = props.Alias;

            UnityAction<CreateApplicationUserOutput> cbSuccess = (output) =>
            {
                callbackOnSuccess(string.Empty);
            };
            UnityAction<Error> cbFail = (err) =>
            {
                callbackOnFail(_conflictCode == err.Code ?
                    new ConflictError(err.Message) : err);
            };

            try
            {
                SendRequest(WebRequestMethods.Http.Put,
                    GameStackRESTClientUtil.GenerateHeaders(GetToken()),
                    GameStackClientConfig.GetGameStackLeaderboardURL(path),
                    JsonUtility.ToJson(input), cbSuccess, cbFail);
            }
            catch (RequiresAuthenticationException ex)
            {
                callbackOnFail(new Error((long)HttpStatusCode.Unauthorized,
                    ex.Message));
            }
        }

        /// <summary>
        /// GetUserForApplication gets a GameStack players user in the context
        /// of an application.
        /// </summary>
        /// <param name="props" cref="GetUserForApplicationProps">
        /// The parameters of the call.
        /// </param>
        private void GetUserForApplication(GetUserForApplicationProps props,
            UnityAction<GetUserForApplicationOutput> callbackOnSuccess,
            UnityAction<Error> callbackOnFail)
        {
            var path = $"app/{props.ApplicationID}/user";

            UnityAction<GetUserForApplicationOutput> cbSuccess = (output) =>
            {

                SetApplicationUser(output.user);
                callbackOnSuccess(output);
            };

            SendRequestCoroutine(HttpMethod.Get.Method,
                GameStackRESTClientUtil.GenerateHeaders(GetToken()),
                GameStackClientConfig.GetGameStackLeaderboardURL(path),
                cbSuccess, callbackOnFail);
        }

        /// <summary>
        /// SetApplicationUser assigns the user as the current application user
        /// using a write lock to allow async access.
        /// </summary>
        /// <param name="user">
        /// The user user that will be set as the current application user.
        /// </param>
        private void SetApplicationUser(User user)
        {
            _applicationLock.EnterWriteLock();
            try
            {
                _applicationUser = user;
            }
            finally
            {
                _applicationLock.ExitWriteLock();
            }
        }

        /// <summary>
        /// GetLeaderboardStats retrieves statistics from a specific
        /// leaderboard.
        /// </summary>
        /// <param name="props" cref="GetLeaderboardStatsProps">
        /// The parameters of the call.
        /// </param>
        public void GetLeaderboardStats(GetLeaderboardStatsProps props,
            UnityAction<GetLeaderboardStatsOutput> callbackOnSuccess,
            UnityAction<Error> callbackOnFail)
        {
            var path = $"app/{props.ApplicationID}/leaderboard/{props.LeaderboardID}/stats";

            var input = new GetLeaderboardStatsInput();
            input.stats = props.Stats;
            input.filters = props.Filters;
            input.sort = props.Sort;
            input.pagination = props.Pagination;

            Func<string, GetLeaderboardStatsOutput> jsonUnmarshaler = (data) =>
            {
                return GetLeaderboardStatsOutput.FromJSON(data);
            };

            try
            {
                SendRequest(WebRequestMethods.Http.Post,
                    GameStackRESTClientUtil.GenerateHeaders(GetToken()),
                    GameStackClientConfig.GetGameStackLeaderboardURL(path),
                    JsonUtility.ToJson(input), jsonUnmarshaler,
                    callbackOnSuccess, callbackOnFail);
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
        /// <param name="props" cref="PutLeaderboardStatsProps">
        /// The parameters of the call.
        /// </param>
        /// <param name="callbackOnSuccess">
        /// A callback that will be called if the request was successful.
        /// </param>
        /// <param name="callbackOnFail">
        /// A callback that will be called if the request failed.
        /// </param>
        public void PutLeaderboardStats(PutLeaderboardStatsProps props,
            UnityAction<PutLeaderboardStatsOutput> callbackOnSuccess,
            UnityAction<Error> callbackOnFail)
        {
            var path = $"app/{props.ApplicationID}/leaderboard/{props.LeaderboardID}/stats";

            var input = new PutLeaderboardStatsInput(props.Dimensions);

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
        /// IsLoggedIn returns true if there is a currently logged in user and
        /// returns false if no user is logged in.
        /// </summary>
        /// <returns>
        /// True if the player is currently logged in, false if they are not.
        /// </returns>
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
                var nowBeforeSessionExpires = now < sessionExpiresIn;
                var nowBeforeTokenExpires = now < _tokenExpires;
                return nowBeforeSessionExpires && nowBeforeTokenExpires;
            }
            finally
            {
                _credentialsLock.ExitReadLock();
            }
        }

        /// <summary>
        /// GetPlayerApplicationAlias returns the logged in GameStack
        /// players alias for a specific application.
        /// </summary>
        /// <returns>
        /// The logged in players alias for the application
        /// they are logged in to.
        /// </returns>
        public string GetPlayerApplicationAlias()
        {
            _applicationLock.EnterReadLock();
            try
            {
                return _applicationUser == null ? string.Empty : _applicationUser.alias;
            }
            finally
            {
                _applicationLock.ExitReadLock();
            }
        }

        #endregion
    }
}
