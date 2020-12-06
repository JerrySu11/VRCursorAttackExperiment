using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using UnityEngine.Events;
public class Teleport : MonoBehaviour
{
    public GameObject m_Dot;
    public SteamVR_Action_Boolean teleportAction;

    private SteamVR_Behaviour_Pose pose = null;
    private LineRenderer m_LineRenderer = null;
    private bool m_hasPosition = false;
    private bool pointAtDangerZone = false;
    private int layerMask;
    public UnityEvent triggerMalicious;
    public UnityEvent triggerNormal;
    private bool isTeleporting = false;

    private float fadeTime = 0.5f;
    // Start is called before the first frame update
    void Start()
    {
        pose = GetComponent<SteamVR_Behaviour_Pose>();
        layerMask = LayerMask.GetMask("TeleportArea");
        m_LineRenderer = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        //Pointer
        m_hasPosition = UpdatePointer();
        m_Dot.SetActive(m_hasPosition);
        m_LineRenderer.enabled = m_hasPosition;
        //Teleport
        if (teleportAction.GetStateUp(pose.inputSource))
        {
            TryTeleport();
        }
    }
    private void TryTeleport()
    {
        if (isTeleporting  || !m_hasPosition)
        {
            return;
        }
        Transform cameraRig = SteamVR_Render.Top().origin;
        Vector3 headPosition = SteamVR_Render.Top().head.position;

        Vector3 groundPosition = new Vector3(headPosition.x, cameraRig.position.y, headPosition.z);
        Vector3 translateVector = m_Dot.transform.position - groundPosition;
        if (pointAtDangerZone)
        {
            triggerMalicious.Invoke();
        }
        else
        {
            triggerNormal.Invoke();
        }
        StartCoroutine(MoveRig(cameraRig, translateVector));

    }
    private IEnumerator MoveRig(Transform cameraRig, Vector3 translation)
    {
        isTeleporting = true;

        SteamVR_Fade.Start(Color.black, fadeTime, true);

        yield return new WaitForSeconds(fadeTime);
        cameraRig.position += translation;

        SteamVR_Fade.Start(Color.clear, fadeTime, true);

        isTeleporting = false;
    }
    private bool UpdatePointer()
    {
        //Ray from controller
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        //If hit
        if (Physics.Raycast(ray, out hit,layerMask))
        {
            if (hit.collider.gameObject.tag == "Attack")
            {
                pointAtDangerZone = true;
            }
            else
            {
                pointAtDangerZone = false;
            }
            m_Dot.transform.position = hit.point;
            m_LineRenderer.SetPosition(0, transform.position);
            m_LineRenderer.SetPosition(1, hit.point);
            return true;
        }

        //If not a hit
        return false;
    }
}
