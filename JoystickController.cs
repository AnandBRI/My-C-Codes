using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class JoystickController : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    [SerializeField]
    private float maxDisplacement = 200;
    Vector2 startPos;
    Transform handle;

    public static float Horizontal = 0, Vertical = 0;

    void Start()
    {
        handle = transform.GetChild(0);
        startPos = handle.position;
    }

    void UpdateHandleposition(Vector2 pos)
    {
        var delta = pos - startPos;
        delta = Vector2.ClampMagnitude(delta, maxDisplacement);
        handle.position = startPos + delta;
        Horizontal = delta.x / maxDisplacement;
        Vertical = delta.y / maxDisplacement;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        UpdateHandleposition(eventData.position);
    }
    public void OnMouseDrag(PointerEventData eventData)
    {
        UpdateHandleposition(eventData.position);
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        UpdateHandleposition(startPos);
    }
}
/*
using System.Collectiond;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class JoystickMove : MonoBehaviour
{
  public float speed = 5;
  Rigidbody rigidBody;

  void Awake()
  {
      rigidBody = GetComponent<Rigidbody>();
  }

  void FixedUpdate()
  {
      float inputH = JoystickController.Horizontal;
      float inputV = JoystickController.Vertical;
      rigidBody.velocity = new Vector3(inputH * speed, rigidBody.velocity.y, inputV * speed);
  }
}
*/
