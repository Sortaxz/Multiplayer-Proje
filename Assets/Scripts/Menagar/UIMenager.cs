using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class UIMenager : MonoBehaviour
{
    private static UIMenager instance;
    public static UIMenager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<UIMenager>();
            }
            return instance;
        }
    }
    
    [Space]
    [Space]
    [Space]

    [SerializeField] private PlayerScriptableObject playerScriptableObject;
    public PlayerScriptableObject PlayerScriptableObject { get { return playerScriptableObject; } }

    [Space]
    [Space]
    [Space]

    #region  Paneller
    [SerializeField] private GameObject connecting_Panel;
    public GameObject ConnectingPanel { get { return connecting_Panel;}}

    [SerializeField] private GameObject cheat_Panel;
    public GameObject Cheat_Panel { get { return cheat_Panel;}}

    [SerializeField] private GameObject oyunaBaglanma_Panel;
    public GameObject OyunaBaglanma_Panel { get { return oyunaBaglanma_Panel;}}

    [SerializeField] private GameObject playerProps_Panel;
    public GameObject PlayerProps_Panel {get { return playerProps_Panel;}}

    [SerializeField] private GameObject menu_Panel;
    public GameObject Menu_Panel {get { return menu_Panel;}}

    [SerializeField] private GameObject settings_Panel;
    public GameObject Settings_Panel {get { return settings_Panel;}}

    [SerializeField] private GameObject arakadasİslem_Panel;
    public GameObject Arakadasİslem_Panel {get { return arakadasİslem_Panel;}}


    [SerializeField] private GameObject odaKurma_Panel;
    public GameObject OdaKurma_Panel {get { return odaKurma_Panel;}}

    [SerializeField] private GameObject odaIslemleri_Panel;
    public GameObject OdaIslemleri_Panel {get { return odaIslemleri_Panel;}}
    
    [SerializeField] private GameObject randomOdaModSecim_Panel;
    public GameObject RandomOdaModSecim_Panel {get { return randomOdaModSecim_Panel;}}


    [SerializeField] private GameObject odaKurmaYüklemeEkran_Panel;
    public GameObject OdaKurmaYüklemeEkran_Panel {get { return odaKurmaYüklemeEkran_Panel;}}

    [SerializeField] private GameObject warning_Panel;
    public GameObject Warning_Panel {get { return warning_Panel;}}

    [SerializeField] private GameObject yeniOyuncu_Panel;
    public GameObject YeniOyuncu_Panel {get { return yeniOyuncu_Panel;}}

    [SerializeField] private GameObject kayitliOyuncu_Panel;
    public GameObject KayitliOyuncu_Panel {get { return kayitliOyuncu_Panel;}}

    [SerializeField] private GameObject playerIcon_Panel;

    [SerializeField] private GameObject playerColor_Panel;
    [SerializeField] private GameObject MatchWaitingScreen;

    #endregion


    #region  Random Oda ile ilgili İşlemler

    [Header("Random Oda ile ilgili İşlemler")]
    [SerializeField] private GameObject randomOda_Panel;
    public GameObject RandomOda_Panel { get { return randomOda_Panel;}}
    [SerializeField] private GameObject randomOdaUst_Panel;
    [SerializeField] private GameObject randomOdaAlt_Panel;

    #endregion

    [Space]
    [Space]

    #region  Arkadas Panel ile ilgili işlemler

    [Header("Arkadas Panel ile ilgili işlemler")]

    [SerializeField] private GameObject friendListContent;
    public GameObject FriendListContent { get { return friendListContent;}}

    [SerializeField] private GameObject friendListPrefab;
    public GameObject  FriendListPrefab { get { return friendListPrefab;}}
    
    private Dictionary<string,GameObject> friendList = new Dictionary<string, GameObject>();

    #endregion

    [Space]
    [Space]

    #region  Arkadaş Kabul veya Reddetme ile ilgili işlemler

    [Header("Arkadaş Kabul veya Reddetme ile ilgili işlemler")]

    [SerializeField] private GameObject FriendAcceptOrReject_Panel;

    #endregion
    
    #region  Player Props Panel İşlenler

    [Header("Player Props Panel İşlenleri")]
    [SerializeField] private List<Image> playerIconImageProps;
    [SerializeField] private List<Image> playerColorImageProps;
    [SerializeField] private Transform PlayerIcon_Content;
    [SerializeField] private Transform PlayerColor_Content;
    [SerializeField] private GameObject playerPropImagePrefab;
    #endregion

    [Space]
    [Space]

    #region  Conneting Panel İşlenleri
    
    [Header("Conneting Panel İşlenleri")]
    [SerializeField] private string[] oyunIpUclari; 
    [SerializeField] private TextMeshProUGUI oyunIpUcu_Text;
    [SerializeField] private TextMeshProUGUI connecting_Text;
    
    #endregion

    [Space]
    [Space]

    #region  Maç Arama Panel İşlemler
    
    [Header("Maç Arama Panel İşlemler")]
    [SerializeField] private GameObject findingMatch_Panel;
    public GameObject FindingMatch_Panel { get { return findingMatch_Panel;}}
    [SerializeField] private GameObject karşilaşmaKabulReddet_Panel;
    public GameObject KarşilaşmaKabulReddet_Panel { get { return karşilaşmaKabulReddet_Panel;}}

    #endregion

    [Space]
    [Space]
    

    #region  Karşilaşma Kabul veya Reddet Panel İşlemler

    [Header("Karşilaşma Kabul veya Reddet Panel İşlemleri")]
    [SerializeField] private  Button karşilaşmayiKabulEt_Button;
    [SerializeField] private  Button karşilaşmayiReddet_Button;

    #endregion

    [Space]
    [Space]

    #region  Oda Kurma İşlemleri
    
    [Header("Oda Kurma İşlemleri")]
    [SerializeField] private TMP_InputField odaAdi_InputField;
    public TMP_InputField OdaAdi_InputField { get { return odaAdi_InputField;} set { odaAdi_InputField = value;}}
    
    #endregion

    [Space]
    [Space]

    #region  Other Properties ve Variables
    [SerializeField] private Button exitImageButton;
    [SerializeField] private Image[] playerIcons;
    public Image[] PlayerIcons { get { return playerIcons;}}

    [SerializeField] private Image[] playerColor;
    public Image[] PlayerColors { get { return playerColor;}}

    [SerializeField] private Button playerPropBaslatButton;
    [SerializeField] private Button playerPropKaydetButton;


    [SerializeField] private TMP_InputField kullaniciAdi_InputField;
    public TMP_InputField KullaniciAdi_InputField { get { return kullaniciAdi_InputField;}}

    [SerializeField] private TextMeshProUGUI menuKullaniciAdi_Text;
    [SerializeField] private Image menuPlayerIcon_Image;
    
    [SerializeField] private Button kayitliOyuncu_Button;
    [SerializeField] private Button yeniOyuncu_Button;

    [SerializeField] private Button oyunuBaslatButton;
    

    public TextMeshProUGUI friendPlayerNickName_Text;
    
    private PhotonView pv;
    public PhotonView PV {get {return pv;}}
    private CheatController cheatController;
    private ExitGames.Client.Photon.Hashtable playerProps;

    private GameMode gameMode;
    public GameMode GameMode {get {return gameMode;}}

    private bool playerIconBaslatButtonActive = false;
    private bool playerColorBaslatButtonActive = false;
    private bool playerPropKaydetButtonClick = false;

    private bool menuPlayerProfil = false;
    private bool isPlayerReady = false;
    private bool value= false;
    private bool findCheatController = false;

    private string roomName;
    public string RoomName {get {return roomName;}}
    string playerName;
    private int iconIndex;
    private int colorIndex;

    private int i = 0;

    private int playerPropIconCount = 0;
    private int playerPropColorCount = 0;

    #endregion

    public bool oyunaYenidenGiris = false;  
    private bool kayitliButtonClick = false;
    public bool KayitliButtonClick {get {return kayitliButtonClick;} set {kayitliButtonClick = value;}}
    private void Awake() 
    {

        if(SunucuYonetim.Instance.ServerControl())
        {
            oyunaYenidenGiris = true;
        }

        pv = GetComponent<PhotonView>();

    }

    private void Start() 
    {
        if(!oyunaYenidenGiris)
        {

            SetActiveUIObject(oyunaBaglanma_Panel.gameObject.name);
        }
        
    }

    public void PlayerPropsShow()
    {
            FriendSystem.Instance.LoadFriendDate();

            string playerName = (string)SaveSystem.PlayerPrefsDataLoad("playerName", "string");
            SaveSystem.PlayerPrefsDataSave("playerName", playerName);


            menuKullaniciAdi_Text.text += PhotonNetwork.LocalPlayer.NickName;

            iconIndex = (int)SaveSystem.PlayerPrefsDataLoad("icon", "int");
            colorIndex = (int)SaveSystem.PlayerPrefsDataLoad("color", "int");


            SetPlayerProps();

            menuPlayerIcon_Image.sprite = playerScriptableObject.PlayerIconSprites[iconIndex];
            menuKullaniciAdi_Text.text = playerName;

            SetActiveUIObject(menu_Panel.name);
    }

    private void Update() 
    {
        CheatActive(); // düzeltmem gerekiyor

        
    }

    public void CheatActive()
    {
        if(Input.GetKeyDown(KeyCode.Q))
        {
            cheat_Panel.SetActive(!cheat_Panel.activeSelf);
            if(!findCheatController)
            {
                cheatController = cheat_Panel.GetComponent<CheatController>();
                findCheatController = true;
            }
            cheatController.SetChatActive();
        }
    }

    public void BaglanButton_Method()
    {
        playerName = kullaniciAdi_InputField.text;
        
        
        if(playerName != (string)SaveSystem.PlayerPrefsDataLoad("playerName","string"))
        {
            PlayerPrefs.DeleteAll();


            SaveSystem.PlayerPrefsDataSave("playerName",playerName);
            SetActiveUIObject(playerProps_Panel.name);
        }
        else
        {
            warning_Panel.SetActive(true);
            kullaniciAdi_InputField.text = "";
            kullaniciAdi_InputField.Select();

        }
    }


    public void Method()
    {
        if(Input.GetKey(KeyCode.Return) || Input.GetKey(KeyCode.KeypadEnter))
        {
            playerName = kullaniciAdi_InputField.text;

            
            if(playerName != (string)SaveSystem.PlayerPrefsDataLoad("playerName","string"))
            {
                PlayerPrefs.DeleteAll();

                SaveSystem.PlayerPrefsDataSave("playerName",playerName);
                SetActiveUIObject(playerProps_Panel.name);
            }
            else
            {
                warning_Panel.SetActive(true);
                kullaniciAdi_InputField.text = "";
                kullaniciAdi_InputField.Select();

            }

        
        }
    }
   

    public void BaslatButton_Method()
    {
        FriendSystem.Instance.LoadFriendDate();

        ControlPlayerPropsActive();
        
        SunucuYonetim.Instance.isConnected = true;
        SunucuYonetim.Instance.ConnetingServer(playerName);
        
        SetActiveUIObject(connecting_Panel.name);

        menuKullaniciAdi_Text.text += PhotonNetwork.LocalPlayer.NickName;

        PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue("icon",out object iconIndex);
        PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue("color",out object colorIndex);
        
        PlayerPrefs.SetInt("icon",(int)iconIndex);
        PlayerPrefs.SetInt("color",(int)colorIndex);

        menuPlayerIcon_Image.sprite =  playerScriptableObject.PlayerIconSprites[(int)iconIndex];
        
        playerPropKaydetButton.gameObject.SetActive(false);

    }


    public void KayitliOyuncuButton_Method()
    {
        if(SaveSystem.PlayerPrefsDataQuery("icon") && SaveSystem.PlayerPrefsDataQuery("color") && SaveSystem.PlayerPrefsDataQuery("playerName"))
        {
            kayitliButtonClick = true;
            FriendSystem.Instance.LoadFriendDate();
            
            string playerName = (string)SaveSystem.PlayerPrefsDataLoad("playerName","string");
            SaveSystem.PlayerPrefsDataSave("playerName",playerName);
            SunucuYonetim.Instance.isConnected = true;
            SunucuYonetim.Instance.ConnetingServer(playerName);


            menuKullaniciAdi_Text.text += PhotonNetwork.LocalPlayer.NickName;

            iconIndex = (int)SaveSystem.PlayerPrefsDataLoad("icon","int");
            colorIndex = (int)SaveSystem.PlayerPrefsDataLoad("color","int");
            

            SetPlayerProps();

            menuPlayerIcon_Image.sprite =  playerScriptableObject.PlayerIconSprites[iconIndex];
            

            SetActiveUIObject(connecting_Panel.name);

            

        }
        else
        {
            kayitliOyuncu_Button.interactable =  false;
        }
    }

    private IEnumerator PlayerTipTextAnimation()
    {
        while(true)
        {
            if(odaKurmaYüklemeEkran_Panel.activeSelf)
            {
                oyunIpUcu_Text.text = oyunIpUclari[i];
                if(i < oyunIpUclari.Length && !value)
                {
                    i++;
                    if(i == oyunIpUclari.Length-1)
                    {
                        value = true;
                        i = oyunIpUclari.Length-1;
                    }
                }
                else
                {
                    i--;

                    if(i == 0)
                    {
                        value = false;
                        i = 0;
                    }
                }
            }
            else
            {
                StopCoroutine(PlayerTipTextAnimation());
            }
            yield return new WaitForSeconds(.3f);
        }

    }

    public void YeniOyuncuButton_Method()
    {
        SetActiveUIObject(yeniOyuncu_Panel.name);
    }


    public void GeriDon_Button()
    {
        if(!menuPlayerProfil)
        {
            kullaniciAdi_InputField.text = "";
            SetActiveUIObject(oyunaBaglanma_Panel.name);
        }
        else 
        {
            SetActiveUIObject(menu_Panel.name);
        
        }
       

    }

    #region  Menu Panel İşlenleri
    public void MenuStartButton_Method()
    {
        SetActiveUIObject(odaIslemleri_Panel.name);
        menuPlayerProfil = true;
        
    }


    public void MenuProfilButton_Method()
    {
        SetActiveUIObject(playerProps_Panel.name);
        playerIcon_Panel.SetActive(false);
        playerColor_Panel.SetActive(false);
        playerPropKaydetButtonClick = false;
        menuPlayerProfil = true;
        playerPropBaslatButton.gameObject.SetActive(playerPropKaydetButtonClick);
        playerPropKaydetButton.GetComponent<Image>().color = playerPropKaydetButtonClick ?  Color.green : Color.white;

    }


    public void RandomOdaKuButton_Method()
    {
        SetActiveUIObject(randomOdaModSecim_Panel.name);
        print(SunucuYonetim.Instance.GamePlayerControl());
    }

    public void OdaKurmaButton_Method()
    {
        PhotonNetwork.LocalPlayer.CustomProperties["inGame"] = false;


        SetActiveUIObject(odaKurma_Panel.name);
       
    }


    public void SetttingsButton_Method()
    {
        SetActiveUIObject(settings_Panel.name);
    }

    public void MenuExitButton_Method()
    {
        Application.Quit();
    }

    #endregion

    public void PlayerPropSaveButton_Method()
    {

        playerPropKaydetButtonClick = !playerPropKaydetButtonClick;
        
        playerPropKaydetButton.GetComponent<Image>().color = playerPropKaydetButtonClick ?  Color.green : Color.white;
        
        if(!menuPlayerProfil)
        {
            if(playerColorBaslatButtonActive && playerIconBaslatButtonActive)
            {
                playerPropBaslatButton.gameObject.SetActive(playerPropKaydetButtonClick);
                
                SetPlayerProps();

                
            }
        }
        else
        {
            SetActiveUIObject(menu_Panel.name);
            playerPropKaydetButtonClick = false;

            playerPropKaydetButton.GetComponent<Image>().color = playerPropKaydetButtonClick ?  Color.green : Color.white;

            //menuPlayerIcon_Image.sprite = playerIcons[iconIndex].sprite;
            menuPlayerIcon_Image.sprite = playerScriptableObject.PlayerIconSprites[iconIndex];

            SetPlayerProps();

            
            ControlPlayerPropsActive();
            
        }

    }

    private void ControlPlayerPropsActive()
    {
        for (int i = 0; i < playerIconImageProps.Count; i++)
        {
            playerIconImageProps[i].GetComponent<Button>().interactable = true;
        }

        for (int i = 0; i < playerColorImageProps.Count; i++)
        {
            playerColorImageProps[i].GetComponent<Button>().interactable = true;
        }
        playerIconBaslatButtonActive = false;
        playerColorBaslatButtonActive = false;
        EventSystem.current.currentSelectedGameObject.SetActive(false);

    }

    public void PlayerIconButton_Method()
    {
        playerIcon_Panel.SetActive(!playerIcon_Panel.activeSelf);
        playerColor_Panel.SetActive(false);

        PlayerPropsSettingsUI("icon",playerPropIconCount);
        playerPropIconCount ++;
    
    }

    public void PlayerColorButton_Method()
    {
        playerIcon_Panel.SetActive(false);
        playerColor_Panel.SetActive(!playerColor_Panel.activeSelf);

        PlayerPropsSettingsUI("color",playerPropColorCount);
        playerPropColorCount ++;
    
    }


    private void SetPlayerProps()
    {
        playerProps = new ExitGames.Client.Photon.Hashtable()
        {
            {"icon",iconIndex},
            {"color",colorIndex}
        };

        
        PhotonNetwork.LocalPlayer.SetCustomProperties(playerProps);

        PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue("icon",out object icon_Index);
        PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue("color",out object color_Index);
            
           
        SaveSystem.PlayerPrefsDataSave("icon",icon_Index);
        SaveSystem.PlayerPrefsDataSave("color",color_Index);
    }

    public void SetActiveUIObject(string panelName)
    {
        connecting_Panel.SetActive(panelName.Equals(connecting_Panel.name));
        odaKurmaYüklemeEkran_Panel.SetActive(panelName.Equals(odaKurmaYüklemeEkran_Panel.name));
        cheat_Panel.SetActive(panelName.Equals(cheat_Panel.name) );
        oyunaBaglanma_Panel.gameObject.SetActive(panelName.Equals(oyunaBaglanma_Panel.name) || panelName.Equals(yeniOyuncu_Panel.name));
        playerProps_Panel.gameObject.SetActive(panelName.Equals(playerProps_Panel.name));
        menu_Panel.gameObject.SetActive(panelName.Equals(menu_Panel.name));
        yeniOyuncu_Panel.SetActive(panelName.Equals(yeniOyuncu_Panel.name));
        kayitliOyuncu_Panel.SetActive(panelName.Equals(kayitliOyuncu_Panel.name) || panelName.Equals(oyunaBaglanma_Panel.name));
        randomOda_Panel.SetActive(panelName.Equals(randomOda_Panel.name) || panelName.Equals(FindingMatch_Panel.name)|| panelName.Equals(KarşilaşmaKabulReddet_Panel.name));
        odaIslemleri_Panel.SetActive(panelName.Equals(odaIslemleri_Panel.name));
        findingMatch_Panel.SetActive(panelName.Equals(FindingMatch_Panel.name) );
        KarşilaşmaKabulReddet_Panel.SetActive(panelName.Equals(KarşilaşmaKabulReddet_Panel.name));
        odaKurma_Panel.SetActive(panelName.Equals(odaKurma_Panel.name));
        settings_Panel.SetActive(panelName.Equals(settings_Panel.name));
        randomOdaModSecim_Panel.SetActive(panelName.Equals(randomOdaModSecim_Panel.name));
        arakadasİslem_Panel.SetActive(panelName.Equals(arakadasİslem_Panel.name));


        if(menu_Panel.activeSelf)
        {
            StartCoroutine(FriendSystem.Instance.CheckFriends());
        }
        else if(!menu_Panel.activeSelf)
        {
            StopCoroutine(FriendSystem.Instance.CheckFriends());
        }
    }


    public void PlayerPropButton_Method()
    {
        if(playerIcon_Panel.activeSelf)
        {
            iconIndex = EventSystem.current.currentSelectedGameObject.transform.GetSiblingIndex();
            
        }
        else if(playerColor_Panel.activeSelf)
        {
            colorIndex = EventSystem.current.currentSelectedGameObject.transform.GetSiblingIndex();

        }

        if(playerIcon_Panel.activeSelf)
        {
            playerIconBaslatButtonActive =!playerIconBaslatButtonActive;
            
            if(playerIconBaslatButtonActive)
            {
                for (int i = 0; i < playerIconImageProps.Count; i++)
                {
                    if(i!= iconIndex)
                    {
                        playerIconImageProps[i].GetComponent<Button>().interactable = false;
                    }
                }
            }
            else
            {
                for (int i = 0; i < playerIconImageProps.Count; i++)
                {
                    playerIconImageProps[i].GetComponent<Button>().interactable = true;
                }  
            }
        }

        if(playerColor_Panel.activeSelf)
        {
            playerColorBaslatButtonActive =!playerColorBaslatButtonActive;


            if(playerColorBaslatButtonActive)
            {
                for (int i = 0; i < playerColorImageProps.Count; i++)
                {
                    if(i!= colorIndex)
                    {
                        playerColorImageProps[i].GetComponent<Button>().interactable = false;
                    }
                }
            }
            else
            {

                for (int i = 0; i < playerColorImageProps.Count; i++)
                {
                    playerColorImageProps[i].GetComponent<Button>().interactable = true;
                }  
            }
        }

        if(!menuPlayerProfil)
        {
            playerPropKaydetButton.gameObject.SetActive(playerIconBaslatButtonActive && playerColorBaslatButtonActive);
        }
        else
        {
            playerPropKaydetButton.gameObject.SetActive(playerIconBaslatButtonActive || playerColorBaslatButtonActive);
        }
        playerProps = new ExitGames.Client.Photon.Hashtable()
        {
            {"icon",iconIndex},
            {"color",colorIndex}
        };

        
        PhotonNetwork.LocalPlayer.SetCustomProperties(playerProps);
    }


    public void PlayerPropButton_Method2()
    {

        if(playerIconBaslatButtonActive && playerColorBaslatButtonActive)
        {
            value = true;
        }
        else
        {
            value = false;
        }
        
        if(playerPropKaydetButtonClick)
        {
            playerPropBaslatButton.gameObject.SetActive(value);
        }
    }

    public IEnumerator ConnetingAnimation(int i = 0, bool start = false)
    {
        while(true)
        {
            yield return new WaitForSeconds(.1f);
            if(i < 4 && !start)
            {
                connecting_Text.text += ".";
                i++;

                if(i == 3)
                {
                    i = 3;
                    start = true;
                }
            }
            else 
            {
                connecting_Text.text  = "Connecting";
                i--;

                if(i == 0)
                {
                    i = 0;
                    start = false;
                }
            }
        }
    }

    public void KarşilaşmaKabulEtButton_Method()
    {
        isPlayerReady = true;
        
        playerProps = new ExitGames.Client.Photon.Hashtable()
        {
            {"isPlayerReady",isPlayerReady}
        };

        PhotonNetwork.LocalPlayer.SetCustomProperties(playerProps);

        karşilaşmaKabulReddet_Panel.SetActive(false);

    
    }



    public void KarşilaşmaReddetButton_Method()
    {
        isPlayerReady = false;
        playerProps = new ExitGames.Client.Photon.Hashtable()
        {
            {"isPlayerReady",isPlayerReady}
        };

        PhotonNetwork.LocalPlayer.SetCustomProperties(playerProps);

        karşilaşmaKabulReddet_Panel.SetActive(false);
        
    }

    
    #region  Oda Kurma İşlemleri Methodlari

    public void DereceliMode_Method()
    {

        if(PhotonNetwork.IsConnectedAndReady)
        {
            if(!randomOdaModSecim_Panel.activeSelf)
            {
                gameMode = GameMode.Dereceli;

                string mod = gameMode.ToString(); 
                SaveSystem.PlayerPrefsDataSave("gameMode",mod);
                
                SunucuYonetim.Instance.OdaKurdu = true;
                
            }
            else
            {

                SunucuYonetim.Instance.RandomOdaKurdu = true;
                SunucuYonetim.Instance.NormalRoom = true;


                gameMode = GameMode.Dereceli;

                string mod = gameMode.ToString(); 
                SaveSystem.PlayerPrefsDataSave("gameMode",mod);

                SunucuYonetim.Instance.CreateRandomRoom(gameMode);

                SetActiveUIObject(odaKurmaYüklemeEkran_Panel.name);
                StartCoroutine(PlayerTipTextAnimation());
                
            }
        }

    }

    public void DerecesizMode_Method()
    {
        if(!randomOdaModSecim_Panel.activeSelf)
        {
            gameMode = GameMode.Derecesiz;

            string mod = gameMode.ToString(); 
            SaveSystem.PlayerPrefsDataSave("gameMode",mod);

            SunucuYonetim.Instance.OdaKurdu = true;

        }
        else
        {

            gameMode = GameMode.Derecesiz;
    
            string mod = gameMode.ToString(); 
            SaveSystem.PlayerPrefsDataSave("gameMode",mod);

            SunucuYonetim.Instance.CreateRandomRoom(gameMode);

            SetActiveUIObject(odaKurmaYüklemeEkran_Panel.name);
            StartCoroutine(PlayerTipTextAnimation());
           
        }
        
    }

    
    public void OdaKur()
    {
        
        roomName = odaAdi_InputField.text;

        SunucuYonetim.Instance.CreateRoom(gameMode,roomName);
        
        SetActiveUIObject(odaKurmaYüklemeEkran_Panel.name);

        StartCoroutine(PlayerTipTextAnimation());
    }

    #endregion

    #region  Arkadaş Panel İle İlgili Methodlar 

    public void ArkadaslarButton_Method()
    {
        SetActiveUIObject(arakadasİslem_Panel.name);
        menuPlayerProfil = true;
        FriendSystem.Instance.CurrentFriendsList();
    }

    

    #endregion


    
    public void WarningExitButton_Method()
    {
        warning_Panel.SetActive(false);
        kullaniciAdi_InputField.Select();
    }

    private bool isCreatFriendList = false;
    
    public void CreateCurrentFriendList(List<string> _friendList,List<string> friendIconList)
    {
       
        for (int i = 0; i < _friendList.Count; i++)
        {
            string friendNickName = "";
            int friendIconIndex= 0;

            if(friendNickName != PhotonNetwork.LocalPlayer.UserId)
            {
                friendNickName = _friendList[i];
                friendIconIndex = int.Parse(friendIconList[i]);
                isCreatFriendList = true;
            }
            

            if(isCreatFriendList)
            {
                if(!friendList.ContainsKey(friendNickName) && friendNickName != PhotonNetwork.LocalPlayer.UserId)
                {
                    GameObject currentFriendListObjesi = Instantiate(friendListPrefab,friendListContent.transform);
                    currentFriendListObjesi.transform.localScale = Vector3.one;

                    
                    currentFriendListObjesi.GetComponent<FriendListControl>().FriendListInitialize(friendNickName,friendIconIndex);
                    friendList.Add(friendNickName,currentFriendListObjesi);
                }
            }
        }
    } 
    


    public GameObject FriendshipAnswer(bool value)
    {
        FriendAcceptOrReject_Panel.SetActive(value);

        return FriendAcceptOrReject_Panel;
    }

    public void RemoveFriendList(string removeFriendList)
    {
        Destroy(friendList[removeFriendList]);
        friendList.Remove(removeFriendList);
    }

    public void ExitIconButton_Method()
    {
        SunucuYonetim.Instance.LeftRoom();
        SetActiveUIObject(menu_Panel.name);
    }

    private void PlayerPropsSettingsUI(string playerPropsType,int count)
    {
       
        if(playerPropsType == "icon" && count < 1)
        {
            for (int i = 0; i < playerScriptableObject.PlayerIconSprites.Length; i++)
            {
                GameObject image = Instantiate(playerPropImagePrefab,PlayerIcon_Content);
                image.GetComponent<Image>().sprite = playerScriptableObject.PlayerIconSprites[i];
                playerIconImageProps.Add( image.GetComponent<Image>());
            }
        }
        else if(playerPropsType == "color" && count < 1)
        {
            for (int i = 0; i < playerScriptableObject.PlayerColors.Length; i++)
            {
                GameObject image2 = Instantiate(playerPropImagePrefab,PlayerColor_Content);
                image2.GetComponent<Image>().color = playerScriptableObject.PlayerColors[i];
                playerColorImageProps.Add( image2.GetComponent<Image>());
            }
        }
    }
}
