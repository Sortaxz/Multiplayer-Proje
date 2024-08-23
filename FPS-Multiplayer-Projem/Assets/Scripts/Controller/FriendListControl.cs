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

    [Header("Friend objesi ile ilgili işlemler")]
    [SerializeField] private Image _friendIcon_Image;
    [SerializeField] private TextMeshProUGUI _friendName_Text;
    [SerializeField] private TextMeshProUGUI _friendState_Text;
    

    private int friendCount = 0;

    void Start()
    {
        if(addFriend_Button != null)
        {
            addFriend_Button.onClick.AddListener(delegate
            {
                SaveSystem.Instance.SetFriendPlayer(friendPlayer);
                SunucuYonetim.Instance.CreatFriendObject(friendPlayer,"online",false);


                Destroy(gameObject);
            });
        }
        
    }

    void Update()
    {
        
    }

    public void FriendListInitialize(Player player)
    {
        friendName = player.NickName;
        friendPlayer = player;
        friendName_Text.text = player.NickName;
        player.CustomProperties.TryGetValue("icon",out object iconIndex);

        friendIcon_Image.sprite = UIMenager.Instance.PlayerIcons[(int)iconIndex].sprite;

    }

    
    public void FriendObjectInitialize(Player friend,string friendState)
    {
        friend.CustomProperties.TryGetValue("icon",out object iconIndex);
        _friendIcon_Image.sprite = UIMenager.Instance.PlayerIcons[(int)iconIndex].sprite;
        _friendName_Text.text = friend.NickName;
        _friendState_Text.text = friendState;


    }
}
