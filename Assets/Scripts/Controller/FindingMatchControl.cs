using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FindingMatchControl : MonoBehaviourPunCallbacks, IPunObservable
{   
    private static FindingMatchControl instance;
    public static FindingMatchControl Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<FindingMatchControl>();
            }
            return instance;    
        }
    }

    [SerializeField] private TextMeshProUGUI matchProps_Text;
    [SerializeField] private TextMeshProUGUI findMatchTime_Text;
    public TextMeshProUGUI FindMatchTime_Text { get { return findMatchTime_Text; } set { findMatchTime_Text = value; } }
    [SerializeField] private TextMeshProUGUI findMatchYazisi_Text;
    [SerializeField] private Button startMatchFinding_Button;
    public Button  StartMatchFinding_Button { get { return startMatchFinding_Button; } }
    [SerializeField] private Button cancelMatchFinding_Button;
    private ExitGames.Client.Photon.Hashtable playerProps;
    private bool gameStarted = false; 
    public bool FindMatchGameStarted {get { return gameStarted;} set { gameStarted = value; } }
    private bool readyCallMatch = false;
    private bool findRoomGameMode = false;
    private float deger = 0;
    PhotonView PV;
    
    private void Awake() 
    {
        //IsPlayerReady = true;
        PV = GetComponent<PhotonView>();
       
    }

    void Start()
    {
        
        if (!PhotonNetwork.IsMasterClient)
        {
            startMatchFinding_Button.gameObject.SetActive(false); 
            cancelMatchFinding_Button.gameObject.SetActive(false); 
        }
        
        findMatchTime_Text.gameObject.SetActive(true);

        Initialize(deger,false);
    }

    private void Update() 
    {
        if(findRoomGameMode)
        {
            Initialize(deger,true);
        }
        
    }

    public void StartMatchFindingButton_Method()
    {
        
        PV.RPC("RPC_StartFindMatch", RpcTarget.MasterClient, null); 
        gameStarted = true;
        UpdateButtons();
    }

    public void CancelMatchFindingButton_Method()
    {
        if (gameStarted)
        {
            PV.RPC("RPC_CancelFindMatch", RpcTarget.AllBuffered, null); 
        }
    }

    public void RoomGameModeShow()
    {
        if(PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue("gameMode",out object gameMode))
        {
            matchProps_Text.text = ((int)gameMode == 1? GameMode.Dereceli : GameMode.Derecesiz).ToString();
        }
    }

    public void Initialize(float waitTimeValue,bool counterStart)
    {
        if(PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue("gameMode",out object gameMode))
        {
            string mod = ((int)gameMode == 1? GameMode.Dereceli : GameMode.Derecesiz).ToString();
            matchProps_Text.text = ((int)gameMode == 1? GameMode.Dereceli : GameMode.Derecesiz).ToString();
            findRoomGameMode = mod == matchProps_Text.text ? true : false;
        }

       
       

        if(counterStart)
        {
            int minutes = Mathf.FloorToInt(waitTimeValue / 60);
            int seconds = Mathf.FloorToInt(waitTimeValue % 60);
            findMatchTime_Text.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
        else
        {
            findMatchTime_Text.gameObject.SetActive(false);
        }
    }

    public void FindingButtonActive()
    {
        if(SunucuYonetim.Instance.PlayerIsMasterClient())
        {
            startMatchFinding_Button.gameObject.SetActive(true);
            cancelMatchFinding_Button.gameObject.SetActive(false);
        }
        else
        {
            startMatchFinding_Button.gameObject.SetActive(false);
            cancelMatchFinding_Button.gameObject.SetActive(false);
        }
    }
   

    public void UpdateButtons()
    {
        if (gameStarted)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                startMatchFinding_Button.gameObject.SetActive(false);
                cancelMatchFinding_Button.gameObject.SetActive(true); 
            }
            else
            {
                startMatchFinding_Button.gameObject.SetActive(false);
                cancelMatchFinding_Button.gameObject.SetActive(true); 
            }
            findMatchTime_Text.gameObject.SetActive(gameStarted);
        }
        else
        {
            startMatchFinding_Button.gameObject.SetActive(PhotonNetwork.IsMasterClient);
            cancelMatchFinding_Button.gameObject.SetActive(false);
            findMatchTime_Text.gameObject.SetActive(gameStarted);
        }

        
    }

    [PunRPC]
    private void RPC_StartFindMatch()
    {
        if(gameObject.activeSelf)
        {
            readyCallMatch = true;
            gameStarted = true;
            UpdateButtons();
            StartCoroutine(MatchTime());
        }
    }

    [PunRPC]
    private void RPC_CancelFindMatch()
    {
        readyCallMatch = false;
        StopCoroutine(MatchTime());
        deger = 0;
        Initialize(deger,false);
        gameStarted = false;
        UpdateButtons();

    }

    public IEnumerator MatchTime()
    {
        while (gameStarted && gameObject.activeSelf)
        {
            yield return null;

            if (PhotonNetwork.PlayerList.Length != PhotonNetwork.CurrentRoom.MaxPlayers)
            {
                deger += Time.deltaTime;
                Initialize(deger,true);
            }   
            else
            {
                deger = 0;
                Initialize(deger,false);
                gameStarted = false;
                if(readyCallMatch)
                {
                    PV.RPC("FinedMatch",RpcTarget.AllViaServer,true);
                }
            }
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.Serialize(ref deger);
            stream.Serialize(ref gameStarted);
        }
        else
        {
            float sayi = (float)stream.ReceiveNext();
            deger = sayi;
            Initialize(sayi,true);
            gameStarted = (bool)stream.ReceiveNext();
            UpdateButtons();
        }
    }


    [PunRPC]
    public void FinedMatch(bool open)
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            startMatchFinding_Button.gameObject.SetActive(false); 
            cancelMatchFinding_Button.gameObject.SetActive(false); 
        }
        else
        {
            startMatchFinding_Button.gameObject.SetActive(true); 
            cancelMatchFinding_Button.gameObject.SetActive(false); 
        }
        
        string karşilaşmaKabulReddetPanelName = UIMenager.Instance.KarşilaşmaKabulReddet_Panel.name;
        UIMenager.Instance.SetActiveUIObject(karşilaşmaKabulReddetPanelName);
        
    }

    public void FindMatchButtonsClose(PhotonView pv)
    {
        PV.RPC("RPC_FindMatchButtonsClose",RpcTarget.AllViaServer,pv);
        matchProps_Text.text = "Maç Özellikleri";
    }

    [PunRPC]
    private void RPC_FindMatchButtonsClose(PhotonView pv)
    {
        deger = 0;
        int minutes = Mathf.FloorToInt(deger / 60);
        int seconds = Mathf.FloorToInt(deger % 60);
        findMatchTime_Text.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        
        findMatchTime_Text.gameObject.SetActive(false);
        
        cancelMatchFinding_Button.gameObject.SetActive(false); 
        startMatchFinding_Button.gameObject.SetActive(true); 

        
    }
}