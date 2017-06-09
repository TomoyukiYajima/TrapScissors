using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class OneDay : MonoBehaviour
{

    private Image _oneDay;
    private float _ptime;
    private float _mtime;
    private int _i, _j, _h, _v;
    public float _time;

    public enum Day
    {
        morning,
        evening,
        night
    }

    public Day _day;
    // Use this for initialization
    void Start()
    {
        _oneDay = GetComponent<Image>();
        _day = Day.morning;
    }

    // Update is called once per frame
    void Update()
    {
        _time += Time.deltaTime;

        if (_time >= 90)
        {
            _day = Day.evening;
            _ptime += Time.deltaTime;

        }
        if (_time >= 180)
        {
            _day = Day.night;
        }
        if (_time >= 270)
        {
            _day = Day.morning;
            _time = 0;
        }
        StateDay();
    }
    void StateDay()
    {
        if (_day == Day.morning)
            _oneDay.color = new Color(0, 0, 0, 0);
        else if (_day == Day.evening)
        {
            _oneDay.color = new Color(0.015f * _ptime, 0.01f * _ptime, 0, 0.005f * _ptime);
            if (_time >= 135)
            {
                _i++;
                if (_i % 2 == 0) { } else { _j = _i; }

                _oneDay.color = new Color((0.015f * _ptime) - (_j * 0.0005f),
                    (0.01f * _ptime) - (_j * 0.001f),
                    0.004f * (_ptime - (_ptime - 10)),
                    0.005f * _ptime);
            }
        }

        else if (_day == Day.night)
        {
            if (_time >= 240)
            {
                _h++;
                if (_h % 2 == 0) { } else { _v = _h; }
                _oneDay.color = new Color(0.0f, 0.0f, 0.0f,
                    (0.005f * _ptime) - (_v * 0.001f));
            }

        }

    }
}
