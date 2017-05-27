﻿using UnityEngine;
using System.Collections;

public class Trap_Small : MonoBehaviour
{
    public enum TrapState
    {
        WAIT_TRAP,       //動物が掛かるのを待っている状態
        CAPTURE_TRAP,    //動物を捕まえている状態
        MOVE_TRAP
    }


    public TrapState _state;
    private bool recovery;
    private bool collider;
    public GameObject getTarget;

    #region 鋏むときに必要な変数
    //鋏むんでいるオブジェクトを入れる
    [SerializeField]
    private GameObject _targetAnimal;
    private Vector3 _offset = Vector3.zero;
    #endregion

    //スタートよりも前に動く変数
    void Awake()
    {
    }

    // Use this for initialization
    void Start()
    {
        _targetAnimal = null;
        _state = TrapState.WAIT_TRAP;
        recovery = true;
        collider = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (_targetAnimal != null)
        {
            Vector3 newPosition = transform.position;

            newPosition = _targetAnimal.transform.position + _offset;
            transform.position = newPosition;
            return;
        }
        else
        {
            _state = TrapState.WAIT_TRAP;
        }
    }

    //当たっている最中も取得する当たり判定
    void OnTriggerEnter(Collider col)
    {
        //if (_targetAnimal == null)
        //{
        //    return;
        //}
        if (_state == TrapState.CAPTURE_TRAP) return;
        if (col.tag == "LargeEnemy" || col.tag == "SmallEnemy")
        {
            GameManager.gameManager.HuntCountAdd();
            _targetAnimal = col.gameObject;
            Enemy3D enemy = col.GetComponent<Enemy3D>();
            enemy.ChangeTrap(gameObject);
            if (col.tag == "LargeEnemy")
            {
                recovery = false;
            }
            ChengeState(TrapState.CAPTURE_TRAP);
        }

    }

    public void ChengeState(TrapState state)
    {
        _offset = transform.position - _targetAnimal.transform.position;
        _state = state;
    }

    public bool Recovery()
    {
        return recovery;
    }
    public void Null()
    {
        _targetAnimal = null;
    }

    public GameObject GetAnimal()
    {
        return _targetAnimal;
    }
}
