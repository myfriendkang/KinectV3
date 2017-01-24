using UnityEngine;
using System.Collections;

public class LeftDist : MonoBehaviour {

    Vector3 lastPos;
    Vector3 currentPos;
    int frameCount = 0;
    Vector3 temp;
    bool flag;
    public GameObject debugBox;
    int MAX = 60;

    public bool leftLocked;
    // Use this for initialization
    void Awake()
    {
        leftLocked = false;
        debugBox.SetActive(false);
        flag = false;
        lastPos = this.gameObject.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
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
            if (dist <= 0.07f)
            {
                leftLocked = true;
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
