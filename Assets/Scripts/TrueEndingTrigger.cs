using UnityEngine;

public class TrueEndingTrigger : MonoBehaviour
{
    private AudioSource m_AudioSource;
    private void Start()
    {
        m_AudioSource = GetComponent<AudioSource>();
    }
    private void OnTriggerEnter(Collider other)
    {
        GameManager.Instance.EndGame(3);
        m_AudioSource.Play();
    }
}
