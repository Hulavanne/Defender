using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ProjectConstants
{
    public const float VERTICAL_BOUNDARY_TOP = 2.2f;
    public const float VERTICAL_BOUNDARY_BOTTOM = -2.5f;

    public const float SECTOR_WIDTH = 8.0f;

    public static Vector2 PickRandomPositionNearby(Vector2 originalPosition, float minX, float maxX)
    {
        // Pick a random value on the X-axis in relation to current position, then randomly set it as negative or positive
        float _x = originalPosition.x + Random.Range(minX, maxX) * (Random.Range(0, 2) * 2 - 1);

        // Pick a random value on the Y-axis that is between the allowed boundaries
        float _y = Random.Range(VERTICAL_BOUNDARY_BOTTOM, VERTICAL_BOUNDARY_TOP);

        return new Vector2(_x, _y);
    }

    public static string AddOrdinal(int number)
    {
        if (number <= 0) return number.ToString();

        switch (number % 100)
        {
            case 11:
            case 12:
            case 13:
                return number + "th";
        }

        switch (number % 10)
        {
            case 1:
                return number + "st";
            case 2:
                return number + "nd";
            case 3:
                return number + "rd";
            default:
                return number + "th";
        }
    }
}
