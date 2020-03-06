using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlienScript : MonoBehaviour
{
    private GameObject Spaceship;

    [Header("Alien Speed")]
    [SerializeField]
    private float Speed = 0.04f;

    // Start is called before the first frame update
    void Start()
    {
        Spaceship = GameObject.FindGameObjectWithTag("Spaceship");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position = Vector2.MoveTowards(transform.position, Spaceship.transform.position, Speed);
    }
}
