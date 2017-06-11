using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class NavMeshPlayer : MonoBehaviour {

    public enum AnimationState
    {
        Idle,
        Move,
        Set,
        Down
    }

    public AnimationState _AState = AnimationState.Idle;

    Animator m_Animator;
    public GameObject _mainCamera;
    public GameObject _gameOver;

    public float playerSpeed = 5; 
    NavMeshAgent agent;
    private GameObject _targetAnimal;

    private GameManager _childSprite;
    private SpriteRenderer _myRenderer;
    public Sprite[] _sprite;
    //public GameObject _gameOver;


    // Use this for initialization
    void Start () {
        agent = GetComponent<NavMeshAgent>();
        m_Animator = GetComponentInChildren<Animator>();

        _myRenderer = this.transform.FindChild("PlayerSprite").GetComponent<SpriteRenderer>();

        _targetAnimal = null;
	}
	
	// Update is called once per frame
	void Update () {

        if (GameManager.gameManager.GameStateCheck() != GameManager.GameState.PLAY || _mainCamera.GetComponent<CameraMove>().LockCheck() == true || _AState == AnimationState.Set) return;
        Vector3 move = (Vector3.forward - Vector3.right)* Input.GetAxis("Vertical") + (Vector3.right + Vector3.forward)* Input.GetAxis("Horizontal");
        agent.Move(move * Time.deltaTime * playerSpeed);

        //print("agent" + move.magnitude);

        m_Animator.SetFloat("Move", move.magnitude);

        move.y = 0;
        if(move.magnitude > 0)
        {
            transform.rotation = Quaternion.LookRotation(move);
        }

        //ビルボード
        //Vector3 p = _mainCamera.transform.localPosition;
        //transform.LookAt(p);
    }

    private void OnCollisionEnter(Collision col)
    {
        if (GameManager.gameManager.GameStateCheck() != GameManager.GameState.PLAY) return;
        if (col.gameObject.tag == "LargeEnemy")
        {
            GameManager.gameManager.GameStateSet(GameManager.GameState.END);
            _gameOver.SetActive(true);
        }

    }

}
