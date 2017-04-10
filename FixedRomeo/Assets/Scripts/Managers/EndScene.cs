using UnityEngine;
using System.Collections;

public class EndScene : MonoBehaviour
{
    [SerializeField]
    GameObject noMoreContentText, fuckYouText;

	void Start ()
    {
        Invoke("SetTextActive", 1.5f);
	}
	
    void SetTextActive()
    {
        noMoreContentText.SetActive(true);
        Invoke("SetMoreTextActive", 4);
    }

    void SetMoreTextActive()
    {
        fuckYouText.SetActive(true);
    }
}
