using UnityEngine;
using DG.Tweening;

public class SlidingDoor : MonoBehaviour
{
    public Transform Door;
    public float StayOpenTime = 5f;

    public float OpenXPosition;
    private bool isOpen = false;
    
    private float initPositionX;
    private void Awake()
    {
        initPositionX = Door.transform.localPosition.x;
    }

    private void OnTriggerStay(Collider other)
    {
        if (!isOpen)
        {
            Debug.Log("OPEN DOOR");
            isOpen = true;
            //Open the door and move to closing animation when completed
            Door.DOLocalMoveX(OpenXPosition, 1f).SetEase(Ease.InOutSine).OnComplete(() => DoorOpenedCloseIt());            
        }
    }

    private void DoorOpenedCloseIt()
    {
        //...wait for StayOpenTime and run close animation and allow re-opening
        Door.DOLocalMoveX(initPositionX, 1f).SetDelay(StayOpenTime).SetEase(Ease.InOutSine).OnComplete(() => isOpen = false);
    }
}
