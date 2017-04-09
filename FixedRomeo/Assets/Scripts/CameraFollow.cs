using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour 
{
	[SerializeField] float xSmooth = 2, ySmooth = 2, shakeIntensity = .5f, shakeDuration = 1;

	Transform player;
    bool cameraIsShaking;

    void OnEnable()
    {
        Boss1.OnBoss1Stomp += CallShakeCamera;
    }
    void OnDisable()
    {
        Boss1.OnBoss1Stomp -= CallShakeCamera;
    }

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
        
		if (!cameraIsShaking) transform.position = new Vector3(targetX, targetY, transform.position.z);
	}

    void CallShakeCamera()
    {
        StartCoroutine(CameraShake());
    }

    IEnumerator CameraShake()
    {
        cameraIsShaking = true;

        Vector3 originalCamPos = Camera.main.transform.position;

        float elapsedTime = 0;
        while (elapsedTime < shakeDuration)
        {
            transform.localPosition = new Vector3
                (originalCamPos.x + Random.insideUnitSphere.x * shakeIntensity,
                originalCamPos.y + Random.insideUnitSphere.y * shakeIntensity,
                originalCamPos.z + Random.insideUnitSphere.z * shakeIntensity);

            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        cameraIsShaking = false;
    }
}
