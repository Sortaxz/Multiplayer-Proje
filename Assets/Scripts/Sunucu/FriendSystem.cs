using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class FriendSystem : MonoBehaviour,IOnEventCallback
{
    private static FriendSystem instance;
    public static FriendSystem Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<FriendSystem>();
            }
            return instance;
        }
    }


    [Header("Current Friend Objesi ile ilgili işlemler")]
    [SerializeField] private GameObject friendPrefab;
    [SerializeField] private GameObject friendContent;
    private Dictionary<string,GameObject> currentFriends = new Dictionary<string,GameObject>();
    public Dictionary<string,GameObject> CurrentFriends { get { return currentFriends; } }
    private List<string> friendList = new List<string>();
    public List<string> FriendList { get { return friendList; } }

    private List<string> friendIconsIndex = new List<string>();
    private List<Player> friends= new List<Player>();
    public List<Player> Friends { get { return friends;}}
    private Player unFriend;

    private string senderFriendPlayer;

    private void Update() 
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
           print(friends.Count);
           foreach (var item in friends)
           {
                print(item.UserId);
           }
        }    
    }

    private void OnEnable()
    {
        PhotonNetwork.AddCallbackTarget(this);
    }

    private void OnDisable()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
    }

    public void LoadFriendDate()
    {
        friendList = SaveSystem.LoadFriedns("friendsList");
        friendIconsIndex = SaveSystem.LoadFriedns();
        print("friendList.Count : " + friendList.Count + "friendIconsIndex.Count : " + friendIconsIndex.Count);
    }


    public void AddFriend(string friendName,string friendIconIndex)
    {
        if(!friendList.Contains(friendName))
        {
            friendList.Add(friendName);
            friendIconsIndex.Add(friendIconIndex);

            SaveSystem.SaveFriend(friendList,friendIconsIndex);
        }
        else
        {
            print("Ekliceğiniz kişi zaten arkadaşiniz...");
        }
    }

   

    public IEnumerator CheckFriends()
    {
        while(true)
        {

            if(friendList.Count > 0)
            {
                if(!PhotonNetwork.InRoom && PhotonNetwork.IsConnectedAndReady && PhotonNetwork.InLobby)
                {
                    PhotonNetwork.FindFriends(friendList.ToArray());
                }
            }
            else
            {
                print("Arkadaş listeniz boş");
            }

            yield return new WaitForSeconds(1);

        }
    }

   
    public void CreatCurrentFriend()
    {
        if(UIMenager.Instance.Menu_Panel.activeSelf)
        {
            for (int i = 0; i < friendList.Count; i++)
            {
                if(!currentFriends.ContainsKey(friendList[i]))
                {
                    CreateFriendObject(friendList[i]);
                }



            }
        }
    }


    public void CurrentFriendState(List<FriendInfo> friendInfos)
    {
        for (int i = 0; i < friendInfos.Count; i++)
        {
            if(currentFriends.ContainsKey(friendInfos[i].UserId))
            {
                if(friendIconsIndex[i] != "")
                {
                    int frienIconIndex = int.Parse(friendIconsIndex[i]);

                    currentFriends[friendInfos[i].UserId].GetComponent<FriendListControl>().FriendObjectInitialize(friendInfos[i],frienIconIndex);

                }
            }
            else
            {
                continue;
            }
        }
    }


    private void CreateFriendObject(string friendNickName)
    {
        GameObject currentFriend = Instantiate(friendPrefab, friendContent.transform);
        currentFriend.transform.localScale = Vector3.one;
        currentFriends.Add(friendNickName, currentFriend);
    }


    public void CurrentFriendsList()
    {
        UIMenager.Instance.CreateCurrentFriendList(friendList,friendIconsIndex);
    }

    public void SendFriendRequest(Player friend)
    {
        print(friend.UserId);
        object[] content = new object[]{PhotonNetwork.LocalPlayer.UserId,friend.UserId};
        
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions();
        raiseEventOptions.TargetActors = new int[]{friend.ActorNumber};

        PhotonNetwork.RaiseEvent((byte)CustomEvents.FriendRequest, content,raiseEventOptions,SendOptions.SendReliable);
        print("arkadaşlik isteği gönderildi");
        
        friends.Add(friend);

    }

    public void OnEvent(EventData photonEvent)
    {
        switch (photonEvent.Code)
        {
            case (byte)CustomEvents.FriendRequest:
                HandleFriendRequest(photonEvent);
                break;
            /*
            case (byte)CustomEvents.RemoveFriend:
                HandleRemoveFriend(photonEvent);
                break;
            */
        }
    }
    
    private void HandleFriendRequest(EventData photonEvent)
    {
        object[] data = (object[])photonEvent.CustomData;
        string senderUserId = data[0].ToString();
        string receiverUserId = data[1].ToString();
        
        if(!friendList.Contains(senderUserId)) 
        {
            GameObject friendAcceptOrReject_Panel = UIMenager.Instance.FriendshipAnswer((receiverUserId == PhotonNetwork.LocalPlayer.UserId));
            string friendRequestText = $"{senderUserId} sana arkadaşlik isteği atti arkadaş olmak ister misin ?";
            friendAcceptOrReject_Panel.GetComponent<FriendListControl>().FriendRequestInitialize(friendRequestText);

        }       
        
       
        senderFriendPlayer = senderUserId;

    }

    

    public void AddFriend_Method1()
    {
        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            if(PhotonNetwork.PlayerList[i].UserId == senderFriendPlayer)
            {
                PhotonNetwork.PlayerList[i].CustomProperties.TryGetValue("icon",out object iconIndex);
                
                int friendIconIndex = (int)iconIndex;

                AddFriend(PhotonNetwork.PlayerList[i].UserId,friendIconIndex.ToString());

                SunucuYonetim.Instance.PlayerList[senderFriendPlayer].GetComponent<PlayerListControl>().CloseAddFrienButtonActive();
            }
        }
    }

    public void UnFriend(string unfriendNickName,string unfriendIconIndex)
    {
        RemoveFriend(unfriendNickName, unfriendIconIndex);

        
    }

    private void RemoveFriend(string unfriendNickName, string unfriendIconIndex)
    {
        friendList.Remove(unfriendNickName);
        friendIconsIndex.Remove(unfriendIconIndex);

    

        Destroy(currentFriends[unfriendNickName]);
        currentFriends.Remove(unfriendNickName);

        SaveSystem.SaveFriend(friendList, friendIconsIndex);
    }


    [PunRPC]
    public void RPC_ClosePlayerAddButtonActive()
    {
        
    }
}
