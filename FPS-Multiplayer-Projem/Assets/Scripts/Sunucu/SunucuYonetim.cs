using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SunucuYonetim : MonoBehaviourPunCallbacks
{
    private static SunucuYonetim instance;
    public static SunucuYonetim Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<SunucuYonetim>();
            }
            return instance;
        }
    }
    private UIMenager uIMenager;


    [Header("Player List Obje ile ilgili işlemler")]
    [SerializeField] private GameObject playerListPrefab;
    [SerializeField] private GameObject playerListParent;
    private Dictionary<int,GameObject> playerList = new Dictionary<int, GameObject>();
    private ExitGames.Client.Photon.Hashtable playerProps = new ExitGames.Client.Photon.Hashtable();
    private PhotonView PV;
    private SaveSystem saveSystem;

    [Space]
    [Space]

    [Header("Friend List Obje ile ilgili işlemler")]
    [SerializeField] private GameObject friendListPrefab;
    [SerializeField] private GameObject friendListContent;
    private Dictionary<int,GameObject> friendList = new Dictionary<int, GameObject>();
    private List<string> friendPlayer = new List<string>();
    public List<string> FrienPlayer { get { return friendPlayer; } }

    [Space]
    [Space]

    [Header("Friend Objesi ile ilgili işlemler")]
    [SerializeField] private GameObject friendPrefab;
    [SerializeField] private GameObject friendContent;
    private Dictionary<int,GameObject> friend = new Dictionary<int, GameObject>();
    private List<string> friends = new List<string>();
    public List<string> Friends { get {return friends;}} 
    [Space]
    [Space]

    public Button button;
    private GameMode gameMode;

    private string roomName;
    private int playerReadyCount = 0;

    private bool normalRoom = false;
    public bool NormalRoom { get { return normalRoom; }  set { normalRoom = value; } }
    private bool friendRoom = false;
    public bool FriendRoom { get { return friendRoom;} set { friendRoom = value; } }

    public bool isConnected = false;
    private bool odaKurdu = false;
    public bool OdaKurdu { get { return odaKurdu;} set { odaKurdu = value;}}
    private bool randomOdaKurdu = false;
    public bool RandomOdaKurdu { get { return randomOdaKurdu;} set { randomOdaKurdu = value;}}
    private void Awake()
    {
        uIMenager = UIMenager.Instance;
        PV = GetComponent<PhotonView>();
        saveSystem = SaveSystem.Instance;

        int friendCount = (int)saveSystem.PlayerPrefsDataLoad("friendCount","int");

        for (int i = 0; i < friendCount; i++)
        {
            int friendNumber = i+1;
            string friendName = (string)saveSystem.GetFriendPlayer(friendNumber); 
            friendPlayer.Add(friendName);
        }

    }

    private void Start()
    {
        uIMenager.friendPlayerNickName_Text.gameObject.SetActive(true);
        foreach (string friend in friendPlayer)
        {
        }
    }

    public void GetFriend()
    {
        FriendData friendData = BinarySaveSystem.FriendDataLoad(this);

        friends = friendData.Friends;

        
    }

    public override void OnConnectedToMaster()
    {
        print("server'a bağlandi");
        StopCoroutine(uIMenager.ConnetingAnimation());
        if(isConnected)
        {
            if(saveSystem.PlayerPrefsDataQuery("icon") && saveSystem.PlayerPrefsDataQuery("color") && saveSystem.PlayerPrefsDataQuery("playerName"))
            {
                string menuPanelName = uIMenager.Menu_Panel.name;
                uIMenager.SetActiveUIObject(menuPanelName); 
            }
            else
            {
                string playerPropsPanelName = uIMenager.PlayerProps_Panel.name;
                uIMenager.SetActiveUIObject(playerPropsPanelName); 

            }
        }
        else if(!isConnected)
        {
            string menuPanelName = uIMenager.RandomOda_Panel.name;
            uIMenager.SetActiveUIObject(menuPanelName); 
        }
        else if(PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue("isPlayerReady",out object isPlayerReady))
        {
            if(!(bool)isPlayerReady)
            {
                odaKurdu = false;
                randomOdaKurdu = false;
                friendRoom = true;
                string menuPanelName = uIMenager.Menu_Panel.name;
                uIMenager.SetActiveUIObject(menuPanelName); 

            }
        }
        
        
        PhotonNetwork.JoinLobby();

        
    }

    public override void OnJoinedLobby()
    {
        if(isConnected)
        {
            friendRoom = true;
        }
        else
        {
            friendRoom = false;
        }

        if(friendRoom)
        {
            print("friendRoom : "+friendRoom);
            string roomName = "friendRoom";
            string[] roomPropsString = new []{"odaTuru"};
            ExitGames.Client.Photon.Hashtable roomProps = new ExitGames.Client.Photon.Hashtable()
            {
                {"odaTuru","friend"}
            };

            RoomOptions roomOptions = new RoomOptions()
            {
                IsOpen = true,
                IsVisible = false,
                CustomRoomProperties = roomProps,
                CustomRoomPropertiesForLobby = roomPropsString
            };

            PhotonNetwork.JoinOrCreateRoom(roomName,roomOptions,TypedLobby.Default,null);
        }
        else if(odaKurdu)
        {
            if(!PhotonNetwork.InRoom)
            {
                string roomName = uIMenager.OdaAdi_InputField.text;
                CreateRoom(saveSystem.GetRoomMod(),roomName);
            }
        }

        else if(randomOdaKurdu)
        {
            if(!PhotonNetwork.InRoom)
            {
                string roomName = uIMenager.OdaAdi_InputField.text;
                CreateRandomRoom(saveSystem.GetRoomMod());
            }
        }
        
        
    }

    public void CreateRoom(GameMode mod,string odaAdi)
    {
        if(PhotonNetwork.IsConnectedAndReady)
        {
            normalRoom = true;
            gameMode = mod;
            roomName = odaAdi;
            

            string[] roomPropsString = new string[] { "gameMode" };

            ExitGames.Client.Photon.Hashtable roomProps = new ExitGames.Client.Photon.Hashtable()
                {
                    {"gameMode",gameMode}
                };

            RoomOptions roomOptions = new RoomOptions()
            {
                MaxPlayers = 3,
                CustomRoomProperties = roomProps,
                CustomRoomPropertiesForLobby = roomPropsString
            };
            PhotonNetwork.JoinOrCreateRoom(roomName, roomOptions, TypedLobby.Default, null);

            
            string randomOdaPanelName = uIMenager.RandomOda_Panel.name;
            uIMenager.SetActiveUIObject(randomOdaPanelName);
        }
        
    }

    public void CreateRandomRoom(GameMode mod)
    {
        if(PhotonNetwork.IsConnectedAndReady)
        {
            normalRoom = true;
            gameMode = mod;
            ExitGames.Client.Photon.Hashtable roomProps = new ExitGames.Client.Photon.Hashtable()
            {
                {"gameMode",gameMode}
            };

            PhotonNetwork.JoinRandomRoom(roomProps, 3);
        }
    }


    public override void OnJoinedRoom()
    {
        if(normalRoom)
        {
            print("normalRoom : " + normalRoom);
            if(playerList == null)
            {
                playerList = new Dictionary<int, GameObject>();
            }

            string randomOdaPanelName = uIMenager.RandomOda_Panel.name;
            uIMenager.SetActiveUIObject(randomOdaPanelName);

            foreach (Player player in PhotonNetwork.PlayerList)
            {
                GameObject playerListObje = PlayerListOlustur(player.ActorNumber,player.NickName,player);
            
                playerList.Add(player.ActorNumber,playerListObje);
                    
                string findingMatchPanelName = uIMenager.FindingMatch_Panel.name;
                    
                uIMenager.SetActiveUIObject(findingMatchPanelName); 
                    
                //FindingMatchControl.Instance.StartMatchFindingButton_Method();
            }
        }
        if(friendRoom)
        {
            

            CreateFrinendsList();

        }
        
    }
    
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        print("OnJoinRandomFailed");
        if(normalRoom)
        {
            string randomRoomName = "Oda-" + Random.Range(1,100);

            string[] roomPropStrings = new string[]{"gameMode"};

            ExitGames.Client.Photon.Hashtable roomProps = new ExitGames.Client.Photon.Hashtable()
            {
                {"gameMode",gameMode}
            };
            RoomOptions roomOptions = new RoomOptions()
            {
                MaxPlayers = 3,
                CustomRoomProperties = roomProps,
                CustomRoomPropertiesForLobby = roomPropStrings
            };

            PhotonNetwork.JoinOrCreateRoom(randomRoomName,roomOptions,TypedLobby.Default,null);

            string randomOdaPanelName = uIMenager.RandomOda_Panel.name;
            uIMenager.SetActiveUIObject(randomOdaPanelName);
        }
       
        
    }

    

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        if(normalRoom)
        {
            GameObject playerListObje =  PlayerListOlustur(newPlayer.ActorNumber,newPlayer.NickName,newPlayer);

            playerList.Add(newPlayer.ActorNumber,playerListObje);
        }
        if(friendRoom)
        {   
            
            if(PhotonNetwork.NickName != newPlayer.NickName && newPlayer.NickName != saveSystem.GetFriendPlayer(newPlayer.ActorNumber)  && !friendPlayer.Contains(newPlayer.NickName))
            {
                GameObject friendListObject =  FriendListOlustur();
                
                print(newPlayer.UserId);
                
                friendListObject.GetComponent<FriendListControl>().FriendListInitialize(newPlayer);

                friendList.Add(newPlayer.ActorNumber,friendListObject);
            }
        }
        
        
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        print("OnPlayerLeftRoom");
        if(normalRoom)
        {
            print("OnPlayerLeftRoom : " + normalRoom);
            Destroy(playerList[otherPlayer.ActorNumber].gameObject);
            playerList.Remove(otherPlayer.ActorNumber);

            PV.RPC("Method2",RpcTarget.AllViaServer,null);
        }
        if(friendRoom)
        {
            print("OnPlayerLeftRoom : " + normalRoom);
            if(friendList.ContainsKey(otherPlayer.ActorNumber))
            {
                Destroy(friendList[otherPlayer.ActorNumber].gameObject);
                friendList.Remove(otherPlayer.ActorNumber);

            }
        }
        
        isConnected =true;
        normalRoom = false;
        
        if(uIMenager.FindingMatch_Panel.activeSelf)
        {
            FindingMatchControl.Instance.UpdateButtons();
        }
    }

    public override void OnLeftRoom()
    {
        if(normalRoom)
        {
            foreach (GameObject entry in playerList.Values)
            {
                Destroy(entry);
            }
            playerList.Clear();
            playerList = null;
        }
        if(friendRoom)
        {
            foreach (GameObject entry in friendList.Values)
            {
                if(entry != null)
                {
                    Destroy(entry);
                }
            }
            friendList.Clear();
            friendList = null;

        }

    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        if(targetPlayer.CustomProperties.TryGetValue("isPlayerReady",out object isPlayerReady))
        {
            if((bool)isPlayerReady)
            {
                playerReadyCount++;
            }
            else
            {
                playerReadyCount = 0;
            }

            if(playerReadyCount == PhotonNetwork.CurrentRoom.PlayerCount)
            {
                PV.RPC("IntroScene",RpcTarget.AllViaServer,1);
                for (int i = 0; i < PhotonNetwork.CurrentRoom.Players.Count; i++)
                {
                    ExitGames.Client.Photon.Hashtable props = new ExitGames.Client.Photon.Hashtable()
                    {
                        {"isPlayerReady",false}
                    };

                    PhotonNetwork.CurrentRoom.Players[i+1].SetCustomProperties(props);
                }
            }
        }
        
    }

    public override void OnFriendListUpdate(List<FriendInfo> friendList)
    {
        
    }

    public void CreateFrinendsList()
    {
        if(PhotonNetwork.InRoom)
        {
            if (friendList == null)
            {
                friendList = new Dictionary<int, GameObject>();
            }
            foreach (Player _friendPlayer in PhotonNetwork.PlayerList)
            {
                if (PhotonNetwork.NickName != _friendPlayer.NickName && _friendPlayer.NickName != saveSystem.GetFriendPlayer(_friendPlayer.ActorNumber) && !friendPlayer.Contains(_friendPlayer.NickName))
                {
                    
                    GameObject friendListObject = FriendListOlustur();

                    friendListObject.GetComponent<FriendListControl>().FriendListInitialize(_friendPlayer);


                    friendList.Add(_friendPlayer.ActorNumber, friendListObject);
                }
                

            }
        }
    }

    public void CreatFriendObject(Player _friend,string friendState,bool value)
    {
        if(value)
        {
            if(!friendPlayer.Contains(_friend.NickName))
            {
                if (_friend.NickName == saveSystem.GetFriendPlayer(_friend.ActorNumber))
                {

                    GameObject friendObject = Instantiate(friendPrefab);

                    _friend.CustomProperties.TryGetValue("icon",out object friendIconIndex);
                    friendObject.GetComponent<FriendListControl>().FriendObjectInitialize((int)friendIconIndex,_friend.NickName,friendState);

                    friendObject.transform.SetParent(friendContent.transform);
                    friendObject.transform.localScale = Vector3.one;

                    friend.Add(_friend.ActorNumber, friendObject);

                }
            }
        }
        else
        {
            if (_friend.NickName == saveSystem.GetFriendPlayer(_friend.ActorNumber))
            {

                GameObject friendObject = Instantiate(friendPrefab);
                
                _friend.CustomProperties.TryGetValue("icon",out object friendIconIndex);
                friendObject.GetComponent<FriendListControl>().FriendObjectInitialize((int)friendIconIndex,_friend.NickName,friendState);

                friendObject.transform.SetParent(friendContent.transform);
                friendObject.transform.localScale = Vector3.one;

                friend.Add(_friend.ActorNumber, friendObject);
            }
        }
    }

    


    private GameObject PlayerListOlustur(int playerId,string playerName,Player player)
    {
        
        GameObject playerList = Instantiate(playerListPrefab);
        playerList.transform.SetParent(playerListParent.transform);
        playerList.transform.localScale = Vector3.one;


        playerList.GetComponent<PlayerListControl>().Initialize(playerId,playerName,player);
        

        return playerList;
    }

    private GameObject FriendListOlustur()
    {

        GameObject friendListObject = Instantiate(friendListPrefab);
        friendListObject.transform.SetParent(friendListContent.transform);
        friendListObject.transform.localScale = Vector3.one;


        

        return friendListObject;
    }  

    public void ConnetingServer(string playerName)
    {
        PhotonNetwork.ConnectUsingSettings();
        string name = playerName;
        

        PhotonNetwork.LocalPlayer.NickName = playerName;
       
        StartCoroutine(uIMenager.ConnetingAnimation());
    }
   

    
    
    
    public void KabulEtButton()
    {
        uIMenager.KarşilaşmaKabulEtButton_Method();
    }

    public void ReddetButton()
    {
        PV.RPC("RPC_ReddetButton",RpcTarget.AllViaServer,null);
    }

    [PunRPC]
    public void RPC_ReddetButton()
    {
        uIMenager.KarşilaşmaReddetButton_Method();
        isConnected =true;
        normalRoom = false;
        PhotonNetwork.LeaveRoom();
    }

    

    [PunRPC]
    private void IntroScene(int sceneIndex)
    {
        PhotonNetwork.LoadLevel(1);
    }


    [PunRPC]
    public void Method2()
    {
        string menuPanelName = uIMenager.Menu_Panel.name;
        uIMenager.SetActiveUIObject(menuPanelName);
        PhotonNetwork.LeaveRoom();

    }    


    public void ExitFriendRoom()
    {
        isConnected = false;
        PhotonNetwork.LeaveRoom();
    }

    public void Refresh()
    {
        PV.RPC("RPC_Refresh",RpcTarget.OthersBuffered);
    }


    [PunRPC]
    private void RPC_Refresh()
    {
        print("RPC_Refresh");
        foreach (GameObject friendObject in friendList.Values)
        {
            Destroy(friendObject,0);
        }
        friendList.Clear();
        friendList = null;

        CreateFrinendsList();
    }

    public void OdaIslemlerRefresh()
    {
        print("Refresh");

        foreach (GameObject friendObject in friendList.Values)
        {
            Destroy(friendObject,0);
        }
        friendList.Clear();
        friendList = null;

        CreateFrinendsList();
    }
    

    
}
