using UnityEngine;
using System.Collections;

public class FoodUIMove : MonoBehaviour
{
    public GameObject[] _foodUI;
    public int[] _foodNumber;
    [SerializeField]
    private int _selectFood;

	// Use this for initialization
	void Start ()
    {
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetButtonDown("FoodChange_L"))
        {
            LeftRotation();
        }
        else if (Input.GetButtonDown("FoodChange_R"))
        {
            RightRotation();
        }
	}

    /// <summary>
    /// 右回りに回転する
    /// </summary>
    public void LeftRotation()
    {
        _selectFood++;
        if (_selectFood >= _foodUI.Length) _selectFood = 0;
        _foodUI[_selectFood].transform.SetAsLastSibling();
        for (int i = 0; i < _foodUI.Length; i++)
        {
            _foodUI[i].GetComponent<FoodUI>().RightMoveRotation(SelectFood());
        }
    }

    /// <summary>
    /// 左回りに回転する
    /// </summary>
    public void RightRotation()
    {
        _selectFood--;
        if (_selectFood < 0) _selectFood = _foodUI.Length - 1;
        _foodUI[_selectFood].transform.SetAsLastSibling();
        for (int i = 0; i < _foodUI.Length; i++)
        {
            _foodUI[i].GetComponent<FoodUI>().LeftMoveRotation(SelectFood());
        }
    }

    /// <summary>
    /// どのの餌を選んでいるか値を返す
    /// </summary>
    /// <returns></returns>
    public GameObject SelectFood()
    {
        return _foodUI[_selectFood];
    }

    /// <summary>
    /// 選んでいる餌の番号を返す
    /// </summary>
    /// <returns></returns>
    public int SelectFoodNumber()
    {
        return _selectFood;
    }

    /// <summary>
    /// 餌の所持数を増やす
    /// </summary>
    /// <param name="num">増やす餌の番号/param>
    public void FoodCountAdd(int num)
    {
        //対象の餌の所持数が9以上ならそれ移動増やさない
        if (_foodNumber[num] >= 5) return;
        _foodNumber[num]++;
    }

    /// <summary>
    /// 餌の所持数を減らす
    /// </summary>
    /// <param name="num">減らす餌の番号</param>
    public void FoodCountSub(int num)
    {
        _foodNumber[num]--;
    }

    /// <summary>
    /// 餌の所持数を確認する
    /// </summary>
    /// <param name="num">確認する餌の番号</param>
    /// <returns></returns>
    public int FoodCountCheck(int num)
    {
        return _foodNumber[num];
    }
}
