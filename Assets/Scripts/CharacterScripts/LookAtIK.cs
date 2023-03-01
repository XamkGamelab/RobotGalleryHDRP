using UnityEngine;

public class LookAtIK : MonoBehaviour
{
    private Animator anim => GetComponent<Animator>();
    private Vector3 lookAtPosition;
    private Vector3 currentlookAtPosition;
    private float interpolationSmoothing = 4f;
    public void SetIKLookTarget(Vector3 lookAtPos)
    {
        lookAtPosition = lookAtPos;
    }

    private void OnAnimatorIK()
    {
        currentlookAtPosition = Vector3.Lerp(currentlookAtPosition, lookAtPosition, Time.deltaTime * interpolationSmoothing);
        anim.SetLookAtWeight(.7f);
        anim.SetLookAtPosition(currentlookAtPosition);
    }
}
