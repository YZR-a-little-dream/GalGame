using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterState
{
    public string characterID;
    public int affection;

    public CharacterState(string id)
    {
        characterID = id;
        affection = 0;
    }
}
