using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistractionManager : MonoBehaviour
{
    public GameObject distractionObj;
    public Transform targetSpawnCenter;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    public void distract()
    {
        StartCoroutine("distraction");
    }
    IEnumerator distraction()
    {
        yield return new WaitForSeconds(0.2f);
        GameObject distractionObjTemp = Instantiate(distractionObj,SpawnPos(), Quaternion.identity);
        yield return new WaitForSeconds(1f);
        Destroy(distractionObjTemp);
    }
    private Vector3 SpawnPos()
    {
        int z_sign = Random.Range(1, 3), y_sign = Random.Range(1, 3);
        float z_offset = Random.Range(0f, 0.5f) * Mathf.Pow(-1, z_sign), y_offset = Random.Range(0f, 0.5f) * Mathf.Pow(-1, y_sign);

        return new Vector3(targetSpawnCenter.position.x, targetSpawnCenter.position.y+y_offset, targetSpawnCenter.position.z+z_offset);
    }
}
