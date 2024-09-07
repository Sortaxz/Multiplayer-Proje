using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    void Start()
    {
        CharacterSpawn();
    }

    void Update()
    {
        
    }

    private void CharacterSpawn()
    {
        PhotonNetwork.Instantiate("Player",Vector3.zero,Quaternion.identity,0,null);
    }

}
