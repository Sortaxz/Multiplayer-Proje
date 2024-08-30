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
    
   
    void Start()
    {
        
    }

    
    public void Initialize(int oyuncuId,string oyuncuName,Player player)
    {
        playerId = oyuncuId;
        playerListName_Text.text = oyuncuName;
        player.CustomProperties.TryGetValue("icon",out object iconIndex);
        playerListIcon_Image.sprite = UIMenager.Instance.PlayerIcons[(int)iconIndex].sprite;
    }

    
   
}
