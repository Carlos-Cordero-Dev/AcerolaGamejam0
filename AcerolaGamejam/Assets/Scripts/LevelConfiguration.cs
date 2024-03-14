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
    public float NonRepeatedLevelDuration(int level)
    {
        if (levels.Length < level) return 1.0f;

        return levels[level - 1].x;
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
                    float interpTime = t / NonRepeatedLevelDuration(1);
                    float interpPxl = interpTime * imgSize;

                    offset.offsetX = interpPxl;
                    offset.offsetY = Mathf.Sin(offset.offsetX * 0.1f) * 25.0f;
                }
                break;

            case 2:
                { //ciruclar motion
                    float interpTime = t / NonRepeatedLevelDuration(2);
                    float interpAngle = interpTime * Mathf.PI * 2; // Full circle in Level1Duration time
                    float radius = 35.0f; // Radius of circular motion

                    offset.offsetX = Mathf.Cos(interpAngle) * radius;
                    offset.offsetY = Mathf.Sin(interpAngle) * radius;
                }
                break;

            case 3:
                { //spiral motion
                    float interpTime = t / NonRepeatedLevelDuration(3);
                    float interpAngle = interpTime * Mathf.PI * 2; // Full circle in Level1Duration time
                    float radiusIncreaseRate = 10.0f; // Rate at which radius increases

                    float radius = interpAngle * radiusIncreaseRate; // Increasing radius

                    offset.offsetX = Mathf.Cos(interpAngle) * radius;
                    offset.offsetY = Mathf.Sin(interpAngle) * radius;

                }
                break;
            case 4: 
                { //double "heart" shape 
                    float interpTime = t / NonRepeatedLevelDuration(4);
                    float interpAngle = interpTime * Mathf.PI * 2; // Full circle in Level1Duration time
                    float scale = 45.0f; // Scale factor for the heart shape

                    offset.offsetX = scale * (Mathf.Sin(interpAngle) * Mathf.Pow(Mathf.Abs(Mathf.Cos(interpAngle)), 1.0f / 3.0f));
                    offset.offsetY = scale * (Mathf.Cos(interpAngle) * Mathf.Pow(Mathf.Abs(Mathf.Cos(interpAngle)), 1.0f / 3.0f));
                }
                break;
            case 5:
                { //lissajous curve
                    float interpTime = t / NonRepeatedLevelDuration(5);
                    float interpAngleX = interpTime * Mathf.PI * 2; // Full circle in Level1Duration time for X-axis
                    float interpAngleY = interpTime * Mathf.PI * 2 * 1.5f; // Full circle in Level1Duration time for Y-axis
                    float amplitudeX = 35.0f; // Amplitude of X-axis
                    float amplitudeY = 25.0f; // Amplitude of Y-axis

                    float x = Mathf.Sin(interpAngleX) * amplitudeX;
                    float y = Mathf.Sin(interpAngleY) * amplitudeY;

                    offset.offsetX = x;
                    offset.offsetY = y;
                }
                break;

            case 6:
                { //eight pattern
                    float interpTime = t / NonRepeatedLevelDuration(6);
                    float interpAngle = interpTime * Mathf.PI * 2; // Full circle in Level1Duration time
                    float radius = 30.0f; // Radius of the figure eight

                    float x = Mathf.Sin(interpAngle) * radius;
                    float y = Mathf.Sin(2 * interpAngle) * radius * 0.5f;

                    offset.offsetX = x;
                    offset.offsetY = y;
                }
                break;
            case 7:
                { //pulsating and rotating
                    float interpTime = t / NonRepeatedLevelDuration(7);
                    float amplitude = 20.0f; // Amplitude of pulsation
                    float frequency = 2.0f; // Frequency of pulsation
                    float rotationSpeed = 4.0f; // Speed of rotation in radians per second
                    Vector2 pivotPoint = new Vector2(0.0f, 0.0f); // Pivot point for rotation

                    float pulseFactor = Mathf.Sin(interpTime * Mathf.PI * 2 * frequency) * amplitude + amplitude;

                    // Calculate rotation angle based on time and rotation speed
                    float rotationAngle = interpTime * rotationSpeed;

                    // Calculate position using polar coordinates
                    float x = pivotPoint.x + Mathf.Cos(rotationAngle) * pulseFactor;
                    float y = pivotPoint.y + Mathf.Sin(rotationAngle) * pulseFactor;

                    offset.offsetX = x;
                    offset.offsetY = y;
                }
                break;
            case 8:
                { //spiral + galaxy
                    float interpTime = t / NonRepeatedLevelDuration(8);
                    float spiralRadius = 20.0f; // Radius of the spiral motion
                    float spiralArmWidth = 20.0f; // Width of the spiral arms
                    float spiralAngularSpeed = 1.5f; // Angular speed of rotation in radians per second
                    float helixRadius = 10.0f; // Radius of the helix
                    float helixHeight = 5.0f; // Height of each loop in the helix
                    float helixRotationSpeed = 20.0f; // Speed of rotation in radians per second
                    Vector2 helixPivotPoint = new Vector2(0.0f, 0.0f); // Pivot point for rotation

                    // Spiral motion
                    float spiralRotationAngle = interpTime * spiralAngularSpeed;
                    float spiralX = Mathf.Cos(spiralRotationAngle) * (spiralRadius + spiralArmWidth * interpTime);
                    float spiralY = Mathf.Sin(spiralRotationAngle) * (spiralRadius + spiralArmWidth * interpTime);

                    // Helix motion
                    float helixRotationAngle = interpTime * helixRotationSpeed;
                    float helixX = helixPivotPoint.x + Mathf.Cos(helixRotationAngle) * helixRadius;
                    float helixY = helixPivotPoint.y + interpTime * helixHeight;

                    offset.offsetX = spiralX + helixX;
                    offset.offsetY = spiralY + helixY;
                }
                break;
            case 9:
                { //lemniscate motion
                    float interpTime = t / NonRepeatedLevelDuration(9);
                    float interpAngle = interpTime * Mathf.PI * 2; // Full circle in Level1Duration time
                    float scale = 50.0f; // Scale factor for the lemniscate shape

                    float x = scale * Mathf.Cos(interpAngle) / (1 + Mathf.Sin(interpAngle) * Mathf.Sin(interpAngle));
                    float y = scale * Mathf.Sin(interpAngle) * Mathf.Cos(interpAngle) / (1 + Mathf.Sin(interpAngle) * Mathf.Sin(interpAngle));

                    offset.offsetX = x;
                    offset.offsetY = y;
                }
                break;
            //case 10:
            //    { //hypocycloid motion
            //        float interpTime = t / NonRepeatedLevelDuration(10);
            //        float interpAngle = interpTime * Mathf.PI * 2; // Full circle in Level1Duration time
            //        float scale = 35.0f; // Scale factor for the hypocycloid shape
            //        float innerRadius = 15.0f; // Inner radius of the hypocycloid
            //        float outerRadius = 35.0f; // Outer radius of the hypocycloid

            //        float x = (outerRadius - innerRadius) * Mathf.Cos(interpAngle) + innerRadius * Mathf.Cos((outerRadius - innerRadius) * interpAngle / innerRadius);
            //        float y = (outerRadius - innerRadius) * Mathf.Sin(interpAngle) - innerRadius * Mathf.Sin((outerRadius - innerRadius) * interpAngle / innerRadius);

            //        offset.offsetX = x * scale;
            //        offset.offsetY = y * scale;
            //    }
            //    break;
            case 10:
                { //bounded cardioid motion
                    float interpTime = t / NonRepeatedLevelDuration(10);
                    float interpAngle = interpTime * Mathf.PI * 2; // Full circle in Level1Duration time
                    float scale = 35.0f; // Scale factor for the cardioid shape

                    float x = scale * (Mathf.Cos(interpAngle) * (1 - Mathf.Cos(interpAngle)));
                    float y = scale * (Mathf.Sin(interpAngle) * (1 - Mathf.Cos(interpAngle)));

                    offset.offsetX = Mathf.Clamp(x, -50f, 50f);
                    offset.offsetY = Mathf.Clamp(y, -50f, 50f);
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
