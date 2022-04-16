using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomGreedy : MonoBehaviour
{
    protected Character myCharacter;
    public bool proceeding { get; private set; }
    public virtual void InitializeGreedy(Character _character)
    {
        proceeding = false;
        myCharacter = _character;
    }
}
