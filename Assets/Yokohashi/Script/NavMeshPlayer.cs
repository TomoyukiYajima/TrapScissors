using UnityEngine;
using System.Collections;

public class NavMeshPlayer : MonoBehaviour {

    public GameObject _mainCamera;

    NavMeshAgent agent;
    //SceneManagerScript sceneManager;
    private GameObject _targetAnimal;

	// Use this for initialization
	void Start () {
        agent = GetComponent<NavMeshAgent>();
        //sceneManager = GetComponent<SceneManagerScript>();

        _targetAnimal = null;
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 move = Vector3.forward * Input.GetAxis("Vertical") + Vector3.right * Input.GetAxis("Horizontal");
        agent.Move(move * Time.deltaTime * 5);

        Vector3 p = _mainCamera.transform.localPosition;
        transform.LookAt(p);
    }

    private void OnTriggerEnter(Collider col)
    {
        print("a");

        if (col.tag == "LargeEnemy")
        {
            SceneManagerScript.sceneManager.FadeBlack();
        }
    }
}
