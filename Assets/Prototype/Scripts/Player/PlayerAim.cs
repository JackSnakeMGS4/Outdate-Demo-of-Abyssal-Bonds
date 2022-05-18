using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAim : MonoBehaviour
{
    //get aim cursor sprite
    [SerializeField]
    private Sprite reticle;

    public void AimCursor(Vector2 vector2)
    {
        //check if aim input is being moved and do the following
        //draw aiming sprite at input position in relation to player position
        //draw line to aim point
        //pass aim direction to shooting class
        //if aim input isn't being moved then do nothing
    }
}
