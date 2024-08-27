using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Photon.Realtime;

public class BinarySaveSystem : MonoBehaviour
{
    public static void FriendDataSave(SunucuYonetim sunucuYonetim)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/friend.fun";
        FileStream stream = new FileStream(path, FileMode.Create);

        FriendData friendData = new FriendData(sunucuYonetim, false);

        formatter.Serialize(stream, friendData);
        stream.Close();

        Debug.Log("Friend data saved to: " + path);
    }

    public static FriendData FriendDataLoad(SunucuYonetim sunucuYonetim)
    {
        string path = Application.persistentDataPath + "/friend.fun";

        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            FriendData data = formatter.Deserialize(stream) as FriendData;
            Debug.Log("Friend data loaded from: " + path);
            stream.Close();

            return data;
        }
        else
        {
            Debug.LogWarning("Friend data not found, creating new file at: " + path);
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Create);

            FriendData friendData = new FriendData(sunucuYonetim, true);
            formatter.Serialize(stream, friendData);
            stream.Close();

            return friendData;
        }
    }

    public static void FriendDataDelete()
    {
        string path = Application.persistentDataPath + "/friend.fun";
        File.Delete(path);
    }
}
