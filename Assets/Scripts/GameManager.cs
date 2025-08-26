using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public int biscuits { get; private set; }
    public PlayerController PlayerController;
    public bool startGame;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        InputSystem.DisableAllEnabledActions();
        PlayerController.LockCam = false;
    }

    private void Update()
    {
        if (startGame)
        {
            StartGame();
            startGame = false;
        }
    }

    public void AddBiscuits(int amount)
    {
        biscuits += amount;
    }

    public Vector3 GetPlayerPosition()
    {
        return PlayerController.transform.position;
    }

    public void StartGame()
    {
        InputSystem.actions.Enable();
        PlayerController.LockCam = true;
        PlayerController.transform.position = new Vector3(0,1,-90); 
    }
    public void EndGame()
    {
        //InputSystem.DisableAllEnabledActions();
        //SceneManager.LoadScene("Level1", LoadSceneMode.Single);
    }
}
