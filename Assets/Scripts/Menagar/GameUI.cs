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
    [SerializeField] private GameObject skorTableContent;
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
        InputControl();
    }
    
    private void InputControl()
    {
        if(uiEsc)
        {   
            GameManager.Instance.GameStopted = !pausePanel.activeSelf;
            print(GameManager.Instance.GameStopted);
            GameManager.Instance.StopGameStreaming(!pausePanel.activeSelf);
            pausePanel.SetActive(!pausePanel.activeSelf);
            
        }
       

        if(tab)
        {
            skorTable_Panel.SetActive(!skorTable_Panel.activeSelf);
            GameManager.Instance.GameStopted = skorTable_Panel.activeSelf;
            GameManager.Instance.StopGameStreaming(skorTable_Panel.activeSelf);

            GameManager.Instance.PlayerSkorUpdate();
        }


    }

    public void Active()
    {
        playerHealtBarBackground.gameObject.SetActive(true);
    
    }
    public void Close()
    {
        if(playerHealtBarBackground) //null mı değil mi sorusu
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
        //GameManager.Instance.StopGameStreaming(true);
        GameManager.Instance.CharacterDead = true;
    }


    public void ReturnMainMenu()
    {
        GameManager.Instance.GameOut();
    }

    public void BackGame()
    {
        pausePanel.SetActive(false);
        GameManager.Instance.GameStopted = false;
        GameManager.Instance.StopGameStreaming(false);
    }


    public  void CreateSkorLine(int playerIconIndex,string playerGameObjectName,int playerKillCount,int playerDeathCount)
    {
        GameObject spanwObject = Instantiate(skorLinePrefab,Vector3.one,Quaternion.identity,skorTableContent.transform);
        spanwObject.name = playerGameObjectName;
        GameManager.Instance.SkorLines.Add(playerGameObjectName,spanwObject.GetComponent<SkorLineControl>());    
        spanwObject.GetComponent<SkorLineControl>().PlayerSkorInitialize(playerIconIndex,playerGameObjectName,playerKillCount,playerDeathCount);
    }
}
 