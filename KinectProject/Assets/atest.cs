using UnityEngine;
using System.Collections;

public class atest : MonoBehaviour {
    private Rigidbody someRigidBody;
    private float someRigidBodySpeed;
    // Use this for initialization
    void Awake () {
        if (GetComponent<Rigidbody>() != null)
        {
            //Set the object you just proved existed
            someRigidBody = GetComponent<Rigidbody>();
        }
        else
        {
            //If it does not exist report the problem ( normally a null would cause a 
            //crash/null reference exception during compilation ) depending on what's
            //happening. Both should be avoided
            Debug.LogError("Rigidbody missing in , can put name of class here,  in awake method");
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Extracts the speed from the rigibody 
        someRigidBodySpeed = someRigidBody.velocity.x;
        Debug.Log("asdf;lkjasdlkfjalsdkf '; " + someRigidBodySpeed);
    }
}
