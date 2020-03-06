using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    [SerializeField]
    private GameObject _bullet;
    private Rigidbody2D rb;
    private GameObject _createdBullet;
    private Transform _gun;
    private SpawnerScript _spawner;

    [Header("Player Сharacteristic")]
    public float Speed = 0.03f;
    public float RotateSpeed = 2;
    public float MaxSpeed = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        _spawner = GameObject.FindGameObjectWithTag("Spawner").GetComponent<SpawnerScript>();
        rb = GetComponent<Rigidbody2D>();
        _gun = this.transform.GetChild(0);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            _createdBullet = Instantiate(_bullet, _gun.position,Quaternion.identity) as GameObject;
            _createdBullet.SendMessage("SetDirection", transform.rotation);
        }
    }

    void FixedUpdate()
    {
        if (Input.GetAxis("Vertical") > 0)
        {
            if (rb.velocity.magnitude <= MaxSpeed)
            {
                rb.velocity += new Vector2(transform.up.x, transform.up.y) * Speed;
            }
        }

        transform.Rotate(0, 0, -Input.GetAxis("Horizontal") * RotateSpeed);
    }

    void OnTriggerEnter2D(Collider2D colider)
    {
        if (colider.tag == "Asteroid")
        {
            _spawner.SendMessage("Death");
            Destroy(colider.gameObject);
            Destroy(this.gameObject);
        }

        if (colider.tag == "Alien")
        {
            _spawner.SendMessage("Death");
            Destroy(colider.gameObject);
            Destroy(this.gameObject);
        }
    }
}
