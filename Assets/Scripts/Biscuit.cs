using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Biscuit : MonoBehaviour
{
    [SerializeField] private float rotateRate;
    public static int totalBiscuits;
    private bool startTimer;
    private float time;
    private void Start()
    {
        totalBiscuits += 1;
        time = 0;
        startTimer = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        GameManager.Instance.AddBiscuits(1);
        GetComponent<Collider>().enabled = false;
        startTimer = true;
    }

    private void FixedUpdate()
    {
        if (startTimer)
        {
            time += Time.fixedDeltaTime;
            if (time >= 2f)
            {
                Destroy(gameObject);
                return;
            }
            else
            {
                rotateRate += 1000 * Time.fixedDeltaTime;
                transform.localPosition += Vector3.up * rotateRate / 5000;
            }
        }
        transform.Rotate(Vector3.up, rotateRate * Time.fixedDeltaTime);
    }
}
