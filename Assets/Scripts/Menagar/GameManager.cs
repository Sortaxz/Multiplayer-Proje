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
    public delegate void GunEffectDelegate();
    public static DeatDelegate deatDelegate;
    public  GunEffectDelegate gunEffectDelegate;

    [SerializeField] private PlayerScriptableObject playerScriptableObject;
    public PlayerScriptableObject PlayerScriptableObject { get { return playerScriptableObject; } }
    [SerializeField]private CharacterControl[] characterOfPlayers;
    public CharacterControl[] CharacterOfPlayers { get { return characterOfPlayers; } set { characterOfPlayers = value; } }
    [SerializeField] private CharacterControl[] newCharacterOfPlayers;

    [SerializeField] private GameObject loadingScren;
    [SerializeField] private Text infoText;
    private bool findCharacterOfPlayers = false;
    public bool FindCharacterOfPlayers { get { return findCharacterOfPlayers;} set { findCharacterOfPlayers = value;}}
    private PhotonView PV;
    public PhotonView GameMager_PV { get { return PV;}}
    private GameObject character;
    [SerializeField] private CountdownTimer countdownTimer;
    [SerializeField] private GameObject bulletsParent;

    private List<GameObject> scanner = new List<GameObject>();
    public List<GameObject> Scanner { get { return scanner;} set { scanner = value; } }
    private List<GameObject> mp5 = new List<GameObject>();
    public List<GameObject> Mp5 { get { return mp5;} set { mp5 = value; } }
    private Dictionary<string,SkorLineControl> skorLines = new Dictionary<string,SkorLineControl>(); 
    public Dictionary<string,SkorLineControl> SkorLines {get { return skorLines;} set { skorLines = value; } }
    public Coroutine timerCoroutine;
    ExitGames.Client.Photon.Hashtable roomOptions;
    private bool characterDead = false;
    public bool CharacterDead { get { return characterDead;} set { characterDead = value; } }

    private bool gameStopted = false;
    public bool GameStopted { get { return gameStopted;} set { gameStopted = value; } }
    private float time;

    private float minutes;
    private int seconds;
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
        roomOptions = new ExitGames.Client.Photon.Hashtable()
        {
            {"timerStarted",false},
            {"roomMinutes",0f},
            {"roomSeconds",0f}
        };
        PhotonNetwork.CurrentRoom.SetCustomProperties(roomOptions);

        if(!countdownTimer.isActiveAndEnabled)
        {
            OnCountDownTimerIsExpired();
        }
    }
    
    void Start()
    {
        StartCoroutine(FindCharacter());
    }

    private void OnApplicationQuit() 
    {
        SaveSystem.PlayerPrefsDataRemove("GameLogin");    
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

    }
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        if (otherPlayer.IsMasterClient)
        {
            for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
            {
                PhotonNetwork.SetMasterClient(PhotonNetwork.PlayerList[i]);
            }

        }

        RestartTimer();

    }

    public void RestartTimer()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            StartCoroutine(TimerInumerator(minutes, true, seconds));
        }
    }

    
    public IEnumerator FindCharacter()
    {
        while (!findCharacterOfPlayers)
        {
            newCharacterOfPlayers = FindObjectsOfType<CharacterControl>();

            if (characterOfPlayers == null || characterOfPlayers.Length == 0)
            {
                characterOfPlayers = new CharacterControl[PhotonNetwork.CurrentRoom.PlayerCount];
            }

            if(newCharacterOfPlayers.Length == PhotonNetwork.PlayerList.Length)
            {
                for (int i = 0; i < newCharacterOfPlayers.Length; i++)
                {
                    CharacterControl characterControl =newCharacterOfPlayers[i];
                    
                    if(numberStudies < newCharacterOfPlayers.Length)
                    {

                        int result = Query(characterControl,newCharacterOfPlayers);
                        int index = characterOfPlayers.Length-1  - result;
                        characterOfPlayers[index] = characterControl;
                        numberStudies++;
                    }

                    
                }
            }

           
            if(IsActorsFound())
            {
                findCharacterOfPlayers = true;
                StartCoroutine(FindOtherPlayerCharacter());
                numberStudies = 0;
            }

            yield return new WaitForSeconds(0.1f); 
        }
    }

    private int numberStudies = 0;

  
    private int Query(CharacterControl characterControl,CharacterControl[] characterControls)
    {
        
        int number = 0;
        for (int i = 0; i < characterControls.Length; i++)
        {
            if(characterControl.PlayerNickName != characterControls[i].PlayerNickName)
            {
                if(characterControl.PlayerActorNumber < characterControls[i].PlayerActorNumber)
                {
                    number++;
                }
            }
        }

        return number;
    }

    private bool IsActorsFound()
    {
        for (int i = 0; i < characterOfPlayers.Length; i++)
        {
            if(characterOfPlayers[i] == null)
            {
                return false;
            }
        }
        return true;
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

       

        if(PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue("timerStarted",out object timerStarted))
        {
            if(!(bool)timerStarted)
            {
                if(PhotonNetwork.LocalPlayer.IsMasterClient)
                {
                   
                    StartCoroutine(TimerInumerator(time));
                }
                else
                {
                    GameUI.Instance.TimerPanel.SetActive(true);

                }
                PhotonNetwork.CurrentRoom.CustomProperties["timerStarted"] = true;
            }
        }      

    }

    public void Die()
    {


        PhotonNetwork.Destroy(character);

        character = SpawnManager.Instance.CharacterSpawn(PV);

        PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue("color",out object color);
        int playerColorIndex = (int)color;

        character.GetComponent<CharacterControl>().CharacterMSHRenderer.materials[1].color = playerScriptableObject.PlayerColors[playerColorIndex];

        GameUI.Instance.Active();
       
        characterDead = false;
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

    private bool secondsUp = false;
    private IEnumerator TimerInumerator(float time,bool value = false,int second = 0)
    {
        if(!GameUI.Instance.TimerPanel.activeSelf)
        {
            GameUI.Instance.TimerPanel.SetActive(true);
        }
        
        if(!value)
        {
            int totalSeconds = Mathf.FloorToInt(time);


            while(time < 15)
            {

                totalSeconds += 1 ;

                int _seconds = Mathf.FloorToInt(totalSeconds % 60);
                

                if(_seconds >= 14 )
                {
                    totalSeconds = 0;
                    secondsUp = true;
                }
                
                roomOptions["roomMinutes"] = time;
                roomOptions["roomSeconds"] = _seconds;

                PhotonNetwork.CurrentRoom.SetCustomProperties(roomOptions);            

                PV.RPC("Timer",RpcTarget.AllBufferedViaServer,time,_seconds);

                yield return new WaitForSeconds(.5f);
                if(secondsUp)
                {
                    time += 1;
                    secondsUp = false;

                }
            }

        }
        else
        {
            int totalSeconds = second;   
            while(time < 15)
            {

                totalSeconds += 1 ;

                int _seconds = Mathf.FloorToInt(totalSeconds % 60);
                

                if(_seconds >= 14 )
                {
                    totalSeconds = 0;
                    secondsUp = true;
                }
                
                roomOptions["roomMinutes"] = time;
                roomOptions["roomSeconds"] = _seconds;

                PhotonNetwork.CurrentRoom.SetCustomProperties(roomOptions);            

                PV.RPC("Timer",RpcTarget.AllBufferedViaServer,time,_seconds);

                yield return new WaitForSeconds(.5f);
                if(secondsUp)
                {
                    time += 1;
                    secondsUp = false;

                }
            }
        }

        PV.RPC("FinishGame",RpcTarget.All,null);

    }


    [PunRPC]
    private void Timer(float _minutes,int _seconds)
    {
        GameUI.Instance.TimerText.text = string.Format("{0:00}:{1:00}", _minutes, _seconds);
        gameObject.GetComponent<GameManager>().minutes = _minutes;
        gameObject.GetComponent<GameManager>().seconds = _seconds;
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

    private bool değişken = false;

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

            değişken = closeOrOpen;
            
            characterControl.PlayerCourse_Image.SetActive(değişken);
            
            combatController.enabled = closeOrOpen;
            characterAnimation.enabled = closeOrOpen;
            characterControl.enabled = closeOrOpen;

        }
    }

    public void GameOut()
    {
        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            if(characterOfPlayers[i] != null)
            {
                characterOfPlayers[i].GetComponent<CharacterControl>().enabled = true;
            }
            
            if(PhotonNetwork.LocalPlayer.UserId == PhotonNetwork.PlayerList[i].UserId)
            {
                if(characterOfPlayers[i] != null && characterOfPlayers[i].GetComponent<PhotonView>().IsMine)
                {
                    PhotonNetwork.Destroy(characterOfPlayers[i].gameObject);
                }
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

    public void StartEveryOne()
    {
        PV.RPC("RPC_StartEveryOne",RpcTarget.AllViaServer);
    }

    [PunRPC]
    public void RPC_StartEveryOne()
    {
        gunEffectDelegate();
    }


  

}
