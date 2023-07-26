using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    [SerializeField] private GameObject[] enemies;
    private Vector3[] initialPosition; 
    // Start is called before the first frame update
    void Start()
    {
        // Save initital positions of the enemies
        initialPosition = new Vector3[enemies.Length];
        for ( int i = 0; i < enemies.Length; i++)
        {
            if (enemies[i] != null)
                initialPosition[i] = enemies[i].transform.position;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ActivateRoom(bool _status)
    {
        //Activate / Deactivate enemies
        for (int i = 0; i < enemies.Length; i++)
        {
            if (enemies[i] != null)
            {
                enemies[i].SetActive(_status);
                enemies[i].transform.position = initialPosition[i];
            }
        }
    }
}
