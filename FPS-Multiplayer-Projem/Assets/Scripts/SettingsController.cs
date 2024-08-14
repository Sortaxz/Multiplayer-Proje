using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsController : MonoBehaviour
{
    private GameObject settings_Panel;

    [Header("Ekran Boyutu Ayarlama İşlemleri")]
    [SerializeField] private GameObject EkranBoyutAyarlama_Panel;
    


    private void Awake() 
    {
        DontDestroyOnLoad(gameObject);
        if(settings_Panel != null)
        {
            settings_Panel = GameObject.FindWithTag("Settings_Panel");
        }    
                    
    }


    public void EkranBoyutButton_Method()
    {
        EkranBoyutAyarlama_Panel.SetActive(!EkranBoyutAyarlama_Panel.activeSelf);    
    }

}
