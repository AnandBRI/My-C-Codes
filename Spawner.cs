using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject Cube;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = 10;
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePos);
            Instantiate(Cube, worldPosition, Quaternion.identity);
        }
    }
}