using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PlayFab;
using PlayFab.ClientModels;
public class PlayFabAuth : MonoBehaviour
{
    public MPManager mp;
    public InputField user;
    public InputField pass;
    public InputField email;
    public Text message;

    public GetPlayerCombinedInfoRequestParams InfoRequest;
    public int Coins1 = 0;
    public bool IsAuthenticated = false;
    LoginWithPlayFabRequest loginRequest;
 
    void Start()
    {
        email.gameObject.SetActive(false);
    }
    void Update()
    {
       
    }
    public List<PlayerLeaderboardEntry> GetWins()
    {
      GetLeaderboardRequest request = new GetLeaderboardRequest();
        request.MaxResultsCount = 10;
        request.StatisticName = "Wins";

        List<PlayerLeaderboardEntry> temp = new List<PlayerLeaderboardEntry>();
        PlayFabClientAPI.GetLeaderboard(request, result =>
        {
            temp = result.Leaderboard;
        }, error =>
        {

        });
        if (temp.Count < 1)
        {
            return null;
        }
        return temp;
    }
    public void SetWins(int value)
    {
        GetPlayerStatisticsRequest findRequest = new GetPlayerStatisticsRequest();

        List<string> names = new List<string>();
        names.Add("Wins");
        findRequest.StatisticNames = names;
        int temp = 0;
        PlayFabClientAPI.GetPlayerStatistics(findRequest, result =>
         {
            temp = result.Statistics[0].Value;
             UpdatePlayerStatisticsRequest request = new UpdatePlayerStatisticsRequest();

             List<StatisticUpdate> listUpdates = new List<StatisticUpdate>();
             value += temp;
             StatisticUpdate su = new StatisticUpdate();
             su.StatisticName = "Wins";
             su.Value = value;
             listUpdates.Add(su);

             request.Statistics = listUpdates;

             PlayFabClientAPI.UpdatePlayerStatistics(request, result2 =>
             {
                 Debug.Log("Wins Set -->");
             }, error =>
             {

             });
         }, error => 
         {

         });
  
       
    }
    public void Login()           
    {
        loginRequest = new LoginWithPlayFabRequest();
        loginRequest.Username = user.text;
        loginRequest.Password = pass.text;
        loginRequest.InfoRequestParameters = InfoRequest;
        PlayFabClientAPI.LoginWithPlayFab(loginRequest, result =>
        {
            message.text = "Welcome " + user.text + ", Conecting..";
            IsAuthenticated = true;
            mp.username = user.text;

           Coins1 = result.InfoResultPayload.UserVirtualCurrency["BL"];


            mp.ConnectToMaster();
            Debug.Log("Logged IN");
        }, error => 
        {
            IsAuthenticated = false;
     
            email.gameObject.SetActive(true);
            Debug.Log(error.ErrorMessage);
        }, null);
    }
    public void BuyItem(string ItemID)
    {
        PurchaseItemRequest pr = new PurchaseItemRequest();
        pr.CatalogVersion = "Player Abilitys";
        pr.ItemId = ItemID;
        pr.VirtualCurrency = "BL";
        pr.Price = 100;

        GetUserInventoryRequest ir = new GetUserInventoryRequest();

        PlayFabClientAPI.GetUserInventory(ir, result => {
        List<ItemInstance> ii = result.Inventory;
            bool hasItem = false;
            foreach (ItemInstance i in ii)
            {
                if (i.ItemId == ItemID)
                {
                    hasItem = true;
                    Debug.LogWarning("User Already owns " + i.DisplayName);
                }
                else
                {
                    hasItem = false;
                }
            }
            if (hasItem == false)
            {
                PlayFabClientAPI.PurchaseItem(pr, result2 =>
                {
                    Coins1 -= 100;
                    Debug.Log("Betting Item Purchased: " + result2.Items[0].DisplayName);
                }, error =>
                {
                });
            }
        }, error => { }); 
    
    }
    
    public void Register()
    {
        RegisterPlayFabUserRequest request = new RegisterPlayFabUserRequest();
        request.Email = email.text;
        request.Username = user.text;
        request.Password = pass.text;

        PlayFabClientAPI.RegisterPlayFabUser(request, result =>
        {
            message.text = "Your Account Created!";
        }, error =>
        {
            email.gameObject.SetActive(true);
            message.text = "Please Enter Your Email";
        });
    }
    
}
