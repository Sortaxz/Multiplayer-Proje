using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
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

    [SerializeField] private GameObject yeniOyuncu_Panel;
    public GameObject YeniOyuncu_Panel {get { return yeniOyuncu_Panel;}}

    [SerializeField] private GameObject kayitliOyuncu_Panel;
    public GameObject KayitliOyuncu_Panel {get { return kayitliOyuncu_Panel;}}

    [SerializeField] private GameObject playerIcon_Panel;

    [SerializeField] private GameObject playerColor_Panel;

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

    private ExitGames.Client.Photon.Hashtable playerProps;

    private bool playerBaslatButtonActive = false;
    private bool playerPropKaydetButtonClick = false;
    private int iconIndex;
    private int colorIndex;



    private void Awake() 
    {
        SetActiveUIObject(oyunaBaglanma_Panel.gameObject.name);
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

            }
            
        }
    }
   

    public void OyunuBaslatButton_Method()
    {
        SunucuYonetim.Instance.ConnetingServer();


        menuKullaniciAdi_Text.text += PhotonNetwork.LocalPlayer.NickName;

        PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue("icon",out object iconIndex);
        PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue("color",out object colorIndex);
        
        PlayerPrefs.SetInt("icon",(int)iconIndex);
        PlayerPrefs.SetInt("color",(int)colorIndex);

        menuPlayerIcon_Image.sprite = playerIcons[(int)iconIndex].sprite;
        
        SetActiveUIObject(connecting_Panel.name);
    }


    public void KayitliOyuncuButton_Method()
    {
        if(PlayerPrefs.HasKey("icon") && PlayerPrefs.HasKey("color"))
        {
            SunucuYonetim.Instance.ConnetingServer();


            menuKullaniciAdi_Text.text += PhotonNetwork.LocalPlayer.NickName;

            iconIndex = PlayerPrefs.GetInt("icon");
            colorIndex = PlayerPrefs.GetInt("color");

            SetPlayerProps();

            menuPlayerIcon_Image.sprite = playerIcons[(int)iconIndex].sprite;
            
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
        SetActiveUIObject(oyunaBaglanma_Panel.name);
    }


    public void PlayerPropSaveButton_Method()
    {

        playerPropKaydetButtonClick = !playerPropKaydetButtonClick;
        if(playerPropKaydetButtonClick)
        {
            playerPropKaydetButton.GetComponent<Image>().color = Color.green;
            playerPropBaslatButton.gameObject.SetActive(playerBaslatButtonActive );
        }
        else
        {
            playerPropKaydetButton.GetComponent<Image>().color = Color.white;
            playerPropBaslatButton.gameObject.SetActive(playerPropKaydetButtonClick );
        }
        SetPlayerProps();


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

    public void PlayerPropButton_Method()
    {
        
        playerBaslatButtonActive =!playerBaslatButtonActive;

        if(playerIcon_Panel.activeSelf)
        {
            iconIndex = EventSystem.current.currentSelectedGameObject.transform.GetSiblingIndex();
        }
        else if(playerColor_Panel.activeSelf)
        {
            colorIndex = EventSystem.current.currentSelectedGameObject.transform.GetSiblingIndex();
        }

        if(playerBaslatButtonActive)
        {
            EventSystem.current.currentSelectedGameObject.GetComponent<Image>().color = new Color(0.5283019f,0.5283019f,0.5283019f,1);
        }
        else
        {
            EventSystem.current.currentSelectedGameObject.GetComponent<Image>().color = Color.white;
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
   
}
