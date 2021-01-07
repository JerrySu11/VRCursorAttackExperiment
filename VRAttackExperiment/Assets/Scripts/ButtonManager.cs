using System.Collections;
using System.Collections.Generic;
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

    public GameObject introduction;
    public GameObject end;
    public GameObject startButton;
    public GameObject bait;
    public GameObject attack;
    public GameObject origin;
    public GameObject normal;
    public GameObject weird;

    public Pointer pointer;
    
    private float[] allAngles = { 10f, 12f, 14f, 16f, 18f, 20f, 22f };
    private int currentSceneNumber = 0;
    private Dictionary<float, Vector3> tempTimeLocationData = new Dictionary<float, Vector3>();
    
    // Start is called before the first frame update
    void Start()
    {
        //Randomize the order of the experiment angles
        for (int i = 0; i < allAngles.Length; i++)
        {
            int rnd = Random.Range(0, allAngles.Length);
            float temp = allAngles[rnd];
            allAngles[rnd] = allAngles[i];
            allAngles[i] = temp;
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
    public void startExperiment()
    {
        
        introduction.SetActive(false);
        startButton.SetActive(false);
        setUpScene(allAngles[currentSceneNumber]);
       
    }

    public void setUpScene(float angle)
    {
        tempTimeLocationData.Clear();
        bait.SetActive(true);
        attack.SetActive(true);
        origin.SetActive(true);
        pointer.inZone = true;

        randomizeAngle(bait);
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
        ExperimentDataManager.setTimeLocationData(currentSceneNumber, tempTimeLocationData);
        bait.SetActive(false);
        attack.SetActive(false);
        origin.SetActive(false);
        pointer.inZone = false;
        normal.SetActive(true);
        weird.SetActive(true);
    }
    public void goToNextScene(bool isNormal)
    {
        ExperimentDataManager.setNormal(currentSceneNumber, isNormal);
        currentSceneNumber += 1;

        normal.SetActive(false);
        weird.SetActive(false);
        if (currentSceneNumber < allAngles.Length)
        {
            setUpScene(allAngles[currentSceneNumber]);
        }
        else
        {
            end.SetActive(true);
        }
        
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
