using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon;
using Photon.Pun;


public class Player : MonoBehaviourPun, IPunObservable
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
    private Animator anim;
    private PlayFabAuth pauth;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        if (photonView.IsMine)
        {

            pauth = GameObject.FindGameObjectWithTag("PlayFab").GetComponent<PlayFabAuth>();

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
            if (photonView.IsMine)
            {
                photonView.RPC("Damage", RpcTarget.All);
            }
        }
        if (collision.gameObject.tag == "End")
        {
            pauth.SetWins(1);
            mp.SetWinner(username);
        }
    }
    [PunRPC]
    void Damage()
    {
        Health -= 30;
    }
  
 
    void Update()
    {
        if (photonView.IsMine)
        {
            if (!mp.gameStart)
                return;
            if (mp.aPlayerHasWon)
                return;
            x = Input.GetAxisRaw("Horizontal");
            z = Input.GetAxisRaw("Vertical");
            float move = x + z;
            anim.SetFloat("hop", move);
            Vector3 camPos = Camera.main.transform.position;
            Vector3 pPos = transform.position;
            Camera.main.transform.position = Vector3.Lerp(camPos, new Vector3(pPos.x, pPos.y, pPos.z) + camOffset, 0.4f);
         
            if (Health > MaxHealth)
            {
                Health = MaxHealth;
            }
            if (Health < MinHealth)
            {
                Vector3 pos = mp.spawnPoints[Random.Range(0, mp.spawnPoints.Count)].transform.position;
                transform.position = pos;
                Health = MaxHealth;
              //  Health = MinHealth;
            }
        }
        else
        {
            
        }
    }
    private void FixedUpdate()
    {
        healthText.text = Health.ToString();
        if (photonView.IsMine)
        {
            rb.AddForce(new Vector3(x, 0, z) * Speed);
        }
    }

    public virtual void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(Health);
            stream.SendNext(username);
        }
        else if (stream.IsReading)
        {
            Health = (float)stream.ReceiveNext();
            username = (string)stream.ReceiveNext();
            userText.text = username;
        }
    }
}
