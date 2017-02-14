using UnityEngine;
using System.Collections;
using System.IO;
//using UnityEditor;

public class ScreenShot : MonoBehaviour {

    [Tooltip("Camera that will be used to render the background.")]
    public Camera backroundCamera;

    [Tooltip("Camera that will be used to overlay the 3D-objects over the background.")]
    public Camera foreroundCamera;

    [Tooltip("GUI-Text used to display information messages.")]
    public GUIText infoText;
    // Use this for initialization
    public GameObject orbs;
    public int playerIndex = 0;

    public GameObject head;
    public GameObject left;
    public GameObject headRot;

    public Texture2D defaultTexture;
    public Texture2D printingTexture;

    bool doPrinting;
    Texture2D test;
    int MAXCOUNT = 3;
    int screenShotCount = 0;

    public bool testForPrinting;
    public int printNum = 0;

    void Start () {
        doPrinting = false;
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine("TakePicture_Space");
        }
        if (Input.GetKeyDown(KeyCode.F9))
        {
            PrinterPlugin.print(printingTexture, false, PrinterPlugin.PrintScaleMode.PAGE_HEIGHT);
        }
        if (Input.GetKeyDown(KeyCode.F10))
        {
            PrinterPlugin.print(printingTexture, false, PrinterPlugin.PrintScaleMode.PAGE_WIDTH);
        }
        if (Input.GetKeyDown(KeyCode.F11))
        {
            PrinterPlugin.print(printingTexture, false, PrinterPlugin.PrintScaleMode.NO_SCALE);
        }
        if (Input.GetKeyDown(KeyCode.F12))
        {
            PrinterPlugin.print(printingTexture, false, PrinterPlugin.PrintScaleMode.FILL_PAGE);
        }
        if (testForPrinting == true)
        {
            if (doPrinting == true)
            {
                Debug.Log("LETS PRINTING NOW");
                doPrinting = false;
                // PrinterPlugin.print(printingTexture, false, PrinterPlugin.PrintScaleMode.FILL_PAGE);
                PrinterPlugin.print(printingTexture, false, PrinterPlugin.PrintScaleMode.PAGE_WIDTH);
            }
            if (GameObject.Find("BackgroundManger").GetComponent<BackgroundRemoval_V1>().firstSeceneChanged == true || GameObject.Find("BackgroundManger").GetComponent<BackgroundRemoval_V1>().secondSceneChanged == true)
            {
                if (head.GetComponent<HeadDist>().headLocked == true && left.GetComponent<LeftDist>().leftLocked == true && headRot.GetComponent<HeadTracking>().headRotLocked == true)
                {
                    head.GetComponent<HeadDist>().headLocked = false;
                    left.GetComponent<LeftDist>().leftLocked = false;
                    headRot.GetComponent<HeadTracking>().headRotLocked = false;
                    if (screenShotCount <= MAXCOUNT)
                    {
                        StartCoroutine("TakePicture");
                    }
                }
            }
        }
    }

    // ScreenShot
    IEnumerator TakePicture()
    {
        yield return new WaitForEndOfFrame();
        Debug.Log("SHoT!!");
        screenShotCount++;
        Texture2D screenTexture = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        screenTexture.ReadPixels(new Rect(0f, 0f, Screen.width, Screen.height), 0, 0);
        defaultTexture.ReadPixels(new Rect(0f, 0f, Screen.width, Screen.height), 0, 0);
      
        screenTexture.Apply();
        defaultTexture.Apply();

        Texture2D changedScale = ScaleTexture(screenTexture, 500, 750);
        byte[] dataToSave = changedScale.EncodeToPNG();
        //byte[] dataToSave = screenTexture.EncodeToPNG();
        byte[] dataForPrinting = defaultTexture.EncodeToPNG();
        string sDirName = Application.dataPath + "/Screenshots";

        if (!Directory.Exists(sDirName))
            Directory.CreateDirectory(sDirName);
        string sFileName = sDirName + "/" + string.Format("{0}", System.DateTime.Now.ToString("yyyy-MM-dd-HHmmss") + ".png");
        //string sFileName2 = sDirName + "/" + string.Format("{0}", System.DateTime.Now.ToString("Printing-yyyy-MM-dd-HHmmss") + ".png");
        //File.WriteAllBytes(sFileName, dataToSave);
        new System.Threading.Thread(() =>
        {
            System.Threading.Thread.Sleep(100);
            File.WriteAllBytes(sFileName, dataToSave);
        //File.WriteAllBytes(sFileName2, dataToSave);
        }).Start();
        printNum++;

        if (printNum == 3)
        {
            printingTexture = screenTexture;
            doPrinting = true;
        }
        //AssetDatabase.Refresh();
    }

    IEnumerator TakePicture_Space()
    {
        yield return new WaitForEndOfFrame();
        Debug.Log("SHoT!!");
        screenShotCount++;
        Texture2D screenTexture = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        //defaultTexture = new Texture2D(defaultTexture.width, defaultTexture.height, TextureFormat.RGB24, false);
        screenTexture.ReadPixels(new Rect(0f, 0f, Screen.width, Screen.height), 0, 0);
        defaultTexture.ReadPixels(new Rect(0f, 0f, Screen.width, Screen.height), 0, 0);

        screenTexture.Apply();
        defaultTexture.Apply();

        // Texture2D temp= ScaleTexture(screenTexture, 1000, 1500);
        Texture2D temp = ScaleTexture(screenTexture, 500, 750);
        byte[] dataToSave = temp.EncodeToPNG();

        //byte[] dataToSave = screenTexture.EncodeToPNG();
        byte[] dataForPrinting = defaultTexture.EncodeToPNG();
        string sDirName = Application.dataPath + "/Screenshots";

        if (!Directory.Exists(sDirName))
            Directory.CreateDirectory(sDirName);
        string sFileName = sDirName + "/" + string.Format("{0}", System.DateTime.Now.ToString("Space-yyyy-MM-dd-HHmmss") + ".png");
        //string sFileName2 = sDirName + "/" + string.Format("{0}", System.DateTime.Now.ToString("Space-Printing-yyyy-MM-dd-HHmmss") + ".png");
        //File.WriteAllBytes(sFileName, dataToSave);
        new System.Threading.Thread(() =>
        {
            System.Threading.Thread.Sleep(100);
            File.WriteAllBytes(sFileName, dataToSave);
            //File.WriteAllBytes(sFileName2, dataToSave);
        }).Start();
        printingTexture = screenTexture;
        //AssetDatabase.Refresh();
    }

    private Texture2D ScaleTexture(Texture2D source, int targetWidth, int targetHeight)
    {
        Texture2D result = new Texture2D(targetWidth, targetHeight, source.format, true);
        Color[] rpixels = result.GetPixels(0);
        float incX = ((float)1 / source.width) * ((float)source.width / targetWidth);
        float incY = ((float)1 / source.height) * ((float)source.height / targetHeight);
        for (int px = 0; px < rpixels.Length; px++)
        {
            rpixels[px] = source.GetPixelBilinear(incX * ((float)px % targetWidth),
                              incY * ((float)Mathf.Floor(px / targetWidth)));
        }
        result.SetPixels(rpixels, 0);
        result.Apply();
        return result;
    }

    public void ResetPrintNum()
    {
        printNum = 0;
    }
}
