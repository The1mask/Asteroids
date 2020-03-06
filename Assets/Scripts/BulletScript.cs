using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    private Quaternion _rotaton;
    private SpawnerScript _spawner;

    [Header("Bullet Speed")]
    [SerializeField]
    private float Speed = 0.01f;

    void Start()
    {
        transform.rotation = _rotaton;
        Invoke("DestroySelf", 2f);
        _spawner = GameObject.FindGameObjectWithTag("Spawner").GetComponent<SpawnerScript>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position = Vector3.MoveTowards(transform.position,transform.position+transform.up, Speed);
    }

    void OnTriggerEnter2D(Collider2D colider)
    {
        if(colider.tag == "Asteroid")
        {
            colider.SendMessage("NextStage");
            _spawner.SendMessage("SetNewScore");
            Destroy(this.gameObject);
        }

        if (colider.tag == "Alien")
        {
            _spawner.SendMessage("SetNewScore");
            Destroy(colider.gameObject);
            Destroy(this.gameObject);
        }
    }

    #region MessageMethods

    void SetDirection(Quaternion rotate)
    {
        _rotaton = rotate;
    }

    #endregion

    #region InvokeMethods

    void DestroySelf()
    {
        Destroy(this.gameObject);
    }

    #endregion
}
