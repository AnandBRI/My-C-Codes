using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class openmanager : MonoBehaviour
{
    public Text Ntext;

    void Start()
    {
        Ntext.text = PlayerPrefs.GetString("name1");
    }

    public void Logout()
    {
        PlayerPrefs.DeleteKey("name1");
        SceneManager.LoadScene("Login1");
    }
  
}
