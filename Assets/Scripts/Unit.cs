using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public char Initial;
    public char Player;

    public override string ToString()
    {
        return "" + Initial + Player;
    }

    // this will be implemented later
}
