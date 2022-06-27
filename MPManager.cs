using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;
using UnityEngine.UI;
using PlayFab;

public class MPManager : Photon.MonoBehaviour
{
    public PlayFabAuth auth;

    public string GameVersion;
    public Text connectState;

    // Canvas , Panel & UI On Off 
    public GameObject[] DisableOnConnected;
    public GameObject[] DisableOnJoinRoom;
    public GameObject[] EnableOnJoinRoom;
    public GameObject[] EnableOnConnected;
    public GameObject[] DisableOnPlayerCountMax;

    public string username;
    public int curPlayers = 0;
    public bool gameStart = false;
    public Text timerText;
    private List<GameObject> spawnPoints = new List<GameObject>();
    private float timer = 3;


    void Start()
    {
        foreach (Transform child in gameObject.transform)
        {
            spawnPoints.Add(child.gameObject);
        }
    }
    void Update()
    {
    //    curPlayers = PhotonNetwork.playerList.Length + 1;
        Debug.Log(curPlayers);
        if (!gameStart)
        {
            if (curPlayers == 2)
            {
                timerText.gameObject.SetActive(true);
                foreach (GameObject disable in DisableOnPlayerCountMax)
                {
                    disable.SetActive(false);
                }
                timer -= Time.deltaTime;
                timerText.text = "Starts In:" + Mathf.Round(timer);
                if (timer < 0)
                {
                    gameStart = true;
                    timerText.gameObject.SetActive(false);
                }
            }
        }
    }
    private void FixedUpdate()
    {
        connectState.text = PhotonNetwork.connectionStateDetailed.ToString();
    }

    public void ConnectToMaster()
    {
      ///  PhotonNetwork.connectionstateDetailed
        PhotonNetwork.ConnectUsingSettings(GameVersion);
    }

    public virtual void OnConnectedToMaster()
    {
        foreach (GameObject disable in DisableOnConnected)
        {
            disable.SetActive(false);
        }
        foreach (GameObject enable in EnableOnConnected)
        {
            enable.SetActive(true);
        }
    }

    public void CreateOrJoin()
    {
        PhotonNetwork.JoinRandomRoom();
    }

    public virtual void OnPhotonRandomJoinFailed()
    {
        RoomOptions rm = new RoomOptions
            {
            MaxPlayers =  2,
            IsVisible = true
            };
        int rndID = Random.Range(0, 3000);
        PhotonNetwork.CreateRoom("Default: " + rndID, rm, TypedLobby.Default);
    }
    public virtual void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            stream.SendNext(curPlayers);
        }
        else if (stream.isReading)
        {
            curPlayers = (int)stream.ReceiveNext();
        }
    }
    public virtual void OnJoinedRoom()
    {
        photonView.RPC("AddPlayerCount", PhotonTargets.All);
              foreach (GameObject disable in DisableOnJoinRoom)
        {
            disable.SetActive(false);
        }
        foreach (GameObject enable in EnableOnJoinRoom)
        {
            enable.SetActive(true);
        }
        Vector3 pos = spawnPoints[Random.Range(0, spawnPoints.Count)].transform.position;
        GameObject player = PhotonNetwork.Instantiate("Player", pos, Quaternion.identity, 0);
        player.GetComponent<Player>().username = username;
        player.GetComponent<Player>().mp = this;
    }
    [PunRPC]
    void AddPlayerCount()
    {
        curPlayers++;
    }
}
