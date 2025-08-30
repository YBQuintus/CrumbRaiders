using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class EndingTrigger : MonoBehaviour
{
    [SerializeField] private GameObject[] gameObjects;
    private InputAction m_JumpAction;

    private void OnEnable()
    {
        m_JumpAction = InputSystem.actions.FindAction("Jump");
    }
    private void OnTriggerEnter(Collider other)
    {
        if (GameManager.Instance.biscuits < Biscuit.totalBiscuits)
        {
            m_JumpAction.started += StartSequenceOnJump;
        }
    }

    private void OnDestroy()
    {
        m_JumpAction.started -= StartSequenceOnJump;
    }

    IEnumerator EndingSequence()
    {
        Time.timeScale = 0.25f;
        GameManager.Instance.gameMusic.Stop();
        yield return new WaitForSecondsRealtime(2);
        Time.timeScale = 0;
        foreach (var gameObject in gameObjects)
        {
            Destroy(gameObject);
        }
        yield return new WaitForSecondsRealtime(2);
        Time.timeScale = 1;
        GameManager.Instance.PlayerController.transform.GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
        yield return new WaitForSecondsRealtime(1.25f);
        GameManager.Instance.EndGame(2);

    }

    private void StartSequenceOnJump(InputAction.CallbackContext context)
    {
        m_JumpAction.started -= StartSequenceOnJump;
        StartCoroutine(EndingSequence());
    }
}
