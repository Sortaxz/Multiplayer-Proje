using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
    private List<string> currentFriendNumber = new List<string>();
    private List<string> friendIconsIndex = new List<string>();
    private List<Player> friends= new List<Player>();
    public List<Player> Friends { get { return friends;} set { friends = value; } }
    private Player senderPlayer;
    private Player receiverPlayer;
    private string senderFriendPlayer;
    private string receiverFriendPlayer;

    [SerializeField] private Image Image;
    [SerializeField] private TextMeshProUGUI text;

    
    private void Update() 
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            for (int i = 0; i < currentFriendNumber.Count; i++)
            {
                print($"{i} : "+currentFriendNumber[i]);
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
        friendIconsIndex = SaveSystem.LoadFriedns("friendsIconList");
        currentFriendNumber = SaveSystem.LoadFriedns();
    }


    public void AddFriend(string friendName,string friendIconIndex,int friendActorNumber)
    {
        if(!friendList.Contains(friendName))
        {
            friendList.Add(friendName);
            friendIconsIndex.Add(friendIconIndex);

            currentFriendNumber.Add(friendActorNumber.ToString());
           
            SaveSystem.SaveFriend(friendList,friendIconsIndex,currentFriendNumber);
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
                //print("Arkadaş listeniz boş");
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
        object[] content = new object[]{PhotonNetwork.LocalPlayer.UserId,friend.UserId};
        print($"arkadaşlik isteği gönderen {PhotonNetwork.LocalPlayer.UserId} gönderilen kişi {friend.UserId}");
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions();
        raiseEventOptions.TargetActors = new int[]{friend.ActorNumber};

        PhotonNetwork.RaiseEvent((byte)CustomEvents.FriendRequest, content,raiseEventOptions,SendOptions.SendReliable);
    }

    public void OnEvent(EventData photonEvent)
    {
        switch (photonEvent.Code)
        {
            case (byte)CustomEvents.FriendRequest:
                HandleFriendRequest(photonEvent);
                break;
            case (byte)CustomEvents.FriendAccept:
                HandleAcceptFriend(photonEvent);
                break;
            
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
        receiverFriendPlayer = receiverUserId;

    }
    
    

    public void AddFriend_Method()
    {
        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            if(PhotonNetwork.PlayerList[i].UserId == senderFriendPlayer)
            {
                senderPlayer = PhotonNetwork.PlayerList[i];

                PhotonNetwork.PlayerList[i].CustomProperties.TryGetValue("icon",out object iconIndex);
                
                int friendIconIndex = (int)iconIndex;

                AddFriend(senderPlayer.UserId,friendIconIndex.ToString(),senderPlayer.ActorNumber);


                SunucuYonetim.Instance.PlayerList[senderFriendPlayer].GetComponent<PlayerListControl>().CloseAddFrienButtonActive();
            }
            else if(PhotonNetwork.PlayerList[i].UserId == receiverFriendPlayer)
            {
                receiverPlayer = PhotonNetwork.PlayerList[i];
            }
        }
        Arkadaşlik_Method(senderPlayer,receiverPlayer);
    }

    private void HandleAcceptFriend(EventData photonEvent)
    {
       
        object[] data = (object[])photonEvent.CustomData;
        string receiverUserId = data[0].ToString();
        string senderUserId = data[1].ToString();
        string reciverUserIconIndex = data[2].ToString();
        
        int receiverPlayerActorNumber = (int)data[3];


        AddFriend(receiverUserId,reciverUserIconIndex,receiverPlayerActorNumber);

        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            if(PhotonNetwork.PlayerList[i].UserId == receiverUserId)
            {
                SunucuYonetim.Instance.PlayerList[receiverUserId].GetComponent<PlayerListControl>().CloseAddFrienButtonActive();
            }
        }
    }

    private void Arkadaşlik_Method(Player senderPlayer,Player receiverPlayer)
    {
        receiverPlayer.CustomProperties.TryGetValue("icon",out object reciverIconIndex);
        object[] content = new object[]{receiverPlayer.UserId,senderPlayer.UserId,reciverIconIndex,receiverPlayer.ActorNumber};
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions();
        raiseEventOptions.TargetActors = new int[]{senderPlayer.ActorNumber};
        
        PhotonNetwork.RaiseEvent((byte)CustomEvents.FriendAccept, content,raiseEventOptions,SendOptions.SendReliable);
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

        SaveSystem.SaveFriend(friendList, friendIconsIndex, currentFriendNumber);



    }

    
    
    

}
