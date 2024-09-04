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
    
    public static void SaveFriend(List<string> friendsList,List<string> friendsIconList)
    {

        foreach (var item in friendsList)
        {
            print(item);
        }

        string friends = string.Join(",",friendsList);

        PlayerPrefs.SetString("FriendsList", friends);

        string friendsIcon = string.Join(",",friendsIconList);

        PlayerPrefs.SetString("FriendsIconList", friendsIcon);
        

        PlayerPrefs.Save();

        print("Arkadaş Listesine eklendi");


    }

    public static List<string> LoadFriedns(string isSave ="")
    {
        if(isSave == "friendsList")
        {
            if(PlayerPrefsDataQuery("FriendsList"))
            {
                string friends = PlayerPrefs.GetString("FriendsList");
                
                List<string> friendsList = new List<string>(friends.Split(','));
                
                for (int i = 0; i < friendsList.Count; i++)
                {
                    if(friendsList[i] == "")
                    {
                        friendsList.Remove(friendsList[i]);
                    }
                }
                
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
        else
        {
            if(PlayerPrefsDataQuery("FriendsIconList"))
            {
                string friendsIcon = PlayerPrefs.GetString("FriendsIconList");
                
                List<string> friendsIconList = new List<string>(friendsIcon.Split(','));
                
                for (int i = 0; i < friendsIconList.Count; i++)
                {
                    if(friendsIconList[i] == "")
                    {
                        friendsIconList.Remove(friendsIconList[i]);
                    }
                }

                Debug.Log("Arkadaş listesi yüklendi.");

                return friendsIconList;
            }
            else 
            {
                List<string> friendsIconList = new List<string>();
                
                print("Kaydedilmiş arkadaş listesi bulunamadi");
                return friendsIconList;
            }
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
