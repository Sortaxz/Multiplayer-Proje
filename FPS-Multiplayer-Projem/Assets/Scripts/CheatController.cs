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
    [SerializeField] private GameObject playerMessage_Prefab;
    [SerializeField] private GameObject playerMessagePrefab_Content;
    [SerializeField] private TMP_InputField message_InputField;
    [SerializeField] private TextMeshProUGUI cheatMessage_Text;
    [SerializeField] private TextMeshProUGUI kullaniciAdiText;
    private void Awake() 
    {
        PV = GetComponent<PhotonView>(); 
        kullaniciAdiText.text += PhotonNetwork.NickName;   

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


        playerMessageObje.GetComponent<PlayerMessageController>().Intialize(info.Sender.NickName,message);

    }  

    public void Send_Message()
    {
        string message = message_InputField.text;
        Show_Message(message);
        message_InputField.text = "";

        
    }

    



}
