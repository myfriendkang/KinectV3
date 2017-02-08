using UnityEngine;
using System.Collections;

public class GamaManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }
        if (Input.GetKey(KeyCode.R))
        {
            ResetAll();
        }
	
	}

    public void ResetAll()
    {
        //Reset All value that can ready for the next user
        //1. Screenshot (number reset)
        //2. background reset to normal.
        //3. sound reset
        //4. Arduino flag change to false(Arduino, BackgroundRemoval)
        
        
    }
}
