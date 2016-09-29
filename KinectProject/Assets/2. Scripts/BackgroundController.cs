using UnityEngine;
using System.Collections;

public class BackgroundController : MonoBehaviour
{

    public GUITexture backgroundImage;    //Image Texture
    public GUITexture backgroundImageRaw; //Raw Kinect FOV
    public GameObject dustEffect;

    private bool _fade;
    private bool _origin;
    private bool _isReady = false;
    private bool _originRaw;
    private int _mouseButtonStatus = -1;
    // Use this for initialization
    void Start()
    {
        _fade = false;
        _origin = true;
        _originRaw = false; 
        KinectManager kinectManager = KinectManager.Instance;
        dustEffect.GetComponent<ParticleSystem>().Stop();
        if (kinectManager && kinectManager.IsInitialized())
        {
            if (backgroundImageRaw.texture == null)
            {
                //BackgroundImageRaw == null (That means there is no texture. so show camer view from Kinect (FOV))
                backgroundImageRaw.transform.localScale = new Vector3(1, -1, 1); //Flip over 180 degree
                backgroundImageRaw.texture = kinectManager.GetUsersClrTex();    //Apply Live FOV to Main Texture
            }
        }

    }

    // Update is called once per frame
    void Update()
    {
        KinectManager manager = KinectManager.Instance;
        Debug.Log(string.Format("fade : {0}  - OrignalRaw : {1} ", _fade, _originRaw));     
         if (manager && manager.IsInitialized())
        {
            if (Input.GetMouseButtonDown(0)) //When Trigger!
            {
                dustEffect.GetComponent<ParticleSystem>().Play();
                _mouseButtonStatus = 0;
                StartCoroutine("EmitDust");
            }

            if (Input.GetMouseButtonDown(1))
            {
                _mouseButtonStatus = 1;
                dustEffect.GetComponent<ParticleSystem>().Stop();
                StartCoroutine(FadeOut(0.0f, 3.0f));
            }
        }
        if (_fade == true)
        {
            backgroundImageRaw.transform.localScale = new Vector3(1, 1, 1);
            backgroundImageRaw.texture = backgroundImage.texture;
        }
        if(_originRaw == true)
        {
            backgroundImageRaw.transform.localScale = new Vector3(1, -1, 1);
            backgroundImageRaw.texture = manager.GetUsersClrTex();
        }
    }

    IEnumerator EmitDust()
    {
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(FadeOut(0.0f, 3.0f));
    }

    IEnumerator FadeOut(float aValue, float aTime)
    {
        float alphaValue = backgroundImageRaw.color.a;
        for (float t = 0.0f; t<1.0f; t+= Time.deltaTime / aTime)
        {
            Color newColor = new Color(backgroundImageRaw.color.r, backgroundImageRaw.color.g, backgroundImageRaw.color.b, Mathf.Lerp(alphaValue, aValue, t));
            backgroundImageRaw.color = newColor;
           
            if (_mouseButtonStatus == 0)
            {
                if (_isReady == true)
                {
                    _isReady = false;
                }
            }
            if (_mouseButtonStatus == 1)
            {
                _origin = false;
                _isReady = true;
                _fade = false;
            }
            StartCoroutine(FadeIn(1, 3.0f));
            yield return null;
        }
   
    }

    IEnumerator FadeIn(float aValue, float aTime)
    {
        float alphaValue = backgroundImageRaw.color.a;
        for(float t = 0.0f; t<1.0f; t+= Time.deltaTime / aTime)
        {
            Color newColor = new Color(backgroundImageRaw.color.r, backgroundImageRaw.color.g, backgroundImageRaw.color.b, Mathf.Lerp(alphaValue, aValue, t));
            backgroundImageRaw.color = newColor;
            yield return null;
            if(_isReady == false)
            {
                StartCoroutine(TimerForFadeIN(3.0f));
                _isReady = true;
                if(_originRaw == true)
                {
                    _originRaw = false;
                }
            }
            if(_origin == false)
            {
                _origin = true;
                StartCoroutine(TimeForFadeOUT(3.0f));
                
            }
        }
    }

    IEnumerator TimerForFadeIN(float time)
    {
        yield return new WaitForSeconds(time);
        _fade = true;
    }

    IEnumerator TimeForFadeOUT(float time)
    {
        yield return new WaitForSeconds(time);
        _originRaw = true;
    }
}
