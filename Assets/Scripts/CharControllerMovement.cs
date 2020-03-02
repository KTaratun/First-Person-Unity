using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharControllerMovement : MonoBehaviour
{
    private CharacterController m_characterController;

    [SerializeField]
    private float m_speed;
    [SerializeField]
    private float m_jumpHeight;
    [SerializeField]
    private float m_gravity;
    private Vector3 m_velocity;

    [SerializeField]
    private Transform m_groundCheck;
    private float m_groundDistance;
    private LayerMask m_groundMask;
    private bool m_isGrounded;

    // Start is called before the first frame update
    void Start()
    {
        m_characterController = GetComponent<CharacterController>();
        m_groundMask = 1 << LayerMask.NameToLayer("Ground");
        m_groundDistance = 0.4f;
    }

    // Update is called once per frame
    void Update()
    {
        GroundCheck();
        Movement();
        Jump();
    }

    private void GroundCheck()
    {
        m_isGrounded = Physics.CheckSphere(m_groundCheck.position, m_groundDistance, m_groundMask);

        if (m_isGrounded && m_velocity.y < 0)
            m_velocity.y = -2f;
    }

    private void Movement()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 r = transform.right * x;
        Vector3 f = transform.forward * z;

        Vector3 move = r + f;
        m_characterController.Move(move * m_speed * Time.deltaTime);

        // Apply gravity
        m_velocity.y += m_gravity * Time.deltaTime;
        m_characterController.Move(m_velocity * Time.deltaTime);
    }

    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && m_isGrounded)
            m_velocity.y = Mathf.Sqrt(m_jumpHeight * -2f * m_gravity);
    }
}
