using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using GameStack.Model.Auth.Entity;
using GameStack.Model.Errors;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

namespace GameStack
{
    static class GameStackRESTClientUtil
    {
        private static readonly string _timeoutError = "Request timeout";

        /// <summary>
        /// This method finishes request process as we have received answer
        /// from server.
        /// </summary>
        /// <param name="data">Data received from server in JSON format.</param>
        /// <param name="jsonUnmarshaler">
        /// Optional unmarshaler that will be used if supplied, otherwise the
        /// Unity Json Utility will be used to unmarshal the response.
        /// </param>
        /// <param name="callbackOnSuccess">Callback on success.</param>
        /// <param name="callbackOnFail">Callback on fail.</param>
        /// <typeparam name="T">Data Model Type.</typeparam>
        /// <exception cref="UnknownHTTPVerbException">
        /// Throws UnknownHTTPVerbException if the HTTP verb for the request
        /// is not supported.
        /// </exception>
        static internal void ParseResponse<T>(string data,
            Func<string, T> jsonUnmarshaler, UnityAction<T> callbackOnSuccess,
            UnityAction<Error> callbackOnFail)
        {
            var output = (jsonUnmarshaler == null) ? JsonUtility.FromJson<T>(
                data) : jsonUnmarshaler(data);
            callbackOnSuccess?.Invoke(output);
        }

        /// <summary>
        /// An overridden version of ParseResponse that does not include the
        /// custom JSON unmarshaler parameter.
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
            UnityAction<T> callbackOnSuccess, UnityAction<Error> callbackOnFail)
        {
            ParseResponse(data, null, callbackOnSuccess, callbackOnFail);
        }

        static internal IEnumerator RequestCoroutine<T>(
            Dictionary<string, string> headers, string url, string body,
            string verb, Func<string, T> jsonUnmarshaler,
            UnityAction<T> callbackOnSuccess, UnityAction<Error> callbackOnFail)
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

            if (request.isHttpError || request.isNetworkError)
            {
                var headerText = "";
                foreach (KeyValuePair<string, string> kvp in headers)
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
                    callbackOnFail?.Invoke(new Error(request.responseCode, request.error));
                }
            }
            else
            {
                ParseResponse(request.downloadHandler.text, jsonUnmarshaler,
                    callbackOnSuccess, callbackOnFail);
            }
        }

        static internal IEnumerator RequestCoroutine<T>(
            Dictionary<string, string> headers, string url, string body,
            string verb, UnityAction<T> callbackOnSuccess,
            UnityAction<Error> callbackOnFail)
        {
            return RequestCoroutine(headers, url, body, verb, null,
                callbackOnSuccess, callbackOnFail);
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
        public static Dictionary<string, string> GenerateHeaders(Token token)
        {
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
