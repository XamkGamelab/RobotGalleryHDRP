using UnityEngine;
using System.Collections.Generic;

public class CharacterBase : MonoBehaviour {

    [Header("Basic Attributes")]
    public string CharacterName;
    public int HitPoints;
    [Header("Basic Movement")]
    public float WalkSpeed = 2.5f;
    public float RunSpeed = 6f;

    [HideInInspector]
    public bool IsAlive = true;
    [HideInInspector]
    public virtual bool Running
    {
        get { return _running; }
        set { _running = value; }
    }
    
    protected bool _running;

    protected CharacterController UnityCharacterController;

    public virtual void FiringHit(Ray _ray, Collider _colliderHit)
    {
        Debug.Log("Firing hits enemy COLLIDER!");
    }
}
