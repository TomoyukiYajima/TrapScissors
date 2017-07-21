using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class NavMeshPlayer : MonoBehaviour {

    public enum AnimationState
    {
        Idle,
        Move,
        Set,
        Food,
        Down
    }

    public AnimationState _AState = AnimationState.Idle;

    Animator m_Animator;
    public GameObject _mainCamera;
    public GameObject _gameOver;

    public Vector3 move;
    public float playerSpeed = 5; 
    UnityEngine.AI.NavMeshAgent agent;
    //private GameObject _targetAnimal;

    //private GameManager _childSprite;
    private SpriteRenderer _myRenderer;
    public Sprite[] _sprite;
    //public GameObject _gameOver;


    // Use this for initialization
    void Start () {
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        m_Animator = GetComponentInChildren<Animator>();

        _myRenderer = this.transform.Find("PlayerSprite").GetComponent<SpriteRenderer>();

        //_targetAnimal = null;
	}
	
	// Update is called once per frame
	void Update () {

        if (GameManager.gameManager.GameStateCheck() == GameManager.GameState.END || GameManager.gameManager.GameStateCheck() == GameManager.GameState.PAUSE)
        {
            _AState = AnimationState.Idle;
        }

        if (GameManager.gameManager.GameStateCheck() != GameManager.GameState.PLAY || _mainCamera.GetComponent<CameraMove>().LockCheck() == true || _AState == AnimationState.Set || _AState == AnimationState.Food) return;
        move = (Vector3.forward - Vector3.right)* Input.GetAxis("Vertical") + (Vector3.right + Vector3.forward)* Input.GetAxis("Horizontal");
        agent.Move(move * Time.deltaTime * playerSpeed);

        if(Input.GetAxis("Lock") >= 0.5f)
        {
            _AState = AnimationState.Idle;
        }

        //Vector3 move = (Vector3.forward - Vector3.right) * Input.GetAxis("Vertical") + (Vector3.right + Vector3.forward) * Input.GetAxis("Horizontal");
        //agent.Move(move * Time.deltaTime * playerSpeed);

        m_Animator.SetFloat("Move", move.magnitude);

        move.y = 0;
        if(move.magnitude > 0)
        {
            if (_AState == AnimationState.Idle)
            {
                _AState = AnimationState.Move;
            }
            transform.rotation = Quaternion.LookRotation(move);
        }
        else
        {
            _AState = AnimationState.Idle;    
        }
    }

    private void OnTrigerEnter(Collision col)
    {
        if (GameManager.gameManager.GameStateCheck() != GameManager.GameState.PLAY) return;
        if (col.gameObject.tag == "LargeEnemy")
        {
            m_Animator.CrossFade("Down", 0.1f, -1);
            GameManager.gameManager.GameStateSet(GameManager.GameState.END);
            _gameOver.SetActive(true);
        }

    }

}
