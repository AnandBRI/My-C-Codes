using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TabLikePages : MonoBehaviour
{

    public GameObject tabbutton1;
    public GameObject tabbutton2;
    public GameObject tabbutton3;

    public GameObject tabcontent1;
    public GameObject tabcontent2;
    public GameObject tabcontent3;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void HideAlltabs()
    {
        tabcontent1.SetActive(false);
        tabcontent2.SetActive(false);
        tabcontent3.SetActive(false);

        tabbutton1.GetComponent<Button>().image.color = new Color32(212, 212, 212, 255);
        tabbutton2.GetComponent<Button>().image.color = new Color32(212, 212, 212, 255);
        tabbutton3.GetComponent<Button>().image.color = new Color32(212, 212, 212, 255);
    }

    public void ShowTab1()
    {
        HideAlltabs(); 
        tabcontent1.SetActive(true);
        tabbutton1.GetComponent<Button>().image.color = new Color32(255, 255, 255, 255);


    }
    public void ShowTab2()
    {
        HideAlltabs();
        tabcontent2.SetActive(true);
        tabbutton2.GetComponent<Button>().image.color = new Color32(255, 255, 255, 255);


    }
    public void ShowTab3()
    {
        HideAlltabs();
        tabcontent3.SetActive(true);
        tabbutton3.GetComponent<Button>().image.color = new Color32(255, 255, 255, 255);

    }
}
