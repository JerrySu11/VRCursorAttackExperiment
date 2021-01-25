using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ButtonManager : MonoBehaviour
{
    public GameObject canvas;
    //private List<GameObject> targetButtons;
    //private List<GameObject> baitButtons;
    //private GameObject origin;
    public float canvasWidthMax;
    public float canvasWidthMin;
    public float canvasHeightMax;
    public float canvasHeightMin;

    public float buttonWidthMax;
    public float buttonWidthMin;
    public float buttonHeightMax;
    public float buttonHeightMin;

    public GameObject menu;
    public GameObject end;
    
    //public GameObject startButton;
    public GameObject bait;
    public GameObject attack;
    public GameObject origin;
    public GameObject normal;
    public GameObject weird;
    public GameObject breakMenu;
    //public GameObject breakContinue;

    public Pointer pointer;
    
    private float[] allAngles = {10f ,0f,-10f,5f,-5f,15f,-15f,20f,-20f,25f,-25f};
    //private float[] allAngles = {0f};
    private float[] allExperimentAngles;
    private int currentSceneNumber = 0;
    private Dictionary<float, Vector3> tempTimeLocationData = new Dictionary<float, Vector3>();
    private Dictionary<float, Vector3> tempTimeHeadsetLocationData = new Dictionary<float, Vector3>();
    private Dictionary<float, Quaternion> tempTimeHeadsetRotationData = new Dictionary<float, Quaternion>();

    public GameObject stageCompletionUI;

    private bool isTutorial = true;
    public GameObject tutorial1;
    public GameObject tutorial2;
    public GameObject tutorial3;
    public GameObject tutorialEndMenu;
    // Start is called before the first frame update
    void Start()
    {
        int repetition = 10;
        allExperimentAngles = new float[allAngles.Length* repetition];
        for (int i = 0; i < allAngles.Length; i++)
        {
            for (int j = 0; j < repetition; j++)
            {
                allExperimentAngles[i * repetition + j] = allAngles[i];
            }

        }
        //Randomize the order of the experiment angles
        for (int i = 0; i < allExperimentAngles.Length; i++)
        {
            int rnd = Random.Range(0, allExperimentAngles.Length);
            float temp = allExperimentAngles[rnd];
            allExperimentAngles[rnd] = allExperimentAngles[i];
            allExperimentAngles[i] = temp;
        }
        
        /*
        targetButtons = new List<GameObject>();
        baitButtons = new List<GameObject>();
        foreach (Transform child in canvas.transform)
        {
            if (child.gameObject.tag == "Target")
            {
                targetButtons.Add(child.gameObject);
                //Debug.Log("target "+child.gameObject.GetComponent<RectTransform>().anchoredPosition);
                //Debug.Log(child.gameObject.GetComponent<RectTransform>().sizeDelta);
            }
            else if (child.gameObject.tag == "Bait")
            {
                baitButtons.Add(child.gameObject);
                //Debug.Log("bait " + child.gameObject.GetComponent<RectTransform>().anchoredPosition);
                //Debug.Log(child.gameObject.GetComponent<RectTransform>().sizeDelta);
            }
            else if (child.gameObject.tag == "Origin")
            {
                origin = child.gameObject;
            }
            
        }
        */
    }
    //Experiment measures 10,15,20,25,30,35,40

    public void startTutorial()
    {
        menu.SetActive(false);
        
        setUpTutorial();
    }
    public void setUpTutorial()
    {
        bait.SetActive(true);
        //attack.SetActive(true);
        origin.SetActive(true);
        tutorial1.SetActive(true);
        
        
        //Vector2 vec = bait.GetComponent<RectTransform>().anchoredPosition - origin.GetComponent<RectTransform>().anchoredPosition;
        //setButton(attack, origin.GetComponent<RectTransform>().anchoredPosition + vec, attack.GetComponent<RectTransform>().sizeDelta);
    }
    public void startExperiment()
    {
        ExperimentDataManager.createNewFolder();
        ExperimentDataManager.setUpFiles();
        //introduction.SetActive(false);
        //startButton.SetActive(false);
        setUpScene(allExperimentAngles[currentSceneNumber]);
       
    }
    public void endTutorial1()
    {
        tutorial1.SetActive(false);
        tutorial2.SetActive(true);
    }
    public void endTutorial2()
    {
        tutorial2.SetActive(false);
        tutorial3.SetActive(true);
    }
    public void endTutorial3()
    {
        tutorial3.SetActive(false);
        tutorialEndMenu.SetActive(true);
    }
    public void endTutorialMenu()
    {
        tutorialEndMenu.SetActive(false);
        startExperiment();
    }
    public void setUpScene(float angle)
    {
        tempTimeLocationData.Clear();
        tempTimeHeadsetLocationData.Clear();
        tempTimeHeadsetRotationData.Clear();
        bait.SetActive(true);
        attack.SetActive(true);
        origin.SetActive(true);
        pointer.inZone = true;

        randomizeAngle(bait);
        /*
        int randomDirection = Random.Range(0,1);
        //0: rotate attack around origin clockwise, with 'angle' amount from bait
        if (randomDirection == 0)
        {
            Vector2 vec = bait.GetComponent<RectTransform>().anchoredPosition - origin.GetComponent<RectTransform>().anchoredPosition;
            vec = Rotate(vec, -angle);
            setButton(attack, origin.GetComponent<RectTransform>().anchoredPosition + vec, attack.GetComponent<RectTransform>().sizeDelta);
        }
        //1: rotate attack around origin counter clockwise, with 'angle' amount from bait
        else
        {
            Vector2 vec = bait.GetComponent<RectTransform>().anchoredPosition - origin.GetComponent<RectTransform>().anchoredPosition;
            vec = Rotate(vec, angle);
            setButton(attack, origin.GetComponent<RectTransform>().anchoredPosition + vec, attack.GetComponent<RectTransform>().sizeDelta);
        }
        */
        Vector2 vec = bait.GetComponent<RectTransform>().anchoredPosition - origin.GetComponent<RectTransform>().anchoredPosition;
        vec = Rotate(vec, angle);
        setButton(attack, origin.GetComponent<RectTransform>().anchoredPosition + vec, attack.GetComponent<RectTransform>().sizeDelta);

        ExperimentDataManager.setButtonLocation(currentSceneNumber, attack.transform.position, bait.transform.position);
        ExperimentDataManager.setAngle(currentSceneNumber, angle);
    }
    public static Vector2 Rotate(Vector2 v, float degrees)
    {
        float radians = degrees * Mathf.Deg2Rad;
        float sin = Mathf.Sin(radians);
        float cos = Mathf.Cos(radians);

        float tx = v.x;
        float ty = v.y;

        return new Vector2(cos * tx - sin * ty, sin * tx + cos * ty);
    }
    public void endScene()
    {
        if (!isTutorial)
        {
            ExperimentDataManager.setTimeLocationData(currentSceneNumber, tempTimeLocationData);
            ExperimentDataManager.setTimeHeadsetLocationData(currentSceneNumber, tempTimeHeadsetLocationData);
            ExperimentDataManager.setTimeHeadsetRotationData(currentSceneNumber, tempTimeHeadsetRotationData);
        }
        
        bait.SetActive(false);
        attack.SetActive(false);
        origin.SetActive(false);
        pointer.inZone = false;
        normal.SetActive(true);
        weird.SetActive(true);
    }
    public void endGame()
    {
        Application.Quit();
    }



    public void goToNextScene(bool isNormal)
    {
        if (isTutorial)
        {
            normal.SetActive(false);
            weird.SetActive(false);
            isTutorial = false;
            endTutorial3();
        }
        else
        {
            ExperimentDataManager.setNormal(currentSceneNumber, isNormal);
            currentSceneNumber += 1;
            stageCompletionUI.GetComponent<TextMeshProUGUI>().text = currentSceneNumber + "/110";

            normal.SetActive(false);
            weird.SetActive(false);
            if (currentSceneNumber < allExperimentAngles.Length)
            {
                if (currentSceneNumber == 40 || currentSceneNumber == 80)
                {
                    breakTime();
                }
                else
                {
                    setUpScene(allExperimentAngles[currentSceneNumber]);
                }


            }
            else
            {
                //ExperimentDataManager.exportData();
                end.SetActive(true);


            }
        }
        
        
    }
    private void breakTime()
    {
        breakMenu.SetActive(true);
        
    }
    public void endBreak()
    {
        breakMenu.SetActive(false);
        
        setUpScene(allExperimentAngles[currentSceneNumber]);
    }
    public void addTimeLocationData(float time, Vector3 location)
    {
        if (tempTimeLocationData.ContainsKey(time))
        {
            tempTimeLocationData[time] = location;
        }
        else
        {
            tempTimeLocationData.Add(time, location);
        }
    }

    public void addTimeHeadsetLocationData(float time, Vector3 location)
    {
        if (tempTimeHeadsetLocationData.ContainsKey(time))
        {
            tempTimeHeadsetLocationData[time] = location;
        }
        else
        {
            tempTimeHeadsetLocationData.Add(time, location);
        }
    }

    public void addTimeHeadsetRotationData(float time, Quaternion rotation)
    {
        if (tempTimeHeadsetRotationData.ContainsKey(time))
        {
            tempTimeHeadsetRotationData[time] = rotation;
        }
        else
        {
            tempTimeHeadsetRotationData.Add(time, rotation);
        }
    }


    /*
    public void randomizeAll()
    {
        foreach (GameObject button in targetButtons)
        {
            randomizeButtonPosition(button);
            randomizeButtonSize(button);
        }
        foreach (GameObject button in baitButtons)
        {
            randomizeButtonPosition(button);
            randomizeButtonSize(button);
        }
    }
    public void randomizeAllPosition()
    {
        foreach (GameObject button in targetButtons)
        {
            randomizeButtonPosition(button);
        }
        foreach (GameObject button in baitButtons)
        {
            randomizeButtonPosition(button);
        }
    }

    public void randomizeAllSize()
    {
        foreach (GameObject button in targetButtons)
        {
            randomizeButtonSize(button);
        }
        foreach (GameObject button in baitButtons)
        {
            randomizeButtonSize(button);
        }
    }
    public void randomizeAllAngle()
    {
        foreach (GameObject button in targetButtons)
        {
            randomizeAngle(button);
            
        }
        foreach (GameObject button in baitButtons)
        {
            randomizeAngle(button);
            
        }
    }
    */
    public void randomizeAngle(GameObject button)
    {
        float dist = (button.GetComponent<RectTransform>().anchoredPosition - origin.GetComponent<RectTransform>().anchoredPosition).magnitude;
        Vector2 newVec = (new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f))).normalized;
        setButton(button, newVec*dist+ origin.GetComponent<RectTransform>().anchoredPosition, button.GetComponent<RectTransform>().sizeDelta);
    }

    public void randomizeButtonPosition(GameObject button)
    {
        setButton(button, new Vector2(Random.Range(canvasWidthMin,canvasWidthMax),Random.Range(canvasHeightMin,canvasHeightMax)), button.GetComponent<RectTransform>().sizeDelta);
    }

    public void randomizeButtonSize(GameObject button)
    {
        setButton(button, button.GetComponent<RectTransform>().anchoredPosition, new Vector2(Random.Range(buttonWidthMin, buttonWidthMax), Random.Range(buttonHeightMin, buttonHeightMax)));
    }

    // Update is called once per frame
    public void setButton(GameObject button, Vector2 position, Vector2 size)
    {
        button.GetComponent<RectTransform>().anchoredPosition = position;
        button.GetComponent<RectTransform>().sizeDelta = size;
    }


}
