using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IItem
{
    int ItemID { get; }
    string ItemName { get; }
    string ItemDesc { get; }

    public void Use(Character itemTarget)
    {
        
    }
}

