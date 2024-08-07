using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class SunucuYonetim : MonoBehaviourPunCallbacks
{
    private static SunucuYonetim instance;
    public static SunucuYonetim Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<SunucuYonetim>();
            }
            return instance;
        }
    }
    public override void OnConnectedToMaster()
    {
        print("Server'a bağlanildi");
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        print("Lobiye bağlanildi");
        
        PhotonNetwork.JoinRandomOrCreateRoom();
    }

    public override void OnJoinedRoom()
    {
        print("Herhangi bir odaya giriş yapildi");
        UIMenager.Instance.SetActiveUIObject(UIMenager.Instance.Cheat_Panel.name);
    }
    
    public void ConnetingServer()
    {
        PhotonNetwork.ConnectUsingSettings();
        PhotonNetwork.LocalPlayer.NickName = UIMenager.Instance.KullaniciAdi_InputField.text;
    }

}
