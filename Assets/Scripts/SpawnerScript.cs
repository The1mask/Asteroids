using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class SpawnerScript : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject Asteroid;
    public GameObject SpaceShip;
    public GameObject Alien;

    private GameObject _spawnedAsteroid;   // Created obj
    private GameObject _spawnedAlien;
    private GameObject _spawnedPlayer;

    private float _timerAst = 0;           // Timers
    private float _timerAlien = 0;

    [Header("Spawn Delay")]
    [SerializeField]
    private float _nextAsteroidSpawnTime = 1f;
    [SerializeField]
    private float _nextAlienSpawnTime = 10f;

    private int Heach = 3;                 // Current player heach and score
    private int Score = 0;
    private bool _playerAlive;

    private GameObject[] _heach;           // UI
    private TextMeshProUGUI _score;

    private Vector3 camSize;

    void Start()
    {
        camSize = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
        SpawnNewSpaceShip();
        _heach = new GameObject[3];

        for (int i = 0;i<3;i++)
        {
            _heach[i] = GameObject.FindGameObjectWithTag("Heach").transform.GetChild(i).gameObject;
        }

        _score = GameObject.FindGameObjectWithTag("Score").GetComponent<TextMeshProUGUI>();
    }

    void FixedUpdate()
    {
        _timerAst += Time.fixedDeltaTime;
        _timerAlien += Time.fixedDeltaTime;
        if (_timerAst >= _nextAsteroidSpawnTime)
        {
            if (_timerAlien >= _nextAlienSpawnTime)
            {
                _nextAsteroidSpawnTime -= 0.1f;
                _spawnedAlien = Instantiate(Alien, FindDirection(Random.Range(0, 4)),Quaternion.identity) as GameObject;  //Positions: 0 - up, 1 - left, 2 - down, 3 - right
                _timerAlien = 0;
            }

            _timerAst = 0;
            SpawnNewAsteroid(Random.Range(0, 4));    //Positions: 0 - up, 1 - left, 2 - down, 3 - right
        }

        if(_playerAlive)
            PlayerOutOfBoundsControl();
    }

    #region PlayerControll

    void PlayerOutOfBoundsControl()
    {
        if (_spawnedPlayer.transform.position.x > camSize.x)
            _spawnedPlayer.transform.position = new Vector2(-camSize.x, _spawnedPlayer.transform.position.y);
        if (_spawnedPlayer.transform.position.x < -camSize.x)
            _spawnedPlayer.transform.position = new Vector2(camSize.x, _spawnedPlayer.transform.position.y);
        if (_spawnedPlayer.transform.position.y > camSize.y)
            _spawnedPlayer.transform.position = new Vector2(_spawnedPlayer.transform.position.x, -camSize.y);
        if (_spawnedPlayer.transform.position.y < -camSize.y)
            _spawnedPlayer.transform.position = new Vector2(_spawnedPlayer.transform.position.x, camSize.y);
    }

    #endregion

    #region SpawnMethods

    void SpawnNewSpaceShip()
    {
        _playerAlive = true;
        _spawnedPlayer = Instantiate(SpaceShip, new Vector2(0, 0), Quaternion.identity) as GameObject;
    }

    void SpawnNewAsteroid(int _spawnDirection)
    {
        _spawnedAsteroid = Instantiate(Asteroid, FindDirection(_spawnDirection), Quaternion.identity) as GameObject;
        _spawnedAsteroid.SendMessage("SetRotation", Quaternion.LookRotation(Vector3.forward, FindDirection(ReverceDirection(_spawnDirection))));
        _spawnedAsteroid.SendMessage("SetStage", Random.Range(1,4));
    }

    #endregion

    #region AsteroidDirectionControl

    int ReverceDirection(int _spawnDirection)
    {
        switch (_spawnDirection)
        {
            case 0:
                return 2;
            case 1:
                return 3;
            case 2:
                return 0;
            case 3:
                return 1;
        }
        return -1;
    }

    Vector2 FindDirection(int _spawnDirection)
    {

        switch (_spawnDirection)
        {
            case 0:

                return new Vector2(Random.Range(-camSize.x, camSize.x), camSize.y + 1);
                
            case 1:

                return new Vector2(-camSize.x - 1, Random.Range(-camSize.y, camSize.y));

            case 2:

                return new Vector2(Random.Range(-camSize.x, camSize.x), -camSize.y - 1);

            case 3:

                return new Vector2(camSize.x + 1, Random.Range(-camSize.y, camSize.y));
        }
        return new Vector2(0,0);
    }

    #endregion

    #region MessageMethods

    void SetNewScore()
    {
        Score += 5;
        _score.SetText(Score.ToString());
    }

    void Death()
    {
        _playerAlive = false;

        if (_spawnedAlien != null)
            Destroy(_spawnedAlien.gameObject);

        _timerAlien = 0;

        Destroy(_heach[Heach - 1]);
        Heach--;

        if (Heach >= 1)
            Invoke("SpawnNewSpaceShip", 5);
        else
            SceneManager.LoadScene(0);
    }
    #endregion
}
