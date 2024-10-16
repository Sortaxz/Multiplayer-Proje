using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerListControl : MonoBehaviour
{
    [SerializeField] private Image playerListIcon_Image;
    [SerializeField] private TextMeshProUGUI playerListName_Text;
    [SerializeField] private Button addFriend_Button;
    private ExitGames.Client.Photon.Hashtable playerProp = new ExitGames.Client.Photon.Hashtable();
    private int playerId;
    private string playerNickName;
    private Player addedFriend; 
   
    void Start()
    {
        if(PhotonNetwork.LocalPlayer.ActorNumber != playerId && !FriendSystem.Instance.FriendList.Contains(playerNickName))
        {
            addFriend_Button.onClick.AddListener(()=>
            {   
                addedFriend.CustomProperties.TryGetValue("icon",out object _friendIconIndex);
                
                string friendUserId = addedFriend.UserId;
                int friendIconIndex = (int)_friendIconIndex;


                //FriendSystem.Instance.AddFriend(addedFriend.UserId, friendIconIndex.ToString());

                FriendSystem.Instance.SendFriendRequest(addedFriend);
                
                //addFriend_Button.gameObject.SetActive(false);
            });
        }
        else
        {
            addFriend_Button.gameObject.SetActive(false);
        }
    }

    
    public void Initialize(int oyuncuId,string oyuncuName,Player player)
    {
        addedFriend = player;
        playerId = oyuncuId;
        playerListName_Text.text = oyuncuName;
        player.CustomProperties.TryGetValue("icon",out object iconIndex);
        playerListIcon_Image.sprite = UIMenager.Instance.PlayerScriptableObject.PlayerIconSprites[(int)iconIndex];
        playerNickName = oyuncuName;
    }

    public void CloseAddFrienButtonActive()
    {
        addFriend_Button.gameObject.SetActive(false);
    }
    
   
}
