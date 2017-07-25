using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TrapUI : MonoBehaviour
{
    [SerializeField]
    private GameObject _traps;
    private Text _text;
    private int _trapMax;   //最大のtrap  
    public int _trapRe;     //残りのtrap

    // Use this for initialization
    void Start()
    {
        _trapMax = GameManager.gameManager.TrapNumber();
        _text = this.GetComponent<Text>();
    }

    void Update()
    {
        CheackTrap();
        //_text.text = _trapRe.ToString();
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
