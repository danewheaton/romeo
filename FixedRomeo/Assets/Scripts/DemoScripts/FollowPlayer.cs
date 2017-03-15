using UnityEngine;
using System.Collections;

public class FollowPlayer : MonoBehaviour
{
	public Vector3 offset;
	
	Transform player;

	void Start ()
	{
		player = GameObject.FindGameObjectWithTag("Player").transform;
	}

	void Update ()
	{
		if (player != null) transform.position = player.position + offset;
	}
}
