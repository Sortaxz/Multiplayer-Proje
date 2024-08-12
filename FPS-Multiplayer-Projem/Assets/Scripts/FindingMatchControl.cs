using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FindingMatchControl : MonoBehaviourPunCallbacks,IPunObservable
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
    public TextMeshProUGUI FindMatchTime_Text {get {return matchProps_Text;}set {matchProps_Text = value;}}
    [SerializeField] private TextMeshProUGUI findMatchYazisi_Text;
    [SerializeField] private Button startMatchFinding_Button;
    [SerializeField] private Button cancelMatchFinding_Button;
    private ExitGames.Client.Photon.Hashtable playerProps;
    private bool IsPlayerReady = true;
    private bool value = false;
    private float deger = 0;
    PhotonView PV;
    private void Awake() 
    {
        PV = GetComponent<PhotonView>();
        PhotonNetwork.CurrentRoom.IsOpen = false;
    }

    void Start()
    {
        if(!PhotonNetwork.IsMasterClient)
        {
            SetButtonActive(cancelMatchFinding_Button.name);

        }
    }

    void Update()
    {
        
    }
    
    public void StartMatchFindingButton_Method()
    {
        IsPlayerReady = false;
        PhotonNetwork.CurrentRoom.IsOpen = true;
        playerProps = new ExitGames.Client.Photon.Hashtable()
        {
            {"IsPlayerReady",IsPlayerReady}
        };

        PhotonNetwork.LocalPlayer.SetCustomProperties(playerProps);

        PV.RPC("RPC_StartFindMatch",RpcTarget.MasterClient,null);
        SetButtonActive(cancelMatchFinding_Button.name);
        if(PhotonNetwork.IsMasterClient)
        {
            SetButtonActive(cancelMatchFinding_Button.name);
        }
    }

    public void CancelMatchFindingButton_Method()
    {
        IsPlayerReady = true;
        playerProps = new ExitGames.Client.Photon.Hashtable()
        {
            {"IsPlayerReady",IsPlayerReady}
        };

        PhotonNetwork.LocalPlayer.SetCustomProperties(playerProps);

        PV.RPC("RPC_StartFindMatch",RpcTarget.MasterClient,null);
        
        if(PhotonNetwork.IsMasterClient)
        {
            SetButtonActive(startMatchFinding_Button.name);

        }
        else
        {
            SetButtonActive("");
        }
    }


    public void Initialize(float waitTimeValue)
    {
        int minutes = Mathf.FloorToInt(waitTimeValue / 60);
        int seconds = Mathf.FloorToInt(waitTimeValue % 60);
        findMatchTime_Text.text = string.Format("{0:00}:{1:00}", minutes, seconds);

    }

    private void SetButtonActive(string buttonName)
    {
        startMatchFinding_Button.gameObject.SetActive(buttonName.Equals(startMatchFinding_Button.name));
        cancelMatchFinding_Button.gameObject.SetActive(buttonName.Equals(cancelMatchFinding_Button.name));
    }

    [PunRPC]
    private void RPC_StartFindMatch()
    {
        StartCoroutine(deneme());
    }


    public IEnumerator deneme()
    {
        
        if(PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue("IsPlayerReady",out object isPlayerReady))
        {
            if(!(bool)isPlayerReady)
            {
                value = true;
            }
            else
            {
                value = false;
            }
        }
        


        while(true && !value)
        {
            yield return null;

            if(PhotonNetwork.PlayerList.Length != PhotonNetwork.CurrentRoom.MaxPlayers)
            {
                
                deger += Time.deltaTime;
                Initialize(deger);
            }   
            else
            {
                deger = 0;
                value = true;
            }


            
           
        }
        yield return null;
    }
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if(stream.IsWriting)
        {
            stream.Serialize(ref deger);
        }
        else
        {
            float sayi = (float)stream.ReceiveNext();
            Initialize(sayi);
            
        }

    }

}
