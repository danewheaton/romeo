using UnityEngine;
using UnityStandardAssets._2D;
using System.Collections;

// all code by DW unless otherwise noted

public class CameraFollow : MonoBehaviour 
{
    [SerializeField] float
        xSmooth = 2,
        ySmooth = 2,
        xOffset = 10,
        yOffset = 10,
        shakeIntensity = .5f,
        shakeDuration = 1,
        zoomedOutZPos = -30;

	Transform player;
    Vector3 originalPos;
    bool cameraIsShaking;

    void OnEnable()
    {
        Boss1.OnBoss1Stomp += CallShakeCamera;
        PlayerCheckpoints.OnEnterArena += CallZoomOut;
    }
    void OnDisable()
    {
        Boss1.OnBoss1Stomp -= CallShakeCamera;
        PlayerCheckpoints.OnEnterArena -= CallZoomOut;
    }

    void Start()
	{
		player = GameObject.FindGameObjectWithTag("Player").transform;
        originalPos = transform.position;
	}

	void Update ()
	{
		if (player != null) TrackPlayer();
	}
	
	void TrackPlayer ()
	{
		float targetX = transform.position.x;
		float targetY = transform.position.y;
        
		targetX = Mathf.Lerp(transform.position.x,
            player.position.x + (player.GetComponent<PlatformerCharacter2D>().m_FacingRight ? xOffset : -xOffset),
            xSmooth * Time.deltaTime);
		targetY = Mathf.Lerp(transform.position.y, player.position.y + yOffset, ySmooth * Time.deltaTime);
        
		if (!cameraIsShaking) transform.position = new Vector3(targetX, targetY, transform.position.z);
	}

    void CallShakeCamera()
    {
        StartCoroutine(CameraShake());
    }

    void CallZoomOut()
    {
        StartCoroutine(ZoomOut());
    }

    IEnumerator ZoomOut()
    {
        float oldYOffset = yOffset / 4;
        float newYOffset = yOffset * 20;

        float elapsedTime = 0;
        while (elapsedTime < 5)
        {
            transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, transform.position.y, zoomedOutZPos), elapsedTime / 5);
            yOffset = Mathf.Lerp(oldYOffset, newYOffset, elapsedTime / 5);

            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
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
