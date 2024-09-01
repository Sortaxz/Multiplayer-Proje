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
    public Dictionary<int,GameObject> CurrentFriends { get { return currentFriends; } }
    private List<string> friendList = new List<string>();

    private List<string> friendIconsIndex = new List<string>();
 
  
    public void LoadFriendDate()
    {
        friendList = SaveSystem.LoadFriedns("friendsList");
        friendIconsIndex = SaveSystem.LoadFriedns();
    }


    public void AddFriend(string friendName,string friendIconIndex)
    {
        if(!friendList.Contains(friendName))
        {
            friendList.Add(friendName);
            friendIconsIndex.Add(friendIconIndex);

            SaveSystem.SaveFriend(friendList,friendIconsIndex);
        }
        else
        {
            print("Ekliceğiniz kişi zaten arkadaşiniz...");
        }
    }

    public void AddFriendIcon()
    {
    }

    public void ShowFriend()
    {
        foreach (var item in friendList)
        {
            print(item);
        }
    }

    public IEnumerator CheckFriends()
    {
        while(true)
        {
            if(friendList.Count > 0)
            {
                if(!PhotonNetwork.InRoom && PhotonNetwork.IsConnectedAndReady && PhotonNetwork.InLobby)
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

   
    public void CreatCurrentFriend()
    {
        if(UIMenager.Instance.Menu_Panel.activeSelf)
        {
            for (int i = 0; i < friendList.Count; i++)
            {
                if(!currentFriends.ContainsKey(i))
                {
                    CreateFriendObject(i);
                }



            }
        }
    }


    public void CurrentFriendState(List<FriendInfo> friendInfos)
    {
        for (int i = 0; i < friendInfos.Count; i++)
        {
            if(currentFriends.ContainsKey(i))
            {
                if(friendIconsIndex[i] != "")
                {
                    int frienIconIndex = int.Parse(friendIconsIndex[i]);

                    currentFriends[i].GetComponent<FriendListControl>().FriendObjectInitialize(friendInfos[i],frienIconIndex);

                }
            }
            else
            {
                continue;
            }
        }
    }


    private void CreateFriendObject(int i)
    {
        GameObject currentFriend = Instantiate(friendPrefab, friendContent.transform);
        currentFriend.transform.localScale = Vector3.one;
        currentFriends.Add(i, currentFriend);
    }


    public void CurrentFriendsList()
    {
        UIMenager.Instance.CreateCurrentFriendList(friendList,friendIconsIndex);
    }



}
