using UnityEngine;
using System.Collections;

public class FirstAndThirdPersonCamera : MonoBehaviour
{
    public Transform FollowTarget;
    public bool SetOffsetPositionAtStart = true;
    public bool CopyRotationFromCameraTarget = false;
    public float DistanceAway = 2f;			            // distance from the back of the craft
	public float DistanceUp = .1f;			            // distance above the craft
    public float SidewaysOffset = 0f;                   // offset sideways

    public float SafeDistanceUp = .5f;                  //use this to lift camera above player's head if it's about to go through
    public float SafeSideWaysOffsetRight = -.5f;        //To avoid clipping through walls when close on right side
    //public float SafeSideWaysOffsetLeft = .5f;        //To avoid clipping through walls when close on left side - not needed if aligned to left

    public float Smooth = 5f;				            // how smooth the camera movement is
            
	private Vector3 targetPosition;                     // the position the camera is trying to be in
    private Quaternion targetRotation;                  // the rotation ->>-
    private Vector3 lerpedPosition;                     // next smoothed position

    private float currentDistanceAway;
    private float currentDistanceUp;
    private float currentSidewaysOffset;

	private void Start() {
        if (FollowTarget == null) {
            Debug.LogError("FollowTarget null! Set character's head position object as FollowTarget.");
        }

        if (SetOffsetPositionAtStart)
        {
            transform.position = lerpedPosition = FollowTarget.position;
            transform.rotation = FollowTarget.rotation;
        }
        else
            lerpedPosition = transform.position;

        //If camera is parented to rig, first break camera free from it's parent
        if (this.transform.parent != null)
            this.transform.SetParent(null);

        currentDistanceAway = DistanceAway;
        currentDistanceUp = DistanceUp;
        currentSidewaysOffset = SidewaysOffset;
}


    private void LateUpdate() {
        transform.rotation = FollowTarget.rotation;
    }

    private void FixedUpdate () {

        targetPosition = FollowTarget.position + FollowTarget.up * currentDistanceUp - FollowTarget.forward * currentDistanceAway + FollowTarget.right * currentSidewaysOffset;

        CheckSafeDistance();

        // making a smooth transition between it's current position and the position it wants to be in        
        lerpedPosition = Vector3.Lerp(lerpedPosition, targetPosition, Time.fixedDeltaTime * Smooth);
        transform.position = lerpedPosition;
	}

    private void CheckSafeDistance()
    {
        /*
        Debug.DrawRay(FollowTarget.position, -FollowTarget.forward * 2f, Color.blue);
        Debug.DrawRay(FollowTarget.position, FollowTarget.right * .6f, Color.green);
        Debug.DrawRay(FollowTarget.position, -FollowTarget.right * .6f, Color.red);
        Debug.DrawRay(FollowTarget.position - FollowTarget.forward * .6f, FollowTarget.right * .6f, Color.green);
        */

        Ray behindRay = new Ray(FollowTarget.position, -FollowTarget.forward);
        Ray cameraRightRay = new Ray(FollowTarget.position, FollowTarget.right * .4f);
        //This helps with thin targets like doorways or pillars (1m behind target). Pretty fucked up, but kinda works...
        Ray secondRay = new Ray(FollowTarget.position - FollowTarget.forward * 1f, FollowTarget.right);

        RaycastHit hit;

        if (Physics.Raycast(behindRay, out hit, DistanceAway))
        {
            currentDistanceAway = hit.distance;
            currentDistanceUp = SafeDistanceUp;
        }
        else
        {
            currentDistanceAway = DistanceAway;
            currentDistanceUp = DistanceUp;
        }

        if (Physics.Raycast(cameraRightRay, out hit, DistanceAway))
        {
            //Terrible hack.
            bool secondHit = Physics.Raycast(secondRay, DistanceAway);
            
            if (secondHit && hit.distance < (Mathf.Abs(SidewaysOffset) + Mathf.Abs(SafeSideWaysOffsetRight)))
            {
                float minusOffset = 1f - hit.distance;
                float safeSideDist = SidewaysOffset - minusOffset;                
                currentDistanceUp = SafeDistanceUp;
                currentSidewaysOffset = SafeSideWaysOffsetRight;
            }
        }
        else
        {            
            currentDistanceUp = DistanceUp;
            currentSidewaysOffset = SidewaysOffset;
        }
    }
}
