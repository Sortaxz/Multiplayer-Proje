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
    [SerializeField] private Button removeFriend_Button;
    private int friendActorNumber;
    public int FriendActorNumber { get { return friendActorNumber; } set { friendActorNumber = value; } }

    [Header("Friend objesi ile ilgili işlemler")]
    [SerializeField] private Image _friendIcon_Image;
    [SerializeField] private TextMeshProUGUI _friendName_Text;
    [SerializeField] private TextMeshProUGUI _friendState_Text;
    
    [Space]
    [Space]

    [Header("Friend isteği ile ilgili işlemler")]
    [SerializeField] private TextMeshProUGUI friendRequest_Text;
    [SerializeField] private Button friendRequesAccept_Button;
    [SerializeField] private Button friendRequesDecline_Button;

    private bool addedFriend = false;
    

    private int friendIconIndex = -1;

    private void Awake() 
    {
        if(gameObject.CompareTag("FriendAcceptOrReject_Panel"))
        {
            //print("Arakaşlik isteği paneli aktif");

            friendRequesAccept_Button.onClick.AddListener(delegate
            {
                //print("Arkadaş isteğ kabul edildi artik arkadaşsiniz..");

                addedFriend = true;

                FriendSystem.Instance.AddFriend_Method();
               


                gameObject.SetActive(false);

            });

            friendRequesDecline_Button.onClick.AddListener(delegate
            {
                

                gameObject.SetActive(false);

            });
        }
        
       

        
    }


    public void FriendListInitialize(string frienNickName,int _friendIconIndex)
    {
        friendIcon_Image.sprite = UIMenager.Instance.PlayerScriptableObject.PlayerIconSprites[_friendIconIndex];
        friendName_Text.text = frienNickName;
        friendIconIndex = _friendIconIndex;
    }


    public void FriendObjectInitialize(FriendInfo friendInfo,int friendIconIndex)
    {
        _friendState_Text.text = friendInfo.IsOnline ? "Online" : "Offline";
        _friendName_Text.text = friendInfo.UserId;
        _friendIcon_Image.sprite = UIMenager.Instance.PlayerScriptableObject.PlayerIconSprites[friendIconIndex];
    }

    public void FriendRequestInitialize(string friendRequest)
    {
        if(gameObject.activeSelf)
        {
            friendRequest_Text .text = friendRequest;
        }
    }
    
    public void RemoveFriendButton_Method()
    {
        FriendSystem.Instance.UnFriend(friendName_Text.text,friendIconIndex.ToString());
        UIMenager.Instance.RemoveFriendList(friendName_Text.text);
    }


    
}
