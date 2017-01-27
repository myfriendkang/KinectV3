using UnityEngine;
using System.Collections;

public class HeadTracking : MonoBehaviour
{
    public int playerIndex = 0;
    private Vector3 headPosition;
    private bool headPosValid = false;
    private KinectManager _kinectManager;//= KinectManager.Instance;
    public float headDistance;
    public bool? isTrigger;
    public bool headRotLocked;
    public bool headFlagWithKinectDetected;
    public GameObject arduino;

    void Start()
    {
        headRotLocked = false;
        headFlagWithKinectDetected = false;
        _kinectManager = KinectManager.Instance;
        isTrigger = null;
    }
    bool isPhoneRinged = false;
    void Update()
    {
        headPosValid = false;
        if (_kinectManager && _kinectManager.IsInitialized())
        {
            long userId = _kinectManager.GetUserIdByIndex(playerIndex);
            if (/*_kinectManager.IsUserTracked(userId) &&*/ _kinectManager.IsJointTracked(userId, (int)KinectInterop.JointType.Head))
            {
                Vector3 jointHeadPos = _kinectManager.GetJointPosition(userId, (int)KinectInterop.JointType.Head);
                Quaternion headPosRot = _kinectManager.GetJointOrientation(userId, (int)KinectInterop.JointType.Head, true);
                headPosition = jointHeadPos;
                // Debug.Log(headDistance);
                isPhoneRinged = arduino.GetComponent<ArduinoSerial>().GetPhoneCheck();
                Debug.Log("FUCKKCKK  " + isPhoneRinged);
                
                if (isPhoneRinged == true)
                {
                   // Debug.Log("at head tracking");
                    if (headDistance < 1.8f && headDistance >= 1.4f)
                    {
                        Debug.Log("WTF arduino");
                        headFlagWithKinectDetected = true;
                    }
                }
                

                headDistance = headPosition.z;
                Vector3 newRot = headPosRot * Vector3.forward;

                if (newRot.z >= 0.9)
                {
                    headRotLocked = true;
                    isTrigger = true;
                }
                else if(newRot.z < 0.9)
                {
                    isTrigger = false;
                }
                else
                {
                    isTrigger = null;
                }
                headPosValid = true;
            }
        }
    }
    
    public bool DetectMovement()
    {
        return false;
    }

}
