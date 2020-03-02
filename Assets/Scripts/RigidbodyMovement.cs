using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigidbodyMovement : MonoBehaviour
{
    public float m_speed = 5f;
    public float m_jumpHeight = 2f;
    public float m_groundDistance = 0.2f;
    public float m_dashDistance = 5f;
    public LayerMask m_ground;

    private Rigidbody m_body;
    private Vector3 m_inputs = Vector3.zero;
    private bool m_isGrounded = true;
    private Transform m_groundChecker;

    // Start is called before the first frame update
    void Start()
    {
        m_body = GetComponent<Rigidbody>();
        m_groundChecker = transform.GetChild(0);
    }

    // Update is called once per frame
    void Update()
    {
        m_isGrounded = Physics.CheckSphere(m_groundChecker.position, m_groundDistance, m_ground, QueryTriggerInteraction.Ignore);

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 r = transform.right * x;
        Vector3 f = transform.forward * z;

        m_inputs = r + f;

        if (Input.GetButtonDown("Jump") && m_isGrounded)
        {
            m_body.AddForce(Vector3.up * Mathf.Sqrt(m_jumpHeight * -2f * Physics.gravity.y), ForceMode.VelocityChange);
        }
        if (Input.GetButtonDown("Dash"))
        {
            Vector3 dashVelocity = Vector3.Scale(transform.forward, m_dashDistance * new Vector3((Mathf.Log(1f / (Time.deltaTime * m_body.drag + 1)) / -Time.deltaTime), 0, (Mathf.Log(1f / (Time.deltaTime * m_body.drag + 1)) / -Time.deltaTime)));
            m_body.AddForce(dashVelocity, ForceMode.VelocityChange);
        }
    }

    void FixedUpdate()
    {
        m_body.MovePosition(m_body.position + m_inputs * m_speed * Time.fixedDeltaTime);
    }
}
