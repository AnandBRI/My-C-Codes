using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickableObject : MonoBehaviour
{

    public GameObject PickUpCarBack;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.name == "PickUpCarSensor")
        {
            transform.position = PickUpCarBack.transform.position;
            transform.parent = PickUpCarBack.transform;
        }

        if (col.gameObject.name == "DropZone")
        {
            GameObject DropPoint = col.gameObject.transform.Find("DropPoint").gameObject;
            transform.position = DropPoint.transform.position;
            transform.parent = DropPoint.transform;
        }


    }



}