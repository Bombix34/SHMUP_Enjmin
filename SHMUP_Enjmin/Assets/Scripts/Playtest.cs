using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System;

public class Playtest : MonoBehaviour {
    
    private List<string[]> rowData = new List<string[]>();

    DateTime timeFolder;

    // nombre de bulles tirées  DONE
    // durée de la partie       DONE
    // score                    DONE
    // a rejoué                 DONE
    // partie du LD             
    

    // Use this for initialization
    void Start () 
    {
        InitDatas();
        timeFolder=System.DateTime.Now;
    }

    void InitDatas()
    {
         // Creating First row of titles manually..
        string[] rowDataTemp = new string[2];
        rowDataTemp[0] = "Name";
        rowDataTemp[1] = "Value";
        rowData.Add(rowDataTemp);
    }
    
    public void Save()
    {

        //A ENLEVER QUAND ON A BESOIN DES CSV PLAYTESTS
        return;

        string[][] output = new string[rowData.Count][];

        for(int i = 0; i < output.Length; i++)
        {
            output[i] = rowData[i];
        }

        int     length         = output.GetLength(0);
        string     delimiter     = ";";

        StringBuilder sb = new StringBuilder();
        
        for (int index = 0; index < length; index++)
            sb.AppendLine(string.Join(delimiter, output[index]));
        
        
        string filePath = getPath();

        StreamWriter outStream = System.IO.File.CreateText(filePath);
        outStream.WriteLine(sb);
        outStream.Close();
    }

    public void AddMetric(string metric, string val)
    {
        string[] rowDataTemp = new string[2];
        rowDataTemp[0] = metric; // name
        rowDataTemp[1] = val; // ID
        rowData.Add(rowDataTemp);
    }

    // Following method is used to retrive the relative path as device platform
    private string getPath(){
        #if UNITY_EDITOR
        return Application.dataPath +"/CSV/"+timeFolder.Hour+":"+timeFolder.Minute+"_" +timeFolder.ToString("dd-MM")+"_" +"PLAYDATAS.csv";
        #elif UNITY_ANDROID
        return Application.persistentDataPath+timeFolder.Hour+":"+timeFolder.Minute+"_" +timeFolder.ToString("dd-MM")+"_" +"PLAYDATAS.csv";
        #elif UNITY_IPHONE
        return Application.persistentDataPath+"/"+timeFolder.Hour+":"+timeFolder.Minute+"_" +timeFolder.ToString("dd-MM")+"_" +"PLAYDATAS.csv";
        #else
        return Application.dataPath +"/CSV/"+timeFolder.Hour+":"+timeFolder.Minute+"_" +timeFolder.ToString("dd-MM")+"_" +"PLAYDATAS.csv";
        #endif
    }
}
