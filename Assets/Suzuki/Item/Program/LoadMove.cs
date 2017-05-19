using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LoadMove : MonoBehaviour
{
    
    private bool _load;

    // Use this for initialization
    void Start()
    {
        _load = false;
    }

    // Update is called once per frame
    void Update()
    {
       // Vector2 _pos = transform.position;
       // if (_pos.x<=800)
       // {
       // _pos.x -= 1.0f;
       // transform.position = _pos;
       // }
       //else if (_pos.x >= 0)
       // {
       //     _pos.x += 1.0f;
       //     transform.position = _pos;
       // }
       // return;

    }
    public void Move()
    {
        _load = true;
    }
}
