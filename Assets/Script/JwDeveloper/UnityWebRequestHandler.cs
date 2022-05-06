using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Events;
using SimpleJSON;

namespace JwDeveloper
{

    //a monobehaviour class that help to use UnityWebRequest in inspector
    public class UnityWebRequestHandler : MonoBehaviour
    {

        public enum RequestType { GET, POST, DELETE }

        [Header("Variable used for Debug:")]
        [SerializeField] WebRequestDebugVariable debugVariable = new WebRequestDebugVariable();
        [Space(25)]
        [SerializeField] private RequestType requestType = RequestType.GET;
        [SerializeField] private string apiUrl = "";
        [Tooltip("Set it to true if you want to post data as JSON data format, \nSet it false if you want to use default WWWform data class")]
        [SerializeField] private bool postUsingJsonFormat = false;
        [Tooltip("Set the variable for the post key, can set using scripting as well\n NOTE: value in the inspector will be overwrite by scripting")]
        public List<PostItem> postItem = new List<PostItem>();
        public UnityEvent OnWebRequestSent;
        // public UnityEvent OnWebRequestInProgress;
        public UnityEvent OnWebRequestComplete;
        public UnityEvent<string> OnWebRequestError;
        public UnityEvent<string> OnWebRequestSuccess;

        //private variable
        UnityWebRequest webRequest;
        Coroutine sendWebRequestCor;
        string selectedPostKey;

        public UnityWebRequestHandler(){}       

        /// <summary>
        /// A custom Unity Web Reqeust Handler to handle the web request
        /// </summary>
        /// <param name="apiUrl"> the url to send the web request </param>
        /// <param name="requestType"> the request type to sent </param>
        /// <param name="postItems"> the item to post if using POST request type </param>
        /// <param name="OnWebRequestSent"> the Unity Event that will invoke once the web request is sent </param>
        /// <param name="OnWebRequestComplete"> the Unity Event that will invoke once the web request complete </param>
        /// <param name="OnWebRequestSuccess"> the Unity Event that will invoke when the web request is success, 
        ///                                     \nthe string return is the web respond  </param>
        /// <param name="OnWebRequestError"> the Unity Event that will invoke when the web request is error
        ///                                     \nthe string return is the web respond  </param>
        public UnityWebRequestHandler(string apiUrl, RequestType requestType = RequestType.GET, List<PostItem> postItems = null, 
                                    UnityEvent OnWebRequestSent = null, UnityEvent OnWebRequestComplete = null,
                                    UnityEvent<string> OnWebRequestSuccess = null, UnityEvent<string> OnWebRequestError = null,
                                    bool includeDebugLog = false, string webRequestName = "Default Web Request Name"){
            this.apiUrl = apiUrl;
            this.requestType = requestType;
            this.postItem = postItems;
            this.OnWebRequestSent = OnWebRequestSent;
            this.OnWebRequestComplete = OnWebRequestComplete;
            this.OnWebRequestSuccess = OnWebRequestSuccess;
            this.OnWebRequestError = OnWebRequestError;
            this.debugVariable.includeDebugLog = includeDebugLog;
            this.debugVariable.WebRequestName = webRequestName;
        }       

        /// <summary>
        /// Start Send Web Request
        /// </summary>
        public void SendWebRequest()
        {
            sendWebRequestCor = StartCoroutine(_SendWebRequest());
        }

        #region Input Field Methods -> methods for input field to edit post field in inspector
        /// <summary>
        /// A method for Input Field Unity Events: "On Value Changed 'String' (Dynamic)"
        /// </summary>
        public void EditPostKey(string value)
        {
            if (selectedPostKey == string.Empty) {
                DebugInConsole("EditPostKey", "selected Post Key is empty.");
                return;}

            postItem.Find(x => x.fieldName == selectedPostKey).value = value;
        }

        /// <summary>
        /// A method for Input Field Unity Events: "On Deselect <String> (Static)"
        /// </summary>
        public void SelectPostKeyFieldName(string fieldName)
        {
            if (postItem.Find(x => x.fieldName == fieldName) == null)
            {
                DebugInConsole("SelectPostKey", $"Added \"{fieldName}\" to the post key list.");
                postItem.Add(new PostItem(fieldName));
                selectedPostKey = fieldName;
            }
            else selectedPostKey = fieldName;
        }

        /// <summary>
        /// A method for Input Field Unity Events: "On Select <String> (Static)"
        /// </summary>
        public void DeselectPostKey()
        {
            selectedPostKey = string.Empty;
        }
        #endregion

