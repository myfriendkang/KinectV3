using UnityEngine;
using System.Collections;

public class ChestDist : MonoBehaviour {
    Vector3 lastPos;
    Vector3 currentPos;
    int frameCount = 0;
    Vector3 temp;
    bool flag;
    public GameObject debugBox;
    int MAX = 70;

    public bool chestLocked;
    // Use this for initialization
    void Awake()
    {
        debugBox.SetActive(false);
        flag = false;
        lastPos = this.gameObject.transform.position;
        chestLocked = false;
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
               if (dist <= 0.1f)
            {
                debugBox.SetActive(true);
                chestLocked = true;
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
