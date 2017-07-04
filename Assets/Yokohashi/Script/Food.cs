using UnityEngine;
using System.Collections;

public class Food : MonoBehaviour {

    public enum Food_Kind
    {
        NULL,
        Carrot,
        Meat,
        SmellMeat
    }

    private float smellTime;
    public GameObject _particle;

    public Sprite[] _sprite;

    private FoodUIMove moveUI;
    private GameObject _foodMoveUI;

    public Food_Kind food_Kind = Food_Kind.NULL;

    // Use this for initialization
    void Start () {
        _foodMoveUI = GameObject.Find("FoodParent");
        moveUI = _foodMoveUI.GetComponent<FoodUIMove>();
        smellTime = 0.0f;

    }
	
	// Update is called once per frame
	void Update () {
        smellTime += Time.deltaTime;

        if (smellTime >= 30 && food_Kind == Food_Kind.Meat)
        {
            SelectFood(2);
        }
    }

    public void SelectFood(int food)
    {
        if(food >= 3)
        {
            Destroy(this.gameObject);
            StenchEffe();
            return;
        }
        switch (food)
        {
            case 0:
                food_Kind = Food_Kind.Carrot;
                break;
            case 1:
                food_Kind = Food_Kind.Meat;
                break;
            case 2:
                food_Kind = Food_Kind.SmellMeat;
                StenchEffe();
                break;
        }
        this.gameObject.GetComponent<SpriteRenderer>().sprite = _sprite[food];
    }
    public Food_Kind CheckFoodKind()
    {
        return food_Kind;
    }

    private void StenchEffe()
    {
        GameObject child = Instantiate(_particle);
        child.transform.SetParent(this.transform);
        //child.transform.localRotation = new Quaternion(-90.0f, 0.0f, 0.0f, 1.0f);
        child.transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f);
    }

}
