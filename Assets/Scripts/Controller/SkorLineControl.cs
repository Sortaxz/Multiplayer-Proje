using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkorLineControl : MonoBehaviour
{
    [SerializeField] private Image playerIcon_Image;
    [SerializeField] private TextMeshProUGUI playerName_Text;
    [SerializeField] private TextMeshProUGUI playerKillSkor_Text;
    [SerializeField] private TextMeshProUGUI playerDeathSkor_Text;
    [SerializeField] private PlayerScriptableObject playerScriptableObject;
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void PlayerSkorInitialize(int playerIconIndex,string playerName,int playerKillCount,int playerDeathCount)
    {
        playerIcon_Image.sprite = playerScriptableObject.PlayerIconSprites[playerIconIndex];
        playerName_Text.text = playerName;
        playerKillSkor_Text.text = playerKillCount.ToString();
        playerDeathSkor_Text.text = playerDeathCount.ToString();
    }

    public void PlayerSkor(int playerKillCount,int playerDeathCount)
    {
        playerKillSkor_Text.text = playerKillCount.ToString();
        playerDeathSkor_Text.text = playerDeathCount.ToString();

    }
}
