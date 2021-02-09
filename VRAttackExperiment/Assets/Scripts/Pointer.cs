using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using Valve.VR;
using UnityEngine.UI;

public class Pointer : MonoBehaviour
{
    public float m_defaultLength = 50.0f;
    public GameObject m_Dot;
    public VRInputModule m_inputModule;
    public DistractionManager distract;

    private LineRenderer m_LineRenderer = null;
    //inZone denotes if we should attempt to detect/start an attack
    public bool inZone = false;
    private bool attackAttempt = false;

    private bool displayRealCursor = false;
    public LineRenderer RealLineRenderer;

    public GameObject origin;
    public GameObject bait;
    public GameObject attackTarget;
    public float thresholdAngle = 15;
    public float thresholdDistRatio = 0.1f;
    
    private Vector3 targetOffset;
    private Vector3 currentOffset;

    [Range(0, 2)]
    public int attackOption = 0;

    private Vector3 lastDisplayedCursorPos;
    public Vector3 lastRealCursorPos;


    private float timer;
    private bool timerStop = false;
    //public GameObject timerUI;

    public ButtonManager buttonManager;

    public Transform allObjects;
    public SteamVR_Action_Boolean m_grabAction;
    public SteamVR_Action_Boolean m_clickAction;
    public SteamVR_Input_Sources m_targetSource;

    
    public Transform headsetTransform;
    public Transform plane;
    // Start is called before the first frame update
    void Start()
    {
        Recenter();
        m_LineRenderer = GetComponent<LineRenderer>();

    }

