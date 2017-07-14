using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowMovie : MonoBehaviour
{  
    public MovieTexture _movieTexture;
    private Renderer _renderer;
    void Start()
    {
        _renderer = GetComponent<Renderer>();
        _movieTexture.Play();
    }
    void Update()
    {
        if (  Input.GetButtonDown("Trap")||Input.GetButtonDown("Whistle")
            ||Input.GetButtonDown("Food")||Input.GetButtonDown("Pause")) 
        {
            MovieTexture _movieTexture = (MovieTexture)_renderer.material.mainTexture;
            if (_movieTexture.isPlaying)
                _movieTexture.Stop();
        }
    }
}
