using UnityEngine;
using System.Collections;

public class HeadDist : MonoBehaviour {

    Vector3 lastPos;
    Vector3 currentPos;
    int frameCount = 0;
    Vector3 temp;
    bool flag;
    public GameObject debugBox;
    int MAX = 70;
    public bool headLocked;
    // Use this for initialization
    void Awake()
    {
        debugBox.SetActive(false);
        flag = false;
        lastPos = this.gameObject.transform.position;
        headLocked = false;
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
            if (dist <= 0.08f)
            {
                debugBox.SetActive(true);
                headLocked = true;
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
