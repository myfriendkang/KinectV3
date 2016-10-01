using UnityEngine;
using System.Collections;

public class HeadTracking : MonoBehaviour {


    [Tooltip("GUI-Text to display status messages.")]
    public GUIText statusText = null;

    public int playerIndex = 0;

    private KinectManager kinectManager;
   
    private Vector3 headPosition;
    private bool headPosValid = false;


    void Start()
    {
        kinectManager = KinectManager.Instance;
    }

    void Update()
    {
        headPosValid = false;
        if (kinectManager && kinectManager.IsInitialized())
        {
            long userId = kinectManager.GetUserIdByIndex(playerIndex);

            if (kinectManager.IsUserTracked(userId) && kinectManager.IsJointTracked(userId, (int)KinectInterop.JointType.Head))
            {
                Vector3 jointHeadPos = kinectManager.GetJointPosition(userId, (int)KinectInterop.JointType.Head);
                headPosition = jointHeadPos;
                headPosValid = true;

                if (statusText)
                {
                    string sStatusMsg = string.Format("Head position: {0}", jointHeadPos);
                    Debug.Log(sStatusMsg);
                    statusText.text = sStatusMsg;
                }
            }
        }
    }
}
