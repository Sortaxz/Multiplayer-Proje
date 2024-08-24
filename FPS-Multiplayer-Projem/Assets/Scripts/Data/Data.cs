using System.Collections;
using System.Collections.Generic;
using Photon.Realtime;
using UnityEngine;

[System.Serializable]
public class FriendData
{
    private List<string> friends;
    public List<string> Friends { get { return friends; } }
    public FriendData(SunucuYonetim sunucuYonetim,bool firstRegistration)
    {
        if(firstRegistration)
        {
            friends = new List<string>();
        }
        else
        {
            friends = sunucuYonetim.Friends;
        }
    }

}

