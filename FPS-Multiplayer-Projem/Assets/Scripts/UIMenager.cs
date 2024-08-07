using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
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

    [SerializeField] private TMP_InputField kullaniciAdi_InputField;
    public TMP_InputField KullaniciAdi_InputField { get { return kullaniciAdi_InputField;}}

    private void Awake() 
    {
        SetActiveUIObject(oyunaBaglanma_Panel.gameObject.name);    
    }

    public void OyunaBaslaButton_Method()
    {
        SunucuYonetim.Instance.ConnetingServer();
        SetActiveUIObject(connecting_Panel.name);

    }

    public void SetActiveUIObject(string panelName)
    {
        connecting_Panel.SetActive(panelName.Equals(connecting_Panel.name));
        cheat_Panel.SetActive(panelName.Equals(cheat_Panel.name));
        oyunaBaglanma_Panel.gameObject.SetActive(panelName.Equals(oyunaBaglanma_Panel.name));

    }
}
