using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using TMPro;
using Unity.VisualScripting;
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
    [SerializeField] private GameObject bulletsParent;

    private List<GameObject> scanner = new List<GameObject>();
    public List<GameObject> Scanner { get { return scanner;} set { scanner = value; } }
    private List<GameObject> mp5 = new List<GameObject>();
    public List<GameObject> Mp5 { get { return mp5;} set { mp5 = value; } }

    private List<string> playerName = new List<string>();

    private bool characterDead = false;
    public bool CharacterDead { get { return characterDead;} set { characterDead = value; } }

    [SerializeField] private CountdownTimer countdownTimer;

    private float time;
    private int sayi = 5;

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
        if(PV.IsMine)
        {
            if(Input.GetKeyDown(KeyCode.P))
            {
                print("p");
                sayi++;
            }
        }
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
        //GameUI.Instance.RunTimer_Method();

        if(PhotonNetwork.IsMasterClient)
        {
            StartCoroutine(TimerInumerator(time));
        }
        else
        {
            GameUI.Instance.TimerPanel.SetActive(true);
        }
        
    }

    public void Die()
    {
        characterDead = false;
        /*
        WeaponBulletClear(scanner);
        WeaponBulletClear(mp5);
        */
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
                            //characterOfPlayers[i].Weapon_Info_Image.SetActive(false);
            
                        }
                        else
                        {
                            GameUI.Instance.WeapomInfoImage.SetActive(true);
                            //characterOfPlayers[i].Weapon_Info_Image.SetActive(true);
                            //GameUI.Instance.WeapomInfoImage = characterOfPlayers[i].Weapon_Info_Image;
                            //GameUI.Instance.SetWeopanInfoUi(characterOfPlayers[i].Weapon_Info_Image);
                        }
                    }
                    
                }

                StopCoroutine(FindOtherPlayerCharacter());
            }

            yield return new WaitForSeconds(.1f);
        }
    }

   

    private IEnumerator TimerInumerator(float time)
    {
        if(!GameUI.Instance.TimerPanel.activeSelf)
        {
            GameUI.Instance.TimerPanel.SetActive(true);
        }

        int totalSeconds = Mathf.FloorToInt(time);

        while(time < 15)
        {

            totalSeconds += 1 ;
            int minutes = Mathf.FloorToInt(totalSeconds / 60);
            int seconds = Mathf.FloorToInt(totalSeconds % 60);

            GameUI.Instance.TimerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
            
            PV.RPC("Timer",RpcTarget.AllBufferedViaServer,totalSeconds);
            yield return new WaitForSeconds(1);
            //GameManager.Instance.TimeOfOtherPlayer(time,timerText);
        }


    }



    [PunRPC]
    private void Timer(int value)
    {
        int minutes = Mathf.FloorToInt(value / 60);
        int seconds = Mathf.FloorToInt(value % 60);

        GameUI.Instance.TimerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    [PunRPC]
    private void FinishGame()
    {

    }
}