        #region Private Methods
        private IEnumerator _SendWebRequest()
        {

            DebugInConsole("Post Method: " + requestType.ToString(), "Api URL: " + apiUrl);

            //write form
            WWWForm form = new WWWForm();
            JSONObject jsonData = new JSONObject();
            if (requestType == RequestType.POST)
            { //only write form for post method
                int i = 0;


                if (postItem != null || postItem.Count >= 1)
                { //check whether post form is empty
                    string debugLine = "";

                    if (!postUsingJsonFormat)
                    { //if not using JSON format post data, use Unity built in data WWWform class type data
                      //add the post item to WWW form
                        foreach (PostItem item in postItem)
                        {
                            form.AddField(postItem[i].fieldName, postItem[i].value);
                            if (debugVariable.includeDebugLog) debugLine += postItem[i].fieldName + "\t: " + postItem[i].value + "\n";
                            i++;
                        }

                    }
                    else
                    { //if wanted to post using JSON data format
                        foreach (PostItem item in postItem)
                        {
                            jsonData.Add(postItem[i].fieldName, postItem[i].value);
                            if (debugVariable.includeDebugLog) debugLine += postItem[i].fieldName + "\t: " + postItem[i].value + "\n";
                            i++;
                        }
                    }

                    DebugInConsole("---Post Item---", debugLine);

                }
                else
                {
                    Debug.LogWarning("Post item is null or empty!\nWebRequestName: " + debugVariable.WebRequestName + "\nUrl: " + apiUrl);
                }
            }

            //send request to server
            switch (requestType)
            {
                default:
                case RequestType.GET:
                    webRequest = UnityWebRequest.Get(apiUrl);
                    break;
                case RequestType.POST:
                    if (postUsingJsonFormat) webRequest = UnityWebRequest.Post(apiUrl, jsonData); //post using JSON data
                    else webRequest = UnityWebRequest.Post(apiUrl, form); //post using built in Unity WWWform class
                    break;
                case RequestType.DELETE:
                    webRequest = UnityWebRequest.Delete(apiUrl);
                    break;
            }
            DebugInConsole("Request Sent.");
            OnWebRequestSent.Invoke();
            yield return webRequest.SendWebRequest();

            OnWebRequestComplete?.Invoke();

            // if (webRequest.result == UnityWebRequest.Result.InProgress)
            // {
            //     OnWebRequestInProgress.Invoke();
            //     DebugInConsole("Web request is in progress.");
            //     yield break;
            // }

            //check whether request is failed
            if (webRequest.result == UnityWebRequest.Result.ProtocolError || webRequest.result == UnityWebRequest.Result.ConnectionError)
            {
                DebugInConsole("Web Request sent is error.", "Error Message: " + webRequest.error);
                OnWebRequestError.Invoke(webRequest.error);
                yield break;
            }

            //if success, get server response
            if (webRequest.result == UnityWebRequest.Result.Success)
            {
                DebugInConsole("Web Request sent is success!", "Message: " + webRequest.downloadHandler.text);
                OnWebRequestSuccess.Invoke(webRequest.downloadHandler.text);
                yield break;
            }

        }

        #endregion

        #region Publics Method
        public string GetApiUrl(){
            return apiUrl;
        }

        public void SetApiUrl(string url){
            apiUrl = url;
        }

        public void SetRequestType(RequestType requestType){
            this.requestType = requestType;
        }

        public void AddPostItem(string fieldName, string value){
            SelectPostKeyFieldName(fieldName);
            EditPostKey(value);
        }

        public List<PostItem> GetPostItems()
        {
            return postItem;
        }

        // //a public methoud to check whether web request is complete (still in testing)
        // public bool? IsInProgress()
        // {
        //     if (webRequest != null)
        //     {
        //         if (webRequest.result == UnityWebRequest.Result.InProgress)
        //         {
        //             DebugInConsole("Web request is in progress.");
        //             return true;
        //         }
        //         else
        //         {
        //             DebugInConsole("Web request is complete.");
        //             return false;
        //         }
        //     }
        //     else
        //     {
        //         DebugInConsole("No webRequest sent yet!");
        //         return null;
        //     }
        // }
        #endregion
        

        private void DebugInConsole(string titleText, string debugText = "----")
        {
            if (!debugVariable.includeDebugLog) return;

            Debug.Log(
                titleText
                + "\n" + debugText
                + "\nWebRequesName: " + debugVariable.WebRequestName
                + "\nGameObject: " + gameObject.name
                + "\n----------------------------------------------------------------------------------------------------"
            );
        }

        [System.Serializable]
        private class WebRequestDebugVariable
        { //a specific class to store data for debug (for inspector use purpose)
            [Tooltip("Optional, add a name for this web request for easier identify in debug log"), SerializeField] public string WebRequestName = "";
            [Tooltip("Tick to enable debug responds while untick to disable"), SerializeField] public bool includeDebugLog = true;
        }
    }
    
    //a class to store the post item value
        [System.Serializable]
        public class PostItem
        {
            public string fieldName;
            public string value;
            public PostItem()
            {
                this.fieldName = null;
                this.value = null;
            }

            public PostItem(string fieldName = null, string value = null)
            {
                this.fieldName = fieldName;
                this.value = value;
            }
        }
}
