using UnityEngine;
using System.Collections;

public class HeadTracking : MonoBehaviour
{


    [Tooltip("GUI-Text to display status messages.")]
    public GUIText statusText = null;

    public int playerIndex = 0;
    private Vector3 headPosition;
    private bool headPosValid = false;
    private KinectManager _kinectManager;//= KinectManager.Instance;
    public float headDistance;
    public bool? isClose;
    Vector3[] posData = new Vector3[100];

    void Start()
    {
        _kinectManager = KinectManager.Instance;
        isClose = null;
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
                headPosition = jointHeadPos;
                headDistance = headPosition.z;
                if (headDistance < 1.3f) //40 Feet
                {
                    isClose = true;
                }
                else if (headDistance >1.3f && headDistance < 2.8f)
                {
                    isClose = false;
                }
                else
                {
                    isClose = null;
                }
                headPosValid = true;
                if (statusText)
                {
                    string sStatusMsg = string.Format("Head position: {0}", jointHeadPos);
                    statusText.text = sStatusMsg;
                }
            }
        }
    }
    
    public bool DetectMovement()
    {
        return false;
    }

}
