using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public bool up;
    public bool down;
    public bool right;
    public bool left;

    public int type;

    public void DestroyItself()
    {
        Destroy(this.gameObject);
    }
}
