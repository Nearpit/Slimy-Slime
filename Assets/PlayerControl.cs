using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerControl : MonoBehaviour
{
    [SerializeField] float speed = 5.0f;

    Rigidbody2D rb;
    Vector3 mousePosition;
    Vector2 forceDirection;
    Vector3 velocityObl;
    Vector3 objectPlace = Vector3.zero;
    bool isString;
    bool isFirst;
    int hp = 10;
    int gnum = 3;
    int got = 0;
    int[,] tokenArray = new int[,] { { -3,0 },{ 5,0 } };
    [SerializeField] GameObject oblast;
    [SerializeField] GameObject goal;
    [SerializeField] Tilemap tilemap;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        for (int i = 1; i < gnum; i++)
        {
            Instantiate(goal, new Vector3(0,0), Quaternion.identity);
        }
    }

    private void Update()
    {
        ControlChar();
        if (hp < 1)
        {
            Debug.Log("You are ded");
        }

        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == oblast.name + "(Clone)" || collision.gameObject.name == oblast.name)
        {
            if (isFirst)
            {
                isFirst = false;
            }
            else
            {
                Debug.Log("You are ded");
                tilemap.ClearAllTiles();
            }
        }
        if (collision.gameObject.name == goal.name + "(Clone)" || collision.gameObject.name == goal.name)
        {
            got++;
            if (got == gnum)
            {
                Debug.Log("WIN");
            }
            collision.gameObject.SetActive(false);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        rb.constraints = RigidbodyConstraints2D.FreezeAll;

        velocityObl = rb.velocity;

        if (velocityObl.x > 0.5f)
        {
            velocityObl.x = 0.5f;
        }
        if (velocityObl.y > 0.5f)
        {
            velocityObl.y = 0.5f;
        }

        Vector2 hitsum = Vector2.zero;
        float hitnum = 0;
        if (tilemap.gameObject == collision.gameObject)
        {
            foreach (ContactPoint2D hit in collision.contacts)
            {
                hitnum += 1;
                hitsum.x = hitsum.x + hit.point.x - 0.01f * hit.normal.x;
                hitsum.y = hitsum.y + hit.point.y - 0.01f * hit.normal.y;
            }
            objectPlace.x = hitsum.x / hitnum;
            objectPlace.y = hitsum.y / hitnum;
            Instantiate(oblast, objectPlace, Quaternion.identity);
            Vector3Int vec = Vector3Int.zero;
            objectPlace = tilemap.WorldToCell(objectPlace);
            vec.x = Mathf.RoundToInt(objectPlace.x);
            vec.y = Mathf.RoundToInt(objectPlace.y);
            Debug.Log(vec);
            tilemap.SetTile(vec, null);
            isFirst = true;
        }
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

