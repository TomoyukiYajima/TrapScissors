using UnityEngine;
using System.Collections;

public class PointClearChacker : ClearChacker
{

	// Use this for initialization
	//public override void Start () {
	
	//}
	
	//// Update is called once per frame
	//public override void Update () {
	
	//}

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.name == "PlayerSprite")
        {
            // チュートリアルのクリア
            TutorialClear();
        }
    }
}
