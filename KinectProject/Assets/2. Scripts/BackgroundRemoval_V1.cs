using UnityEngine;
using System.Collections;

public class BackgroundRemoval_V1 : MonoBehaviour {

    public GUITexture backgroundImageRaw; //Raw Kinect FOV
    public Texture[] backgrounds;

    public GameObject dustEffect;
    public GameObject removeMgr;      
                           
    void Start () {
        
    }
	
	// Update is called once per frame
	void Update () {

        KinectManager manager = KinectManager.Instance;
        if(manager && manager.IsInitialized())
        {

            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                backgroundImageRaw.transform.localScale = new Vector3(1, -1, 1);
                removeMgr.GetComponent<BackgroundRemovalManager>().flag = false;
                backgroundImageRaw.texture = manager.GetUsersClrTex();
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                removeMgr.GetComponent<BackgroundRemovalManager>().flag = true;
                backgroundImageRaw.transform.localScale = new Vector3(1, 1, 1);
                backgroundImageRaw.texture = backgrounds[0];
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                removeMgr.GetComponent<BackgroundRemovalManager>().flag = true;
                backgroundImageRaw.transform.localScale = new Vector3(1, 1, 1);
                backgroundImageRaw.texture = backgrounds[1];
            }
            else if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                removeMgr.GetComponent<BackgroundRemovalManager>().flag = true;
                backgroundImageRaw.transform.localScale = new Vector3(1, 1, 1);
                backgroundImageRaw.texture = backgrounds[2];
            }
        }
	}
}
