using System;
using UnityEngine;
using System.Collections;

public class BackgroundRemoval_V1 : MonoBehaviour {

    public GUITexture backgroundImageRaw; //Raw Kinect FOV
    public Texture[] backgrounds;
    public GameObject dustEffect;
    public GameObject removeMgr;
    public GameObject arduino;

    public delegate void functionPointer();

    private bool _arduinoFlag;
    void Start() {
        //Stop smoke effect
        dustEffect.GetComponent<ParticleSystem>().Stop();

        _arduinoFlag = false;
    }

    // Update is called once per frame
    void Update() {
        KinectManager manager = KinectManager.Instance;
        int a = ArduinoSerial.count;
      //  int a = arduino.GetComponent<ArduinoSerial>().
        Debug.Log("static varable = " + a);
        if (manager && manager.IsInitialized())
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                backgroundImageRaw.transform.localScale = new Vector3(1, -1, 1);
                removeMgr.GetComponent<BackgroundRemovalManager>().flag = false;
                backgroundImageRaw.texture = manager.GetUsersClrTex();
                StartCoroutine(EmitSmokeEffect(0.5f, false));
               //_arduinoFlag = false; 
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2) || (a == 1 && _arduinoFlag == false))
            {
                Debug.Log("change the scene to 1");
                StartCoroutine(EmitSmokeEffect(1.5f, true));
                StartCoroutine(FadeOut(0, 2.0f, 0));
                _arduinoFlag = true;
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                StartCoroutine(EmitSmokeEffect(1.5f, true));
                StartCoroutine(FadeOut(0, 2.0f, 1));
            }
            else if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                StartCoroutine(EmitSmokeEffect(1.5f, true));
                StartCoroutine(FadeOut(0, 2.0f, 2));
            }
        }
    }

    void ChangeScene(int index)
    {
           backgroundImageRaw.transform.localScale = new Vector3(1, 1, 1);
           backgroundImageRaw.texture = backgrounds[index];
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

    IEnumerator FadeOut(float value, float time, int sceneNumber)
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
        StartCoroutine(TransitionDelay(() => { StartCoroutine(FadeIn(1, 2.0f)); }, 1f, sceneNumber));
    }

    //Transition Func overload
    public IEnumerator TransitionDelay(functionPointer method, float waitTime, int sceneNumber)
    {
        yield return new WaitForSeconds(waitTime);
        ChangeScene(sceneNumber);
        method();
    }
    public IEnumerator TransitionDelay(functionPointer method, float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        method();
    }

     /*
    public IEnumerator TransitionDelay(Action method, float waitTime)
    {
        Debug.Log("delegate Coroutine!");
        yield return new WaitForSeconds(waitTime);
        method();
    }
    */
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
    
}
