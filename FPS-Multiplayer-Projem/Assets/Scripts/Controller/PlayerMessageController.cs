using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMessageController : MonoBehaviour
{
    [SerializeField] private Image playerIcon_Image;
    [SerializeField] private TextMeshProUGUI kullaniciAdi_Text;
    [SerializeField] private TextMeshProUGUI message_Text;
    public void Intialize(string playerName,string message,Player player)
    {
        kullaniciAdi_Text.text = playerName;
        message_Text.text = message;
        //playerIcon_Image.sprite = UIMenager.Instance.PlayerIcons[(int)icon].sprite;
        player.CustomProperties.TryGetValue("icon", out object icon);
        playerIcon_Image.sprite = UIMenager.Instance.PlayerIcons[(int)icon].sprite;
    }
}
