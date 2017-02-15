using UnityEngine;
using System.Collections;
using System.IO;
public class testScreenshot : MonoBehaviour {

    public Texture2D photoFrame;
    public Texture2D printingTexture;
    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            StartCoroutine("TakePicture");
        }
        if (Input.GetKeyDown(KeyCode.V))
        {

            PrinterPlugin.print(printingTexture, false, PrinterPlugin.PrintScaleMode.PAGE_WIDTH);
        }
    }
    IEnumerator TakePicture()
    {
        yield return new WaitForEndOfFrame();
        Debug.Log("SHoT!!");

        Texture2D screenTexture = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        screenTexture.ReadPixels(new Rect(0f, 0f, Screen.width, Screen.height), 0, 0);


        screenTexture.Apply();
 
        Texture2D changedScale = ScaleTexture(screenTexture, 500, 750);
        changedScale = AddWatermark(changedScale, photoFrame, 0, 0);
        byte[] dataToSave = changedScale.EncodeToPNG();
        //byte[] dataToSave = screenTexture.EncodeToPNG();
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
        printingTexture = changedScale;

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

    public Texture2D AddWatermark(Texture2D background, Texture2D watermark, int startX, int startY)
    {
        Texture2D newTex = new Texture2D(background.width, background.height, background.format, false);
        for (int x = 0; x < background.width; x++)
        {
            for (int y = 0; y < background.height; y++)
            {
                if (x >= startX && y >= startY && x < watermark.width && y < watermark.height)
                {
                    Color bgColor = background.GetPixel(x, y);
                    Color wmColor = watermark.GetPixel(x - startX, y - startY);

                    Color final_color = Color.Lerp(bgColor, wmColor, wmColor.a / 1.0f);

                    newTex.SetPixel(x, y, final_color);
                }
                else
                    newTex.SetPixel(x, y, background.GetPixel(x, y));
            }
        }

        newTex.Apply();
        return newTex;
    }
}
