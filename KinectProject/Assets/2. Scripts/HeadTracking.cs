using UnityEngine;
using System.Collections;

public class HeadTracking : MonoBehaviour
{


    [Tooltip("GUI-Text to display status messages.")]
    public GUIText statusText = null;
    public GUIText statusTextForRotation = null;
    public int playerIndex = 0;
    private Vector3 headPosition;
    private bool headPosValid = false;
    private KinectManager _kinectManager;//= KinectManager.Instance;
    public float headDistance;
    public bool? isTrigger;
    public bool headRotLocked;
    public bool headFlagWithKinectDetected;
    Vector3[] posData = new Vector3[100];
 

    void Start()
    {
        headRotLocked = false;
        headFlagWithKinectDetected = false;
        _kinectManager = KinectManager.Instance;
        isTrigger = null;
    }
    bool isFlag = false;
    void Update()
    {
        headPosValid = false;
        if (_kinectManager && _kinectManager.IsInitialized())
        {
            long userId = _kinectManager.GetUserIdByIndex(playerIndex);

            if (_kinectManager.IsUserTracked(userId) && _kinectManager.IsJointTracked(userId, (int)KinectInterop.JointType.Head))
            {
                Vector3 jointHeadPos = _kinectManager.GetJointPosition(userId, (int)KinectInterop.JointType.Head);
                Quaternion headPosRot = _kinectManager.GetJointOrientation(userId, (int)KinectInterop.JointType.Head, true);
                headPosition = jointHeadPos;

                if (headDistance < 1.8f && headDistance >= 1.5f)
                {
                       headFlagWithKinectDetected = true;
                }
                headDistance = headPosition.z;
                Vector3 newRot = headPosRot * Vector3.forward;

                if (newRot.z >= 0.95)
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
                if (statusText)
                {
                    string sStatusMsg = string.Format("Head position: {0}", jointHeadPos);
                    string sStatusMsgRot = string.Format("Head rotation: {0}", newRot);
                    statusText.text = sStatusMsg;
                    statusTextForRotation.text = sStatusMsgRot;
                }
            }
        }
    }
    
    public bool DetectMovement()
    {
        return false;
    }

}
