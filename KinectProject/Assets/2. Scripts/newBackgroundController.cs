using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class newBackgroundController : MonoBehaviour {

    //public GUITexture[] backgroundImages;    //Image Texture
    private GUITexture backgroundImage;
    public GUITexture backgroundImageRaw; //Raw Kinect FOV
    public GameObject dustEffect;
    public GameObject debug_Texture;
    public Sprite[] debug_imgs;
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
    int textureCount;
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
            Debug.Log(_fade);
            closeTrigger = headTracking.GetComponent<HeadTracking>().isClose;
            if (backgroundImageRaw.texture == null)
            {
                backgroundImageRaw.transform.localScale = new Vector3(1, -1, 1); //Flip over 180 degree
                backgroundImageRaw.texture = manager.GetUsersClrTex();    //Apply Live FOV to Main Texture
            }
            {
                backgroundImageRaw.transform.localScale = new Vector3(1, 1, 1);
                /* here */
                //textureCount = GameObject.Find("Arduino").GetComponent<ArduinoController>().count;
                if (Input.GetKeyDown(KeyCode.Alpha1))
                {
                    backgroundImage.texture = (Texture)Resources.Load("Background/Background_1") as Texture;
                    debug_Texture.GetComponent<Image>().sprite = debug_imgs[0];
                }
                else if (Input.GetKeyDown(KeyCode.Alpha2))
                {
                    backgroundImage.texture = (Texture)Resources.Load("Background/Background_2") as Texture;
                    debug_Texture.GetComponent<Image>().sprite = debug_imgs[1];
                }
                else if (Input.GetKeyDown(KeyCode.Alpha3))
                {
                    backgroundImage.texture = (Texture)Resources.Load("Background/Background_3") as Texture;
                    debug_Texture.GetComponent<Image>().sprite = debug_imgs[2];
                }
                else if (Input.GetKeyDown(KeyCode.Alpha4))
                {
                    backgroundImage.texture = (Texture)Resources.Load("Background/Background_4") as Texture;
                    debug_Texture.GetComponent<Image>().sprite = debug_imgs[3];
                }
                // backgroundImage.texture = backgroundImages[textureCount].texture;
                /* end */
                //backgroundImageRaw.texture = backgroundImage.texture;
            }
            if (Input.GetMouseButtonDown(0) || (closeTrigger == true && sFlag == false)) //When Trigger!
            {
                test.GetComponent<BackgroundRemovalManager>().flag = true;
                dustEffect.GetComponent<ParticleSystem>().Play();
                _mouseButtonStatus = 0;
                StartCoroutine("EmitDust");
                StartCoroutine(FadeOut(0.0f, 3.0f));
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
            if (Input.GetMouseButtonDown(1) || (closeTrigger == false && dFlag == false))
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
            backgroundImageRaw.transform.localScale = new Vector3(1, 1, 1);
            /* here */
            //textureCount = GameObject.Find("Arduino").GetComponent<ArduinoController>().count;
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
              
                debug_Texture.GetComponent<Image>().sprite= debug_imgs[0];
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
              
                debug_Texture.GetComponent<Image>().sprite = debug_imgs[1];
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
         
                debug_Texture.GetComponent<Image>().sprite = debug_imgs[2];
            }
            else if (Input.GetKeyDown(KeyCode.Alpha4))
            {
              
                debug_Texture.GetComponent<Image>().sprite = debug_imgs[3];
            }
           // backgroundImage.texture = backgroundImages[textureCount].texture;
            /* end */
            //backgroundImageRaw.texture = backgroundImage.texture;
        }
        if (_originRaw == true)
        {
            backgroundImageRaw.transform.localScale = new Vector3(1, -1, 1);
            backgroundImageRaw.texture = manager.GetUsersClrTex();
        }
    }

    IEnumerator EmitDust()
    {
        yield return new WaitForSeconds(0.5f);
        //StartCoroutine(FadeOut(0.0f, 3.0f));
    }

    IEnumerator FadeOut(float aValue, float aTime)
    {
        float alphaValue = backgroundImageRaw.color.a;
        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / aTime)
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
        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / aTime)
        {
            Color newColor = new Color(backgroundImageRaw.color.r, backgroundImageRaw.color.g, backgroundImageRaw.color.b, Mathf.Lerp(alphaValue, aValue, t));
            backgroundImageRaw.color = newColor;
            yield return null;
            if (_isReady == false)
            {
                StartCoroutine(TimerForFadeIN(3.0f));
                _isReady = true;
                if (_originRaw == true)
                {
                    _originRaw = false;
                }
            }
            if (_origin == false)
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
