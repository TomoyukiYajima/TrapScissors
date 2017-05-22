using UnityEngine;
using System.Collections;

public class Food : MonoBehaviour {

    public enum Food_Kind
    {
        NULL,
        Goat,
        Tanuki,
        Rabbit
    }

    public Sprite[] _sprite;

    public Food_Kind food_Kind = Food_Kind.NULL;

    // Use this for initialization
    void Start () {
    }
	
	// Update is called once per frame
	void Update () {    
	    
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
            food_Kind = Food_Kind.Goat;
        }
        else if (food == 1)
        {
            food_Kind = Food_Kind.Tanuki;
        }
        else if (food == 2)
        {
            food_Kind = Food_Kind.Rabbit;
        }
        this.gameObject.GetComponent<SpriteRenderer>().sprite = _sprite[food];
    }
    public Food_Kind CheckFoodKind()
    {
        return food_Kind;
    }
}
