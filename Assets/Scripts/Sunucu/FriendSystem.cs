using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class FriendSystem : MonoBehaviourPunCallbacks
{
    private static FriendSystem instance;
    public static FriendSystem Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<FriendSystem>();
            }
            return instance;
        }
    }


    [Header("Current Friend Objesi ile ilgili işlemler")]
    [SerializeField] private GameObject friendPrefab;
    [SerializeField] private GameObject friendContent;
    private Dictionary<int,GameObject> currentFriends = new Dictionary<int,GameObject>();

    private List<string> friendList = new List<string>();

    private List<string> friendIconsIndex = new List<string>();


    private void Start() 
    {
        friendList = SaveSystem.LoadFriedns("friend");
        ShowFriend();
        
    }

    private void Update() 
    {
          
    }

    public void AddFriend(string friendName)
    {
        if(!friendList.Contains(friendName))
        {
            friendList.Add(friendName);
            SaveSystem.SaveFriend(friendList);
        }
        else
        {
            print("Ekliceğiniz kişi zaten arkadaşiniz...");
        }
    }

    public void ShowFriend()
    {
        foreach (string friend in friendList)
        {
            print("Arkadaşin : " + friend);
        }
    }

    public IEnumerator CheckFriends()
    {
        while(true)
        {
            if(friendList.Count > 0)
            {
                if(!PhotonNetwork.InRoom && PhotonNetwork.IsConnectedAndReady)
                {
                    PhotonNetwork.FindFriends(friendList.ToArray());
                }
            }
            else
            {
                print("Arkadaş listeniz boş");
            }

            yield return new WaitForSeconds(1);

        }
    }

    public void CreatCurrentFriend(List<FriendInfo> friendInfo)
    {
        for (int i = 0; i < friendInfo.Count; i++)
        {
            if(!currentFriends.ContainsKey(i))
            {
                GameObject currentFriend = Instantiate(friendPrefab,friendContent.transform);
                currentFriend.transform.localScale = Vector3.one;
                currentFriends.Add(i, currentFriend);
            }
            else
            {
                currentFriends[i].GetComponent<FriendListControl>().FriendObjectInitialize(friendInfo[i]);
            }


        }
    }

}
