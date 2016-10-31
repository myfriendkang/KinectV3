using UnityEngine;
using System.Collections;

public class BackgroundController : MonoBehaviour
{
    public GUITexture backgroundImage;    //Image Texture
    public GUITexture backgroundImageRaw; //Raw Kinect FOV
    public GameObject dustEffect;
    public Camera mainCamera;
    private bool _fade;
    private bool _origin;
    private bool _isReady = false;
    private bool _originRaw;
    private int _mouseButtonStatus = -1;
    public GameObject test;
    public int stage;
    public bool? closeTrigger;  //{ default : -1, close = 1, far = 0);
    public GameObject headTracking;
    bool sFlag;
    bool dFlag;

    public Texture[] debugTextureImages;
    public GameObject debug_Texture;
    public GameObject arduinoInput;
    int debug_Number;
    int numberFromArduino = 0;
    // Use this for initialization
    void Start()
    {
        _fade = false;
        _origin = true;
        _originRaw = false;
        stage = 0;
        dustEffect.GetComponent<ParticleSystem>().Stop();
        closeTrigger = null;
        sFlag = false;
        dFlag = false;
    }

    // Update is called once per frame
    void Update()
    {
        KinectManager manager = KinectManager.Instance;
         if (manager && manager.IsInitialized())
        {
            closeTrigger = headTracking.GetComponent<HeadTracking>().isClose;
            numberFromArduino = arduinoInput.GetComponent<ArduinoController>().count;
            if (backgroundImageRaw.texture == null)
            {
                Debug.Log("here");
                /* this one */
                //backgroundImageRaw.transform.localScale = new Vector3(2, -2, 2);
                backgroundImageRaw.transform.localScale = new Vector3(1, -1, 1); //Flip over 180 degree
                backgroundImageRaw.texture = manager.GetUsersClrTex();    //Apply Live FOV to Main Texture

                //Debug Texture
                debug_Texture.GetComponent<Transform>().localScale = new Vector3(0.2f, -0.15f, 0.2f);
                debug_Texture.GetComponent<GUITexture>().texture = manager.GetUsersClrTex();
                debug_Number = 0;
            }
            else
            {
                if (Input.GetKeyDown(KeyCode.Alpha1) || numberFromArduino == 1)
                {
                    debug_Texture.GetComponent<Transform>().localScale = new Vector3(0.2f, 0.15f, 0.2f);
                    debug_Texture.GetComponent<GUITexture>().texture = debugTextureImages[0];
                    debug_Number = 1;
                }
                else if (Input.GetKeyDown(KeyCode.Alpha2) || numberFromArduino == 2)
                {
                    debug_Texture.GetComponent<Transform>().localScale = new Vector3(0.2f, 0.15f, 0.2f);
                    debug_Texture.GetComponent<GUITexture>().texture = debugTextureImages[1];
                    debug_Number = 2;
                }
                else if (Input.GetKeyDown(KeyCode.Alpha3) || numberFromArduino == 3)
                {
                    debug_Texture.GetComponent<Transform>().localScale = new Vector3(0.2f, 0.15f, 0.2f);
                    debug_Texture.GetComponent<GUITexture>().texture = debugTextureImages[2];
                    debug_Number = 3;
                }
                else if (Input.GetKeyDown(KeyCode.Alpha4) || numberFromArduino == 4)
                {
                    debug_Texture.GetComponent<Transform>().localScale = new Vector3(0.2f, 0.15f, 0.2f);
                    debug_Texture.GetComponent<GUITexture>().texture = debugTextureImages[3];
                     debug_Number = 4;
                }
            }
            if (closeTrigger == true && sFlag == false) //When Trigger!
            {
                if (numberFromArduino != 0)
                {
                    test.GetComponent<BackgroundRemovalManager>().flag = true;
                    dustEffect.GetComponent<ParticleSystem>().Play();
                    _mouseButtonStatus = 0;
                    StartCoroutine("EmitDust");
                    if (stage == 0)
                    {
                        stage = 1;
                    }
                    sFlag = true;
                    if (dFlag)
                    {
                        dFlag = false;
                    }
                }
                if(Input.GetKeyDown(KeyCode.P))
                {
                   
                    dustEffect.GetComponent<ParticleSystem>().Play();
                    _mouseButtonStatus = 0;
                    StartCoroutine("EmitDust");
                    if (stage == 0)
                    {
                        stage = 1;
                    }
                    sFlag = true;
                    if (dFlag)
                    {
                        dFlag = false;
                    }
                    test.GetComponent<BackgroundRemovalManager>().flag = true;
                }
                
            }
            if (closeTrigger == false && dFlag == false)
            {
                    _mouseButtonStatus = 1;
                    dustEffect.GetComponent<ParticleSystem>().Stop();
                    StartCoroutine(FadeOut(0.0f, 3.0f));
                    dFlag = true;
                    if (sFlag)
                    {
                        sFlag = false;
                    }
                
            }
        }
        if (_fade == true)
        {
            /* this one */
            backgroundImageRaw.transform.localScale = new Vector3(1, 1, 1);
            if(debug_Number == 1)
            {
                backgroundImageRaw.texture = debugTextureImages[0];
            }
            else if(debug_Number == 2)
            {
                backgroundImageRaw.texture = debugTextureImages[1];
            }
            else if (debug_Number == 3)
            {
                backgroundImageRaw.texture = debugTextureImages[2];
            }
            else if (debug_Number == 4)
            {
                backgroundImageRaw.texture = debugTextureImages[3];
            }
            else
            {
                backgroundImageRaw.transform.localScale = new Vector3(1, -1, 1);
                backgroundImageRaw.texture = manager.GetUsersClrTex();
            }
           
           // backgroundImageRaw.texture = backgroundImage.texture;
        }
        if(_originRaw == true)
        {
            /* this one */
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
                StartCoroutine(FlagControl());
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

    IEnumerator FlagControl()
    {
        yield return new WaitForSeconds(2.8f);
        if (stage == 1)
        {
            stage = 0;
        }
    }
}
