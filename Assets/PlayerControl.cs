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
    bool isFuelGot = false;
    private int hp = 10;
    int woodnum = 3;
    int[,] woodArray = new int[,] { { -3,0 },{ 5,0 } ,{ 5, 2 } };
    [SerializeField] GameObject oblast;
    [SerializeField] GameObject wood;
    [SerializeField] GameObject fuel;
    [SerializeField] GameObject fireplace;
    [SerializeField] GameObject psaver;
    [SerializeField] Tilemap tilemap;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        for (int i = 0; i < woodnum; i++)
        {
            Instantiate(wood, new Vector3(woodArray[i,0],woodArray[i, 1]), Quaternion.identity);
        }
        Instantiate(fuel, new Vector3(-5,3), Quaternion.identity);
    }

    private void Update()
    {
        ControlChar();
        if (hp < 1)
        {
            Debug.Log("You are ded");
        }

        
    }
    public void AddHP(int amount)
    {   
        if (hp < 10)
        {
            hp += amount;
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
        if (collision.gameObject.name == wood.name + "(Clone)" || collision.gameObject.name == wood.name)
        {
            psaver.GetComponent<ProgressSaver>().AddWood(1);
            collision.gameObject.SetActive(false);
        }
        if (collision.gameObject.name == fuel.name + "(Clone)" || collision.gameObject.name == fuel.name)
        {
            psaver.GetComponent<ProgressSaver>().AddFuel(1);
            psaver.GetComponent<ProgressSaver>().characterData.isFuelGot = true;
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
            isFirst = true;
            /*Vector3Int vec = Vector3Int.zero;
            objectPlace = tilemap.WorldToCell(objectPlace);
            vec.x = Mathf.RoundToInt(objectPlace.x);
            vec.y = Mathf.RoundToInt(objectPlace.y);
            Debug.Log(vec);
            tilemap.SetTile(vec, null);
            isFirst = true;*/
            hp -= 1;
            Debug.Log(hp);
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

