using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using GameStackUnitySDK.Model.Auth.Entity;
using GameStackUnitySDK.Model.Errors;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

namespace GameStackUnitySDK
{
    static class GameStackRESTClientUtil
    {
        private static readonly string _timeoutError = "Request timeout";

        /// <summary>
        /// EncodePassword takes the supplied password, SHA512 hashs it, then
        /// hex encodes the hash.
        /// </summary>
        /// <param name="password">The password to encode.</param>
        public static string EncodePassword(string password)
        {
            var encodedPassword = "";
            using (SHA512 sha = new SHA512Managed())
            {
                var data = Encoding.UTF8.GetBytes(password);
                // Converts the password to a SHA512 checksum.
                var passChecksum = sha.ComputeHash(data);
                // Converts the SHA512 checksum bytes to a hex string with lower
                // case letters.
                encodedPassword = BitConverter.ToString(
                    passChecksum).Replace("-", string.Empty).ToLower();
            }
            return encodedPassword;
        }

        /// <summary>
        /// This method finishes request process as we have received answer
        /// from server.
        /// </summary>
        /// <param name="data">Data received from server in JSON format.</param>
        /// <param name="callbackOnSuccess">Callback on success.</param>
        /// <param name="callbackOnFail">Callback on fail.</param>
        /// <typeparam name="T">Data Model Type.</typeparam>
        /// <exception cref="UnknownHTTPVerbException">
        /// Throws UnknownHTTPVerbException if the HTTP verb for the request
        /// is not supported.
        /// </exception>
        static internal void ParseResponse<T>(string data,
            UnityAction<T> callbackOnSuccess,
            UnityAction<Error> callbackOnFail)
        {
            callbackOnSuccess?.Invoke(JsonUtility.FromJson<T>(data));
        }

        static internal IEnumerator RequestCoroutine<T>(
            Dictionary<string, string> headers, string url, string body,
            string verb, UnityAction<T> callbackOnSuccess,
            UnityAction<Error> callbackOnFail)
        {
            IUnityWebRequestWrapper request;
            switch (verb)
            {
                case UnityWebRequest.kHttpVerbDELETE:
                    request = UnityWebRequestGenerator.NewDELETERequest(headers,
                        url, body);
                    break;

                case UnityWebRequest.kHttpVerbGET:
                    request = UnityWebRequestGenerator.NewGETRequest(headers,
                        url);
                    break;

                case UnityWebRequest.kHttpVerbPOST:
                    request = UnityWebRequestGenerator.NewPOSTRequest(headers,
                        url, body);
                    break;

                case UnityWebRequest.kHttpVerbPUT:
                    request = UnityWebRequestGenerator.NewPUTRequest(headers,
                        url, body);
                    break;

                default:
                    throw new UnknownHTTPVerbException(
                        $"unknown HTTP verb {verb}");
            }
            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                var headerText = "";
                foreach(KeyValuePair<string, string> kvp in headers)
                {
                    headerText += $"key: {kvp.Key}, value: {kvp.Value}\n";
                }
                Debug.LogError(
                    $"failed to execute request with\nurl: {url}\nheaders:\n{headerText}\nbody: {body}\nerror: {request.error}");

                if (request.error.Contains(_timeoutError))
                {
                    callbackOnFail?.Invoke(new Error(
                        (long)HttpStatusCode.RequestTimeout, request.error));
                }
                else
                {
                    callbackOnFail?.Invoke(
                        new Error(request.responseCode, request.error));
                }
            }
            else
            {
                ParseResponse(request.downloadHandler.text, callbackOnSuccess,
                    callbackOnFail);
            }
        }

        /// <summary>
        /// Creates new default headers for a request to the GameStack API.
        /// </summary>
        public static Dictionary<string, string> GenerateHeaders()
        {
            return GameStackRESTClientUtil.GenerateHeaders(null);
        }

        /// <summary>
        /// Creates new default headers for a request to the GameStack API.
        /// <param name="token">Optional user access token object.</param>
        /// </summary>
        public static Dictionary<string, string> GenerateHeaders(Token token) {
            var headers = new Dictionary<string, string>()
            {
                { "Content-Type", "application/json" }
            };
            if (token != null)
            {
                var authorizationHeaderValue = $"Bearer {token.access_token}";
                headers.Add("Authorization", authorizationHeaderValue);
            }
            return headers;
        }
    }
}
