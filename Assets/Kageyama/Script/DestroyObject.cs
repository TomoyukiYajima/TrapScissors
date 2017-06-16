using UnityEngine;
using System.Collections;

public class DestroyObject : MonoBehaviour
{
    [SerializeField]
    private float _destroyTime;

	// Use this for initialization
	void Start ()
    {
        if (_destroyTime <= 0) _destroyTime = 0;
        Destroy(this.gameObject, _destroyTime);
	}
}
