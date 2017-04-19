using UnityEngine;
using System.Collections;

// all code by DW unless otherwise noted

public class VFXManager : MonoBehaviour
{
    [SerializeField]
    SpriteRenderer backgroundRenderer;

    [SerializeField]
    float colorChangeSpeed = 1;

    Color randomColor1, randomColor2;

    private void Start()
    {
        InvokeRepeating("SetRandomColors", 0, colorChangeSpeed);
    }

    void Update()
    {
        float lerp = Mathf.PingPong(Time.time, colorChangeSpeed) / colorChangeSpeed;
        backgroundRenderer.color = Color.Lerp(randomColor1, randomColor2, lerp);
    }

    void SetRandomColors()
    {
        randomColor1 = new Color(Random.value, Random.value, Random.value);
        randomColor2 = new Color(Random.value, Random.value, Random.value);
    }
}
