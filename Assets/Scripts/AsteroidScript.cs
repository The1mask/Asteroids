using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class AsteroidScript : MonoBehaviour
{
    [SerializeField]
    private GameObject OriginalPrefab;
    private Quaternion _rotaton;
    private GameObject _childAsteroid;


    [Header("Asteroid Сharacteristic")]
    [SerializeField]
    private float[] _speed = { 0.08f, 0.06f, 0.04f };
    private int _stage = 3;                             //3 stages: 3 - big size, low speed; 2 - mid size, mid speed; 1 low size, max speed; 

    void Start()
    {
        transform.rotation = _rotaton;

        switch (_stage)
        {
            case 3:
                transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
                break;
            case 2:
                transform.localScale = new Vector3(0.6f, 0.6f, 0.6f);
                break;
            case 1:
                transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);
                break;
        }

        Invoke("DestroySelf", 8);
    }

    void FixedUpdate()
    {
        transform.position = Vector3.MoveTowards(transform.position, transform.position + transform.up, _speed[_stage-1]);
    }

    void NextStage()                        //Create new child asteroids, send them rotation and destroy self
    {
        _stage--;
        switch (_stage)
        {
            case 2:
                _childAsteroid = Instantiate(OriginalPrefab, transform.position,Quaternion.identity) as GameObject;
                _childAsteroid.SendMessage("SetStage", 2);
                _childAsteroid.SendMessage("SetRotation", new Quaternion(transform.rotation.x, transform.rotation.y, transform.rotation.z - 0.1f, transform.rotation.w));

                _childAsteroid = Instantiate(OriginalPrefab, transform.position, Quaternion.identity) as GameObject;
                _childAsteroid.SendMessage("SetStage", 1);
                _childAsteroid.SendMessage("SetRotation", new Quaternion(transform.rotation.x, transform.rotation.y, transform.rotation.z + 0.1f, transform.rotation.w));
                break;
            case 1:
                _childAsteroid = Instantiate(OriginalPrefab, transform.position, Quaternion.identity) as GameObject;
                _childAsteroid.SendMessage("SetStage", 1);
                _childAsteroid.SendMessage("SetRotation", new Quaternion(transform.rotation.x, transform.rotation.y, transform.rotation.z+0.1f, transform.rotation.w) );

                _childAsteroid = Instantiate(OriginalPrefab, transform.position, Quaternion.identity) as GameObject;
                _childAsteroid.SendMessage("SetStage", 1);
                _childAsteroid.SendMessage("SetRotation", new Quaternion(transform.rotation.x, transform.rotation.y, transform.rotation.z - 0.1f, transform.rotation.w));
                break;
        }
        Destroy(this.gameObject);
    }

    #region MessageMethods

    public void SetRotation(Quaternion _target)
    {
        _rotaton = _target;
    }

    void SetStage(int stage)
    {
        _stage = stage;
    }

    #endregion

    #region InvokeMethods

    void DestroySelf()
    {
        Destroy(this.gameObject);
    }

    #endregion
}
