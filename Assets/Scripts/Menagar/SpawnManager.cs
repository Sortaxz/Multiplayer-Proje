using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    private void Awake() 
    {
        CharacterSpawn();
    }

    private void CharacterSpawn()
    {
        PhotonNetwork.Instantiate("Player",Vector3.zero,Quaternion.identity,0,null);
    }
}
