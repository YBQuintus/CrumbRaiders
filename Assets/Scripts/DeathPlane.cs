using UnityEngine;

public class DeathPlane : MonoBehaviour
{
    private AudioSource deathSound;

    private void Start()
    {
        deathSound = GetComponent<AudioSource>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        if (!deathSound.isPlaying)
        {
            deathSound.Play();
        }
        GameManager.Instance.EndGame(1);
    }
}
