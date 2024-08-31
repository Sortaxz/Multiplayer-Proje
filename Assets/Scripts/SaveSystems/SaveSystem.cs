using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Realtime;
using TMPro;
using UnityEngine;

public class SaveSystem : MonoBehaviour
{
    
    private void Awake() 
    {
        DontDestroyOnLoad(gameObject);    
    }

    public static void PlayerPrefsDataSave(string saveVariableName,object value)
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

    public static object PlayerPrefsDataLoad(string loadVariableName,string returnValue)
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

    public static bool PlayerPrefsDataQuery(string firstDataQueryName )
    {
        return PlayerPrefs.HasKey(firstDataQueryName);
    }
    
    public static void SaveFriend(List<string> friendsList)
    {
        string friends = string.Join(",",friendsList);

        PlayerPrefs.SetString("FriendsList", friends);

        PlayerPrefs.Save();

        print("Arkadaş Listesine eklendi");


    }

    public static List<string> LoadFriedns(string isSave)
    {
        if(PlayerPrefsDataQuery("FriendsList")  && isSave == "friend")
        {
            string friends = PlayerPrefs.GetString("FriendsList");
            
            List<string> friendsList = new List<string>(friends.Split(','));
            
            Debug.Log("Arkadaş listesi yüklendi.");

            return friendsList;
        }
        else
        {
            List<string> friendsList = new List<string>();
            
            print("Kaydedilmiş arkadaş listesi bulunamadi");
            return friendsList;
        }
    }


    public GameMode GetRoomMod()
    {
        GameMode gameMode = GameMode.None;
        switch(PlayerPrefsDataLoad("gameMode","string"))
        {
            case "Dereceli":
                gameMode = GameMode.Dereceli;
            break;
            case "Derecesiz":
                gameMode = GameMode.Derecesiz;
            break;
        }

        return gameMode;
    }

    

}
