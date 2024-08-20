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

    [Header("Frien List Obje ile ilgili işlemler")]
    [SerializeField] private GameObject friendListPrefab;
    [SerializeField] private GameObject friendListContent;
    private Dictionary<int,GameObject> friendList = new Dictionary<int, GameObject>();
    private List<Player> friendPlayer = new List<Player>();
    public List<Player> FriendPlayer { get { return friendPlayer; } }

    [Space]
    [Space]
    public Button button;
    private GameMode gameMode;

    private string roomName;
    private int playerReadyCount = 0;

    private bool normalRoom = false;
    public bool NormalRoom { get { return normalRoom; }  set { normalRoom = value; } }
    private bool friendRoom = false;

    public bool isConnected = false;

    private void Awake() 
    {
        uIMenager = UIMenager.Instance;
        PV = GetComponent<PhotonView>();
        saveSystem = SaveSystem.Instance;
    }



    public override void OnConnectedToMaster()
    {
        print("server'a bağlandi");
        StopCoroutine(uIMenager.ConnetingAnimation());

        if(saveSystem.PlayerPrefsDataQuery("icon") && saveSystem.PlayerPrefsDataQuery("color") && saveSystem.PlayerPrefsDataQuery("playerName"))
        {
            string menuPanelName = uIMenager.Menu_Panel.name;
            uIMenager.SetActiveUIObject(menuPanelName); 
            uIMenager.odaKurdu = true;
        }
        else
        {
            string playerPropsPanelName = uIMenager.PlayerProps_Panel.name;
            uIMenager.SetActiveUIObject(playerPropsPanelName); 

        }
        
        PhotonNetwork.JoinLobby();

        
    }

    public override void OnJoinedLobby()
    {

    }

    public void CreateRoom(GameMode mod)
    {
        normalRoom = true;
        gameMode = mod;
        if (uIMenager.RoomName != null)
        {
            roomName = uIMenager.RoomName;
        }

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

    public void CreateRandomRoom(GameMode mod)
    {
        normalRoom = true;
        gameMode = mod;
        ExitGames.Client.Photon.Hashtable roomProps = new ExitGames.Client.Photon.Hashtable()
        {
            {"gameMode",gameMode}
        };

        PhotonNetwork.JoinRandomRoom(roomProps, 3);
    }


    public override void OnJoinedRoom()
    {
        /*
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
                
            FindingMatchControl.Instance.StartMatchFindingButton_Method();
        }

        */

        if(normalRoom)
        {
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
                    
                uIMenager.SetActiveUIObject(findingMatchPanelName); // düzeltilecek
                    
                FindingMatchControl.Instance.StartMatchFindingButton_Method();
            }
        }
        if(friendRoom)
        {
            if(friendList == null)
            {
                friendList = new Dictionary<int, GameObject>();
            }


            string friendPanelName = uIMenager.Arakadasİslem_Panel.name; 
            uIMenager.SetActiveUIObject(friendPanelName);

           
            foreach (Player friendPlayer in PhotonNetwork.PlayerList)
            {
                if(PhotonNetwork.NickName != friendPlayer.NickName)
                {
                    GameObject friendListObject = FriendListOlustur();

                    friendListObject.GetComponent<FriendListControl>().Initialize(friendPlayer);


                    friendList.Add(friendPlayer.ActorNumber,friendListObject);
                }
                
            }

            StartCoroutine(OlanArkadaslarYokEt());
        }
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        print("OnJoinRoomFailed");
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

            if(PhotonNetwork.NickName != newPlayer.NickName )
            {
                GameObject friendListObject =  FriendListOlustur();

                friendListObject.GetComponent<FriendListControl>().Initialize(newPlayer);
            
                friendList.Add(newPlayer.ActorNumber,friendListObject);
            }

            StartCoroutine(OlanArkadaslarYokEt());


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

        /*
        else
        {
            print("OnPlayerLeftRoom : " + normalRoom);
            Destroy(friendList[otherPlayer.ActorNumber].gameObject);
            friendList.Remove(otherPlayer.ActorNumber);
        }
        */
        
        
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
        else
        {
            foreach (GameObject entry in friendList.Values)
            {
                Destroy(entry);
            }
            friendList.Clear();
            friendList = null;
        }

    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        if(changedProps.TryGetValue("isPlayerReady",out object _isPlayerReady))
        {
            print(_isPlayerReady);
        }
        
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



    public void EnterFriendRoom()
    {
        if(!PhotonNetwork.InRoom)
        {
            friendRoom = true;
            PhotonNetwork.JoinRandomOrCreateRoom();
        }
    }

    public IEnumerator OlanArkadaslarYokEt()
    {
        while(true)
        {
            yield return null;

            foreach (Player friend in PhotonNetwork.PlayerList)
            {
                string friendName = saveSystem.GetFriendPlayer(friend);

                if(friend.NickName == friendName)
                {
                    if (friendList.ContainsKey(friend.ActorNumber))
                    {
                        Destroy(friendList[friend.ActorNumber].gameObject);
                        friendList.Remove(friend.ActorNumber);
                    }
                }
            }
        }
    }
}
