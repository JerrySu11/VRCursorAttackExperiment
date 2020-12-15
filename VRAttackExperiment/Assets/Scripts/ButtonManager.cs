using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonManager : MonoBehaviour
{
    public GameObject canvas;
    private List<GameObject> targetButtons;
    private List<GameObject> baitButtons;
    private GameObject origin;
    public float canvasWidthMax;
    public float canvasWidthMin;
    public float canvasHeightMax;
    public float canvasHeightMin;

    public float buttonWidthMax;
    public float buttonWidthMin;
    public float buttonHeightMax;
    public float buttonHeightMin;
    // Start is called before the first frame update
    void Start()
    {
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
    }

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
