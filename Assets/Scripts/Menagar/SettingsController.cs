using System.Collections;
using System.Collections.Generic;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingsController : MonoBehaviour
{
    [SerializeField] private GameObject menu_Panel;

    [Header("Ekran Boyutu Ayarlama İşlemleri")]
    [SerializeField] private GameObject EkranBoyutAyarlama_Panel;
    [SerializeField] private GameObject ScreenSizeTuning_Panel;
    [SerializeField] private TMP_Dropdown dropdown;
    [SerializeField] private TextMeshProUGUI dropdownLabel;
    [SerializeField] private TMP_InputField screenWith_InputField;
    [SerializeField] private TMP_InputField screenHeight_InputField;
    [SerializeField] private Button SettingsKaydet_Button;
    
    private SaveSystem saveSystem;

    private string selectScreenSizeName;

    private int screenWith;

    private int screenHeight;


    private void Awake() 
    {
        DontDestroyOnLoad(gameObject);
       
        if(SaveSystem.PlayerPrefsDataQuery("screenSize"))
        {
            selectScreenSizeName = (string)SaveSystem.PlayerPrefsDataLoad("screenSize","string");
            
            if(selectScreenSizeName == "FullScreen Window")
            {
                Screen.SetResolution(1920,1080,FullScreenMode.FullScreenWindow);
            }
            else
            {
                screenWith = (int)SaveSystem.PlayerPrefsDataLoad("screenWith","int"); 
                screenHeight = (int)SaveSystem.PlayerPrefsDataLoad("screenHeight","int"); 
                Screen.SetResolution(screenWith,screenHeight,FullScreenMode.Windowed);
            }
        }           
    }


    public void EkranBoyutButton_Method()
    {
        EkranBoyutAyarlama_Panel.SetActive(!EkranBoyutAyarlama_Panel.activeSelf);  
        selectScreenSizeName = dropdownLabel.text;   
    }

    public void EkranBoyutuToogleButton_Method()
    {
        selectScreenSizeName = dropdownLabel.text; 
        print(selectScreenSizeName);
        
        if(dropdownLabel.text != "FullScreen Window")
        {
            ScreenSizeTuning_Panel.SetActive(true);
        }
        else
        {
            ScreenSizeTuning_Panel.SetActive(false);
        }
    }

    public void SettingsKaydetButton_Method()
    {
        gameObject.SetActive(menu_Panel.activeSelf);
        menu_Panel.SetActive(!menu_Panel.activeSelf);

        SaveSystem.PlayerPrefsDataSave("screenSize",selectScreenSizeName);


        if(selectScreenSizeName != "FullScreen Window")
        {
            int screenWith = int.Parse(screenWith_InputField.text);
            int screenHeight = int.Parse(screenHeight_InputField.text);

            SaveSystem.PlayerPrefsDataSave("screenWith",screenWith);
            SaveSystem.PlayerPrefsDataSave("screenHeight",screenWith);
            print("Windowed");
            Screen.SetResolution(screenWith,screenHeight,FullScreenMode.Windowed);
        }
        else
        {
            print("FullScreen Window");
            Screen.SetResolution(1920,1080,FullScreenMode.FullScreenWindow);
        }
        ScreenSizeTuning_Panel.SetActive(false);
    }

    public void SettingGeriDonButton_Method()
    {
        gameObject.SetActive(menu_Panel.activeSelf);
        menu_Panel.SetActive(!menu_Panel.activeSelf);
        ScreenSizeTuning_Panel.SetActive(false);
    }

}
