using System;
using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon;
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
    
    private const byte FriendRequestEventCode = 1;
    private const byte FriendRequestResponseEventCode = 2;
    
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


    public void FriendListInitialize(string frienNickName,int friendIconIndex)
    {
        friendIcon_Image.sprite = UIMenager.Instance.PlayerIcons[friendIconIndex].sprite;
        friendName_Text.text = frienNickName;

    }


    public void FriendObjectInitialize(FriendInfo friendInfo,int friendIconIndex)
    {
        _friendState_Text.text = friendInfo.IsOnline ? "Online" : "Offline";
        _friendName_Text.text = friendInfo.UserId;
        _friendIcon_Image.sprite = UIMenager.Instance.PlayerIcons[friendIconIndex].sprite;
    }

   
}
