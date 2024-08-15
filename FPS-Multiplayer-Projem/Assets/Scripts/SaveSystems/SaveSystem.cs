using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveSystem : MonoBehaviour
{
    private static SaveSystem instance;
    public static SaveSystem Instance
    {
        get
        {
            if(instance == null)
            {
                instance = FindObjectOfType<SaveSystem>();
            }
            return instance;
        }
    }

    private void Awake() 
    {
        DontDestroyOnLoad(gameObject);    
    }


    public void PlayerPrefsDataSave(string saveVariableName,object value)
    {
        if(value is int valueIsInt)
        {
            PlayerPrefs.SetInt(saveVariableName,valueIsInt);    
        }
        else if(value is float valueIsFloat)
        {
            PlayerPrefs.SetFloat(saveVariableName,valueIsFloat);
        }
        else if(value is string valueIsString)
        {
            PlayerPrefs.SetString(saveVariableName,valueIsString);
        }
        
    }

    public object PlayerPrefsDataLoad(string loadVariableName,string returnValue)
    {
        object data = null;
        switch(returnValue)
        {
            case "int":
            data = PlayerPrefs.GetInt(loadVariableName);
            break;

            case "float":
            data = PlayerPrefs.GetFloat(loadVariableName);
            break;

            case "string":
            data = PlayerPrefs.GetString(loadVariableName);
            break;

            default:
            break;
        }

        return data;
    }

    public bool PlayerPrefsDataQuery(string firstDataQueryName )
    {
        return PlayerPrefs.HasKey(firstDataQueryName);
    }
    
}
