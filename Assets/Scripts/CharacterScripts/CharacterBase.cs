using UnityEngine;
using System.Collections.Generic;

public class CharacterBase : MonoBehaviour {

    [Header("Basic Attributes")]
    public string CharacterName;
    public int HitPoints;
    [Header("Basic Movement")]
    public float WalkSpeed = 2.5f;
    public float RotationSpeed = 180f;
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
}
