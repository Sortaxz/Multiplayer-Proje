using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FriendListControl : MonoBehaviour
{
    [SerializeField] private Image friendIcon_Image;
    [SerializeField] private TextMeshProUGUI friendName_Text;
    [SerializeField] private Button addFriend_Button;

    private Player friendPlayer;
    public string friendName;
    void Start()
    {
        addFriend_Button.onClick.AddListener(delegate
        {
            SaveSystem.Instance.SetFriendPlayer(friendPlayer);
        });
        
    }

    void Update()
    {
        
    }

    public void Initialize(Player player)
    {
        friendName = player.NickName;
        friendPlayer = player;
        friendName_Text.text = player.NickName;
        player.CustomProperties.TryGetValue("icon",out object iconIndex);

        friendIcon_Image.sprite = UIMenager.Instance.PlayerIcons[(int)iconIndex].sprite;

    }
    
}
