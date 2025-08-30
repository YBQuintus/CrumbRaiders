using JetBrains.Annotations;
using System.Collections.Generic;
using UnityEngine;

public class MovePrefabs : MonoBehaviour
{
    public GameObject prefabToMove;
    private List<GameObject> clonedPrefabs = new();
    public float vertOffset;
    public int numberOfClones;
    public int timePeriod;
    public float timeBeforeFall;
    public float speed;
    private float timeBetweenPrefabs;

    private void Start()
    {
        for (int i = 0; i < numberOfClones; i++)
        {
            GameObject clone = Instantiate(prefabToMove, transform.position + transform.up * vertOffset, Quaternion.identity);
            clone.transform.parent = transform; 
            clonedPrefabs.Add(clone);
        }
        timeBetweenPrefabs = (float)timePeriod / numberOfClones;
    }

    private void Update()
    {
        
        for (int i = 0; i < numberOfClones; i++)
        {
            float lerpTime = (Time.time + i * timeBetweenPrefabs) % timePeriod;
            clonedPrefabs[i].transform.localPosition = speed * lerpTime * -transform.forward + transform.up * vertOffset;
            if (lerpTime >= timeBeforeFall)
            {
                clonedPrefabs[i].transform.localPosition += 4 * (lerpTime - timeBeforeFall) * (lerpTime - timeBeforeFall) * -transform.up;
            }
        }
    }
}
