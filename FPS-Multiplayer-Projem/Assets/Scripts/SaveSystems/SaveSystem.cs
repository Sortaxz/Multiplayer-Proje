using System.Collections;
using System.Collections.Generic;
using Photon.Realtime;
using TMPro;
using UnityEngine;

public class SaveSystem : MonoBehaviour
{
    private static SaveSystem instance;
    public static SaveSystem Instance
    {
        get
        {
            if(instance == null)
            {
                instance = FindObjectOfType<SaveSystem>();
            }
            return instance;
        }
    }
    int friendCount = 1;

    private void Awake() 
    {
        DontDestroyOnLoad(gameObject);    
    }

    public void PlayerPrefsDataSave(string saveVariableName,object value)
    {
        if(value is int valueIsInt)
        {
            PlayerPrefs.SetInt(saveVariableName,valueIsInt);    
        }
        else if(value is float valueIsFloat)
        {
            PlayerPrefs.SetFloat(saveVariableName,valueIsFloat);
        }
        else if(value is string valueIsString)
        {
            PlayerPrefs.SetString(saveVariableName,valueIsString);
        }

        
    }

    public object PlayerPrefsDataLoad(string loadVariableName,string returnValue)
    {
        object data = null;
        switch(returnValue)
        {
            case "int":
            data = PlayerPrefs.GetInt(loadVariableName);
            break;

            case "float":
            data = PlayerPrefs.GetFloat(loadVariableName);
            break;

            case "string":
            data = PlayerPrefs.GetString(loadVariableName);
            break;

            default:
            break;
        }

        return data;
    }

    public bool PlayerPrefsDataQuery(string firstDataQueryName )
    {
        return PlayerPrefs.HasKey(firstDataQueryName);
    }
    

    public void SetFriendPlayer(Player friendPlayer)
    {
        SunucuYonetim.Instance.FriendPlayer.Add(friendPlayer);
        PlayerPrefsDataSave(friendPlayer.ActorNumber.ToString(),friendPlayer.NickName);
        
    }
    public void GetFriendsPlayer()
    {
        //return (string)PlayerPrefsDataLoad(friendPlayer.ActorNumber.ToString(),"string");
        foreach (Player friendPlayer in SunucuYonetim.Instance.FriendPlayer)
        {
            string player = (string)PlayerPrefsDataLoad(friendPlayer.ActorNumber.ToString(),"string");
            UIMenager.Instance.friendPlayerNickName_Text.text = (string)PlayerPrefsDataLoad(friendPlayer.ActorNumber.ToString(),"string");
        }
    }

    public string GetFriendPlayer(Player friendPlayer)
    {
        return (string)PlayerPrefsDataLoad(friendPlayer.ActorNumber.ToString(),"string");
        
    }
}
