using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour 
{
	public float xMargin = 1f, yMargin = 1f, xSmooth = 8f, ySmooth = 8f;
	public Vector2 maxXAndY, minXAndY;

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
        
		if(CheckXMargin())
			targetX = Mathf.Lerp(transform.position.x, player.position.x, xSmooth * Time.deltaTime);
        
		if(CheckYMargin())
			targetY = Mathf.Lerp(transform.position.y, player.position.y, ySmooth * Time.deltaTime);
        
		targetX = Mathf.Clamp(targetX, minXAndY.x, maxXAndY.x);
		targetY = Mathf.Clamp(targetY, minXAndY.y, maxXAndY.y);
        
		transform.position = new Vector3(targetX, targetY, transform.position.z);
	}

    bool CheckXMargin()
    {
        return Mathf.Abs(transform.position.x - player.position.x) > xMargin;
    }

    bool CheckYMargin()
    {
        return Mathf.Abs(transform.position.y - player.position.y) > yMargin;
    }
}
