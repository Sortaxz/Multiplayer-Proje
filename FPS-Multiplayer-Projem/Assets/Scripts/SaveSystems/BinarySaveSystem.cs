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
        FileStream stream = new FileStream("FriendDataSave.bin", FileMode.Create);

        FriendData friendData = new FriendData(sunucuYonetim,false);
        
        formatter.Serialize(stream,friendData);

        stream.Close();
    }

    public static FriendData FriendDataLoad(SunucuYonetim sunucuYonetim)
    {
        string fileName = "FriendDataSave.bin";

        if(File.Exists(fileName)) 
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(fileName, FileMode.Open);
            FriendData friendData = formatter.Deserialize(stream) as FriendData;

            stream.Close();

            return friendData;
        }
        else
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream("FriendDataSave.bin", FileMode.Create);

            FriendData friendData = new FriendData(sunucuYonetim,true);
            
            formatter.Serialize(stream,friendData);

            stream.Close();

            return friendData;
        }
    }

}
