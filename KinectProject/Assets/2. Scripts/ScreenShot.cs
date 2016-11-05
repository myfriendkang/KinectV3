using UnityEngine;
using System.Collections;
using System.IO;

public class ScreenShot : MonoBehaviour {

    [Tooltip("Camera that will be used to render the background.")]
    public Camera backroundCamera;

    [Tooltip("Camera that will be used to overlay the 3D-objects over the background.")]
    public Camera foreroundCamera;

    [Tooltip("Array of sprite transforms that will be used for displaying the countdown until image shot.")]
    public Transform[] countdown;

    [Tooltip("GUI-Text used to display information messages.")]
    public GUIText infoText;
    // Use this for initialization
    void Start () {
        KinectManager manager = KinectManager.Instance;
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonDown(2))
        {
            StartCoroutine("CountdownAndTakePicture");
        }
	}

    private IEnumerator CountdownAndTakePicture()
    {
        if (countdown.Length > 0)
        {
            for (int i = 0; i < countdown.Length; i++)
            {
                if (countdown[i])
                    countdown[i].gameObject.SetActive(true);

                yield return new WaitForSeconds(1.0f);

                if (countdown[i])
                    countdown[i].gameObject.SetActive(false);
            }
        }

        StartCoroutine("TakePicture");
        yield return null;
    }


    // ScreenShot
    IEnumerator TakePicture()
    {
        yield return new WaitForEndOfFrame();

        Texture2D screenTexture = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, true);
        // put buffer into texture
        screenTexture.ReadPixels(new Rect(0f, 0f, Screen.width, Screen.height), 0, 0);
        // apply
        screenTexture.Apply();

        byte[] dataToSave = screenTexture.EncodeToPNG();
        string sDirName = Application.dataPath + "/Screenshots";
        if (!Directory.Exists(sDirName))
            Directory.CreateDirectory(sDirName);
        string sFileName = sDirName + "/" + string.Format("{0}", System.DateTime.Now.ToString("yyyy-MM-dd-HHmmss") + ".png");
        File.WriteAllBytes(sFileName, dataToSave);
    }
}
