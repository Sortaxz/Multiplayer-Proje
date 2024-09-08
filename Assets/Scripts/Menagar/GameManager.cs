using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]private CharacterControl[] characterOfPlayers;
    [SerializeField]private List<CharacterControl> newCharacterOfPlayers = new List<CharacterControl>();
    private bool findCharacterOfPlayers = false;
    void Start()
    {
        StartCoroutine(FindCharacter());
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.F))
        {
            
            foreach (var item in characterOfPlayers)
            {
                print(item.PlayerNickName+"-"+item.PlayerActorNumber);
            }
        }
    }

    private IEnumerator FindCharacter()
    {
        while(true && !findCharacterOfPlayers)
        {
            if(characterOfPlayers.Length != PhotonNetwork.CurrentRoom.MaxPlayers)
            {
                characterOfPlayers = FindObjectsOfType<CharacterControl>();

            }
            else
            {
                findCharacterOfPlayers = true;
            }

            yield return new WaitForSeconds(.1f);
        
        }
        if(findCharacterOfPlayers)
        {
            for (int i = 0; i < characterOfPlayers.Length; i++)
            {
                int playerIndex = characterOfPlayers[i].PlayerActorNumber;
                characterOfPlayers[playerIndex-1] = characterOfPlayers[i];
            }
        }
        
    }

}
