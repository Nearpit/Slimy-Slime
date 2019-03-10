using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fireplaceScript : MonoBehaviour
{
    private float time = 0;
    float estimatedTime;
    private bool isActive = true;
    [SerializeField] GameObject character;
    [SerializeField] GameObject psaver;
    // Start is called before the first frame update
    void Awake()
    {
        //character.GetComponent<PlayerControl>().AddHP(1);
    }
    void Start()
    {
        psaver.GetComponent<ProgressSaver>().Load();
        psaver.GetComponent<ProgressSaver>().characterData.isFuelGot = false;
        estimatedTime = CountEstimatedTime();
    }

    public float CountEstimatedTime()
    {
        return 1f;
    }
    // Update is called once per frame
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == character.name)
        {
            for (int i = 0; i < 10; i++)
            {
                character.GetComponent<PlayerControl>().AddHP(1);
            }
        }
        if (psaver.GetComponent<ProgressSaver>().characterData.isFuelGot)
        {
            psaver.GetComponent<ProgressSaver>().Save();
            Debug.Log("Fuel " + psaver.GetComponent<ProgressSaver>().characterData.fuel_gathered.ToString());
            Debug.Log("Wood " + psaver.GetComponent<ProgressSaver>().characterData.wood_gathered.ToString());
            Debug.Log("Level Complete");
        }
    }

        void Update()
    {
        time += Time.deltaTime;
        if (time > estimatedTime)
        {
            isActive = false;
        }
    }
}
