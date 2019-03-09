using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    [SerializeField] float speed = 5.0f;

    Rigidbody2D rb;
    Vector3 mousePosition;
    Vector2 forceDirection;
    bool isString;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        ControlChar();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
    }

    private void ControlChar()
    {
        mousePosition = Input.mousePosition;
        mousePosition.z = 10.0f;
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);

        if (Input.GetMouseButtonDown(0) && Vector3.Distance(mousePosition, transform.position) < 1.5f)
        {
            isString = true;
        }

        Debug.Log(isString);
        if (Input.GetMouseButtonUp(0) && isString == true)
        {
            rb.constraints = RigidbodyConstraints2D.None;
            forceDirection = transform.position - mousePosition;
            rb.AddForce(Vector3.ClampMagnitude(forceDirection, 1.5f) * 100.0f * speed);
            isString = false;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 1.5f);
    }
}

