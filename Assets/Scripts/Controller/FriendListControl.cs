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
        friendCount = (int)SaveSystem.Instance.PlayerPrefsDataLoad("friendCount","int");
    }

    void Start()
    {
        if(addFriend_Button != null)
        {
            addFriend_Button.onClick.AddListener(delegate
            {
                print(friendPlayer.ActorNumber);

                friendCount += 1;
                print(friendCount);

                SaveSystem.Instance.SetFriendPlayer(friendPlayer.ActorNumber,friendPlayer.NickName,friendCount);
                
                SunucuYonetim.Instance.CreatFriendObject(friendPlayer,"online",false);

                friendPlayer.CustomProperties.TryGetValue("icon",out object friendIconIndex);

                string friendInfo = $"{friendPlayer.UserId},{friendPlayer.ActorNumber},{friendIconIndex},{friendPlayer.NickName}";
                
                SunucuYonetim.Instance.Friends.Add(friendInfo);


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


    public void FriendObjectInitialize(int friendIconIndex,string friendNickName,string friendState,string friendUserId = "",int friendActorNumber = 0)
    {
        _friendIcon_Image.sprite = UIMenager.Instance.PlayerIcons[friendIconIndex].sprite;
        _friendName_Text.text = friendNickName;
        _friendState_Text.text = friendState;


    }

    
}
