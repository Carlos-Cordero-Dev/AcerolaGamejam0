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
    /*
    [x: level duration,
     y: preview multiplier (making preview go faster than actual level), 
     z: number of times the motion of the level is repeated (most patterns are cyclical]
    */
    public Vector3[] levels;

    // offset for each pattern in x, y
    public Vector2[] levelOffsets;

    public Transform[] spawnpoints;
    public float MYoffset = 1.0f;
    public Vector3 LevelSpawnpoint(int level)
    {
        if (spawnpoints.Length < level) return new Vector3(0.0f, 0.0f, 0.0f);

        return spawnpoints[level - 1].position;
    }

    public float LevelDuration(int level)
    {
        if (levels.Length < level) return 1.0f;

        return levels[level - 1].x * NumberOfPatternRepetitions(level);
    }

    public float PreviewTime(int level, GameManager.GameState state)
    {
        //Debug.Log("pt duration: " + LevelDuration(level));
        //Debug.Log("pt modfier: " + TimeModifierForLevel(level, state));
        //Debug.Log("pt patterns: " + NumberOfPatternRepetitions(level));

        return (LevelDuration(level) / TimeModifierForLevel(level, state));
    }

    public float TimeModifierForLevel(int level,GameManager.GameState state)
    {
        if (state == GameManager.GameState.Gameplay) return 1.0f;

        if (levels.Length < level) return 1.0f;

        return levels[level - 1].y;
    }

    public float NumberOfPatternRepetitions(int level)
    {
        if (levels.Length < level) return 1.0f;

        return levels[level - 1].z;
    }
    public LevelOffset Level(int level ,float t)
    {
        LevelOffset offset = new LevelOffset();
        float imgSize = 100.0f;

        switch (level)
        {
            case 1:
                { //sin motion
                    //x=sen(y*4)*10
                    float interpTime = t / LevelDuration(1);
                    float interpPxl = interpTime * imgSize;

                    offset.offsetX = interpPxl;
                    offset.offsetY = Mathf.Sin(offset.offsetX * 0.1f) * 25.0f;
                }
                break;

            case 2:
                { //ciruclar motion
                    float interpTime = t / LevelDuration(2);
                    float interpAngle = interpTime * Mathf.PI * 2; // Full circle in Level1Duration time
                    float radius = 35.0f; // Radius of circular motion

                    offset.offsetX = Mathf.Cos(interpAngle) * radius;
                    offset.offsetY = Mathf.Sin(interpAngle) * radius;
                }
                break;

            case 3:
                { //spiral motion
                    float interpTime = t / LevelDuration(3);
                    float interpAngle = interpTime * Mathf.PI * 2; // Full circle in Level1Duration time
                    float radiusIncreaseRate = 2.0f; // Rate at which radius increases

                    float radius = interpAngle * radiusIncreaseRate; // Increasing radius

                    offset.offsetX = Mathf.Cos(interpAngle) * radius;
                    offset.offsetY = Mathf.Sin(interpAngle) * radius;

                }
                break;

            default:
                {
                    offset.offsetX = 0.0f;
                    offset.offsetY = 0.0f;
                }
                break;
        }
        offset.offsetX += levelOffsets[level - 1].x;
        offset.offsetY += levelOffsets[level - 1].y;

        return offset;

    }
}
