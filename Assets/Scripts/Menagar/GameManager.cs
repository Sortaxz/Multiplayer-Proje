using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviourPunCallbacks
{
    private static GameManager instance;
    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<GameManager>();
            }
            return instance;
        }
    }

    public delegate void DeatDelegate();
    public static DeatDelegate deatDelegate;

    [SerializeField] private PlayerScriptableObject playerScriptableObject;
    public PlayerScriptableObject PlayerScriptableObject { get { return playerScriptableObject; } }
    [SerializeField]private CharacterControl[] characterOfPlayers;
    public CharacterControl[] CharacterOfPlayers { get { return characterOfPlayers; } set { characterOfPlayers = value; } }
    [SerializeField] private GameObject loadingScren;
    [SerializeField] private Text infoText;
    private bool findCharacterOfPlayers = false;
    public bool FindCharacterOfPlayers { get { return findCharacterOfPlayers;} set { findCharacterOfPlayers = value;}}
    private PhotonView PV;
    private GameObject character;
    [SerializeField] private CountdownTimer countdownTimer;
    [SerializeField] private GameObject bulletsParent;

    private List<GameObject> scanner = new List<GameObject>();
    public List<GameObject> Scanner { get { return scanner;} set { scanner = value; } }
    private List<GameObject> mp5 = new List<GameObject>();
    public List<GameObject> Mp5 { get { return mp5;} set { mp5 = value; } }
    private Dictionary<string,SkorLineControl> skorLines = new Dictionary<string,SkorLineControl>(); 
    public Dictionary<string,SkorLineControl> SkorLines {get { return skorLines;} set { skorLines = value; } }

    private bool characterDead = false;
    public bool CharacterDead { get { return characterDead;} set { characterDead = value; } }

    private bool gameStopted = false;
    public bool GameStopted { get { return gameStopted;} set { gameStopted = value; } }
    private float time;

    private int playerKill = 0;
    private int playerDeath = 0;


    public override void OnEnable()
    {
        CountdownTimer.OnCountdownTimerHasExpired += OnCountDownTimerIsExpired;
        base.OnEnable();
    }

    public override void OnDisable()
    {
        base.OnDisable();
        CountdownTimer.OnCountdownTimerHasExpired -= OnCountDownTimerIsExpired;
    }

    private void Awake() 
    {
        PV = GetComponent<PhotonView>();
        ExitGames.Client.Photon.Hashtable props = new ExitGames.Client.Photon.Hashtable()
        {
            {"PlayerLoadedLevel",true},
        };
        PhotonNetwork.LocalPlayer.SetCustomProperties(props);    

        if(!countdownTimer.isActiveAndEnabled)
        {
            OnCountDownTimerIsExpired();
        }
    }
    
    void Start()
    {
        StartCoroutine(FindCharacter());
    }

    private void Update() 
    {
        

       
    }
  

    #region  Pun Callbacks

    public override void OnDisconnected(DisconnectCause cause)
    {
    }

    public override void OnLeftRoom()
    {
        
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        ExitGames.Client.Photon.Hashtable props =  new ExitGames.Client.Photon.Hashtable()
        {
            {"inGame",false}
        };
        otherPlayer.SetCustomProperties(props);
       
    }
    
    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        if(!PhotonNetwork.IsMasterClient)
        {
            return;
        }

        bool StartTimeIsSet  = CountdownTimer.TryGetStartTime(out int startTimeStamp);

        if(changedProps.ContainsKey("PlayerLoadedLevel"))
        {
            if(CheckAllPlayerLoadedLevel())
            {
                if(!StartTimeIsSet)
                {
                    CountdownTimer.SetStartTime();
                }
            }
            else
            {

                infoText.text = "Diğer oyuncular bekleniyor...";
            }
        }
        
        if(changedProps.ContainsKey("kill"))
        {
            print(targetPlayer.UserId + changedProps["kill"]);
        }

        
        
    }

    #endregion


    public IEnumerator FindCharacter()
    {
        while(true && !findCharacterOfPlayers)
        {
            CharacterControl[] newCharacterOfPlayers = FindObjectsOfType<CharacterControl>();
            characterOfPlayers = new CharacterControl[newCharacterOfPlayers.Length];
            for (int i = 0; i < newCharacterOfPlayers.Length; i++)
            {
                if(i == newCharacterOfPlayers[i].PlayerActorNumber-1)
                {
                    characterOfPlayers[i] = newCharacterOfPlayers[i];
                }
                else
                {
                    if(newCharacterOfPlayers[i].PlayerActorNumber-1 < characterOfPlayers.Length)
                    {
                        characterOfPlayers[newCharacterOfPlayers[i].PlayerActorNumber-1] = newCharacterOfPlayers[i];
                    }
                }
                
               
            }

            

            if(newCharacterOfPlayers.Length == PhotonNetwork.CurrentRoom.PlayerCount)
            {
                findCharacterOfPlayers = true;
                StopCoroutine(FindCharacter());
                StartCoroutine(FindOtherPlayerCharacter());
            }
            yield return new WaitForSeconds(.1f);
        }
        

        //PlayerSkorTable();
    }

    private void StartGame()
    {
        character = SpawnManager.Instance.CharacterSpawn(PV);
        ExitGames.Client.Photon.Hashtable playerProps = new ExitGames.Client.Photon.Hashtable();

        playerProps.Add("life",true);
        playerProps.Add("inGame",true);
        PhotonNetwork.LocalPlayer.SetCustomProperties(playerProps);

        PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue("color",out object color);
        int playerColorIndex = (int)color;

        character.GetComponent<CharacterControl>().CharacterMSHRenderer.materials[1].color = playerScriptableObject.PlayerColors[playerColorIndex];

        if(Camera.main.gameObject.activeSelf)
        {
            Camera.main.gameObject.SetActive(false);
        }

        GameUI.Instance.Active();
        characterDead = false;
    }

    private bool CheckAllPlayerLoadedLevel()
    {
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            if(player.CustomProperties.TryGetValue("PlayerLoadedLevel",out object playerLoadedLevel))
            {
                if((bool)playerLoadedLevel)
                {
                    continue;
                }

                return false;
            }
        }

        return true;
 
     }
    
    private void OnCountDownTimerIsExpired()
    {
        loadingScren.gameObject.SetActive(false);
        StartGame();

        PlayerSkorTable();

        if(PhotonNetwork.IsMasterClient)
        {
            StartCoroutine(TimerInumerator());
        }
        else
        {
            GameUI.Instance.TimerPanel.SetActive(true);
        }
        
    }

    public void Die()
    {
        characterDead = false;


        PhotonNetwork.Destroy(character);

        character = SpawnManager.Instance.CharacterSpawn(PV);

        PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue("color",out object color);
        int playerColorIndex = (int)color;

        character.GetComponent<CharacterControl>().CharacterMSHRenderer.materials[1].color = playerScriptableObject.PlayerColors[playerColorIndex];

        GameUI.Instance.Active();
       
        StartCoroutine(FindOtherPlayerCharacter());

    }
    private void WeaponBulletClear(List<GameObject> weapon)
    {
        for (int i = 0; i < weapon.Count; i++)
        {
            Destroy(weapon[i].gameObject);
            weapon.Remove(weapon[i].gameObject);
        }
    }

    public bool IsStartGame()
    {   
        bool startGame = false;
        if(PhotonNetwork.InRoom)
        {
            startGame = (bool)SaveSystem.PlayerPrefsDataLoad("startGame","bool");
        }
        return startGame;
    }

    private IEnumerator FindOtherPlayerCharacter()
    {
        while(true)
        {
            if(characterOfPlayers != null && characterOfPlayers.Length == PhotonNetwork.CurrentRoom.PlayerCount)
            {
                for (int i = 0; i < characterOfPlayers.Length; i++)
                {
                    if(characterOfPlayers[i] != null)
                    {
                        string userId = characterOfPlayers[i].GetComponent<PhotonView>().Owner.UserId;
                        if(PhotonNetwork.LocalPlayer.UserId != userId)
                        {
                            if(characterOfPlayers[i].OtherPlayerHealtBar.fillAmount <= 0)
                            {
                                characterOfPlayers[i].OtherPlayerHealtBar.fillAmount = 1f;
                                characterOfPlayers[i].OtherPlayerHealtBar.gameObject.SetActive(true);
                            }
                            else
                            {
                                characterOfPlayers[i].OtherPlayerHealtBar.gameObject.SetActive(true);
                            }
            
                        }
                        else
                        {
                            GameUI.Instance.WeapomInfoImage.SetActive(true);
                            
                        }
                    }
                    
                }

                StopCoroutine(FindOtherPlayerCharacter());
            }

            yield return new WaitForSeconds(.1f);
        }
    }

   

    private IEnumerator TimerInumerator()
    {
        if(!GameUI.Instance.TimerPanel.activeSelf)
        {
            GameUI.Instance.TimerPanel.SetActive(true);
        }
        int totalSeconds = Mathf.FloorToInt(time);

        while(time < 15)
        {

            totalSeconds += 1 ;

            int minutes = Mathf.FloorToInt(time / 60);
            int seconds = Mathf.FloorToInt(totalSeconds % 60);
            

            if(seconds >= 15 )
            {
                totalSeconds = 0;
                time += 1;
            }
            
            GameUI.Instance.TimerText.text = string.Format("{0:00}:{1:00}", time, seconds);
            PV.RPC("Timer",RpcTarget.OthersBuffered,time,totalSeconds);
            yield return new WaitForSeconds(.5f);
        }


        PV.RPC("FinishGame",RpcTarget.All,null);

    }



    [PunRPC]
    private void Timer(float totalMinutes,int totalSeconds)
    {
        int minutes = Mathf.FloorToInt(totalMinutes / 60);
        int seconds = Mathf.FloorToInt(totalSeconds % 60);

        GameUI.Instance.TimerText.text = string.Format("{0:00}:{1:00}", totalMinutes, seconds);
    }


    [PunRPC]
    private void FinishGame()
    {
        ExitGames.Client.Photon.Hashtable RoomStatus = new ExitGames.Client.Photon.Hashtable()
        {
            {"roomStatus",false}
        };
        

        PhotonNetwork.CurrentRoom.SetCustomProperties(RoomStatus);
        
        GameUI.Instance.GameOverUi();

    }

    public void StopGameStreaming(bool value)
    {
        if(value == false)
        {
            if(gameStopted)
            {
                StopFlow(false);
            }
            else
            {
                StopFlow(true);
            }

        }
        else
        {
            StopFlow(false);
            StopAllCoroutines();
        }
    }

    private void StopFlow(bool closeOrOpen)
    {
        for (int i = 0; i < characterOfPlayers.Length; i++)
        {
            if(characterOfPlayers[i] == null)
                return;
            
            CharacterControl characterControl = characterOfPlayers[i].GetComponent<CharacterControl>();
            CombatController combatController = characterOfPlayers[i].GetComponent<CombatController>();
            CharacterAnimation characterAnimation = characterOfPlayers[i].GetComponent<CharacterAnimation>();

            if(characterControl.CharacterCamera != null)
            {
                CameraController cameraController = characterControl.CharacterCamera.GetComponent<CameraController>();
                if(cameraController != null)
                {
                    cameraController.enabled= closeOrOpen;
                }

            }

            
            characterControl.PlayerCourse_Image.SetActive(closeOrOpen);
            
            combatController.enabled = closeOrOpen;
            characterAnimation.enabled = closeOrOpen;
            characterControl.enabled = closeOrOpen;

        }
    }

    public void GameOut()
    {
        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            characterOfPlayers[i].GetComponent<CharacterControl>().enabled = true;
            
            if(PhotonNetwork.LocalPlayer.UserId == PhotonNetwork.PlayerList[i].UserId)
            {
                PhotonNetwork.Destroy(characterOfPlayers[i].gameObject);
            }
        }

        PhotonNetwork.LeaveRoom();
        PhotonNetwork.LoadLevel(0);

        StopAllCoroutines();
    }

    public void PlayerKillSkor(int killCount,Player player)
    {
        int _kill =0;
        if(player.CustomProperties.TryGetValue("kill",out object kill))
        {
            _kill= (int)kill;
        }

        _kill+= killCount;
        
        ExitGames.Client.Photon.Hashtable playerKillSkor = new ExitGames.Client.Photon.Hashtable()
        {
            {"kill",_kill}
        };
        player.SetCustomProperties(playerKillSkor);
    }

    public void PlayerDeathSkor(int deathCount,Player player)
    {
        int _death =0;
        if(player.CustomProperties.TryGetValue("death",out object death))
        {
            _death =(int)death;
        }

        _death += deathCount;
        
        ExitGames.Client.Photon.Hashtable playerDeathSkor = new ExitGames.Client.Photon.Hashtable()
        {
            {"death",_death}
        };
        player.SetCustomProperties(playerDeathSkor);
    }


    public void PlayerSkorTable()
    {
        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            Player player =  PhotonNetwork.PlayerList[i];
            if(!skorLines.ContainsKey(player.UserId))
            {
                player.CustomProperties.TryGetValue("icon",out object playerIconIndex);


                int playerKillCount =  PlayerPropertiesControl(player,"kill");
                int playerDeathCount = PlayerPropertiesControl(player,"death");
                GameUI.Instance.CreateSkorLine((int)playerIconIndex,player.UserId,playerKillCount,playerDeathCount);
            }

        }

        
    }


    private int PlayerPropertiesControl(Player player, string contolValue)
    {
        if(player.CustomProperties[contolValue] == null)
        {
            return 0;
        }

        return (int)player.CustomProperties[contolValue];
    }

    public void PlayerSkorUpdate()
    {
        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            Player player = PhotonNetwork.PlayerList[i];
            int playerKillCount = PlayerPropertiesControl(player,"kill");
            int playerDeathCount = PlayerPropertiesControl(player,"death");

            skorLines[player.UserId].PlayerSkor(playerKillCount,playerDeathCount);
        
        }
    }

}
