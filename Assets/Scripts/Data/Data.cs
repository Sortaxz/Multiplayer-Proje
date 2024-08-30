using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class FriendData
{
    private List<string> friends;
    public List<string> Friends { get { return friends; } set { friends = value; } }
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

