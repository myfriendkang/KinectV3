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
    Vector3[] posData = new Vector3[100];
 

    void Start()
    {
        headRotLocked = false;
        _kinectManager = KinectManager.Instance;
        isTrigger = null;
    }

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
             
                headDistance = headPosition.z;
                Vector3 newRot = headPosRot * Vector3.forward;
              //  Debug.Log(newRot.z);
                if(newRot.z >= 0.95)
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
