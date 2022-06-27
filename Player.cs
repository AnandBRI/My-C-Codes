using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;
using UnityEngine.UI;

public class Player : Photon.PunBehaviour
{
    public MPManager mp;
    public Text healthText;
    public Text userText;
    public float Speed = 3f;
    public Rigidbody rb;
    private float Health = 100;
    private float MinHealth = 0;
    private float MaxHealth = 100;
    public string username;
    public Vector3 camOffset;
    private float x;
    private float z;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        
        if (photonView.isMine)
        {
            userText.text = username;
        }
        else
        {
            
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Hurt")
        {
            if (photonView.isMine)
            {
                photonView.RPC("Damage", PhotonTargets.All);
            }
        }
    }
    [PunRPC]
    void Damage()
    {
        Health -= 20;
    }
    void Update()
    {
        if (photonView.isMine)
        {
            if (!mp.gameStart)
                return;
            x = Input.GetAxisRaw("Horizontal");
            z = Input.GetAxisRaw("Vertical");
            Vector3 camPos = Camera.main.transform.position;
            Vector3 pPos = transform.position;
            Camera.main.transform.position = Vector3.Lerp(camPos, new Vector3(pPos.x, pPos.y, pPos.z) + camOffset, 0.4f);

            if (Health > MaxHealth)
            {
                Health = MaxHealth;
            }
            if (Health < MinHealth)
            {
                Health = MinHealth;
            }
        }
        else
        {
            
        }
    }
    private void FixedUpdate()
    {
        healthText.text = Health.ToString();
        if (photonView.isMine)
        {
            rb.AddForce(new Vector3(x, 0, z) * Speed);
        }
    }

    public virtual void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            stream.SendNext(Health);
            stream.SendNext(username);
        }
        else if (stream.isReading)
        {
            Health = (float)stream.ReceiveNext();
            username = (string)stream.ReceiveNext();
        }
    }
}
