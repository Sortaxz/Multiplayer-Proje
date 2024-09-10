using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerPropImageControl : MonoBehaviour
{
    [SerializeField] private Button selectButton;
    void Start()
    {
        selectButton.onClick.AddListener(()=>
        {
            UIMenager.Instance.PlayerPropButton_Method();
            UIMenager.Instance.PlayerPropButton_Method2();
        });
    }

    
}
