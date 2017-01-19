using System;
using UnityEngine;
using System.Collections;

public class BackgroundRemoval_V1 : MonoBehaviour {

    public GUITexture backgroundImageRaw; //Raw Kinect FOV
    public Texture[] backgrounds;
    public GameObject dustEffect;
    public GameObject removeMgr;
    public GameObject arduino;
    public GameObject arduinoDoor;
    //public GameObject arduino_Old;
    public GameObject headTracking;
    public delegate void functionPointer();

    public bool _arduinoFlag_1;
    public bool _arduinoFlag_2;
    private bool? _detectedHead;
    bool kinectFlag;
    private bool _firstSceneFlag;
    private bool _flagP;
    void Start() {
        //Stop smoke effect
        dustEffect.GetComponent<ParticleSystem>().Stop();
        kinectFlag = false;
        _arduinoFlag_1 = false;
        _arduinoFlag_2 = false;
        _firstSceneFlag = true;
        _flagP = false;
    }

    // Update is called once per frame
    void Update() {
        KinectManager manager = KinectManager.Instance;

        int a = ArduinoSerial.count;
        if (manager && manager.IsInitialized())
        {
            _detectedHead = headTracking.GetComponent<HeadTracking>().isTrigger;

            if (_firstSceneFlag == true)
            {
                backgroundImageRaw.transform.localScale = new Vector3(1, -1, 1);
                backgroundImageRaw.texture = manager.GetUsersClrTex();
                _firstSceneFlag = false;
            }
            // problem when it comes back to normal part maybe coroiutine.
            if (Input.GetKeyDown(KeyCode.Alpha1) )//|| _detectedHead == false )
            {
                StartCoroutine(FadeOut(0, 2.0f, manager.GetUsersClrTex()));
                StartCoroutine(EmitSmokeEffect(0.5f, false));
             }
            else if (Input.GetKeyDown(KeyCode.Alpha2) || (a == 1 && _arduinoFlag_1 == false && _detectedHead == true)) //|| (b==1 && _arduinoFlag == false && _detectedHead== true))
            {
                Debug.Log("change the scene to 1");
                StartCoroutine(EmitSmokeEffect(1.5f, true));
                StartCoroutine(FadeOut(0, 2.0f, backgrounds[0]));
                _arduinoFlag_1 = true; 
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3) || (a == 2 && _arduinoFlag_2 == false && _detectedHead == true)) //|| (b == 2 && _arduinoFlag == false && _detectedHead == true))
            {
                Debug.Log("change the scene to 2");
                StartCoroutine(EmitSmokeEffect(1.5f, true));
                StartCoroutine(FadeOut(0, 2.0f, backgrounds[1]));
                _arduinoFlag_2 = true;
                StartCoroutine(TestDoorOpen(15.0f));
            }
            else if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                StartCoroutine(EmitSmokeEffect(1.5f, true));             
                StartCoroutine(FadeOut(0, 2.0f, backgrounds[2]));
            }
        }
    }

    void ChangeScene(Texture tx)
    {
        if ((tx == backgrounds[0]) || (tx == backgrounds[1]) || (tx == backgrounds[2]))
        {
            backgroundImageRaw.transform.localScale = new Vector3(1, 1, 1);
            backgroundImageRaw.texture = tx;
        }
        else
        {
            removeMgr.GetComponent<BackgroundRemovalManager>().flag = false;
            backgroundImageRaw.transform.localScale = new Vector3(1, -1, 1);
            backgroundImageRaw.texture = tx;
        }
    }

    //FadeIn / Out Function for transition
    IEnumerator FadeIn(float value, float time)
    {
        float alphaValue = backgroundImageRaw.color.a;
        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / time)
        {
            Color newColor = new Color(backgroundImageRaw.color.r, backgroundImageRaw.color.g, backgroundImageRaw.color.r,
                                        Mathf.Lerp(alphaValue, value, t));
            backgroundImageRaw.color = newColor;
            yield return null;
        }
    }

    IEnumerator FadeOut(float value, float time, Texture tx)
    {
        removeMgr.GetComponent<BackgroundRemovalManager>().flag = true;
        float alphaValue = backgroundImageRaw.color.a;
        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / time)
        {
            Color newColor = new Color(backgroundImageRaw.color.r, backgroundImageRaw.color.g, backgroundImageRaw.color.r,
                                        Mathf.Lerp(alphaValue, value, t));
            backgroundImageRaw.color = newColor;
            yield return null;
        }

        //FadeOut and FadeIn to main Scene
        //StartCoroutine(TransitionDelay(() => { StartCoroutine(FadeIn(1, 2.0f)); }, 3f));
        StartCoroutine(TransitionDelay(() => { StartCoroutine(FadeIn(1, 2.0f)); }, 1f, tx));
    }
    
    //Transition Func overload
    public IEnumerator TransitionDelay(functionPointer method, float waitTime, Texture tx)
    {
        yield return new WaitForSeconds(waitTime);
        ChangeScene(tx);
        method();
    }
    public IEnumerator TransitionDelay(functionPointer method, float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        method();
    }

    IEnumerator EmitSmokeEffect(float time, bool play)
    {
        yield return new WaitForSeconds(time);
        if (play == true)
        {
            dustEffect.GetComponent<ParticleSystem>().Play();
        }
        else
        {
            dustEffect.GetComponent<ParticleSystem>().Stop();
        }
    }
    IEnumerator TestDoorOpen(float time)
    {
        yield return new WaitForSeconds(time);
        arduinoDoor.GetComponent<ArduinoForDoor>().OperateDoorOpen(true); 

    }
    
}
