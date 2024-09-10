using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    private static SpawnManager instance;
    public static SpawnManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<SpawnManager>();
            }
            return instance;
        }
    }
    [SerializeField] private Transform spawnPoints;
    private void Awake() 
    {
        //CharacterSpawn();
    }

    public GameObject CharacterSpawn(PhotonView PV)
    {
        float x = Random.Range(-35f,35f);
        float y = 1;
        float z = Random.Range(-36,36);
        GameObject spanwCharacter = PhotonNetwork.Instantiate("Player", new Vector3(x,y,z),Quaternion.identity,0,new object[]{PV.ViewID});
        return spanwCharacter;
    }
}
