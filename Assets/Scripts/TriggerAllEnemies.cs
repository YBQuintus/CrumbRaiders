using UnityEngine;

public class TriggerAllEnemies : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            EnemyController.SpottingOverride = true;
        }
    }
}