    // Update is called once per frame
    void Update()
    {
        
        //Debug.Log(headsetTransform.rotation.eulerAngles);
        if (m_grabAction.GetStateDown(m_targetSource))
        {
            Recenter();
        }
        UpdateLine();
        //timerUI.GetComponent<TextMeshProUGUI>().text = timer+"\nsec";
    }
    void Recenter()
    {
        allObjects.position = headsetTransform.position - new Vector3(0,2,0);

        allObjects.rotation = Quaternion.Euler(0, headsetTransform.rotation.eulerAngles.y+90,0);
        //allObjects.rotation = headsetTransform.rotation;
    }
    private void UpdateLine()
    {
        PointerEventData data = m_inputModule.getData();
        float targetlength = data.pointerCurrentRaycast.distance == 0?m_defaultLength: data.pointerCurrentRaycast.distance;
        RaycastHit hit = CreateRayCast(targetlength);
        Vector3 endPosition = transform.position + (transform.forward * targetlength);
        //Debug.Log("Pointer" + endPosition);
        //Debug.Log("Origin"+origin.transform.position);
        
        

        if (hit.collider != null)
        {
            //Debug.Log(hit.point);
            if (inZone & attackAttempt)
            {
                endPosition = manipulatePos(hit.point,attackOption);
                
            }
            else
            {
                endPosition = smoothPos(hit.point);
                
            }
            
            lastDisplayedCursorPos = endPosition;
            //Debug.Log(lastDisplayedCursorPos);
            lastRealCursorPos = hit.point;
            currentOffset = lastDisplayedCursorPos - lastRealCursorPos;
        }
        m_Dot.transform.position = endPosition;

        m_LineRenderer.SetPosition(0, transform.position);
        m_LineRenderer.SetPosition(1, endPosition);

        if (displayRealCursor)
        {
            if (!RealLineRenderer.enabled)
            {
                RealLineRenderer.enabled = true;
            }
            
            RealLineRenderer.SetPosition(0, transform.position);
            RealLineRenderer.SetPosition(1, hit.point);
        }
        else
        {
            RealLineRenderer.enabled = false;
        }
        if (inZone)
        {
            //Debug.Log("cursor pos" + lastDisplayedCursorPos);
            //Debug.Log("origin pos" + origin.transform.position);
            /*
            if (isClose(lastDisplayedCursorPos, origin.transform.position) && !attackAttempt)
            {
                attackAttempt = true;
                targetOffset = (bait.transform.position - attackTarget.transform.position);
                distract.distract();
                Debug.Log("Reached origin");
                thresholdDistRatio = (attackTarget.transform.position - bait.transform.position).magnitude / (bait.transform.position - origin.transform.position).magnitude;
                //time an attack
                timerStop = false;
                StartCoroutine("timerStart");
            }
            
            //if 1) the cursor has reached the targetlocation 2) the cursor is even further from the target than the starting origin (meaning it is not moving towards the target), then do not attempt to attack
            else if (attackAttempt & (isClose(lastDisplayedCursorPos, bait.transform.position) || (bait.transform.position - origin.transform.position).magnitude < (bait.transform.position - lastDisplayedCursorPos).magnitude - 1.0f))
            {

                Debug.Log("Cancel attempt");
                attackAttempt = false;
                timerStop = true;
                StartCoroutine("endSceneDelay");
            }
            */
            
            //In the current experiment, we do not cancel an attack
            if (attackAttempt & (isClose(lastDisplayedCursorPos, bait.transform.position)) & m_clickAction.GetStateDown(m_targetSource))
            {
                //Debug.Log(lastDisplayedCursorPos);
                //Debug.Log(bait.transform.position);
                
                Debug.Log("Clicked bait button");
                attackAttempt = false;
                timerStop = true;
                buttonManager.endScene();
                //StartCoroutine("endSceneDelay");
            }
            

        }
        else
        {
            
            if (isClose(bait.transform.position, endPosition) & m_clickAction.GetStateDown(m_targetSource))
            {
                buttonManager.endTutorial2();
                buttonManager.endScene();
            }
            
        }
    }
    public void onClickOrigin()
    {
        origin.GetComponent<Button>().image.color = Color.grey;
        //This is the case for tutorial
        if (!inZone)
        {
            buttonManager.endTutorial1();

        }
        else
        {
            if (!attackAttempt)
            {
                attackAttempt = true;
                targetOffset = (bait.transform.position - attackTarget.transform.position);
                //distract.distract();
                Debug.Log("Clicked origin");
                thresholdDistRatio = (attackTarget.transform.position - bait.transform.position).magnitude / (bait.transform.position - origin.transform.position).magnitude;
                //time an attack
                timerStop = false;
                StartCoroutine("timerStart");
            }
        }
        
    }
    private bool isClose(Vector3 point1, Vector3 point2)
    {
        float dist = (point1 - point2).magnitude;
        return dist < 0.1f;
    }
    private IEnumerator endSceneDelay()
    {
        yield return new WaitForSeconds(1.5f);
        buttonManager.endScene();
    }
    private IEnumerator timerStart()
    {
        timer = 0f;
        while (!timerStop)
        {
            //Debug.Log(allObjects.position);

            //Debug.Log(Quaternion.Inverse(allObjects.rotation)*(lastRealCursorPos - allObjects.position)- plane.localPosition);
            //Debug.Log("Test: " + (Quaternion.Inverse(allObjects.rotation) * allObjects.rotation *  allObjects.position));
            //Debug.Log("lastRealCursorPos: " + allObjects.position);
            //Debug.Log(() * (lastRealCursorPos  - plane.position) - allObjects.position);
            buttonManager.addTimeLocationData(timer, (Quaternion.Inverse(allObjects.rotation)) * (lastRealCursorPos - allObjects.position) - plane.localPosition);
            //Debug.Log("Headset: "+headsetTransform.rotation);
            buttonManager.addTimeHeadsetLocationData(timer, headsetTransform.position);
            buttonManager.addTimeHeadsetRotationData(timer, headsetTransform.rotation);
            buttonManager.addTimeControllerLocationData(timer, transform.position);
            buttonManager.addTimeControllerRotationData(timer, transform.rotation);
            timer += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        Debug.Log("Previous time taken: " + timer);
    }

    
    

    public void displayHideRealCursor()
    {
        displayRealCursor = !displayRealCursor;
    }
    private RaycastHit CreateRayCast(float length)
    {
        RaycastHit hit;
        Ray ray = new Ray(transform.position, transform.forward);
        Physics.Raycast(ray, out hit, length);

        return hit;
    }
    //Manipulate position of the displayed end point according to the intended position and the smoothing function
    private Vector3 manipulatePos(Vector3 pos, int attackOpt)
    {
        if (attackOpt == 0)
        {
            //This approach ignores deltaXVec
            Vector3 deltaXVec = pos - lastRealCursorPos;
            float deltaXDist = deltaXVec.magnitude;
            
            Vector3 remainingAttackVector = (targetOffset - currentOffset);
            float allowedManipulationDist = thresholdDistRatio * deltaXDist;
            Vector3 currentManipulateVector = allowedManipulationDist>(targetOffset-currentOffset).magnitude?remainingAttackVector:allowedManipulationDist*remainingAttackVector.normalized;
            

            
            //Debug.Log(currentManipulateVector);
            return lastDisplayedCursorPos + deltaXVec+ currentManipulateVector;
        }
        else if (attackOpt == 1)
        {
            //this approach uses the threshold angle and maximum manipulation distance to calculate manipulation vector;
            Vector3 deltaXVec = pos - lastRealCursorPos;
            float deltaXDist = deltaXVec.magnitude;

            Vector3 remainingAttackVector = (targetOffset - currentOffset);
            Vector3 targetCursorMovementVector = deltaXVec + remainingAttackVector;

            //max change in magnitude is currently hard coded
            Vector3 currentManipulateVector = Vector3.RotateTowards(deltaXVec, targetCursorMovementVector, Mathf.Deg2Rad * thresholdAngle, 0.2f);
            


            //Debug.Log(currentManipulateVector);
            return lastDisplayedCursorPos + currentManipulateVector;
        }
        else if (attackOpt == 2)
        {
            return pos;
        }
        else
        {
            return pos;
        }
        
    }
    private Vector3 smoothPos(Vector3 pos)
    {
        
       
        Vector3 deltaXVec = pos - lastRealCursorPos;
        float deltaXDist = deltaXVec.magnitude;

        //in case of smoothing, target offset is 0
        Vector3 remainingAttackVector = ( -currentOffset) ;
        float allowedManipulationDist = thresholdDistRatio * deltaXDist;
        
        Vector3 currentManipulateVector = allowedManipulationDist > (- currentOffset).magnitude ? remainingAttackVector : allowedManipulationDist*remainingAttackVector.normalized;
        


        //Debug.Log(currentManipulateVector);
        return lastDisplayedCursorPos + deltaXVec + currentManipulateVector;


    }



    //Event callback function when the user is in the danger zone
    public void OnEnterMaliciousZone()
    {
        if (!inZone)
        {
            inZone = true;
            Debug.Log("Entered malicioius zone");
        }
        
    }
    public void OnExitMaliciousZone()
    {
        if (inZone)
        {
            inZone = false;

            Debug.Log("Exited malicioius zone");
        }
        
    }
    


}

