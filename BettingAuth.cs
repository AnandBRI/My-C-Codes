using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using SimpleJSON;

public class BettingAuth : MonoBehaviour
{
  
    public string baseUrl = " URl HERE ";

    public string register = " URL HERE ";
    public string login = "URL HERE";

   // public MPmanager mp;

   // public InputField accUser;
   // public InputField accPass;
   // public InputField accPhone;

   //  public Text infoMsg;

    void Start()
    { }
    void Update()
    { }
    IEnumerator LogInAcc(string uPhone, string uPass)
    {
        WWWForm form = new WWWForm();
        form.AddField("loginUser", uPhone);
        form.AddField("loginPass", uPass);
        string loginURL = baseUrl + "" + login;

        using (UnityWebRequest www = UnityWebRequest.Post(login, form))
        {
            www.downloadHandler = new DownloadHandlerBuffer();
            yield return www.SendWebRequest();

            if (www.isNetworkError)
            {
                Debug.Log(www.error);
            }
            else
            {
                string responseText = www.downloadHandler.text;
                JSONNode userData = JSON.Parse(responseText);
                bool statusjson = (bool)userData["status"];
                if (statusjson)
                {
                    
                }
            }

        }
    }

}
