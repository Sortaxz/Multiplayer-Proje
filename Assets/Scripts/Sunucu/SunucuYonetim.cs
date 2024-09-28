using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
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
    private Dictionary<string,GameObject> playerList = new Dictionary<string, GameObject>();
    public Dictionary<string,GameObject> PlayerList { get { return playerList; } }
    private ExitGames.Client.Photon.Hashtable playerProps = new ExitGames.Client.Photon.Hashtable();
    private PhotonView PV;

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


    private bool playersReady = false;
    private void Awake()
    {
        uIMenager = UIMenager.Instance;
        PV = GetComponent<PhotonView>();
    }

    

    public override void OnConnectedToMaster()
    {
        StopCoroutine(uIMenager.ConnetingAnimation());
        
        if(SaveSystem.PlayerPrefsDataQuery("icon") && SaveSystem.PlayerPrefsDataQuery("color") && SaveSystem.PlayerPrefsDataQuery("playerName"))
        {
            string menuPanelName = uIMenager.Menu_Panel.name;
            uIMenager.SetActiveUIObject(menuPanelName); 
        }
        else
        {
            string playerPropsPanelName = uIMenager.PlayerProps_Panel.name;
            uIMenager.SetActiveUIObject(playerPropsPanelName); 

        }
        
        FriendSystem.Instance.CreatCurrentFriend();

        PhotonNetwork.JoinLobby();

        
    }

    public override void OnJoinedLobby()
    {
        
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
                MaxPlayers = 2,
                CustomRoomProperties = roomProps,
                CustomRoomPropertiesForLobby = roomPropsString,
                PublishUserId = true
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
            PhotonNetwork.JoinRandomRoom(roomProps, 2);
        }
    }


    public override void OnJoinedRoom()
    {
        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            if(PhotonNetwork.PlayerList[i].CustomProperties.TryGetValue("inGame",out object inGame))
            {
                if((bool)inGame)
                {
                    playersReady = true;
                }
            }
        }
        if(playersReady)
        {
            print("Oyun sahnesinde bir oyuncu var");
            PhotonNetwork.LoadLevel(1);
        }
        else
        {
            if(playerList == null)
            {
                playerList = new Dictionary<string, GameObject>();
            }

            string randomOdaPanelName = uIMenager.RandomOda_Panel.name;
            uIMenager.SetActiveUIObject(randomOdaPanelName);

            foreach (Player player in PhotonNetwork.PlayerList)
            {
            
                GameObject playerListObje = PlayerListOlustur(player.ActorNumber,player.NickName,player);
                
                playerList.Add(player.NickName,playerListObje);
                        
                string findingMatchPanelName = uIMenager.FindingMatch_Panel.name;
                        
                uIMenager.SetActiveUIObject(findingMatchPanelName); 
                        
            }

        }

    }
    
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        string randomRoomName = "Oda-" + Random.Range(1, 100);

        string[] roomPropStrings = new string[] { "gameMode" };

        ExitGames.Client.Photon.Hashtable roomProps = new ExitGames.Client.Photon.Hashtable()
        {
            {"gameMode",gameMode}
        };
        RoomOptions roomOptions = new RoomOptions()
        {
            MaxPlayers = 2,
            CustomRoomProperties = roomProps,
            CustomRoomPropertiesForLobby = roomPropStrings,
            PublishUserId = true
        };

        PhotonNetwork.JoinOrCreateRoom(randomRoomName, roomOptions, TypedLobby.Default, null);

        string randomOdaPanelName = uIMenager.RandomOda_Panel.name;
        uIMenager.SetActiveUIObject(randomOdaPanelName);


    }

    

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {

        GameObject playerListObje =  PlayerListOlustur(newPlayer.ActorNumber,newPlayer.NickName,newPlayer);

        playerList.Add(newPlayer.NickName,playerListObje);

        if(playersReady)
        {
            PhotonNetwork.LoadLevel(1);
        }
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Destroy(playerList[otherPlayer.NickName].gameObject);
        playerList.Remove(otherPlayer.NickName);

        PV.RPC("Method2",RpcTarget.AllViaServer,null);
        
        
        if(uIMenager.FindingMatch_Panel.activeSelf)
        {
            FindingMatchControl.Instance.UpdateButtons();
        }

    }

    public override void OnLeftRoom()
    {
        foreach (GameObject entry in playerList.Values)
        {
            Destroy(entry);
        }

        

        playerList.Clear();
        playerList = null;
        

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
                        {"isPlayerReady",false},
                        {"inGame",true}
                    };

                    PhotonNetwork.CurrentRoom.Players[i+1].SetCustomProperties(props);
                }
            }
        }
        
    }

    

    public override void OnFriendListUpdate(List<FriendInfo> friendList)
    {
        if(friendList.Count > 0 )
        {
            FriendSystem.Instance.CurrentFriendState(friendList);
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




    public void ConnetingServer(string playerName)
    {

        PhotonNetwork.LocalPlayer.NickName =playerName;
        
        AuthenticationValues AuthenticationValues = new AuthenticationValues()
        {
            UserId = PhotonNetwork.LocalPlayer.NickName
        };
        
        PhotonNetwork.AuthValues = AuthenticationValues;

        PhotonNetwork.ConnectUsingSettings();
        

       
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
        StopAllCoroutines();
    }


    [PunRPC]
    public void Method2()
    {
        string menuPanelName = uIMenager.Menu_Panel.name;
        uIMenager.SetActiveUIObject(menuPanelName);
        
        if(PhotonNetwork.InRoom)
        {
            PhotonNetwork.LeaveRoom();
        }
    }    
    
    public void LeftRoom()
    {
        PV.RPC("Method2",RpcTarget.AllViaServer,null);
    }
    
    public bool ServerControl()
    {
        return PhotonNetwork.IsConnectedAndReady;
    }
   
}

