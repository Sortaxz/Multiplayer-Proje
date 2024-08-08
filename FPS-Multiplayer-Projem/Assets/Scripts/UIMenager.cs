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

    [SerializeField] private GameObject connecting_Panel;
    public GameObject ConnectingPanel { get { return connecting_Panel;}}

    [SerializeField] private GameObject cheat_Panel;
    public GameObject Cheat_Panel { get { return cheat_Panel;}}

    [SerializeField] private GameObject oyunaBaglanma_Panel;
    public GameObject OyunaBaglanma_Panel { get { return oyunaBaglanma_Panel;}}

    [SerializeField] private GameObject playerProps_Panel;
    public GameObject PlayerProps_Panel {get { return playerProps_Panel;}}

    [SerializeField] private GameObject playerIcon_Panel;

    [SerializeField] private GameObject playerColor_Panel;

    [SerializeField] private Image[] playerIcons;
    public Image[] PlayerIcons { get { return playerIcons;}}

    [SerializeField] private Image[] playerColor;
    public Image[] PlayerColors { get { return playerColor;}}

    [SerializeField] private Button playerPropBaslatButton;
    [SerializeField] private TMP_InputField kullaniciAdi_InputField;
    public TMP_InputField KullaniciAdi_InputField { get { return kullaniciAdi_InputField;}}

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

    public void OyunuBaslatButton_Method()
    {
        SunucuYonetim.Instance.ConnetingServer();
        SetActiveUIObject(connecting_Panel.name);
    }

    public void PlayerPropSaveButton_Method()
    {
        playerPropBaslatButton.gameObject.SetActive(playerBaslatButtonActive);

        ExitGames.Client.Photon.Hashtable playerProps = new ExitGames.Client.Photon.Hashtable()
        {
            {"icon",iconIndex},
            {"color",colorIndex}
        };


        PhotonNetwork.LocalPlayer.SetCustomProperties(playerProps);
        print("Player Icon Index Number : "+PhotonNetwork.LocalPlayer.CustomProperties["icon"]);
    }

    public void SetActiveUIObject(string panelName)
    {
        connecting_Panel.SetActive(panelName.Equals(connecting_Panel.name));
        cheat_Panel.SetActive(panelName.Equals(cheat_Panel.name));
        oyunaBaglanma_Panel.gameObject.SetActive(panelName.Equals(oyunaBaglanma_Panel.name));
        playerProps_Panel.gameObject.SetActive(panelName.Equals(playerProps_Panel.name));

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

       

        
    }


   
}
