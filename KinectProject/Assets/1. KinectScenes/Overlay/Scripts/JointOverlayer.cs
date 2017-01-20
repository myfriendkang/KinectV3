using UnityEngine;
using System.Collections;
//using Windows.Kinect;


public class JointOverlayer : MonoBehaviour 
{
	[Tooltip("GUI-texture used to display the color camera feed on the scene background.")]
	public GUITexture backgroundImage;

	[Tooltip("Camera that will be used to overlay the 3D-objects over the background.")]
	public Camera foregroundCamera;
	
	[Tooltip("Index of the player, tracked by this component. 0 means the 1st player, 1 - the 2nd one, 2 - the 3rd one, etc.")]
	public int playerIndex = 0;
	
	[Tooltip("Kinect joint that is going to be overlayed.")]
	public KinectInterop.JointType trackedJoint = KinectInterop.JointType.HandRight;

	[Tooltip("Game object used to overlay the joint.")]
	public Transform overlayObject;
    public Transform rightHand;
    public Transform leftHand;
    public Transform chest;
    //public float moothFactor = 10f;

    //public GUIText debugText;

    private Quaternion initialRotation = Quaternion.identity;
	private bool objFlipped = false;
    private bool objFlipped2 = false;
    private bool objFlipped3 = false;
    private bool objFlipped4 = false;
    [Tooltip("Threshold for movement")]
    [Range(0, 5)]
    public float movingThreshold = 1.5f; // 3;

    Vector3 currentHeadPos;
    Vector3 lastHeadPos;
    float speed;
    public GameObject debugCube;
    void Awake()
	{
        debugCube.SetActive(false);
        lastHeadPos.x = overlayObject.position.x;
		if(overlayObject)
		{
			// always mirrored
			initialRotation = overlayObject.rotation; // Quaternion.Euler(new Vector3(0f, 180f, 0f));
			objFlipped = (Vector3.Dot(overlayObject.forward, Vector3.forward) < 0);
			overlayObject.rotation = Quaternion.identity;
		}
        if (rightHand)
        {
            // always mirrored
            initialRotation = rightHand.rotation; // Quaternion.Euler(new Vector3(0f, 180f, 0f));
            objFlipped2 = (Vector3.Dot(rightHand.forward, Vector3.forward) < 0);
            rightHand.rotation = Quaternion.identity;
        }
        if (leftHand)
        {
            // always mirrored
            initialRotation = leftHand.rotation; // Quaternion.Euler(new Vector3(0f, 180f, 0f));
            objFlipped3 = (Vector3.Dot(leftHand.forward, Vector3.forward) < 0);
            leftHand.rotation = Quaternion.identity;
        }
        if (chest)
        {
            // always mirrored
            initialRotation = chest.rotation; // Quaternion.Euler(new Vector3(0f, 180f, 0f));
            objFlipped4 = (Vector3.Dot(chest.forward, Vector3.forward) < 0);
            chest.rotation = Quaternion.identity;
        }
    }
	
	void Update () 
	{
		KinectManager manager = KinectManager.Instance;
        
		if(manager && manager.IsInitialized() && foregroundCamera)
		{
			//backgroundImage.renderer.material.mainTexture = manager.GetUsersClrTex();
			if(backgroundImage && (backgroundImage.texture == null))
			{
				backgroundImage.texture = manager.GetUsersClrTex();
			}
			
			// get the background rectangle (use the portrait background, if available)
			Rect backgroundRect = foregroundCamera.pixelRect;
			PortraitBackground portraitBack = PortraitBackground.Instance;
			
			if(portraitBack && portraitBack.enabled)
			{
				backgroundRect = portraitBack.GetBackgroundRect();
			}

			// overlay the joint
			long userId = manager.GetUserIdByIndex(playerIndex);
			
			int iJointIndex = (int)trackedJoint;
            int rJointIndex = (int)KinectInterop.JointType.HandRight;
            int lJointIndex = (int)KinectInterop.JointType.HandLeft;
            int cJointIndex = (int)KinectInterop.JointType.SpineMid;

            Vector3 posJoint = manager.GetJointPosColorOverlay(userId, iJointIndex, foregroundCamera, backgroundRect);
            Vector3 posJointRightHand = manager.GetJointPosColorOverlay(userId, rJointIndex, foregroundCamera, backgroundRect);
            Vector3 posJointLeftHand = manager.GetJointPosColorOverlay(userId, lJointIndex, foregroundCamera, backgroundRect);
            Vector3 posJointSpine = manager.GetJointPosColorOverlay(userId, cJointIndex, foregroundCamera, backgroundRect);
            if (manager.IsJointTracked (userId, iJointIndex)) 
			{
		

				if (posJoint != Vector3.zero) 
				{
//						debugText.text = string.Format("{0} - {1}", trackedJoint, posJoint);

					if (overlayObject) 
					{
						overlayObject.position = posJoint;

						Quaternion rotJoint = manager.GetJointOrientation (userId, iJointIndex, !objFlipped);
						rotJoint = initialRotation * rotJoint;

						overlayObject.rotation = Quaternion.Slerp (overlayObject.rotation, rotJoint, 20f * Time.deltaTime);
					}
                    if (rightHand)
                    {
                        rightHand.position = posJointRightHand;

                        Quaternion rotJointR = manager.GetJointOrientation(userId, (int)KinectInterop.JointType.HandRight, !objFlipped2);
                        rotJointR = initialRotation * rotJointR;

                        rightHand.rotation = Quaternion.Slerp(rightHand.rotation, rotJointR, 20f * Time.deltaTime);
                    }
                    if (leftHand)
                    {
                        leftHand.position = posJointLeftHand;

                        Quaternion rotJointL = manager.GetJointOrientation(userId, (int)KinectInterop.JointType.HandLeft, !objFlipped3);
                        rotJointL = initialRotation * rotJointL;

                        leftHand.rotation = Quaternion.Slerp(leftHand.rotation, rotJointL, 20f * Time.deltaTime);
                    }
                    if (chest)
                    {
                        chest.position = posJointSpine;

                        Quaternion rotJointC = manager.GetJointOrientation(userId, (int)KinectInterop.JointType.SpineBase, !objFlipped4);
                        rotJointC = initialRotation * rotJointC;

                        chest.rotation = Quaternion.Slerp(chest.rotation, rotJointC, 20f * Time.deltaTime);
                    }
                }
                lastHeadPos.x = currentHeadPos.x;

			} 
			else 
			{
                // make the overlay object invisible
                /*	if (overlayObject && overlayObject.position.z > 0f) 
                    {
                        Vector3 posJoint = overlayObject.position;
                        posJoint.z = -10f;
                        overlayObject.position = posJoint;
                    }
                    */
            }


        }
	}
}
