using UnityEngine;

public class Biscuit : MonoBehaviour
{
    [SerializeField] int biscuitValue;
    [SerializeField] float rotateRate;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        GameManager.Instance.AddBiscuits(biscuitValue);
        Destroy(gameObject);
    }

    private void FixedUpdate()
    {
        transform.Rotate(Vector3.up, rotateRate * Time.fixedDeltaTime);
    }
}
