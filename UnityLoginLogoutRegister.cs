using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using SimpleJSON;


public class UnityLoginLogoutRegister : MonoBehaviour
{

    public string baseUrl = "https://cutie.co.in/socialmedia/mob_app/game_api/";

    public string register = "https://cutie.co.in/socialmedia/mob_app/game_api/register.php";
    public string login = "https://cutie.co.in/socialmedia/mob_app/game_api/login.php";




    public InputField accountUserName;
    public InputField accountPassword;
    public InputField accountPhone;
    public Text info;
    public Text playerCoins;
    public Text playerScore;

    private string currentUsername;
    public string ukey = "accountusername";
    private string currentCoins;
    private string currentScore;
    public string ucoin = "accountcoins";
    private string uscore = "accountscore";


    // Start is called before the first frame update
    void Start()
    {

        currentUsername = "";

        if (PlayerPrefs.HasKey(ukey))
        {
            if (PlayerPrefs.GetString(ukey) != "")
            {
                currentUsername = PlayerPrefs.GetString(ukey);
                info.text = "You are loged in as " + currentUsername;
                currentCoins = PlayerPrefs.GetString(ucoin);
                playerCoins.text = "coinsss  = " + currentCoins;
                currentScore = PlayerPrefs.GetString(uscore);
                playerScore.text = "Score  = " + currentScore;

            }
            else
            {
                info.text = "You are not logged in.";
            }
        }
        else
        {
            info.text = "You are not loged in.";
        }


    }

    // Update is called once per frame
    void Update()
    {

    }

    public void AccountLogout()
    {
        currentUsername = "";
        PlayerPrefs.SetString(ukey, currentUsername);
        info.text = "You are just loged out.";

        currentCoins = "";
        PlayerPrefs.SetString(ucoin, currentCoins);
        playerCoins.text = "coinsss";


    }

    public void AccountRegister()
    {
        string uName = accountUserName.text;
        string pWord = accountPassword.text;
        string uPhone = accountPhone.text;
        StartCoroutine(RegisterNewAccount(uName, pWord, uPhone));
    }

    public void AccountLogin()
    {
        string uPhone = accountPhone.text;
        string pWord = accountPassword.text;
        StartCoroutine(LogInAccount(uPhone, pWord));
    }

    IEnumerator RegisterNewAccount(string uName, string pWord, string uPhone)
    {
        WWWForm form = new WWWForm();
        form.AddField("regName", uName);
        form.AddField("regPass", pWord);
        form.AddField("regPhone", uPhone);
        string registerURL = baseUrl + "" + register;

        using (UnityWebRequest www = UnityWebRequest.Post(register, form))
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
                JSONNode pokeInfo = JSON.Parse(www.downloadHandler.text);
                Debug.Log("Response = " + responseText);
                info.text = "Response = " + responseText;
            }
        }
    }

    IEnumerator LogInAccount(string uPhone, string pWord)
    {
        WWWForm form = new WWWForm();
        form.AddField("loginUser", uPhone);
        form.AddField("loginPass", pWord);
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

                string coinjson = userData["user"]["coin"].Value;
                string scorejson = userData["user"]["score"].Value;
                bool statusjson = (bool)userData["status"];
                PlayerPrefs.SetString(ucoin, coinjson); // saving Coin value
                PlayerPrefs.SetString(uscore, scorejson); // saving Coin value


                Debug.Log("My Coins = " + coinjson);
                Debug.Log("My Stat : " + statusjson);
                Debug.Log("My Score : " + scorejson);
                //    JsonUtility.FromJson<UnityLoginLogoutRegister>(userData);
                string user = userData["user"];
                string usweCoin = userData["coin"];
                Debug.Log("Response = " + responseText);
                //   Debug.Log("Response user = " + user);
                //   Debug.Log("Response usweCoin = " + usweCoin );

                if (statusjson)
                {
                    PlayerPrefs.SetString(ukey, uPhone);
                    info.text = "Login Success with Mobile No. " + uPhone;

                    SceneManager.LoadSceneAsync("Home");

                }

                else
                {
                    info.text = "Ur Ph. " + uPhone + ",Ur Pass " + pWord + ", url : " + login + "" + responseText;

                }
            }
        }
    }
}
