using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Valve.VR;
public class VRInputModule : BaseInputModule
{
    public Camera m_Camera;
    public SteamVR_Input_Sources m_targetSource;
    public SteamVR_Action_Boolean m_ClickAction;

    private GameObject m_Current = null;
    private PointerEventData m_Data = null;

    protected override void Awake()
    {
        base.Awake();

        m_Data = new PointerEventData(eventSystem);
    }

    public override void Process()
    {
        // Rest data
        m_Data.Reset();
        m_Data.position = new Vector3(m_Camera.pixelWidth / 2, m_Camera.pixelHeight / 2);

        eventSystem.RaycastAll(m_Data, m_RaycastResultCache);
        m_Data.pointerCurrentRaycast = FindFirstRaycast(m_RaycastResultCache);
        m_Current = m_Data.pointerCurrentRaycast.gameObject;

        m_RaycastResultCache.Clear();

        HandlePointerExitAndEnter(m_Data, m_Current);

        if (m_ClickAction.GetStateDown(m_targetSource))
        {
            ProcessPress(m_Data);
        }
        if (m_ClickAction.GetStateUp(m_targetSource))
        {
            ProcessRelease(m_Data);
        }
    }
    public PointerEventData getData()
    {
        return m_Data;
    }

    private void ProcessPress(PointerEventData data)
    {
        Debug.Log("Press");
        //Set raycast
        data.pointerPressRaycast = data.pointerCurrentRaycast;
        //Check for object hit
        GameObject newPointerPress = ExecuteEvents.ExecuteHierarchy(m_Current, data, ExecuteEvents.pointerDownHandler);
        if (newPointerPress == null)
        {
            newPointerPress = ExecuteEvents.GetEventHandler<IPointerClickHandler>(m_Current);
        }
        //set data
        data.pressPosition = data.position;
        data.pointerPress = newPointerPress;
        data.rawPointerPress = m_Current;
    }
    private void ProcessRelease(PointerEventData data)
    {
        Debug.Log("Release");
        ExecuteEvents.Execute(data.pointerPress, data, ExecuteEvents.pointerUpHandler);

        GameObject pointerUpHandler = ExecuteEvents.GetEventHandler<IPointerClickHandler>(m_Current);

        if (data.pointerPress == pointerUpHandler)
        {
            ExecuteEvents.Execute(data.pointerPress, data, ExecuteEvents.pointerClickHandler);
        }
        eventSystem.SetSelectedGameObject(null);

        data.pressPosition = Vector2.zero;
        data.pointerPress = null;
        data.rawPointerPress = null;

    }

}

