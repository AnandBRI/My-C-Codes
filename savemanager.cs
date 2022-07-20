using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class savemanager : MonoBehaviour
{

    public InputField MyField;
    public string scenename;

    public void savename()
    {
        PlayerPrefs.SetString("name1", MyField.text);
        SceneManager.LoadScene(scenename);
    }
}
