using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private InputAction m_MoveAction;
    private InputAction m_JumpAction;
    private InputAction m_CrouchAction; 
    private InputAction m_SprintAction;
    [SerializeField] private GameObject m_Model;
    [SerializeField] private Rigidbody m_Rigidbody;

    private void OnEnable()
    {
        m_MoveAction = InputSystem.actions.FindAction("Move");
        m_JumpAction = InputSystem.actions.FindAction("Jump");
        m_CrouchAction = InputSystem.actions.FindAction("Crouch");
        m_SprintAction = InputSystem.actions.FindAction("Sprint");
        m_Rigidbody = GetComponent<Rigidbody>();  
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Movement input
        Vector2 moveAccel = m_MoveAction.ReadValue<Vector2>();
        m_Rigidbody.AddForce(50 * new Vector3(moveAccel.x, 0, moveAccel.y), ForceMode.Acceleration);

        // Jumping logic
        if (Physics.Raycast(transform.position, Vector3.down, 1.1f))
        {
            if (m_JumpAction.IsPressed())
            {
                m_Rigidbody.AddForce(5 * Vector3.up, ForceMode.VelocityChange);
            }
        }
        else
        {
            // Apply upward force when not grounded (e.g., floating or falling)
            m_Rigidbody.AddForce(3 * Vector3.up, ForceMode.Acceleration);
        }

        // Damping movement
        m_Rigidbody.AddForce(-5 * m_Rigidbody.linearVelocity, ForceMode.Acceleration);

        // Rotate character based on movement direction
        if (m_Rigidbody.linearVelocity.x != 0 && m_Rigidbody.linearVelocity.magnitude > 0.1f)
        {
            m_Model.transform.rotation = Quaternion.Euler(0, Mathf.Rad2Deg * Mathf.Atan2(m_Rigidbody.linearVelocity.x, m_Rigidbody.linearVelocity.z), 0);
        }
    }

}
