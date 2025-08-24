using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private InputAction m_MoveAction;
    private InputAction m_JumpAction;
    private InputAction m_CrouchAction;
    private InputAction m_SprintAction;
    private Vector3 m_velocity = Vector3.zero;
    [SerializeField] private GameObject m_Model;

    private void OnEnable()
    {
        m_MoveAction = InputSystem.actions.FindAction("Move");
        m_JumpAction = InputSystem.actions.FindAction("Jump");
        m_CrouchAction = InputSystem.actions.FindAction("Crouch");
        m_SprintAction = InputSystem.actions.FindAction("Sprint");
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector2 moveAccel = m_MoveAction.ReadValue<Vector2>();
        m_velocity += 5 * Time.fixedDeltaTime * new Vector3(moveAccel.x, 0, moveAccel.y);
        

        if (Physics.Raycast(transform.position, Vector3.down, 1.1f))
        {
            if (m_JumpAction.IsPressed())
            {
                m_velocity.y = 0.5f;
            }
            else
            {
                m_velocity.y = 0;
            }
        }
        else
        {
            m_velocity.y -= 4f * Time.fixedDeltaTime;
        }
        m_velocity -= 10 * Time.fixedDeltaTime * new Vector3(m_velocity.x, 0, m_velocity.z);
        transform.position += m_velocity;
        m_Model.transform.rotation = Quaternion.Euler(180 * m_velocity.x, 0, 180 * m_velocity.y);
    }
}
