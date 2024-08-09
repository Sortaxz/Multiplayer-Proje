using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.EventSystems;
using UnityEngine.UI;

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
        UIMenager.Instance.SetActiveUIObject(UIMenager.Instance.Menu_Panel.name);
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
        
        if(!PlayerPrefs.HasKey("playerName"))
        {
            string playerName = UIMenager.Instance.KullaniciAdi_InputField.text;
            PlayerPrefs.SetString("playerName",playerName);
            PhotonNetwork.LocalPlayer.NickName = playerName;
        }
        else
        {
            PhotonNetwork.LocalPlayer.NickName = PlayerPrefs.GetString("playerName");
        }
    }

    public void ConnectedLobby()
    {
        PhotonNetwork.JoinLobby();
    }

}
