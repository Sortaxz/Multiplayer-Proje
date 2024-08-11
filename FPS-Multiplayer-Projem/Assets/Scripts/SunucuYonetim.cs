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
    private void Awake() 
    {
        uIMenager = UIMenager.Instance;
    }

    private string panelName;

    public override void OnConnectedToMaster()
    {
        print("Server'a bağlanildi");
        
        StopCoroutine(uIMenager.ConnetingAnimation());
        uIMenager.SetActiveUIObject(uIMenager.Menu_Panel.name); 
    }

    public override void OnJoinedLobby()
    {
        print("Lobiye bağlanildi");
        if(panelName == uIMenager.RandomOda_Panel.name)
        {
            RoomOptions roomOptions = new RoomOptions()
            {
                MaxPlayers = 4
            };

            PhotonNetwork.JoinRandomOrCreateRoom(null,0,MatchmakingMode.FillRoom,null,null,null,roomOptions);
        }
    }

    public override void OnJoinedRoom()
    {
        print("Herhangi bir odaya giriş yapildi");
        if(panelName == uIMenager.RandomOda_Panel.name)
        {
            uIMenager.SetActiveUIObject(uIMenager.RandomOda_Panel.name);
            
            foreach (Player player in PhotonNetwork.PlayerList)
            {
                GameObject playerListObje = PlayerListOlustur(player.ActorNumber,player.NickName,player);
                
                if(PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue("isPlayerReady",out object isPlayerReady))
                {
                    playerListObje.GetComponent<PlayerListControl>().SetPlayerReady((bool)isPlayerReady);
                }

                playerList.Add(player.ActorNumber,playerListObje);
            }

            

        }
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
        
        uIMenager.LocalPlayerPropertiesUpdated();

        return playerList;
    }

    
    public void ConnetingServer()
    {
        PhotonNetwork.ConnectUsingSettings();
        
        if(!PlayerPrefs.HasKey("playerName"))
        {
            string playerName = uIMenager.KullaniciAdi_InputField.text;
            PlayerPrefs.SetString("playerName",playerName);
            PhotonNetwork.LocalPlayer.NickName = playerName;

            
        }
        else
        {
            PhotonNetwork.LocalPlayer.NickName = PlayerPrefs.GetString("playerName");
        }
        StartCoroutine(uIMenager.ConnetingAnimation());
    }

   
    public void ConnectedLobby(string panelName)
    {
        this.panelName = panelName;
        PhotonNetwork.JoinLobby();
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        if(playerList.TryGetValue(targetPlayer.ActorNumber,out GameObject entry))
        {
            if(changedProps.TryGetValue("isPlayerReady",out object isPlayerReady))
            {
                entry.GetComponent<PlayerListControl>().SetPlayerReady((bool)isPlayerReady);
            }
        }

        uIMenager.LocalPlayerPropertiesUpdated();
    }

}
