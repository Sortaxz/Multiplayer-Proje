using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
public static class BinarySaveSystem 
{

    public static void SaveBinary(Color saveCanColor)
    {
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        FileStream stream = new FileStream("BinarySave.bin",FileMode.Create);

        binaryFormatter.Serialize(stream,saveCanColor);

        stream.Close();
    }



    public static Color LoadBinary()
    {
        string dosyaAdi = "BinarySave.bin";

        if(File.Exists(dosyaAdi))
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            FileStream stream = new FileStream("BinarySave.bin",FileMode.Create);

            Color loadCanColor = (Color)binaryFormatter.Deserialize(stream);

            stream.Close();

            return loadCanColor;    
        }
        else
        {
            return new Color();
        }
    }
}
