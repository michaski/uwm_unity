using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarProgress : MonoBehaviour
{
    public List<Car> Cars;

    [NonSerialized]
    public Action<Car> OnCarProgressed;

    private int _currentCar = 0;
    private int _points;

    void Start()
    {
        OnCarProgressed(Cars[_currentCar]);
    }

    public void Reset()
    {
        _points = 0;
        _currentCar = 0;
        OnCarProgressed(Cars[_currentCar]);
    }

    public void UpdatePoints(int points)
    {
        _points = points;
        if (_currentCar + 1 <= Cars.Count - 1 && 
            Cars[_currentCar + 1].PointThreshold <= _points)
        {
            OnCarProgressed(GetNextCar());
        }
    }

    public Car GetNextCar()
    {
        if (_currentCar == Cars.Count - 1)
        {
            return Cars[_currentCar];
        }
        return Cars[++_currentCar];
    }

    public Car GetCurrentCar() => Cars[_currentCar];
}
