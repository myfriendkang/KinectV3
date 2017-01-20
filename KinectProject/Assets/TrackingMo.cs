using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class TrackingMo : MonoBehaviour {
    Vector3 lastPos;
    Vector3 currentPos;
    int frameCount = 0;
    Vector3 temp;
    bool flag;
    public GameObject debugBox;
    int MAX = 70;
    // Use this for initialization
    void Awake () {
        debugBox.SetActive(false);
        flag = false;
        lastPos = this.gameObject.transform.position;
    }
	
	// Update is called once per frame
	void Update () {
       
        if (frameCount <= MAX)
        {
            frameCount++;
    
            currentPos = this.gameObject.transform.position;

            if (flag == false)
            {
                temp = currentPos;
                flag = true;
            }

        }
        else
        {

            lastPos = currentPos;
            float dist = Vector3.Distance(temp, lastPos);
            Debug.Log(dist);
            if (dist <= 0.2f)
            {
                debugBox.SetActive(true);
            }
            else
            {
                debugBox.SetActive(false);
            }
            flag = false;
         
            frameCount = 0;
            lastPos = currentPos;

        }
    }
}
