using UnityEngine;
using System.Collections;

public class NavMeshPlayer : MonoBehaviour {

    public GameObject _mainCamera;

    public float playerSpeed = 5; 
    NavMeshAgent agent;
    private GameObject _targetAnimal;

    private GameManager _childSprite;
    private SpriteRenderer _myRenderer;
    public Sprite[] _sprite;


    // Use this for initialization
    void Start () {
        agent = GetComponent<NavMeshAgent>();

        _myRenderer = this.transform.FindChild("PlayerSprite").GetComponent<SpriteRenderer>();

        _targetAnimal = null;
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 move = (Vector3.forward - Vector3.right)* Input.GetAxis("Vertical") + (Vector3.right + Vector3.forward)* Input.GetAxis("Horizontal");
        agent.Move(move * Time.deltaTime * playerSpeed);

        Vector3 p = _mainCamera.transform.localPosition;
        transform.LookAt(p);
    }

    private void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "LargeEnemy")
        {
            SceneManagerScript.sceneManager.FadeBlack();
        }

    }

}
