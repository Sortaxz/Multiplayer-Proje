using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CheatController : MonoBehaviourPunCallbacks
{
    private PhotonView PV;
    [SerializeField] private HandleControl handleControl;
    private List<GameObject> playerMessageObjects = new List<GameObject>();
    public List<GameObject> PlayerMessageObjects { get { return playerMessageObjects; } }

    [SerializeField] private GameObject handle;
    public GameObject Handle { get { return handle;} set { handle = value; } }
    
    [SerializeField] private GameObject playerMessage_Prefab;
    [SerializeField] private GameObject playerMessagePrefab_Content;
    [SerializeField] private TMP_InputField message_InputField;
    [SerializeField] private TextMeshProUGUI cheatMessage_Text;
    [SerializeField] private TextMeshProUGUI kullaniciAdiText;


    private void Awake() 
    {
        PV = GetComponent<PhotonView>(); 
        kullaniciAdiText.text += PhotonNetwork.LocalPlayer.NickName;  

    }

    public void Show_Message(string message)
    {
        PV.RPC("RPC_ShowMessage",RpcTarget.AllViaServer,message);
    }

    [PunRPC]
    private void RPC_ShowMessage(string message, PhotonMessageInfo info)
    {
        
        GameObject playerMessageObje = Instantiate(playerMessage_Prefab);
        playerMessageObje.transform.SetParent(playerMessagePrefab_Content.transform);
        playerMessageObje.transform.localScale = Vector3.one;


        playerMessageObje.GetComponent<PlayerMessageController>().Intialize(info.Sender.NickName,message,info.Sender);

        playerMessageObjects.Add(playerMessageObje);

        if(playerMessageObjects.Count > 4)
        {
            StartCoroutine(handleControl.HandleMove());
        }
        
        
        
    }  

    public void Send_Message()
    {
        string message = message_InputField.text;
        Show_Message(message);
        message_InputField.text = "";
        message_InputField.Select();
        message_InputField.ActivateInputField();
    }

    public void SendIconButton_Method()
    {
        if(message_InputField != null)
        {
            if(Input.GetKey(KeyCode.Return) || Input.GetKey(KeyCode.KeypadEnter))
            {
                string message = message_InputField.text;
                Show_Message(message);
                message_InputField.text = "";
                message_InputField.Select();
                message_InputField.ActivateInputField();
            }
        }
    }

    



}
