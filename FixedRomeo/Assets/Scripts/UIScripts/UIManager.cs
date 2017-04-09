using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

    [SerializeField]
    Slider HealthSlider;

    PlayerHealth playerHealth;

    private float PlayerHealth;
	// Use this for initialization
	void Start () {
        PlayerHealth = GetComponent<PlayerHealth>().health;
        PlayerHealth = playerHealth.health;
        
	}
	
	// Update is called once per frame
	void Update () {
        HealthSlider.value = PlayerHealth;
	}


}
