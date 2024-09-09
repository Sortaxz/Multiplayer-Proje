using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviourPunCallbacks
{
    [SerializeField]private CharacterControl[] characterOfPlayers;
    [SerializeField] private GameObject loadingScren;
    [SerializeField] private Text infoText;
    private bool findCharacterOfPlayers = false;

    public override void OnEnable()
    {
        CountdownTimer.OnCountdownTimerHasExpired += OnCountDownTimerIsExpired;
        base.OnEnable();
    }

    public override void OnDisable()
    {
        base.OnDisable();
        CountdownTimer.OnCountdownTimerHasExpired -= OnCountDownTimerIsExpired;
    }

    private void Awake() 
    {
        ExitGames.Client.Photon.Hashtable props = new ExitGames.Client.Photon.Hashtable()
        {
            {"PlayerLoadedLevel",true}
        };
        PhotonNetwork.LocalPlayer.SetCustomProperties(props);    
    }
    
    void Start()
    {
        

        StartCoroutine(FindCharacter());
    }

    #region  Pun Callbacks

    public override void OnDisconnected(DisconnectCause cause)
    {
    }

    public override void OnLeftRoom()
    {
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        if(!PhotonNetwork.IsMasterClient)
        {
            return;
        }

        bool StartTimeIsSet  = CountdownTimer.TryGetStartTime(out int startTimeStamp);

        if(changedProps.ContainsKey("PlayerLoadedLevel"))
        {
            if(CheckAllPlayerLoadedLevel())
            {
                if(!StartTimeIsSet)
                {
                    CountdownTimer.SetStartTime();
                }
            }
            else
            {

                infoText.text = "Diğer oyuncular bekleniyor...";
            }
        }
    }

    #endregion


    private IEnumerator FindCharacter()
    {
        yield return new WaitForSeconds(4);
        CharacterControl[] newCharacterOfPlayers = FindObjectsOfType<CharacterControl>();
        characterOfPlayers = new CharacterControl[newCharacterOfPlayers.Length];
        for (int i = 0; i < newCharacterOfPlayers.Length; i++)
        {
            if(i == newCharacterOfPlayers[i].PlayerActorNumber-1)
            {
                characterOfPlayers[i] = newCharacterOfPlayers[i];
            }
            else
            {
                characterOfPlayers[newCharacterOfPlayers[i].PlayerActorNumber-1] = newCharacterOfPlayers[i];
            }
        }
        
    }

    private void StartGame()
    {
        SpawnManager.Instance.CharacterSpawn();
    }

    private bool CheckAllPlayerLoadedLevel()
    {
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            if(player.CustomProperties.TryGetValue("PlayerLoadedLevel",out object playerLoadedLevel))
            {
                if((bool)playerLoadedLevel)
                {
                    continue;
                }

                return false;
            }
        }

        return true;
 
     }
    
    private void OnCountDownTimerIsExpired()
    {
        loadingScren.gameObject.SetActive(false);
        StartGame();
    }

}
