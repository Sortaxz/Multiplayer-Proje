using System.Collections;
using System.Collections.Generic;
using Photon.Realtime;

[System.Serializable]
public class FriendData
{
    public List<Player> friend = new List<Player>();
    public FriendData(FriendSystem friendSystem,bool ilkMi)
    {
        if(ilkMi)
        {
            friend = new List<Player>();
        }
        else
        {
            friend = friendSystem.Friends;
            
        }
    }
}

