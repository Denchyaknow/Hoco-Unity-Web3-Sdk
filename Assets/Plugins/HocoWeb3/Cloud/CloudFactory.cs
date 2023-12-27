using System.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;

namespace Hoco.Cloud
{
    public static partial class CloudFactory
    {
        private const string k_FunctionCreateCollectionURL = "https://mongorest.azurewebsites.net/api/MongoDB/CreateCollection";
        private const string k_FunctionGetCollectionURL = "https://mongorest.azurewebsites.net/api/MongoDB/GetAllDataByCollectionName";
        private const string k_FunctionGetMultipleCollectionURL = "https://mongorest.azurewebsites.net/api/MongoDB/GetMultipleCollectionsByName";
        private const string k_FunctionDeleteCollectionURL = "https://mongorest.azurewebsites.net/api/MongoDB/DeleteCollection";
        private const string k_FunctionUpdateCollectionURL = "https://mongorest.azurewebsites.net/api/MongoDB/UpdateCollection";
        private const string k_FunctionGetFilteredCollectionURL = "https://mongorest.azurewebsites.net/api/MongoDB/GetFilteredData";


        /// <summary>
        /// A class that contains the data needed to interact with a collection via WebRequests
        /// </summary>
        [System.Serializable]
        private class LiveDataRequest
        {
            public string collectionName { get; set; }
            public string id { get; set; }
            public string json { get; set; }
        }

        /// <summary>Needed to Create a encoded URL that contained the params needed for filtering a collection.</summary>
        /// <returns>The URL to put in the <see cref="Get(string)"/> task</returns>
        private static string ConstructFilteredUrl(string baseUrl, string collectionName, Dictionary<string, object> filterJson)
        {
            string jsonFilterString = JsonConvert.SerializeObject(filterJson);// Serialize to JSON string
            string encodedJsonFilter = Uri.EscapeDataString(jsonFilterString);// URL-encode the filter string
            string finalUrl = $"{baseUrl}?collectionName={collectionName}&filterJson={encodedJsonFilter}";// Append the encoded filter to the URL
            Debug.Log(finalUrl);
            return finalUrl;
        }

        /// <summary>Needed to Create a encoded URL that contained the params needed for filtering a collection.</summary>
        /// <param name="_filterValue">The value to filter by.</param>
        /// <param name="_filterVariable">The variable to filter by.</param>
        /// <returns>The URL to put in the <see cref="Get(string)"/> task</returns>
        private static Dictionary<string, object> FormatFilter(string _filterVariable, string _filterValue)
        {
            return new Dictionary<string, object>() { { _filterVariable, _filterValue } };
        }

        /// <summary>Used to format a Url for a MongoDb Collection to use in <see cref="Get(string)"/>, <see cref="Post(string, string)"/>, <see cref="Delete(string, string)"/>, and <see cref="Put(string, string)"/></summary>
        /// <param name="_baseUrl"></param>
        /// <param name="_collectionName"></param>
        /// <returns>the formatted Url string.</returns>
        private static string FormatUrl(string _baseUrl, string _collectionName)
        {
            return string.Format("{0}?collectionName={1}", _baseUrl, _collectionName);
        }

        /// <summary>Used for React Get requests.</summary>
        /// <param name="url">The Url for the Server Endpoint</param>
        /// <returns>The result or Code response.</returns>
        public static async UniTask<string> Get(string url)
        {
            using (var request = UnityWebRequest.Get(url))
            {
                await request.SendWebRequest();
                return request.downloadHandler.text;
            }
        }

        /// <summary>Used for React Post requests</summary>
        /// <param name="url">The Url for the Server Endpoint.</param>
        /// <param name="data">The Json data to send to the server.</param>
        /// <returns>Code response.</returns>
        public static async UniTask<string> Post(string url, string data)
        {
            using (var request = new UnityWebRequest(url, "POST"))
            {
                byte[] bodyRaw = new System.Text.UTF8Encoding().GetBytes(data);
                request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
                request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
                request.SetRequestHeader("Content-Type", "application/json");
                await request.SendWebRequest();
                return request.downloadHandler.text;
            }
        }

        /// <summary>Used for React Put requests.</summary>
        /// <param name="url">The Url for the Server Endpoint.</param>
        /// <param name="data">The Json data to send to the server.</param>
        /// <returns>Code response.</returns>
        public static async UniTask<string> Put(string url, string data)
        {
            using (var request = new UnityWebRequest(url, "PUT"))
            {
                byte[] bodyRaw = new System.Text.UTF8Encoding().GetBytes(data);
                request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
                request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
                request.SetRequestHeader("Content-Type", "application/json");
                await request.SendWebRequest();
                return request.downloadHandler.text;
            }
        }

        /// <summary>Used for React Delete requests.</summary>
        /// <param name="url">The Url for the Server Endpoint.</param>
        /// <param name="data">The Json data to send to the server.</param>
        /// <returns>Code response.</returns>
        public static async UniTask<string> Delete(string url, string data)
        {
            using (var request = new UnityWebRequest(url, "DELETE"))
            {
                byte[] bodyRaw = new System.Text.UTF8Encoding().GetBytes(data);
                request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
                request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
                request.SetRequestHeader("Content-Type", "application/json");
                await request.SendWebRequest();
                return request.downloadHandler.text;
            }
        }
    }

}
