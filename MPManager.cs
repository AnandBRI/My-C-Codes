using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;
using Photon.Pun;
using UnityEngine.UI;
using Photon.Realtime;
using UnityEngine.SceneManagement;


public class MPManager : MonoBehaviourPunCallbacks, IPunObservable
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
    public GameObject WinnerPannel;
    public Text WinText;
    public bool aPlayerHasWon = false;

    public string username;
    public int curPlayers = 0;
    public bool gameStart = false;
    public Text timerText;
    public List<GameObject> spawnPoints = new List<GameObject>();
    private float timer = 3;
    public string MapName = "SampleScene";

    void Start()
    {
        foreach (Transform child in gameObject.transform)
        {
            spawnPoints.Add(child.gameObject);
        }
    }
    public void ChangeMap()
    {
        SceneManager.LoadScene(MapName);             
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
    public void Disconnect()
    {
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.Disconnect();
        SceneManager.LoadScene(0);
    }
    public void SetWinner(string s)
    {
        photonView.RPC("HasWon", RpcTarget.All, s);
    }
    [PunRPC]
    void HasWon(string s)
    {
        WinnerPannel.SetActive(true);
        WinText.text = s + " Has Won The Game";
        aPlayerHasWon = true;
    }
    private void FixedUpdate()
    {
        connectState.text ="Connection: " + PhotonNetwork.NetworkClientState;
      //  PhotonNetwork.connec     
    }

    public void ConnectToMaster()
    {
      //  PhotonNetwork.connectionstateDetailed
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected To Master");
        PhotonNetwork.AutomaticallySyncScene = true;

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

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        RoomOptions rm = new RoomOptions
        {
            MaxPlayers = 2,
            IsVisible = true
        };
        int rndID = Random.Range(0, 3000);
        PhotonNetwork.CreateRoom("Default: " + rndID, rm, TypedLobby.Default);
    }
    public virtual void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(curPlayers);
            stream.SendNext(aPlayerHasWon);
        }
        else if (stream.IsReading)
        {
            curPlayers = (int)stream.ReceiveNext();
            aPlayerHasWon = (bool)stream.ReceiveNext();
        }
    }
    public override void OnJoinedRoom()
    {
        
        photonView.RPC("AddPlayerCount", RpcTarget.All);
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
