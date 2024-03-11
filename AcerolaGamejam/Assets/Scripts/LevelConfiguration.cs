using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct LevelOffset
{
    public float offsetX;
    public float offsetY;
}

public class LevelConfiguration: MonoBehaviour
{
    // Start is called before the first frame update

    public float Level1Duration = 2.0f;
    public float Level2Duration = 2.0f;

    public LevelOffset Level(int level ,float t)
    {
        LevelOffset offset = new LevelOffset();
        
        switch(level)
        {
            case 1:
                {
                    //x=sen(y*4)*10
                    float imgSize = 100.0f;
                    float interpTime = t / Level1Duration;
                    float interpPxl = interpTime * imgSize;

                    offset.offsetX = interpPxl;
                    offset.offsetY = Mathf.Sin(offset.offsetX * 0.1f) * 40.0f;

                    return offset;
                }

            default:
                {
                    offset.offsetX = 0.0f;
                    offset.offsetY = 0.0f;
                    return offset;
                }  
        }

    }
}
