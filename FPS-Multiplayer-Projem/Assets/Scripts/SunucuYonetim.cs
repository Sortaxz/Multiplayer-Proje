using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Unity.VisualScripting;

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

    private GameMode randomGameMode;
    private GameMode gameMode;

    private string roomName;
    private int playerReadyCount = 0;
    private void Awake() 
    {
        uIMenager = UIMenager.Instance;
        PV = GetComponent<PhotonView>();
        saveSystem = SaveSystem.Instance;
    }

    private string panelName;

    private void Update() {
    }

    public override void OnConnectedToMaster()
    {
        print("Server'a bağlanildi");
        
        StopCoroutine(uIMenager.ConnetingAnimation());
        uIMenager.SetActiveUIObject(uIMenager.Menu_Panel.name); 
    }

    public override void OnJoinedLobby()
    {
        print("Lobiye bağlanildi");
        if(panelName == uIMenager.RandomOdaModSecim_Panel.name)
        {
            

            
            ExitGames.Client.Photon.Hashtable roomProps = new ExitGames.Client.Photon.Hashtable()
            {
                {"gameMode",gameMode}
            };


            PhotonNetwork.JoinRandomRoom(roomProps,2);
        }
        else
        {
            if(uIMenager.RoomName != null)
            {
                roomName = uIMenager.RoomName;
            }

            string[] roomPropsString = new string[]{"gameMode"};

            ExitGames.Client.Photon.Hashtable roomProps = new ExitGames.Client.Photon.Hashtable()
            {
                {"gameMode",gameMode}
            };

            RoomOptions roomOptions = new RoomOptions()
            {
                MaxPlayers = 2,
                CustomRoomProperties = roomProps,
                CustomRoomPropertiesForLobby = roomPropsString
            };

            PhotonNetwork.JoinOrCreateRoom(roomName,roomOptions,TypedLobby.Default,null);
        }

    }
    
    public override void OnJoinedRoom()
    {
        print("Herhangi bir odaya giriş yapildi");
        uIMenager.SetActiveUIObject(uIMenager.RandomOda_Panel.name);
            
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            GameObject playerListObje = PlayerListOlustur(player.ActorNumber,player.NickName,player);
    
            playerList.Add(player.ActorNumber,playerListObje);
                
            uIMenager.SetActiveUIObject(uIMenager.FindingMatch_Panel.name);

        }

    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        

        string randomRoomName = "Oda-" + Random.Range(1,100);

        string[] roomPropStrings = new string[]{"gameMode"};

        ExitGames.Client.Photon.Hashtable roomProps = new ExitGames.Client.Photon.Hashtable()
        {
            {"gameMode",gameMode}
        };
        RoomOptions roomOptions = new RoomOptions()
        {
            MaxPlayers = 2,
            CustomRoomProperties = roomProps,
            CustomRoomPropertiesForLobby = roomPropStrings
        };

        PhotonNetwork.JoinOrCreateRoom(randomRoomName,roomOptions,TypedLobby.Default,null);
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        GameObject playerListObje =  PlayerListOlustur(newPlayer.ActorNumber,newPlayer.NickName,newPlayer);

        playerList.Add(newPlayer.ActorNumber,playerListObje);

        
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Destroy(playerList[otherPlayer.ActorNumber].gameObject);
        playerList.Remove(otherPlayer.ActorNumber);

    }


    private GameObject PlayerListOlustur(int playerId,string playerName,Player player)
    {
        
        GameObject playerList = Instantiate(playerListPrefab);
        playerList.transform.SetParent(playerListParent.transform);
        playerList.transform.localScale = Vector3.one;


        playerList.GetComponent<PlayerListControl>().Initialize(playerId,playerName,player);
        

        return playerList;
    }

    
    public void ConnetingServer()
    {
        PhotonNetwork.ConnectUsingSettings();
        
        if(!saveSystem.PlayerPrefsDataQuery("playerName"))
        {
            string playerName = uIMenager.KullaniciAdi_InputField.text;
            saveSystem.PlayerPrefsDataSave("playerName",playerName);
            PhotonNetwork.LocalPlayer.NickName = playerName;

            
        }
        else
        {
            PhotonNetwork.LocalPlayer.NickName = (string)saveSystem.PlayerPrefsDataLoad("playerName","string");
        }


        StartCoroutine(uIMenager.ConnetingAnimation());
    }

   
    public void ConnectedLobby(string panelName, GameMode mode = GameMode.None)
    {
        this.panelName = panelName;
        PhotonNetwork.JoinLobby();
        gameMode = mode;
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        
        
        
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
        //uIMenager.PV.RPC("KarşilaşmaReddetButton_Method",RpcTarget.AllViaServer,null);
        uIMenager.KarşilaşmaReddetButton_Method();
    }


    [PunRPC]
    private void IntroScene(int sceneIndex,bool playerReady)
    {
        if(playerReady)
        {
            PhotonNetwork.LoadLevel(1);
        }
    }
}
