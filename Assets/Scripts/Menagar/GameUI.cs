using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
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
    private void Awake() 
    {
        pw = GetComponent<PhotonView>();
        playerHealtBar = playerHealtBarBackground.transform.GetChild(0).GetComponent<Image>();    
    }

    public void Active()
    {
        playerHealtBarBackground.gameObject.SetActive(true);
    
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
    
    public void OtherPlayerHealtBar_Method(float healt)
    {
        pw.RPC("RPC_OtherPlayerHealtBar",RpcTarget.AllBufferedViaServer,healt);
    }

    [PunRPC]
    private void RPC_OtherPlayerHealtBar(float healt)
    {
        otherPlayerHealtBar.fillAmount = healt / 100;
    }
}
 