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
    [SerializeField] private int index;
    private void Awake() 
    {
    }

    public GameObject CharacterSpawn(PhotonView PV,Transform[] spawnPoints)
    {
        int controlValue = Random.Range(0,spawnPoints.Length);

        if(spawnPoints[controlValue].gameObject.activeSelf)
        {
            index = controlValue;
        }
        else
        {
            index  = Random.Range(0,spawnPoints.Length);
            if(index == controlValue)
            {
                index  = Random.Range(0,spawnPoints.Length);
            }
        }

        //new Vector3(spawnPoints[index].position.x,spawnPoints[index].position.y,spawnPoints[index].position.z)
        GameObject spanwCharacter = PhotonNetwork.Instantiate("Player", spawnPoints[index].position,Quaternion.identity,0,new object[]{PV.ViewID});
        

        StartCoroutine(GameManager.Instance.SetPointActive(index));

        return spanwCharacter;
    }
}
