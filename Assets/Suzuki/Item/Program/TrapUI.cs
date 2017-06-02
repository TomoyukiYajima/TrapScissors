using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TrapUI : MonoBehaviour
{
    [SerializeField]
    private GameObject _traps;
    [SerializeField]
    private RectTransform _text;
    private int _trapMax;   //最大のtrap    
    private int _trapCount; //現在のtrap    
    public int _trapRe;     //残りのtrap

    // Use this for initialization
    void Start()
    {
        _trapMax = GameManager.gameManager.TrapNumber();
    }

    void Update()
    {
        CheackTrap();
        _text.GetComponent<Text>().text = _trapRe.ToString();

    }
    void CheackTrap()
    {
        int _trapCount = _traps.transform.childCount;
        _trapRe = _trapMax - _trapCount;
    }
    public int Retrap()
    {
        return _trapRe;
    }
}
