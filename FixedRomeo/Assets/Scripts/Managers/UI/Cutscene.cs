using UnityEngine;
using System.Collections;
using UnityEngine.UI;

//CUTSCENE MANAGER GOES ON CUTSCENE PANEL
[RequireComponent(typeof(AudioSource))]
public class Cutscene : MonoBehaviour
{
    [SerializeField]
    private float textDefaultWaitTime;
    [SerializeField]
    private float spedUpTextWaitTime;
    [SerializeField]
    private float cutsceneFadeOutTime;
    [SerializeField]
    private char spaceChar;
    [SerializeField]
    private Image[] extraImages;
    [SerializeField]
    private CutsceneManager cutsceneManager;

    private string cutsceneString;
    private Text cutsceneText;
    private char[] cutsceneTextChars;
    private WaitForSeconds textAnimationInterval;
    private WaitForSeconds cutsceneFadeOutInterval;
    private Image canvasImage;
    private AudioSource charBlipAudio;
    private bool canContinue = false, polish;

    void Awake()
    {
        cutsceneFadeOutInterval = new WaitForSeconds(cutsceneFadeOutTime);
    }

    // Use this for initialization
    void Start ()
    {
        canvasImage = GetComponent<Image>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        SpeedThroughAndDisableCutscene();
	}

    public void DisplayText()
    {
        textAnimationInterval = new WaitForSeconds(textDefaultWaitTime);
        charBlipAudio = GetComponent<AudioSource>();
        cutsceneText = GetComponentInChildren<Text>();
        cutsceneTextChars = cutsceneString.ToCharArray();
        StartCoroutine(AnimateText(cutsceneTextChars));
    }

    private IEnumerator AnimateText(char[] charArray)
    {
        foreach (char letter in charArray)
        {
            cutsceneText.text += letter;
            if (letter != spaceChar)
            {
                charBlipAudio.pitch = Random.Range(1, 3);
                charBlipAudio.Play();
            }
            yield return textAnimationInterval;
        }
        canContinue = true;
    }

    private IEnumerator DisableCutscene()
    {
        canvasImage.CrossFadeAlpha(0, cutsceneFadeOutTime, false);
        cutsceneText.CrossFadeAlpha(0, cutsceneFadeOutTime, false);
        if (extraImages !=null)
        {
            foreach (Image extraimage in extraImages)
            {
                extraimage.CrossFadeAlpha(0, cutsceneFadeOutTime, false);
            }
        }
        yield return cutsceneFadeOutInterval;
        cutsceneManager.PlayNextCutscene();
        gameObject.SetActive(false);
    }

    private void SpeedThroughAndDisableCutscene()
    {
        if (Input.GetButtonDown("Polish")) polish = true;

        if (Input.GetButtonDown("Jump") && canContinue)
        {
            StartCoroutine(DisableCutscene());
            if (polish) FindObjectOfType<DaneTemp_Cutscene>().PlayAdamLines();
        }

        if (Input.GetButtonDown("Jump") && !canContinue)
        {
            textAnimationInterval = new WaitForSeconds(spedUpTextWaitTime);
        }
    }

    public void SetCutsceneString(string assignedString)
    {
        cutsceneString = assignedString;
    }
}
