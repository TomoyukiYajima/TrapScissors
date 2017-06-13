using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class OneDay : MonoBehaviour
{
    private Image _oneDay;
    private float _ptime;
    private int _ei, _ej, _nh, _nv;
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
        if (_time >= 270)
        {
            _day = Day.morning;
            _time = 0;
            _ptime = 0;
        }
        else if (_time >= 235)
        {
            _ptime += Time.deltaTime;
        }
        else if (_time >= 180)
        {
            _day = Day.night;
        }

        else if (_time >= 90)
        {
            _day = Day.evening;
            _ptime += Time.deltaTime;
        }
        StateDay();
    }
    void StateDay()
    {
        if (_day == Day.morning)
        {
            _nh = 0;
            _oneDay.color = new Color(0, 0, 0, 0);
        }
        else if (_day == Day.evening)
        {
            _oneDay.color = new Color(0.015f * _ptime, 0.0125f * _ptime, 0, 0.005f * _ptime);
            if (_time >= 135)
            {
                _ei++;
                if (_ei % 2 == 0) { } else { _ej = _ei; }

                _oneDay.color = new Color((0.015f * _ptime) - (_ej * 0.00075f),
                    (0.0125f * _ptime) - (_ej * 0.00065f),
                    (0.001f * _ptime),
                    0.005f * _ptime);
            }
        }
        else if (_day == Day.night)
        {
            _ei = 0;
            if (_ptime >= 90)
            {
                _nh++;
                if (_nh % 2 == 0) { } else { _nv = _nh; }
                _oneDay.color = new Color(0, 0,
                   (0.001f * _ptime) - (_nv * 0.0001f),
                   (0.005f * _ptime) - (_nv * 0.00045f));
            }

        }
    }
}

