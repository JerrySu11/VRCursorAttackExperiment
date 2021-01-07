using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
}

public class ExperimentData
{
    //does the user think the cursor feels normal?
    public bool isNormal;
    //The angle between attack button and bait button used in the current scene
    public float angle;
    //time location data. key: time   value: location in Vector3 object form 
    public Dictionary<float, Vector3> timeLocationData = new Dictionary<float, Vector3>();
}
