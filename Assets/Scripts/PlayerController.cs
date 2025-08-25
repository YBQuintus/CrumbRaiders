using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private InputAction m_MoveAction;
    private InputAction m_JumpAction;
    private InputAction m_CrouchAction; 
    private InputAction m_SprintAction;
    [SerializeField] private GameObject m_Model;
    [SerializeField] private GameObject m_CamContainer;
    private Rigidbody m_Rigidbody;
    private Vector2 m_lastAccel = Vector2.zero;
    private Vector3 m_CamPos;
    public float Stamina;
    public bool LockCam = true;

    private void OnEnable()
    {
        m_MoveAction = InputSystem.actions.FindAction("Move");
        m_JumpAction = InputSystem.actions.FindAction("Jump");
        m_CrouchAction = InputSystem.actions.FindAction("Crouch");
        m_SprintAction = InputSystem.actions.FindAction("Sprint");
        m_Rigidbody = GetComponent<Rigidbody>();
        m_CamPos = m_CamContainer.transform.localPosition;
        m_JumpAction.started += Jump;
    }
    void Start()
    {
        
    }

    void FixedUpdate()  
    {
        Vector2 moveAccel = m_MoveAction.ReadValue<Vector2>();
        if (moveAccel.sqrMagnitude > 0.01f)
        {
            m_lastAccel = moveAccel;
        }
        if (LockCam)
        {
            m_CamContainer.transform.localPosition = Vector3.Slerp(m_CamContainer.transform.localPosition, m_CamPos + new Vector3(moveAccel.x, 0, moveAccel.y), 0.025f);
        }
        Quaternion targetRotation;
        if ((new Vector2(m_Rigidbody.linearVelocity.x, m_Rigidbody.linearVelocity.z)).magnitude > 0.1f)
        {
            targetRotation = Quaternion.Euler(0, Mathf.Rad2Deg * Mathf.Atan2(m_Rigidbody.linearVelocity.x, m_Rigidbody.linearVelocity.z), 0);
        }
        else
        {
            targetRotation = Quaternion.Euler(0, Mathf.Rad2Deg * Mathf.Atan2(m_lastAccel.x, m_lastAccel.y), 0);
        }

        // Jumping logic
        if (Physics.Raycast(transform.position, Vector3.down, 1))
        {
            Stamina += Time.fixedDeltaTime;
            if (Stamina >= 5) Stamina = 5;
            if (m_SprintAction.IsPressed())
            {
                Stamina -= 2 * Time.fixedDeltaTime;
                if (Stamina <= 0)
                {
                    Stamina = 0;
                }
                else
                {
                    moveAccel *= 1.875f;
                    targetRotation *= Quaternion.Euler(15, 0, 0);
                }  
            }
            else if (m_CrouchAction.IsPressed())
            {
                moveAccel *= 0.5f;
            }
            m_Rigidbody.AddForce(50 * new Vector3(moveAccel.x, 0, moveAccel.y), ForceMode.Acceleration);
            m_Rigidbody.AddForce(-7.5f * new Vector3(m_Rigidbody.linearVelocity.x, 0, m_Rigidbody.linearVelocity.z), ForceMode.Acceleration);
        }
        else
        {
            if (m_Rigidbody.linearVelocity.x != 0 && m_Rigidbody.linearVelocity.z != 0)
            {
                m_Rigidbody.AddForce(10 * new Vector3(moveAccel.x, 0, moveAccel.y), ForceMode.Acceleration);
            }
            else
            {
                m_Rigidbody.AddForce(2 * new Vector3(moveAccel.x, 0, moveAccel.y), ForceMode.Acceleration);
            }
            m_Rigidbody.AddForce(-1 * new Vector3(m_Rigidbody.linearVelocity.x, 0, m_Rigidbody.linearVelocity.z), ForceMode.Acceleration);
        }
        m_Model.transform.rotation = Quaternion.Slerp(m_Model.transform.rotation, targetRotation, 0.4f); 
    }
    void Jump(InputAction.CallbackContext context)
    {
        if (Physics.Raycast(transform.position, Vector3.down, 1))
        {
            m_Rigidbody.AddForce(6 * Vector3.up, ForceMode.VelocityChange);
        }
    }
}
