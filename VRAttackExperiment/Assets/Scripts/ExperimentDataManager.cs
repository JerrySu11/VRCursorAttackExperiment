using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class ExperimentDataManager : MonoBehaviour
{
    
    //key: scene number. value: an ExperimentData object that contains data for that scene
    private static Dictionary<int, ExperimentData> experimentData = new Dictionary<int, ExperimentData>();
    
    public static void setTimeLocationData(int sceneNumber, Dictionary<float, Vector3> data)
    {
        if (experimentData.ContainsKey(sceneNumber)){
            experimentData[sceneNumber].timeLocationData = data;
        }
        else
        {
            ExperimentData tempData = new ExperimentData();
            tempData.timeLocationData = data;
            experimentData.Add(sceneNumber, tempData);
        }
        
    }
    public static void setTimeHeadsetLocationData(int sceneNumber, Dictionary<float, Vector3> data)
    {
        if (experimentData.ContainsKey(sceneNumber))
        {
            experimentData[sceneNumber].timeHeadsetLocationData = data;
        }
        else
        {
            ExperimentData tempData = new ExperimentData();
            tempData.timeHeadsetLocationData = data;
            experimentData.Add(sceneNumber, tempData);
        }

    }
    public static void setTimeHeadsetRotationData(int sceneNumber, Dictionary<float, Quaternion> data)
    {
        if (experimentData.ContainsKey(sceneNumber))
        {
            experimentData[sceneNumber].timeHeadsetRotationData = data;
        }
        else
        {
            ExperimentData tempData = new ExperimentData();
            tempData.timeHeadsetRotationData = data;
            experimentData.Add(sceneNumber, tempData);
        }

    }
    public static void setButtonLocation(int sceneNumber, Vector3 attackLocation,Vector3 targetLocation)
    {
        if (experimentData.ContainsKey(sceneNumber))
        {
            experimentData[sceneNumber].attackLocation = attackLocation;
            experimentData[sceneNumber].targetLocation = targetLocation;
        }
        else
        {
            ExperimentData tempData = new ExperimentData();
            tempData.attackLocation = attackLocation;
            tempData.targetLocation = targetLocation;
            experimentData.Add(sceneNumber, tempData);
        }
    }
    public static void setNormal(int sceneNumber,bool normal)
    {
        if (experimentData.ContainsKey(sceneNumber))
        {
            experimentData[sceneNumber].isNormal = normal;
        }
        else
        {
            ExperimentData tempData = new ExperimentData();
            tempData.isNormal = normal;
            experimentData.Add(sceneNumber, tempData);
        }
    }
    public static void setAngle(int sceneNumber, float angle)
    {
        if (experimentData.ContainsKey(sceneNumber))
        {
            experimentData[sceneNumber].angle = angle;
        }
        else
        {
            ExperimentData tempData = new ExperimentData();
            tempData.angle = angle;
            experimentData.Add(sceneNumber, tempData);
        }
    }

    public static void exportData()
    {
        
        string sceneData = "";
        sceneData += "scene, isNormal, angle, targetLocationX, targetLocationY, targetLocationZ, attackLocationX, attackLocationY, attackLocationZ\n";
        string timeLocationData = "";
        timeLocationData += "scene,time,locationX,locationY,locationZ\n";
        string timeHeadsetLocationData = "";
        timeHeadsetLocationData += "scene,time,headsetLocationX,headsetLocationY,headsetLocationZ\n";
        string timeHeadsetRotationData = "";
        timeHeadsetRotationData += "scene,time,headsetRotationX,headsetRotationY,headsetRotationZ,headsetRotationW\n";

        foreach (KeyValuePair<int, ExperimentData> entry in experimentData)
        {
            sceneData += entry.Key + "," + entry.Value.isNormal + "," + entry.Value.angle + "," + entry.Value.targetLocation.x + "," + entry.Value.targetLocation.y + "," + entry.Value.targetLocation.z + "," + entry.Value.attackLocation.x + "," + entry.Value.attackLocation.y + "," + entry.Value.attackLocation.z + "\n";
            foreach(KeyValuePair<float, Vector3> entry2 in entry.Value.timeLocationData)
            {
                timeLocationData += entry.Key + "," + entry2.Key + ","+entry2.Value.x + "," + entry2.Value.y + "," + entry2.Value.z + "\n";

            }
            foreach (KeyValuePair<float, Vector3> entry2 in entry.Value.timeHeadsetLocationData)
            {
                timeHeadsetLocationData += entry.Key + "," + entry2.Key + "," + entry2.Value.x + "," + entry2.Value.y + "," + entry2.Value.z + "\n";

            }
            foreach (KeyValuePair<float, Quaternion> entry2 in entry.Value.timeHeadsetRotationData)
            {
                timeHeadsetRotationData += entry.Key + "," + entry2.Key + "," + entry2.Value.x + "," + entry2.Value.y + "," + entry2.Value.z + ","+ entry2.Value.w + "\n";

            }
        }
        string folder = Application.dataPath;

        Directory.CreateDirectory(folder + "data/");
        
        string filePath1 = folder+ "data/"+"SceneData.csv";
        string filePath2 = folder + "data/" + "TimeLocationData.csv";
        string filePath3 = folder + "data/" + "TimeHeadsetLocationData.csv";
        string filePath4 = folder + "data/" + "TimeHeadsetRotationData.csv";
        using (var writer = new StreamWriter(filePath1, false))
        {
            writer.Write(sceneData);
            
        }
        using (var writer = new StreamWriter(filePath2, false))
        {
            writer.Write(timeLocationData);
        }
        using (var writer = new StreamWriter(filePath3, false))
        {
            writer.Write(timeHeadsetLocationData);
        }
        using (var writer = new StreamWriter(filePath4, false))
        {
            writer.Write(timeHeadsetRotationData);
        }
        Debug.Log($"CSV file written to \" {filePath1}\"");
        Debug.Log($"CSV file written to \" {filePath2}\"");
        Debug.Log($"CSV file written to \" {filePath3}\"");
        Debug.Log($"CSV file written to \" {filePath4}\"");

#if UNITY_EDITOR
        AssetDatabase.Refresh();
#endif
    }
}

public class ExperimentData
{
    //does the user think the cursor feels normal?
    public bool isNormal;
    //The angle between attack button and bait button used in the current scene
    public float angle;

    //target location
    public Vector3 targetLocation;

    //attack location
    public Vector3 attackLocation;

    //time location data. key: time   value: location in Vector3 object form 
    public Dictionary<float, Vector3> timeLocationData = new Dictionary<float, Vector3>();

    public Dictionary<float, Vector3> timeHeadsetLocationData = new Dictionary<float, Vector3>();
    public Dictionary<float, Quaternion> timeHeadsetRotationData = new Dictionary<float, Quaternion>();
}
