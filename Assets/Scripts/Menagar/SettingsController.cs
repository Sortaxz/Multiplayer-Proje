using System.Collections;
using System.Collections.Generic;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingsController : MonoBehaviour
{
    [SerializeField] private GameObject menu_Panel;
    [SerializeField] private GameObject Vol_1;
    [SerializeField] private GameObject Vol_2;

    [Header("Vol_1")]
    [SerializeField] private GameObject EkranBoyutAyarlama_Panel;
    [SerializeField] private GameObject ScreenSizeTuning_Panel;
    [SerializeField] private TMP_Dropdown dropdown;
    [SerializeField] private TextMeshProUGUI dropdownLabel;
    [SerializeField] private TMP_InputField screenWith_InputField;
    [SerializeField] private TMP_InputField screenHeight_InputField;
    [SerializeField] private Button SettingsKaydet_Button;
    
    [Header("Vol_2")]
    [SerializeField] private TMP_Dropdown gameSettingsDropDown;
    [SerializeField] private TextMeshProUGUI gameSettingsDropDownLabel;

    [SerializeField] private Button screenMod_Button;
    [SerializeField] private TMP_InputField screenModWith_InputField;
    [SerializeField] private TMP_InputField screenModHeight_InputField;
    [SerializeField] private GameObject screenModPanel;
    [SerializeField] private GameObject screenModSizeTuning;
    private string selectScreenSizeName;

    private int screenWith;

    private int screenHeight;

    private void Awake() 
    {
        //DontDestroyOnLoad(gameObject);
        /*
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
        */          
    }


    public void EkranBoyutButton_Method()
    {
        if(Vol_1.activeSelf)
        {
            EkranBoyutAyarlama_Panel.SetActive(!EkranBoyutAyarlama_Panel.activeSelf);  
            selectScreenSizeName = dropdownLabel.text;   
        }
        else if(Vol_2.activeSelf)
        {
            screenModPanel.SetActive(!screenModPanel.activeSelf);
        }
    }

    public void EkranBoyutuToogleButton_Method()
    {
        if(Vol_1.activeSelf)
        {
            selectScreenSizeName = dropdownLabel.text; 
            if(dropdownLabel.text != "FullScreen Window")
            {
                ScreenSizeTuning_Panel.SetActive(true);
            }
            else
            {
                ScreenSizeTuning_Panel.SetActive(false);
            }
        }
        else if(Vol_2.activeSelf)
        {
            selectScreenSizeName = gameSettingsDropDownLabel.text;
            if(gameSettingsDropDownLabel.text != "FullScreen Window")
            {
                screenModSizeTuning.SetActive(true);
            }
            else
            {
                screenModSizeTuning.SetActive(false);
            }
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
            Screen.SetResolution(screenWith,screenHeight,FullScreenMode.Windowed);
        }
        else
        {
            Screen.SetResolution(1920,1080,FullScreenMode.FullScreenWindow);
        }

        DropDownResetValues();
        
        ScreenSizeTuning_Panel.SetActive(false);


    }

    //resets the typed values in the dropdown ui
    private void DropDownResetValues()
    {
        if(Vol_1.activeSelf)
        {
            screenWith_InputField.text = "";
            screenHeight_InputField.text = "";
            dropdown.value = 0;
        }
        else if(Vol_2.activeSelf)
        {
            screenModWith_InputField.text = "";
            screenModHeight_InputField.text = "";
            gameSettingsDropDown.value = 0;
        }
    }

    public void SettingGeriDonButton_Method()
    {
        DropDownResetValues();
        gameObject.SetActive(menu_Panel.activeSelf);
        menu_Panel.SetActive(!menu_Panel.activeSelf);
        ScreenSizeTuning_Panel.SetActive(false);

    }

    
    public void GameSettingsSaveButton()
    {
        SaveSystem.PlayerPrefsDataSave("screenSize",selectScreenSizeName);

        if(gameSettingsDropDownLabel.text != "FullScreen Window")
        {
            int screenWith = int.Parse(screenModWith_InputField.text);
            int screenHeight = int.Parse(screenModHeight_InputField.text);

            SaveSystem.PlayerPrefsDataSave("screenWith",screenWith);
            SaveSystem.PlayerPrefsDataSave("screenHeight",screenWith);
            Screen.SetResolution(screenWith,screenHeight,FullScreenMode.Windowed);
        }
        else
        {
            Screen.SetResolution(1920,1080,FullScreenMode.FullScreenWindow);
        }

        DropDownResetValues();

        screenModSizeTuning.SetActive(false);
        screenModPanel.SetActive(false);
        
    }
}
