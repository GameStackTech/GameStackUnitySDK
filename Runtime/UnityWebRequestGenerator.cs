using System;
using System.Collections.Generic;
using UnityEngine.Networking;

namespace GameStack
{
    internal static class UnityWebRequestGenerator
    {
        internal static Func<Dictionary<string, string>, string, string,
            IUnityWebRequestWrapper> NewDELETERequest = (headers, url, body) =>
        {
            var unityWebRequest = UnityWebRequest.Delete(url);
            byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(body);
            unityWebRequest.uploadHandler = new UploadHandlerRaw(
                jsonToSend);
            unityWebRequest.downloadHandler = new DownloadHandlerBuffer();
            ApplyHeaders(ref unityWebRequest, headers);

            return new UnityWebRequestWrapper(unityWebRequest);
        };
        internal static Func<Dictionary<string, string>, string,
            IUnityWebRequestWrapper> NewGETRequest = (headers, url) =>
        {
            var unityWebRequest = UnityWebRequest.Get(url);
            ApplyHeaders(ref unityWebRequest, headers);

            return new UnityWebRequestWrapper(unityWebRequest);
        };
        internal static Func<Dictionary<string, string>, string, string,
            IUnityWebRequestWrapper> NewPOSTRequest = (headers, url, body) =>
        {
            var unityWebRequest = new UnityWebRequest(url,
                UnityWebRequest.kHttpVerbPOST);
            byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(body);
            unityWebRequest.uploadHandler = new UploadHandlerRaw(
                jsonToSend);
            unityWebRequest.downloadHandler = new DownloadHandlerBuffer();
            ApplyHeaders(ref unityWebRequest, headers);

            return new UnityWebRequestWrapper(unityWebRequest);
        };
        internal static Func<Dictionary<string, string>, string, string,
            IUnityWebRequestWrapper> NewPUTRequest = (headers, url, body) =>
        {
            var unityWebRequest = UnityWebRequest.Put(url, body);
            ApplyHeaders(ref unityWebRequest, headers);

            return new UnityWebRequestWrapper(unityWebRequest);
        };

        internal static void ApplyHeaders(ref UnityWebRequest unityWebRequest,
            Dictionary<string, string> headers)
        {
            if (headers != null)
            {
                foreach (KeyValuePair<string, string> entry in headers)
                {
                    unityWebRequest.SetRequestHeader(entry.Key, entry.Value);
                }
            }
        }
    }
}
