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
}
