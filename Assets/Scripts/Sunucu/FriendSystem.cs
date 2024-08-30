using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class FriendSystem : MonoBehaviourPunCallbacks
{
    LoadBalancingClient loadBalancingClient;
    private List<string> friendsList = new List<string>();

    void Start()
    {
        PhotonNetwork.AddCallbackTarget(this);

        // Arkadaşları ekleme (örnek)
        
        if(PhotonNetwork.IsConnectedAndReady)
        {
            PhotonNetwork.FindFriends(friendsList.ToArray());
        }
    }
    
    public override void OnConnectedToMaster()
    {
        PhotonNetwork.FindFriends(friendsList.ToArray());
    }


    public override void OnFriendListUpdate(List<FriendInfo> friendList)
    {
        foreach (var friend in friendList)
        {
            if (friend.IsOnline && friendsList.Contains(friend.UserId))
            {
                Debug.Log(friend.UserId + " is online and in room: " + friend.Room);
            }
            else
            {
                Debug.Log(friend.UserId + " is offline.");
            }
        }
    }

    void OnDestroy()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
    }


}
