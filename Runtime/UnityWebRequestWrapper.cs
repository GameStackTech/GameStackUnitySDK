using System;
using UnityEngine.Networking;
using static UnityEngine.Networking.UnityWebRequest;

namespace GameStack
{
    public interface IUnityWebRequestWrapper
    {
        CertificateHandler certificateHandler { get; set; }
        bool chunkedTransfer { get; set; }
        bool disposeCertificateHandlerOnDispose { get; set; }
        bool disposeDownloadHandlerOnDispose { get; set; }
        bool disposeUploadHandlerOnDispose { get; set; }
        ulong downloadedBytes { get; }
        DownloadHandler downloadHandler { get; set; }
        float downloadProgress { get; }
        string error { get; }
        bool isDone { get; }
        bool isHttpError { get; }
        bool isModifiable { get; }
        bool isNetworkError { get; }
        string method { get; set; }
        int redirectLimit { get; set; }
        long responseCode { get; }
        int timeout { get; set; }
        UnityWebRequest unityWebRequest { get; set; }
        ulong uploadedBytes { get; }
        UploadHandler uploadHandler { get; set; }
        float uploadProgress { get; }
        Uri uri { get; set; }
        string url { get; set; }
        bool useHttpContinue { get; set; }

        UnityWebRequestAsyncOperation SendWebRequest();
        void SetRequestHeader(string name, string value);
    }

    public class UnityWebRequestWrapper : IUnityWebRequestWrapper
    {
        public UnityWebRequestWrapper(UnityWebRequest unityWebRequest)
        {
            _unityWebRequest = unityWebRequest;
        }

        UnityWebRequest _unityWebRequest { get; set; }

        public CertificateHandler certificateHandler
        {
            get { return _unityWebRequest.certificateHandler; }
            set { _unityWebRequest.certificateHandler = value; }
        }

        [Obsolete]
        public bool chunkedTransfer
        {
            get { return _unityWebRequest.chunkedTransfer; }
            set { _unityWebRequest.chunkedTransfer = value; }
        }

        public bool disposeCertificateHandlerOnDispose
        {
            get { return _unityWebRequest.disposeCertificateHandlerOnDispose; }
            set { _unityWebRequest.disposeCertificateHandlerOnDispose = value; }
        }

        public bool disposeDownloadHandlerOnDispose
        {
            get { return _unityWebRequest.disposeDownloadHandlerOnDispose; }
            set { _unityWebRequest.disposeDownloadHandlerOnDispose = value; }
        }

        public bool disposeUploadHandlerOnDispose
        {
            get { return _unityWebRequest.disposeUploadHandlerOnDispose; }
            set { _unityWebRequest.disposeUploadHandlerOnDispose = value; }
        }

        public ulong downloadedBytes
        {
            get { return _unityWebRequest.downloadedBytes; }
        }

        public DownloadHandler downloadHandler
        {
            get { return _unityWebRequest.downloadHandler; }
            set { _unityWebRequest.downloadHandler = value; }
        }

        public float downloadProgress
        {
            get { return _unityWebRequest.downloadProgress; }
        }

        public string error
        {
            get { return _unityWebRequest.error; }
        }

        public bool isDone
        {
            get { return _unityWebRequest.isDone; }
        }

        [Obsolete]
        public bool isNetworkError
        {
            get { return _unityWebRequest.isNetworkError; }
        }

        [Obsolete]
        public bool isHttpError
        {
            get { return _unityWebRequest.isHttpError; }
        }

        public bool isModifiable
        {
            get { return _unityWebRequest.isModifiable; }
        }

        public string method
        {
            get { return _unityWebRequest.method; }
            set { _unityWebRequest.method = value; }
        }

        public int redirectLimit
        {
            get { return _unityWebRequest.redirectLimit; }
            set { _unityWebRequest.redirectLimit = value; }
        }

        public long responseCode
        {
            get { return _unityWebRequest.responseCode; }
        }

        public int timeout
        {
            get { return _unityWebRequest.timeout; }
            set { _unityWebRequest.timeout = value; }
        }

        public UnityWebRequest unityWebRequest
        {
            get { return _unityWebRequest; }
            set { _unityWebRequest = value; }
        }

        public ulong uploadedBytes
        {
            get { return _unityWebRequest.uploadedBytes; }
        }

        public UploadHandler uploadHandler
        {
            get { return _unityWebRequest.uploadHandler; }
            set { _unityWebRequest.uploadHandler = value; }
        }

        public float uploadProgress
        {
            get { return _unityWebRequest.uploadProgress; }
        }

        public Uri uri
        {
            get { return _unityWebRequest.uri; }
            set { _unityWebRequest.uri = value; }
        }

        public string url
        {
            get { return _unityWebRequest.url; }
            set { _unityWebRequest.url = value; }
        }

        public bool useHttpContinue
        {
            get { return _unityWebRequest.useHttpContinue; }
            set { _unityWebRequest.useHttpContinue = value; }
        }

        public UnityWebRequestAsyncOperation SendWebRequest()
        {
            return _unityWebRequest.SendWebRequest();
        }

        public void SetRequestHeader(string name, string value)
        {
            _unityWebRequest.SetRequestHeader(name, value);
        }
    }
}
