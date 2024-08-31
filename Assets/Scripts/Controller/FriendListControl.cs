using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FriendListControl : MonoBehaviour
{
    [Header("FriendList objesi ile ilgili işlemler")]
    [SerializeField] private Image friendIcon_Image;
    [SerializeField] private TextMeshProUGUI friendName_Text;
    [SerializeField] private Button addFriend_Button;

    private Player friendPlayer;
    public string friendName;
    public string friendUserId;

    private int friendCount;

    [Header("Friend objesi ile ilgili işlemler")]
    [SerializeField] private Image _friendIcon_Image;
    [SerializeField] private TextMeshProUGUI _friendName_Text;
    [SerializeField] private TextMeshProUGUI _friendState_Text;
    
    private void Awake() 
    {
    }

    void Start()
    {
        if(addFriend_Button != null)
        {
            addFriend_Button.onClick.AddListener(delegate
            {
                Destroy(gameObject);

            });
        }
        
        
    }


    public void FriendListInitialize(Player player)
    {
        friendName = player.NickName;
        friendUserId = player.UserId;
        friendPlayer = player;
        friendName_Text.text = player.NickName;
        player.CustomProperties.TryGetValue("icon",out object iconIndex);

        friendIcon_Image.sprite = UIMenager.Instance.PlayerIcons[(int)iconIndex].sprite;

    }


    public void FriendObjectInitialize(FriendInfo friendInfo)
    {
        _friendState_Text.text = friendInfo.IsOnline ? "Online" : "Offline";
        _friendName_Text.text = friendInfo.UserId;
    }

    
}
