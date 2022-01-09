using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class PackageDelivery : MonoBehaviour
{
    public TextMeshProUGUI ScoreText;
    public TextMeshProUGUI PackagesDeliveredText;
    public TextMeshProUGUI OnHitText;
    public TextMeshProUGUI CurrentDeliveryText;
    public GameObject OnHitMessage;
    
    private int _points = 0;
    private int _deliveryPoints = 0;
    private const int PACKAGE_DELIVERY_POINT_BONUS = 10;
    private const int HIT_PUNISHMENT = 2;
    private bool _hasPackage = false;
    private int _deliveriesCompleted = 0;
    private bool _canCollide = true;
    private float _hitTextDelay = 0;
    private GameObject _package;
    private GameObject _deliveryPoint;
    private int _lastLocation = -1;
    private Timer _gameTimer;
    private Vector3 _playerStartLocation;
    private Vector3 _packageStartLocation;
    private CarProgress _carProgress;

    private List<Vector3> _deliveryLocations = new List<Vector3>()
    {
        new Vector3(-50.0f, 30.5f, 0.0f),
        new Vector3(-3.5f, 1.0f, 0.0f),
        new Vector3(-19.0f, 30.5f, 0.0f),
        new Vector3(16.0f, 30.5f, 0.0f),
        new Vector3(34.5f, 26.0f, 0.0f),
        new Vector3(-64.5f, 5.5f, 0.0f),
        new Vector3(-64.5f, -32.0f, 0.0f),
        new Vector3(59.0f, -33.0f, 0.0f),
        new Vector3(59.0f, -0.5f, 0.0f),
        new Vector3(49.5f, -50.5f, 0.0f),
        new Vector3(24.0f, -55.5f, 0.0f),
        new Vector3(-50.0f, -55.5f, 0.0f),
        new Vector3(-3.5f, -31.0f, 0.0f),
        new Vector3(-45.5f, -13.0f, 0.0f),
        new Vector3(6.5f, -19.0f, 0.0f),
        new Vector3(30.0f, -14.5f, 0.0f),
    };

    void Start()
    {
        _playerStartLocation = GameObject.FindGameObjectWithTag("Player").transform.position;
        _package = GameObject.FindGameObjectWithTag("Package");
        _packageStartLocation = _package.transform.position;
        _deliveryPoint = GameObject.FindGameObjectWithTag("DeliveryPoint");
        _deliveryPoint.SetActive(false);
        _gameTimer = GameObject.FindGameObjectWithTag("Timer")
            .GetComponent<Timer>();
        _gameTimer.OnTimerReset = ResetScene;
        _carProgress = GetComponent<CarProgress>();
    }

    void Update()
    {
        if (_hitTextDelay > 0)
        {
            _hitTextDelay -= Time.deltaTime;
            if (_hitTextDelay <= 0)
            {
                OnHitMessage.SetActive(false);
            }
        }
    }

    void ResetScene()
    {
        var player = GameObject.FindGameObjectWithTag("Player");
        player.transform.position = _playerStartLocation;
        player.transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
        _package.transform.position = _packageStartLocation;
        _package.SetActive(true);
        _deliveryPoint.SetActive(false);
        _points = 0;
        _deliveryPoints = 0;
        _hasPackage = false;
        _deliveriesCompleted = 0;
        _canCollide = true;
        _hitTextDelay = 0;
        _lastLocation = -1;
        CurrentDeliveryText.text = $"Current delivery points: {_deliveryPoints}";
        CurrentDeliveryText.gameObject.SetActive(false);
        PackagesDeliveredText.text = $"Packages delivered: {_deliveriesCompleted}";
        ScoreText.text = $"Score: {_points}";
        _carProgress.Reset();
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (_hasPackage && _canCollide && IsObstacle(other.gameObject))
        {
            if (_deliveryPoints >= HIT_PUNISHMENT)
            {
                _deliveryPoints -= HIT_PUNISHMENT;
                CurrentDeliveryText.text = $"Current delivery points: {_deliveryPoints}";
            }
            OnHitText.text = $"-{HIT_PUNISHMENT} delivery points";
            OnHitMessage.SetActive(true);
            _hitTextDelay = 1.0f;
            _canCollide = false;
        }
    }

    void OnCollisionExit2D(Collision2D other)
    {
        if (_hasPackage && !_canCollide && IsObstacle(other.gameObject))
        {
            _canCollide = true;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!_hasPackage && other.CompareTag("Package"))
        {
            _deliveryPoints = PACKAGE_DELIVERY_POINT_BONUS;
            CurrentDeliveryText.gameObject.SetActive(true);
            CurrentDeliveryText.text = $"Current delivery points: {_deliveryPoints}";
            _hasPackage = true;
            _canCollide = true;
            _package.SetActive(false);
            SpawnAtRandomLocation(_deliveryPoint);
        }
        else if (_hasPackage && other.gameObject.CompareTag("DeliveryPoint"))
        {
            _hasPackage = false;
            _points += _deliveryPoints;
            _deliveryPoints = 0;
            CurrentDeliveryText.text = $"Current delivery points: {_deliveryPoints}";
            CurrentDeliveryText.gameObject.SetActive(false);
            _deliveriesCompleted++;
            ScoreText.text = $"Score: {_points}";
            PackagesDeliveredText.text = $"Packages delivered: {_deliveriesCompleted}";
            _deliveryPoint.SetActive(false);
            _carProgress.UpdatePoints(_points);
            SpawnAtRandomLocation(_package);
        }
    }

    private void SpawnAtRandomLocation(GameObject gameObject)
    {
        var randomIndex = _lastLocation;
        while (randomIndex == _lastLocation)
        {
            randomIndex = (int)Random.Range(1.0f, (float)_deliveryLocations.Count);
        }
        gameObject.transform.position = _deliveryLocations[randomIndex - 1];
        _lastLocation = randomIndex;
        gameObject.SetActive(true);
    }

    private bool IsObstacle(GameObject gameObject)
    {
        return gameObject.CompareTag("Building") || gameObject.CompareTag("Foliage");
    }
}
