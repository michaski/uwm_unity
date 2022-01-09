using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class Driver : MonoBehaviour
{
    public float Acceleration = 30.0f;
    public float TurnSpeed = 3.5f;
    public float Handling = 0.3f;
    public float ForwardMaxSpeed = 20.0f;
    public float ReverseMaxSpeed = 10.0f;
    private float _rotation = 0.0f;
    private float _speed = 0.0f;
    private float _forwardMaxSpeedSquared;
    private Rigidbody2D _rb;
    private float _throttle = 0.0f;
    private float _steer = 0.0f;
    private Timer _gameTimer;

    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        Handling = 1 - Handling;
        ReverseMaxSpeed *= -1;
        _forwardMaxSpeedSquared = ForwardMaxSpeed * ForwardMaxSpeed;
        var carProgress = GetComponent<CarProgress>();
        ChangeCar(carProgress.GetCurrentCar());
        _gameTimer = GameObject.FindGameObjectWithTag("Timer")
            .GetComponent<Timer>();
        carProgress.OnCarProgressed = ChangeCar;
    }

    void ChangeCar(Car newCar)
    {
        Acceleration = newCar.Acceleration;
        TurnSpeed = newCar.TurnSpeed;
        Handling = 1 - newCar.Handling;
        ForwardMaxSpeed = newCar.MaxSpeed;
        ReverseMaxSpeed = -newCar.MaxReverseSpeed;
        _forwardMaxSpeedSquared = ForwardMaxSpeed * ForwardMaxSpeed;
        GetComponent<SpriteRenderer>().sprite = newCar.Skin;
    }

    void Update()
    {
        _throttle = Input.GetAxis("Vertical");
        _steer = Input.GetAxis("Horizontal");
    }

    void FixedUpdate()
    {
        if (!_gameTimer.IsRunning)
        {
            _rb.velocity = Vector2.zero;
            _rb.angularVelocity = 0.0f;
            _speed = 0.0f;
            _rotation = 0.0f;
            return;
        }
        AddSpeedForce();
        AddFriction();
        AddSteerForce();
    }

    void AddSpeedForce()
    {
        _speed = Vector2.Dot(transform.up, _rb.velocity);
        // exceeding max speed
        if (_speed > ForwardMaxSpeed && _throttle > 0)
        {
            return;
        }
        // exceeding max reverse speed
        if (_speed < ReverseMaxSpeed && _throttle < 0)
        {
            return;
        }
        // exceeding speed sideways (while turning or drifting)
        if (_rb.velocity.sqrMagnitude > _forwardMaxSpeedSquared && _throttle > 0)
        {
            return;
        }
        // adding drag to slow the car, when accelerate button is released
        if (_throttle == 0)
        {
            _rb.drag = Mathf.Lerp(_rb.drag, 3.0f, Time.fixedDeltaTime * 3);
        }
        else
        {
            _rb.drag = 0;
        }
        // while reversing car accelerates slower
        float accelerationFactor = (_speed < 0 && _throttle < 0) ? 0.25f : 1.0f;
        Vector2 accelerationForce = transform.up * _throttle * Acceleration * accelerationFactor;
        _rb.AddForce(accelerationForce);
    }

    void AddSteerForce()
    {
        int steerFlip = (_speed < 0) ? -1 : 1;
        float turnTreshold = Mathf.Clamp01(_rb.velocity.magnitude / 8);
        _rotation -= _steer * TurnSpeed * turnTreshold * steerFlip;
        _rb.SetRotation(Quaternion.Euler(0, 0, _rotation));
    }

    void AddFriction()
    {
        Vector2 forwardVelocity = transform.up * Vector2.Dot(_rb.velocity, transform.up);
        Vector2 sideVelocity = transform.right * Vector2.Dot(_rb.velocity, transform.right);
        _rb.velocity = forwardVelocity + sideVelocity * Handling;
    }
}
