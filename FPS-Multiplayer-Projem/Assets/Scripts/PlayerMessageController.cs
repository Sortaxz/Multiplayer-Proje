using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMessageController : MonoBehaviour
{
    [SerializeField] private Image playerIcon_Image;
    [SerializeField] private TextMeshProUGUI kullaniciAdi_Text;
    [SerializeField] private TextMeshProUGUI message_Text;
    public void Intialize(string playerName,string message)
    {
        kullaniciAdi_Text.text = playerName;
        message_Text.text = message;
    }
}
