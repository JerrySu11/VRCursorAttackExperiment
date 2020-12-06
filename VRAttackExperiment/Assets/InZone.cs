using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class InZone : MonoBehaviour
{
    public UnityEvent inDangerZone;
    public Transform targetPoint;
    private bool inZone = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float dist = Mathf.Sqrt(Mathf.Pow(transform.position.x - targetPoint.position.x, 2) + Mathf.Pow(transform.position.z - targetPoint.position.z, 2));
        if (dist < 1.0 && !inZone)
        {
            inZone = true;
            inDangerZone.Invoke();
            Debug.Log("In zone");
        }
        else if (dist > 1.0 && inZone)
        {
            inZone = false;
            Debug.Log("Out of zone");
        }
    }
}
