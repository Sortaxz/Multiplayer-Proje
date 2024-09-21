using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    private static GameUI instance;
    public static GameUI Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<GameUI>();
            }
            return instance;
        }
    }
    private PhotonView pw;
    [SerializeField] private Image playerHealtBarBackground;
    [SerializeField] private Image playerHealtBar;
    [SerializeField] private Image otherPlayerHealtBar;
    public Image OtherPlayerHealtBar { get { return otherPlayerHealtBar;} set { otherPlayerHealtBar = value;}}


    [Header("Character's Weapon related  with Ui")]
    [SerializeField] private GameObject weapomInfo_Image;
    public GameObject WeapomInfoImage { get { return weapomInfo_Image; } set { weapomInfo_Image = value;}}
    [SerializeField] private TextMeshProUGUI currentBulletCount_Text;
    public TextMeshProUGUI CurrentBulletCount_Text { get { return currentBulletCount_Text;}}
    [SerializeField] private TextMeshProUGUI maxBulletCount_Text;

    private void Awake() 
    {
        pw = GetComponent<PhotonView>();
        playerHealtBar = playerHealtBarBackground.transform.GetChild(0).GetComponent<Image>();  
    }
    
    private void Update() 
    {
        
    }

    public void Active()
    {
        playerHealtBarBackground.gameObject.SetActive(true);
    
    }

    public float PlayerHealtBar(float damage)
    {
        HealtBar_Method(damage,playerHealtBar);
        return playerHealtBar.fillAmount;
    }

    public void HealtBar_Method(float damage,Image healtBar)
    {
        healtBar.fillAmount -= damage;
        if(healtBar.fillAmount <= 0)
        {
            healtBar.fillAmount = 1f;
        }
    }
    
   
    
    public void WeaponInformationUi(int bulletCount,int maxBulletCount)
    {
        currentBulletCount_Text.text = bulletCount.ToString();
        maxBulletCount_Text.text = maxBulletCount.ToString();
    }
   
}
 