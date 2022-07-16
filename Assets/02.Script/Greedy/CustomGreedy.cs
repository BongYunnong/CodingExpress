using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomGreedy : MonoBehaviour
{
    protected Character myCharacter;
    [SerializeField] protected bool randomize;
    public bool proceeding { get; protected set; }
    public bool initialized { get; protected set; }
    public virtual void InitializeGreedy(Character _character)
    {
        proceeding = false;
        myCharacter = _character;
    }

    public virtual void StartGreedy()
    {
        proceeding = true;
    }
    public virtual void ResetGreedy()
    {
        proceeding = false;
    }
    
}
