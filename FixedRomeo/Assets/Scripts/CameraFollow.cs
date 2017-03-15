using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour 
{
	public float xSmooth = 8f, ySmooth = 8f;

	Transform player;

	void Start()
	{
		player = GameObject.FindGameObjectWithTag("Player").transform;
	}

	void FixedUpdate ()
	{
		if (player != null) TrackPlayer();
	}
	
	void TrackPlayer ()
	{
		float targetX = transform.position.x;
		float targetY = transform.position.y;
        
		targetX = Mathf.Lerp(transform.position.x, player.position.x, xSmooth * Time.deltaTime);
		targetY = Mathf.Lerp(transform.position.y, player.position.y, ySmooth * Time.deltaTime);
        
		transform.position = new Vector3(targetX, targetY, transform.position.z);
	}
}
