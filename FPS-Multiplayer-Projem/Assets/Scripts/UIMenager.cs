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

    [SerializeField] private GameObject odaIslemleri_Panel;
    public GameObject OdaIslemleri_Panel {get { return odaIslemleri_Panel;}}

    [SerializeField] private GameObject yeniOyuncu_Panel;
    public GameObject YeniOyuncu_Panel {get { return yeniOyuncu_Panel;}}

    [SerializeField] private GameObject kayitliOyuncu_Panel;
    public GameObject KayitliOyuncu_Panel {get { return kayitliOyuncu_Panel;}}

    [SerializeField] private GameObject playerIcon_Panel;

    [SerializeField] private GameObject playerColor_Panel;

    [Header("Random Oda ile ilgili İşlemler")]
    [SerializeField] private GameObject randomOda_Panel;
    public GameObject RandomOda_Panel { get { return randomOda_Panel;}}
    [SerializeField] private GameObject randomOdaUst_Panel;
    [SerializeField] private GameObject randomOdaAlt_Panel;

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
    [SerializeField] private Transform playeIconImageContent;
    [SerializeField] private Transform playeColorImageContent;
    [Space]
    [Space]

    
    [Header("Conneting Panel İşlenleri")]
    [SerializeField] private string[] oyunIpUclari; 
    [SerializeField] private TextMeshProUGUI oyunIpUcu_Text;
    [SerializeField] private TextMeshProUGUI connecting_Text;
    
    [Space]
    [Space]


    [Header("Maç Arama Panel İşlemler")]
    [SerializeField] private GameObject FindingMatch_Panel;

    [Space]
    [Space]

    
    private ExitGames.Client.Photon.Hashtable playerProps;
    private bool playerBaslatButtonActive = false;
    private bool playerPropKaydetButtonClick = false;

    private bool menuPlayerProfil = false;

    private int iconIndex;
    private int colorIndex;

    [SerializeField]private Image tiklanilanImage;
    private int tiklanilanImageIndex = -1;
    private Color color;
    private void Awake() 
    {
        SetActiveUIObject(oyunaBaglanma_Panel.gameObject.name);
        
    }
    private void Update() 
    {
        if(Input.GetKeyDown(KeyCode.Q))
        {
            cheat_Panel.SetActive(!cheat_Panel.activeSelf);
            cheat_Panel.GetComponent<CheatController>().SetChatActive();
        }

        

    }

    public void BaglanButton_Method()
    {
        SetActiveUIObject(playerProps_Panel.name);
    }


    public void Method()
    {
        if(Input.GetKey(KeyCode.Return) || Input.GetKey(KeyCode.KeypadEnter))
        {
            if(!PlayerPrefs.HasKey("icon"))
            {
                SetActiveUIObject(playerProps_Panel.name);
            }
            else
            {
                SunucuYonetim.Instance.ConnetingServer();
                
                menuKullaniciAdi_Text.text += PhotonNetwork.LocalPlayer.NickName;

                iconIndex = PlayerPrefs.GetInt("icon");
                colorIndex = PlayerPrefs.GetInt("color");

                SetPlayerProps();

                menuPlayerIcon_Image.sprite = playerIcons[iconIndex].sprite;
                
                SetActiveUIObject(connecting_Panel.name);
                
                oyunIpUcu_Text.text = oyunIpUclari[Random.Range(0,oyunIpUclari.Length-1)];
            }
            
        }
    }
   

    public void BaslatButton_Method()
    {
        SunucuYonetim.Instance.ConnetingServer();


        menuKullaniciAdi_Text.text += PhotonNetwork.LocalPlayer.NickName;

        PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue("icon",out object iconIndex);
        PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue("color",out object colorIndex);
        
        PlayerPrefs.SetInt("icon",(int)iconIndex);
        PlayerPrefs.SetInt("color",(int)colorIndex);

        menuPlayerIcon_Image.sprite = playerIcons[(int)iconIndex].sprite;
        
        SetActiveUIObject(connecting_Panel.name);
        oyunIpUcu_Text.text = oyunIpUclari[Random.Range(0,oyunIpUclari.Length-1)];
    }


    public void KayitliOyuncuButton_Method()
    {

        if(PlayerPrefs.HasKey("icon") && PlayerPrefs.HasKey("color") )
        {
            SunucuYonetim.Instance.ConnetingServer();


            menuKullaniciAdi_Text.text += PhotonNetwork.LocalPlayer.NickName;

            iconIndex = PlayerPrefs.GetInt("icon");
            colorIndex = PlayerPrefs.GetInt("color");

            SetPlayerProps();

            menuPlayerIcon_Image.sprite = playerIcons[(int)iconIndex].sprite;
            
           oyunIpUcu_Text.text = oyunIpUclari[Random.Range(0,oyunIpUclari.Length-1)];
           SetActiveUIObject(connecting_Panel.name);



        }
        else
        {
            kayitliOyuncu_Button.interactable =  false;
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
            SetActiveUIObject(menu_Panel.name);
        }
    }

    #region  Menu Panel İşlenleri
    public void MenuStartButton_Method()
    {
        SetActiveUIObject(odaIslemleri_Panel.name);
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

    #endregion



    public void RandomOdaKuButton_Method()
    {
        SunucuYonetim.Instance.ConnectedLobby(randomOda_Panel.name);
        SetActiveUIObject(connecting_Panel.name);

    }


    public void PlayerPropSaveButton_Method()
    {

        playerPropKaydetButtonClick = !playerPropKaydetButtonClick;
        
        playerPropKaydetButton.GetComponent<Image>().color = playerPropKaydetButtonClick ?  Color.green : Color.white;
        
        SetPlayerProps();
        if(!menuPlayerProfil)
        {
            playerPropBaslatButton.gameObject.SetActive(playerPropKaydetButtonClick );
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
            
            PlayerPrefs.SetInt("icon",(int)icon_Index);
            PlayerPrefs.SetInt("color",(int)color_Index);

        }



    }

    public void PlayerIconButton_Method()
    {
        playerIcon_Panel.SetActive(!playerIcon_Panel.activeSelf);
        playerColor_Panel.SetActive(false);
        if(playerBaslatButtonActive)
        {
            playerBaslatButtonActive = false;
        }
        else
        {
            playerBaslatButtonActive = true;
        }
    }

    public void PlayerColorButton_Method()
    {
        playerIcon_Panel.SetActive(false);
        playerColor_Panel.SetActive(!playerColor_Panel.activeSelf);
        if(playerBaslatButtonActive)
        {
            playerBaslatButtonActive = false;
        }
        else
        {
            playerBaslatButtonActive = true;
        }
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
        cheat_Panel.SetActive(panelName.Equals(cheat_Panel.name));
        oyunaBaglanma_Panel.gameObject.SetActive(panelName.Equals(oyunaBaglanma_Panel.name) || panelName.Equals(yeniOyuncu_Panel.name));
        playerProps_Panel.gameObject.SetActive(panelName.Equals(playerProps_Panel.name));
        menu_Panel.gameObject.SetActive(panelName.Equals(menu_Panel.name));
        yeniOyuncu_Panel.SetActive(panelName.Equals(yeniOyuncu_Panel.name));
        kayitliOyuncu_Panel.SetActive(panelName.Equals(kayitliOyuncu_Panel.name) || panelName.Equals(oyunaBaglanma_Panel.name));
        randomOda_Panel.SetActive(panelName.Equals(randomOda_Panel.name));
        odaIslemleri_Panel.SetActive(panelName.Equals(odaIslemleri_Panel.name));
    }


    public void PlayerPropButton_Method()
    {
        

        playerBaslatButtonActive =!playerBaslatButtonActive;
        print(playerBaslatButtonActive);

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
            if(playerBaslatButtonActive)
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
                print("playerIcon_Panel-else");
                for (int i = 0; i < playerIconImageProps.Length; i++)
                {
                    playerIconImageProps[i].GetComponent<Button>().interactable = true;
                }  
            }
        }

        if(playerColor_Panel.activeSelf)
        {
            if(playerBaslatButtonActive)
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

        
    }


    public void PlayerPropButton_Method2()
    {
        
        if(playerBaslatButtonActive)
        {
            playerBaslatButtonActive = true;
        }
        else
        {
            playerBaslatButtonActive = false;
        }        

        if(playerPropKaydetButtonClick)
        {
            playerPropBaslatButton.gameObject.SetActive(playerBaslatButtonActive);
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
}
