using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HuntCountText : MonoBehaviour
{
    private int _hunt; 
    private int _huntCount;
    private Text _myText;
    private bool _skipFlag;
    [SerializeField]
    private GameObject _anyButton;

    // Use this for initialization
    void Start ()
    {
        _myText = this.GetComponent<Text>();
        _hunt = GameManager.gameManager.HuntCountCheck();
        _huntCount = 0;
        _skipFlag = false;
        _myText.text = _huntCount.ToString();
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.anyKeyDown && _skipFlag == false)
        {
            _skipFlag = true;
            _myText.text = _hunt.ToString();
        }
    }

    public void CountStart()
    {
        StartCoroutine(Count());
    }

    IEnumerator Count()
    {
        for (int i = 0; i < _hunt; i++)
        {
            if (_skipFlag == true)
            {
                break;
            }
            else if (_skipFlag == false)
            {
                _huntCount++;
                _myText.text = _huntCount.ToString();
                yield return new WaitForSecondsRealtime(0.02f);
            }
        }
        _anyButton.SetActive(true);
    }
}
