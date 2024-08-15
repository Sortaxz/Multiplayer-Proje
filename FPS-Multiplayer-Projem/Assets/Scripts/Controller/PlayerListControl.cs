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
    [SerializeField] private Button playerListHazir_Button;
    private ExitGames.Client.Photon.Hashtable playerProp = new ExitGames.Client.Photon.Hashtable();
    private int playerId;
    private bool isPlayerReady = false;
    
   
    void Start()
    {
        if(PhotonNetwork.LocalPlayer.ActorNumber != playerId)
        {
            playerListHazir_Button.interactable = false;
        }
        else
        {
            playerListHazir_Button.interactable = true;
            playerProp.Add("isPlayerReady",isPlayerReady);
            playerProp["isPlayerReady"] = isPlayerReady;
            
            PhotonNetwork.LocalPlayer.SetCustomProperties(playerProp);

            playerListHazir_Button.onClick.AddListener(()=>
            {
                isPlayerReady = !isPlayerReady;

                SetPlayerReady(isPlayerReady);

                playerProp["isPlayerReady"] = isPlayerReady;

                PhotonNetwork.LocalPlayer.SetCustomProperties(playerProp);

                if(PhotonNetwork.IsMasterClient)
                {
                    UIMenager.Instance.LocalPlayerPropertiesUpdated();
                }
            });

        }
    }

    
    public void Initialize(int oyuncuId,string oyuncuName,Player player)
    {
        playerId = oyuncuId;
        playerListName_Text.text = oyuncuName;
        player.CustomProperties.TryGetValue("icon",out object iconIndex);
        playerListIcon_Image.sprite = UIMenager.Instance.PlayerIcons[(int)iconIndex].sprite;
    }

    
    public void SetPlayerReady(bool playerReady)
    {
        playerListHazir_Button.transform.GetComponentInChildren<TextMeshProUGUI>().text  = playerReady ? "Ok!" : "Hazir";
        playerListHazir_Button.GetComponent<Image>().color = playerReady ? Color.green : Color.white;
        playerListHazir_Button.transform.GetComponentInChildren<TextMeshProUGUI>().color = playerReady ? Color.white : Color.black;

    }
}
