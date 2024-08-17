using System.Collections;
using System.Collections.Generic;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FriendListControl : MonoBehaviour
{
    [SerializeField] private Image friendIcon_Image;
    [SerializeField] private TextMeshProUGUI friendName_Text;
    [SerializeField] private Button addFriend_Button;

    
    void Start()
    {
        addFriend_Button.onClick.AddListener(()=>
        {
            print("Arkadas Eklendi");
        });
    }

    void Update()
    {
        
    }

    public void Initialize(string friendName,Player player)
    {
        friendName_Text.text = friendName;

    }
}
