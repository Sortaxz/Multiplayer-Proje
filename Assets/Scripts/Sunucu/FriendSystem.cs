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
    private Dictionary<int,GameObject> currentFriends = new Dictionary<int,GameObject>();
    public Dictionary<int,GameObject> CurrentFriends { get { return currentFriends; } }
    private List<string> friendList = new List<string>();

    private List<string> friendIconsIndex = new List<string>();
    
    private string senderFriendPlayer;

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

   

    public void ShowFriend()
    {
       
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
                if(!currentFriends.ContainsKey(i))
                {
                    CreateFriendObject(i);
                }



            }
        }
    }


    public void CurrentFriendState(List<FriendInfo> friendInfos)
    {
        for (int i = 0; i < friendInfos.Count; i++)
        {
            if(currentFriends.ContainsKey(i))
            {
                if(friendIconsIndex[i] != "")
                {
                    int frienIconIndex = int.Parse(friendIconsIndex[i]);

                    currentFriends[i].GetComponent<FriendListControl>().FriendObjectInitialize(friendInfos[i],frienIconIndex);

                }
            }
            else
            {
                continue;
            }
        }
    }


    private void CreateFriendObject(int i)
    {
        GameObject currentFriend = Instantiate(friendPrefab, friendContent.transform);
        currentFriend.transform.localScale = Vector3.one;
        currentFriends.Add(i, currentFriend);
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

    }

    public void OnEvent(EventData photonEvent)
    {
        switch (photonEvent.Code)
        {
            case (byte)CustomEvents.FriendRequest:
                HandleFriendRequest(photonEvent);
                break;
            
            /*case (byte)CustomEvents.RemoveFriend:
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
        
        print("receiverUserId : "+receiverUserId + "senderUserId : "+senderUserId + "eklenen oyuncu : " + PhotonNetwork.LocalPlayer.UserId);
        
        GameObject friendAcceptOrReject_Panel = UIMenager.Instance.FriendshipAnswer((receiverUserId == PhotonNetwork.LocalPlayer.UserId));
        string friendRequestText = $"{senderUserId} sana arkadaşlik isteği atti arkadaş olmak ister misin ?";
        friendAcceptOrReject_Panel.GetComponent<FriendListControl>().FriendRequestInitialize(friendRequestText);
        
       
        senderFriendPlayer = senderUserId;

    }

    public void AddFriend_Method1()
    {
        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            print(senderFriendPlayer);
            if(PhotonNetwork.PlayerList[i].UserId == senderFriendPlayer)
            {
                PhotonNetwork.PlayerList[i].CustomProperties.TryGetValue("icon",out object iconIndex);
                
                int friendIconIndex = (int)iconIndex;

                AddFriend(PhotonNetwork.PlayerList[i].UserId,friendIconIndex.ToString());
            
            }
        }
    }
    
}
