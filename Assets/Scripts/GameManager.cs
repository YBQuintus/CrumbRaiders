using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public int biscuits { get; private set; }
    public PlayerController PlayerController;
    public bool startGame;
    public AudioSource gameMusic;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        startGame = false;
        PlayerController.LockCam = false;
        gameMusic = GetComponent<AudioSource>();
    }

    public void Start()
    {
        StartCoroutine(DisableActions());
    }

    IEnumerator DisableActions()
    {
        yield return new WaitForSeconds(0.1f);
        InputSystem.actions.actionMaps[0].Disable();
    }

    private void Update()
    {
        if (startGame)
        {
            StartGame();
            startGame = false;
        }
        else
        {

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
        gameMusic.Play();
    }
    public void EndGame(int index)
    {
        InputSystem.actions.actionMaps[0].Disable();
        PlayerController.transform.position = new Vector3(0, 1, -90);
        gameMusic.Stop();
        GetComponentInChildren<UIManager>().ShowEnding(index);
        Biscuit.totalBiscuits = 0;
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name, LoadSceneMode.Single);
    }
}
