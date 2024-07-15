using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//simple AI interface 
public interface IAI
{
    void MoveTowardsPlayer(PathNode playerPosition,out int endX,out int endY);
}
