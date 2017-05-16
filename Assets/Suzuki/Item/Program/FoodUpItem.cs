using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FoodUpItem : MonoBehaviour
{

    private int _fnum;                   //餌の番号
    private int _checksum;             //現状の餌所有数
    public int _max;                  //餌の最大所持数
    public GameObject _foodUIparent;     //今選んでいる餌の値
    private FoodUIMove _fooduimove;
    // Use this for initialization
    void Start()
    {

        _max = 3;      //（仮）

        _fooduimove = _foodUIparent.GetComponent<FoodUIMove>();
        _fnum = 0;
        _checksum = _fooduimove.FoodCountCheck(_fnum);

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.tag == "Player" && _checksum <= _max)
        {

            _fooduimove.FoodCountAdd(_fnum);
            Destroy(this.gameObject);

        }
        print("a");
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && _checksum <= _max)
        {

            _fooduimove.FoodCountAdd(_fnum);
            Destroy(this.gameObject);

        }
        print("a");
    }
}
