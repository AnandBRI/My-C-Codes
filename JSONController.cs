using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class JSONController : MonoBehaviour
{
    public string jsonURL;
    public TextMeshProUGUI nameList;
    public TextMeshProUGUI idList;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(getData());
    }

    IEnumerator getData()
    {
        Debug.Log("Proccessing");
        WWW _www = new WWW(jsonURL);
        yield return _www;
        if(_www.error == null )
        {
            processJsonData(_www.text);
        }
        else
        {
            Debug.Log("Oops something Went Wrong");
        }
    }
  private void processJsonData(string _url)
    {
        JSONdata jsnData = JsonUtility.FromJson<JSONdata>(_url);
       // Debug.Log(jsnData.current_page);
       // Debug.Log(jsnData.entries);
        foreach (Entry x in jsnData.entries)
        {
           // Debug.Log(x.name);
           // Debug.Log(x.id);
            nameList.text = x.name;
            idList.text = x.id;

        }
    }
     
}
