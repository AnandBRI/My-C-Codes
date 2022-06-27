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

    public bool IsAuthenticated = false;
    LoginWithPlayFabRequest loginRequest;
 
    void Start()
    {
        email.gameObject.SetActive(false);
    }
    void Update()
    {
        
    }
    public void Login()           
    {
        loginRequest = new LoginWithPlayFabRequest();
        loginRequest.Username = user.text;
        loginRequest.Password = pass.text;
        PlayFabClientAPI.LoginWithPlayFab(loginRequest, result =>
        {
            message.text = "Welcome " + user.text + ", Conecting..";
            IsAuthenticated = true;
            mp.username = user.text;
            mp.ConnectToMaster();
            Debug.Log("Logged IN");
        }, error => 
        {
            IsAuthenticated = false;
     
            email.gameObject.SetActive(true);
            Debug.Log(error.ErrorMessage);
        }, null);
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
