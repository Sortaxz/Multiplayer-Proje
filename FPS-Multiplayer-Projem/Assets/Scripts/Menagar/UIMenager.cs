using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEditor;
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

    [SerializeField] private GameObject yeniOyuncu_Panel;
    public GameObject YeniOyuncu_Panel {get { return yeniOyuncu_Panel;}}

    [SerializeField] private GameObject kayitliOyuncu_Panel;
    public GameObject KayitliOyuncu_Panel {get { return kayitliOyuncu_Panel;}}

    [SerializeField] private GameObject playerIcon_Panel;

    [SerializeField] private GameObject playerColor_Panel;

    #endregion

    [Header("Random Oda ile ilgili İşlemler")]
    [SerializeField] private GameObject randomOda_Panel;
    public GameObject RandomOda_Panel { get { return randomOda_Panel;}}
    [SerializeField] private GameObject randomOdaUst_Panel;
    [SerializeField] private GameObject randomOdaAlt_Panel;

    [Space]
    [Space]

    [Header("Arkadas Panel ile ilgili işlemler")]

    [SerializeField] private GameObject friendListContent;
    public GameObject FriendListContent { get { return friendListContent;}}

    [SerializeField] private GameObject friendListPrefab;
    public GameObject  FriendListPrefab { get { return friendListPrefab;}}
    
    [Space]
    [Space]

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

    [Header("Pleyar Props Panel İşlenleri")]
    [SerializeField] private Image[] playerIconImageProps;
    [SerializeField] private Image[] playerColorImageProps;
    [Space]
    [Space]

    
    [Header("Conneting Panel İşlenleri")]
    [SerializeField] private string[] oyunIpUclari; 
    [SerializeField] private TextMeshProUGUI oyunIpUcu_Text;
    [SerializeField] private TextMeshProUGUI connecting_Text;
    
    [Space]
    [Space]


    [Header("Maç Arama Panel İşlemler")]
    [SerializeField] private GameObject findingMatch_Panel;
    public GameObject FindingMatch_Panel { get { return findingMatch_Panel;}}
    [SerializeField] private GameObject karşilaşmaKabulReddet_Panel;
    public GameObject KarşilaşmaKabulReddet_Panel { get { return karşilaşmaKabulReddet_Panel;}}
    [Space]
    [Space]

    [Header("Karşilaşma Kabul veya Reddet Panel İşlemleri")]
    [SerializeField] private  Button karşilaşmayiKabulEt_Button;
    [SerializeField] private  Button karşilaşmayiReddet_Button;

    [Space]
    [Space]

    [Header("Oda Kurma İşlemleri")]
    [SerializeField] private TMP_InputField odaAdi_InputField;
    
    [Space]
    [Space]
    

    public TextMeshProUGUI text;
    
    private PhotonView pv;
    public PhotonView PV {get {return pv;}}
    private CheatController cheatController;
    private SaveSystem saveSystem;
    private ExitGames.Client.Photon.Hashtable playerProps;

    private GameMode gameMode;
    public GameMode GameMode {get {return gameMode;}}

    private bool playerIconBaslatButtonActive = false;
    private bool playerColorBaslatButtonActive = false;
    private bool playerPropKaydetButtonClick = false;

    private bool menuPlayerProfil = false;
    private bool isPlayerReady = false;

    private string roomName;
    public string RoomName {get {return roomName;}}

    private int iconIndex;
    private int colorIndex;



    private int i = 0;
    private bool value= false;
    private bool findCheatController = false;


    private void Awake() 
    {
        SetActiveUIObject(oyunaBaglanma_Panel.gameObject.name);
        pv = GetComponent<PhotonView>();
        saveSystem  = SaveSystem.Instance;
    }
    private void Update() 
    {
        
        CheatActive();
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
        string playerName = kullaniciAdi_InputField.text;

        SunucuYonetim.Instance.ConnetingServer(playerName);

        saveSystem.PlayerPrefsDataSave("playerName",playerName);
            
        SetActiveUIObject(connecting_Panel.name);
    }


    public void Method()
    {
        if(Input.GetKey(KeyCode.Return) || Input.GetKey(KeyCode.KeypadEnter))
        {
            string playerName = kullaniciAdi_InputField.text;

            SunucuYonetim.Instance.ConnetingServer(playerName);
            
            saveSystem.PlayerPrefsDataSave("playerName",playerName);
            
            SetActiveUIObject(connecting_Panel.name);

            
        }
    }
   

    public void BaslatButton_Method()
    {


        menuKullaniciAdi_Text.text += PhotonNetwork.LocalPlayer.NickName;

        PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue("icon",out object iconIndex);
        PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue("color",out object colorIndex);
        
        PlayerPrefs.SetInt("icon",(int)iconIndex);
        PlayerPrefs.SetInt("color",(int)colorIndex);

        menuPlayerIcon_Image.sprite = playerIcons[(int)iconIndex].sprite;
        

        SetActiveUIObject(menu_Panel.name);

    }


    public void KayitliOyuncuButton_Method()
    {
        
        if(saveSystem.PlayerPrefsDataQuery("icon") && saveSystem.PlayerPrefsDataQuery("color") && saveSystem.PlayerPrefsDataQuery("playerName"))
        {
            string playerName = (string)saveSystem.PlayerPrefsDataLoad("playerName","string");

            SunucuYonetim.Instance.ConnetingServer(playerName);


            menuKullaniciAdi_Text.text += PhotonNetwork.LocalPlayer.NickName;

            iconIndex = (int)saveSystem.PlayerPrefsDataLoad("icon","int");
            colorIndex = (int)saveSystem.PlayerPrefsDataLoad("color","int");

            SetPlayerProps();

            menuPlayerIcon_Image.sprite = playerIcons[(int)iconIndex].sprite;
            

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
        PlayerPrefs.DeleteAll();
        SetActiveUIObject(yeniOyuncu_Panel.name);
    }


    public void GeriDon_Button()
    {
        if(!menuPlayerProfil)
        {
            SetActiveUIObject(oyunaBaglanma_Panel.name);
        }
        else 
        {
            if(PhotonNetwork.InRoom)
            {
                PhotonNetwork.LeaveRoom();
            }
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

    }

    public void SetttingsButton_Method()
    {
        SetActiveUIObject(settings_Panel.name);
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
                playerPropBaslatButton.gameObject.SetActive(playerPropKaydetButtonClick );
                SetPlayerProps();
            }
        }
        else
        {
            SetActiveUIObject(menu_Panel.name);
            playerPropKaydetButtonClick = false;

            playerPropKaydetButton.GetComponent<Image>().color = playerPropKaydetButtonClick ?  Color.green : Color.white;

            menuPlayerIcon_Image.sprite = playerIcons[iconIndex].sprite;

            SetPlayerProps();

            PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue("icon",out object icon_Index);
            PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue("color",out object color_Index);
            
           
            saveSystem.PlayerPrefsDataSave("icon",icon_Index);
            saveSystem.PlayerPrefsDataSave("color",color_Index);
        }



    }

    public void PlayerIconButton_Method()
    {
        playerIcon_Panel.SetActive(!playerIcon_Panel.activeSelf);
        playerColor_Panel.SetActive(false);
        
    }

    public void PlayerColorButton_Method()
    {
        playerIcon_Panel.SetActive(false);
        playerColor_Panel.SetActive(!playerColor_Panel.activeSelf);
        
    }


    private void SetPlayerProps()
    {
        playerProps = new ExitGames.Client.Photon.Hashtable()
        {
            {"icon",iconIndex},
            {"color",colorIndex}
        };

        
        PhotonNetwork.LocalPlayer.SetCustomProperties(playerProps);
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
                for (int i = 0; i < playerIconImageProps.Length; i++)
                {
                    if(i!= iconIndex)
                    {
                        playerIconImageProps[i].GetComponent<Button>().interactable = false;
                    }
                }
            }
            else
            {
                for (int i = 0; i < playerIconImageProps.Length; i++)
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
                for (int i = 0; i < playerColorImageProps.Length; i++)
                {
                    if(i!= colorIndex)
                    {
                        playerColorImageProps[i].GetComponent<Button>().interactable = false;
                    }
                }
            }
            else
            {

                for (int i = 0; i < playerColorImageProps.Length; i++)
                {
                    playerColorImageProps[i].GetComponent<Button>().interactable = true;
                }  
            }
        }

        
        playerPropKaydetButton.gameObject.SetActive(playerIconBaslatButtonActive && playerColorBaslatButtonActive);
    }


    public void PlayerPropButton_Method2(bool value)
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

    
    
    public bool CheckPlayersReady()
    {
        if(!PhotonNetwork.IsMasterClient)
        {
            return false;
            
        }

        foreach (Player player in PhotonNetwork.CurrentRoom.Players.Values)
        {
            if(!player.CustomProperties.TryGetValue("isPlayerReady",out object isPlayerReady))
            {
                return false;
            }
            else
            {
                if(!(bool)isPlayerReady)
                {
                    return false;
                }
            }
        }
        return true;
    }

    public void LocalPlayerPropertiesUpdated()
    {
        oyunuBaslatButton.gameObject.SetActive(CheckPlayersReady());
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
        if(!randomOdaModSecim_Panel.activeSelf)
        {
            gameMode = GameMode.Dereceli;
            
        }
        else
        {
            SunucuYonetim.Instance.NormalRoom = true;

            gameMode = GameMode.Dereceli;

            SunucuYonetim.Instance.CreateRandomRoom(gameMode);

            SetActiveUIObject(odaKurmaYüklemeEkran_Panel.name);
            StartCoroutine(PlayerTipTextAnimation());
        }
    }

    public void DerecesizMode_Method()
    {
        if(!randomOdaModSecim_Panel.activeSelf)
        {
            gameMode = GameMode.Derecesiz;
        }
        else
        {
            SunucuYonetim.Instance.NormalRoom = true;
            gameMode = GameMode.Derecesiz;

            SunucuYonetim.Instance.CreateRandomRoom(gameMode);

            SetActiveUIObject(odaKurmaYüklemeEkran_Panel.name);
            StartCoroutine(PlayerTipTextAnimation());
        }
        
    }

    public void OdaKurmaButton_Method()
    {
        SetActiveUIObject(odaKurma_Panel.name);
    }

    public void OdaKur()
    {
        SunucuYonetim.Instance.NormalRoom = true;

        roomName = odaAdi_InputField.text;


        SunucuYonetim.Instance.CreateRoom(gameMode);
        
        SetActiveUIObject(odaKurmaYüklemeEkran_Panel.name);
        StartCoroutine(PlayerTipTextAnimation());
    }

    #endregion

    #region  Arkadaş Panel İle İlgili Methodlar 

    public void ArkadaslarButton_Method()
    {
        SetActiveUIObject(arakadasİslem_Panel.name);
        menuPlayerProfil = true;

        //SunucuYonetim.Instance.GetFriendRoom();
    }

   

    #endregion

}
