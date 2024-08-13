using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
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
    public TextMeshProUGUI FindMatchTime_Text { get { return matchProps_Text; } set { matchProps_Text = value; } }
    [SerializeField] private TextMeshProUGUI findMatchYazisi_Text;
    [SerializeField] private Button startMatchFinding_Button;
    [SerializeField] private Button cancelMatchFinding_Button;
    private ExitGames.Client.Photon.Hashtable playerProps;
    private bool IsPlayerReady = false;
    private bool gameStarted = false; // Oyun başlatıldığında true olur
    private float deger = 0;
    PhotonView PV;
    
    private void Awake() 
    {
        PV = GetComponent<PhotonView>();
    }

    void Start()
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            startMatchFinding_Button.gameObject.SetActive(false); // Oyun kurucu değilse başlatma butonu gizli
            cancelMatchFinding_Button.gameObject.SetActive(false); // Oyun başlamadan durdurma butonu gizli
        }
    }

    void Update()
    {
        PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue("IsPlayerReady", out object isPlayerReady);
        print("IsPlayerReady : " + isPlayerReady);
    }
    
    public void StartMatchFindingButton_Method()
    {
        IsPlayerReady = false;
        playerProps = new ExitGames.Client.Photon.Hashtable()
        {
            { "IsPlayerReady", IsPlayerReady }
        };

        PhotonNetwork.LocalPlayer.SetCustomProperties(playerProps);
        PV.RPC("RPC_StartFindMatch", RpcTarget.MasterClient, null); // Tüm oyunculara güncelleme gönder
        gameStarted = true;
        UpdateButtons();
    }

    public void CancelMatchFindingButton_Method()
    {
        if (gameStarted)
        {
            // Oyunu durdurma işlemi
            PV.RPC("RPC_CancelFindMatch", RpcTarget.AllBuffered, null); // Tüm oyunculara iptal güncellemesi gönder
        }
    }

    public void Initialize(float waitTimeValue)
    {
        int minutes = Mathf.FloorToInt(waitTimeValue / 60);
        int seconds = Mathf.FloorToInt(waitTimeValue % 60);
        findMatchTime_Text.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    private void UpdateButtons()
    {
        if (gameStarted)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                startMatchFinding_Button.gameObject.SetActive(false);
                cancelMatchFinding_Button.gameObject.SetActive(true); // Başlatıldıktan sonra kurucu için durdurma butonu görünür
            }
            else
            {
                startMatchFinding_Button.gameObject.SetActive(false);
                cancelMatchFinding_Button.gameObject.SetActive(true); // Başlatıldıktan sonra sonradan gelen için durdurma butonu görünür
            }
        }
        else
        {
            startMatchFinding_Button.gameObject.SetActive(PhotonNetwork.IsMasterClient);
            cancelMatchFinding_Button.gameObject.SetActive(false);
        }
    }

    [PunRPC]
    private void RPC_StartFindMatch()
    {
        gameStarted = true;
        UpdateButtons();
        StartCoroutine(deneme());
    }

    [PunRPC]
    private void RPC_CancelFindMatch()
    {
        StopCoroutine(deneme());
        deger = 0;
        Initialize(deger);
        gameStarted = false;
        UpdateButtons();
    }

    public IEnumerator deneme()
    {
        while (gameStarted)
        {
            yield return null;

            if (PhotonNetwork.PlayerList.Length != PhotonNetwork.CurrentRoom.MaxPlayers)
            {
                deger += Time.deltaTime;
                Initialize(deger);
            }   
            else
            {
                deger = 0;
                Initialize(deger);
                gameStarted = true;
                PV.RPC("FinedMatch",RpcTarget.AllViaServer);
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
            Initialize(sayi);
            gameStarted = (bool)stream.ReceiveNext();
            UpdateButtons();
        }
    }


    [PunRPC]
    public void FinedMatch()
    {
        UIMenager.Instance.SetActiveUIObject(UIMenager.Instance.KarşilaşmaKabulReddet_Panel.name);
    }
}