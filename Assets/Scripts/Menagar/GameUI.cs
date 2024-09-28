using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : UIInputManager
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

    [Header("Timer Related Operations ")]
    [SerializeField] private GameObject timerPanel;
    public GameObject TimerPanel { get { return timerPanel;}}
    [SerializeField] private TextMeshProUGUI timerText;
    public TextMeshProUGUI TimerText { get { return timerText; }}

    [Header("in-game interaction panels")]
    [SerializeField] private GameObject finishPanel;
    [SerializeField] private Button returnMainMenu_Button;

    [SerializeField] private GameObject pausePanel;
    [SerializeField] private Button exitGame_Button;
    [SerializeField] private Button returnGame_Button;
        
    [Header("operations related to the scoreboard")]
    [SerializeField] private GameObject skorTable_Panel;
    [SerializeField] private GameObject skorLinePrefab;

    private void Awake() 
    {
        playerHealtBar = playerHealtBarBackground.transform.GetChild(0).GetComponent<Image>();  
        GameManager.deatDelegate += Close;

    }
    private void Start() 
    {
        
    }
    
    private void Update() 
    {
        PauseControl();
    }
    
    private void PauseControl()
    {
        if(uiEsc)
        {   
            pausePanel.SetActive(!pausePanel.activeSelf);
            
            GameManager.Instance.GameStopted = pausePanel.activeSelf;
            GameManager.Instance.StopGameStreaming(false);
        }
        
    }
    private void SkorShow()
    {

    }

    public void Active()
    {
        playerHealtBarBackground.gameObject.SetActive(true);
    
    }
    public void Close()
    {
        playerHealtBarBackground.gameObject.SetActive(false);
        otherPlayerHealtBar.gameObject.SetActive(false);
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
    
    public void GameOverUi()
    {
        finishPanel.SetActive(true);
        GameManager.Instance.StopGameStreaming(true);
    }


    public void ReturnMainMenu()
    {
        GameManager.Instance.GameOut();
    }
}
 