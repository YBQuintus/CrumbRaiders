using UnityEngine;

public class Button : Interactable
{
    [SerializeField] private GameObject buttonModel;
    [SerializeField] private float pressDepth;
    private AudioSource pressSound;
    private Vector3 unpressedPosition;
    private Vector3 pressedPosition;
    private bool decreasing = false;

    void Start()
    {
        unpressedPosition = buttonModel.transform.localPosition;
        pressedPosition = unpressedPosition + Vector3.down * pressDepth;
        pressSound = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (decreasing)
        {
            buttonModel.transform.localPosition = Vector3.MoveTowards(buttonModel.transform.localPosition, pressedPosition, 0.75f * Time.deltaTime);
        }
        else
        {
            buttonModel.transform.localPosition = Vector3.MoveTowards(buttonModel.transform.localPosition, unpressedPosition, 0.75f * Time.deltaTime);
        }
    }
        
    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
        if (other.CompareTag("Player"))
        {
            decreasing = true;
            pressSound.Play();
        }
    }

    protected override void OnTriggerExit(Collider other)
    {
        base.OnTriggerExit(other);
        if (other.CompareTag("Player"))
        {
            decreasing = false;
        }
    }
}

