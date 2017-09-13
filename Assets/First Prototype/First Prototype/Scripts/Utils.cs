using UnityEngine;


class Utils
{
    static public int Random(Vector2 range)
    {
        return UnityEngine.Random.Range((int)range.x, (int)range.y);
    }

    static public float Randomf(Vector2 range)
    {
        return UnityEngine.Random.Range(range.x, range.y);
    }

    static public int Random(int min, int max)
    {
        return UnityEngine.Random.Range(min, max);
    }

    static public float Randomf(float min, float max)
    {
        return UnityEngine.Random.Range(min, max);
    }
}

