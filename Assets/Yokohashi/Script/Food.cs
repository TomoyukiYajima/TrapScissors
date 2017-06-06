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
        if (food == 3) 
        {
            Destroy(this.gameObject);
            return;
        }
        else if(food == 0)
        {
            food_Kind = Food_Kind.Carrot;
        }
        else if (food == 1)
        {
            food_Kind = Food_Kind.Meat;
        }
        else if (food == 2)
        {
            food_Kind = Food_Kind.SmellMeat;
        }
        this.gameObject.GetComponent<SpriteRenderer>().sprite = _sprite[food];
    }
    public Food_Kind CheckFoodKind()
    {
        return food_Kind;
    }
}
