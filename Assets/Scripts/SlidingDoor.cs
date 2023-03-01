using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SlidingDoor : MonoBehaviour
{
    public Transform Door;
    public float OpenXPosition;
    private bool isOpen = false;

    private Vector3 initPosition;
    private void Awake()
    {
        initPosition = Door.transform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isOpen)
        {
            Debug.Log("OPEN DOOR");
            Door.DOLocalMoveX(OpenXPosition, 1f).SetEase(Ease.InOutSine);
        }
    }
}
