using UnityEngine;

public class Biscuit : MonoBehaviour
{
    [SerializeField] float rotateRate;
    public static int totalBiscuits;
    private void Awake()
    {
        totalBiscuits += 1;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        GameManager.Instance.AddBiscuits(1);
        Destroy(gameObject);
    }

    private void FixedUpdate()
    {
        transform.Rotate(Vector3.up, rotateRate * Time.fixedDeltaTime);
    }
}
