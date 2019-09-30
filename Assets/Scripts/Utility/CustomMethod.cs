using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CustomMethod
{
    /// <summary>
    /// Permet d'obtenir l'épaisseur d'un point sur un lineRenderer
    /// </summary>
    /// <param name="p_pointIndex"></param>
    /// <param name="p_lineRenderer"></param>
    /// <returns></returns>
    public static float GetLineWidthAtPoint(int p_pointIndex, LineRenderer p_lineRenderer)
    {
        float width = 0;

        Vector3 endPoint = p_lineRenderer.GetPosition(p_lineRenderer.positionCount - 1);
        Vector3 startPoint = p_lineRenderer.GetPosition(0);
        Vector3 targetPoint = p_lineRenderer.GetPosition(p_pointIndex);

        float distance = Vector2.Distance(startPoint, endPoint);
        float targetDistance = Vector2.Distance(startPoint, targetPoint);

        float curveTargetValue = targetDistance / distance;

        width = p_lineRenderer.widthCurve.Evaluate(curveTargetValue);

        return width;
    }

    /// <summary>
    /// Permet de connaitre la presque égalité de deux vectors
    /// </summary>
    /// <param name="v1"></param>
    /// <param name="v2"></param>
    /// <param name="precision"></param>
    /// <returns></returns>
    public static bool AlmostEqual(Vector3 v1, Vector3 v2, float precision)
    {
        bool equal = true;

        if (Mathf.Abs(v1.x - v2.x) > precision) equal = false;
        if (Mathf.Abs(v1.y - v2.y) > precision) equal = false;
        if (Mathf.Abs(v1.z - v2.z) > precision) equal = false;

        return equal;
    }

    /// <summary>
    /// Permet de connaitre la presque égalité de deux vectors 2D
    /// </summary>
    /// <param name="v1"></param>
    /// <param name="v2"></param>
    /// <param name="precision"></param>
    /// <returns></returns>
    public static bool AlmostEqual2D(Vector2 v1, Vector2 v2, float precision)
    {
        bool equal = true;

        if (Mathf.Abs(v1.x - v2.x) > precision) equal = false;
        if (Mathf.Abs(v1.y - v2.y) > precision) equal = false;

        return equal;
    }

    public enum Axis
    {
        X,
        Y,
        Z
    }

    /// <summary>
    /// Permet de connaitre la presque égalité de deux vector sur un seul axe
    /// </summary>
    /// <param name="v1"></param>
    /// <param name="v2"></param>
    /// <param name="precision"></param>
    /// <param name="axis"></param>
    /// <returns></returns>
    public static bool AlmostEqualOnOneAxis(Vector3 v1, Vector3 v2, float precision, Axis axis)
    {
        bool equal = true;

        switch (axis)
        {
            case Axis.X:
                if (Mathf.Abs(v1.x - v2.x) > precision) equal = false;
                break;
            case Axis.Y:
                if (Mathf.Abs(v1.y - v2.y) > precision) equal = false;
                break;
            case Axis.Z:
                if (Mathf.Abs(v1.z - v2.z) > precision) equal = false;
                break;
            default:
                if (Mathf.Abs(v1.x - v2.x) > precision) equal = false;
                break;
        }

        return equal;
    }

    public static int LayerMaskToLayerIndex(LayerMask layerMask)
    {
        int layerNumber = 0;
        int layer = layerMask.value;

        while (layer > 0)
        {
            layer = layer >> 1;
            layerNumber++;
        }

        return layerNumber - 1;
    }

    /// <summary>
    /// Method used to return a number clamped between a min and max value, based on where a variable interpolates between her min and max value.
    /// </summary>
    /// <param name="minValueReturn">The minimum of the returned value.</param>
    /// <param name="maxValueReturn">The maximum of the returned value.</param>
    /// <param name="minValueToCheck">The minimum of the value we're checking.</param>
    /// <param name="maxValueToCheck">The maximum of the value we're checking.</param>
    /// <param name="valueToCheck">The value we're checking.</param>
    public static float Interpolate(float minValueReturn, float maxValueReturn, float minValueToCheck, float maxValueToCheck, float valueToCheck)
    {
        return Mathf.Lerp(minValueReturn, maxValueReturn, Mathf.InverseLerp(minValueToCheck, maxValueToCheck, valueToCheck));
    }

    /// <summary>
    /// Method used to return a number clamped between a min and max value, based on where a variable interpolates between her min and max value.
    /// </summary>
    /// <param name="minValueReturn">The minimum of the returned value.</param>
    /// <param name="maxValueReturn">The maximum of the returned value.</param>
    /// <param name="minValueToCheck">The minimum of the value we're checking.</param>
    /// <param name="maxValueToCheck">The maximum of the value we're checking.</param>
    /// <param name="valueToCheck">The value we're checking.</param>
    public static Vector2 InterpolateVector2(Vector2 minValueReturn, Vector2 maxValueReturn, Vector2 minValueToCheck, Vector2 maxValueToCheck, Vector2 valueToCheck)
    {
        float x = Mathf.Lerp(minValueReturn.x, maxValueReturn.x, Mathf.InverseLerp(minValueToCheck.x, maxValueToCheck.x, valueToCheck.x));
        float y = Mathf.Lerp(minValueReturn.y, maxValueReturn.y, Mathf.InverseLerp(minValueToCheck.y, maxValueToCheck.y, valueToCheck.y));
        return new Vector2(x, y);
    }

    public static Vector3 MultiplyTwoVectors(Vector3 a, Vector3 b)
    {
        float x = a.x * b.x;
        float y = a.y * b.y;
        float z = a.z * b.z;

        return new Vector3(x, y, z);
    }

    public static Color GetColorWithAlpha(Color color, float alpha)
    {
        alpha = Mathf.Clamp(alpha, 0, 1);
        Color newColor = new Color(color.r, color.g, color.b, alpha);
        return newColor;
    }

    public static void DrawCube(Vector3 position, Quaternion rotation, Vector3 scale)
    {
        Matrix4x4 cubeTransform = Matrix4x4.TRS(position, rotation, scale);
        Matrix4x4 oldGizmosMatrix = Gizmos.matrix;

        Gizmos.matrix *= cubeTransform;

        Gizmos.DrawCube(Vector3.zero, Vector3.one);

        Gizmos.matrix = oldGizmosMatrix;
    }

    /// <summary>
    /// Method to find the closest number to n and divisible by m
    /// </summary>
    /// <param name="n"></param>
    /// <param name="m"></param>
    /// <returns></returns>
    public static int ClosestNumber(int n, int m)
    {
        // find the quotient 
        int q = n / m;

        // 1st possible closest number 
        int n1 = m * q;

        // 2nd possible closest number 
        int n2 = (n * m) > 0 ? (m * (q + 1)) : (m * (q - 1));

        // if true, then n1 is the required closest number 
        if (Mathf.Abs(n - n1) < Mathf.Abs(n - n2))
            return n1;

        // else n2 is the required closest number 
        return n2;
    }

    public static Vector3 GetDirection(Vector3 origin, Vector3 destination)
    {
        Vector3 direction = destination - origin;
        return direction;
    }

    public static Vector3 GetNormalizedDirection(Vector3 origin, Vector3 destination)
    {
        Vector3 direction = destination - origin;
        return direction.normalized;
    }

    public static bool IsVisibleFrom(Renderer renderer, Camera camera)
    {
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(camera);
        return GeometryUtility.TestPlanesAABB(planes, renderer.bounds);
    }

    public static void DrawDebugSphere(Vector3 p_sphereCenter, float p_sphereRadius, Color p_sphereColor, float p_sphereAlpha = 0.45f, float p_wireAlpha = 1)
    {
        Gizmos.color = GetColorWithAlpha(p_sphereColor, p_sphereAlpha);
        Gizmos.DrawSphere(p_sphereCenter, p_sphereRadius);
        Gizmos.color = GetColorWithAlpha(p_sphereColor, p_wireAlpha);
        Gizmos.DrawWireSphere(p_sphereCenter, p_sphereRadius);
    }

    public static void DrawDebugCube(Vector3 p_cubeCenter, Vector3 p_cubeSize, Color p_color, float p_meshAlpha = 0.45f, float p_wireAlpha = 1)
    {
        Gizmos.color = GetColorWithAlpha(p_color, p_meshAlpha);
        Gizmos.DrawCube(p_cubeCenter, p_cubeSize);
        Gizmos.color = GetColorWithAlpha(p_color, p_wireAlpha);
        Gizmos.DrawWireCube(p_cubeCenter, p_cubeSize);
    }

    /// <summary>
    /// Quick method to get a random float between 0.0f and 1.0f
    /// </summary>
    /// <returns>Float between 0.0f and 1.0f</returns>
    public static float GetRandomPercent()
    {
        return Random.Range(0.0f, 1.0f);
    }

    /// <summary>
    /// Convert a float to a timer string
    /// </summary>
    /// <param name="toConvert">the value to convert</param>
    /// <param name="format">the format of the desired string (ex : "0:00.00")</param>
    /// <returns></returns>
    public static string FloatToTime(float toConvert, string format)
    {
        switch (format)
        {
            case "00.0":
                return string.Format("{0:00}:{1:0}",
                    Mathf.Floor(toConvert) % 60,//seconds
                    Mathf.Floor((toConvert * 10) % 10));//miliseconds
                break;
            case "#0.0":
                return string.Format("{0:#0}:{1:0}",
                    Mathf.Floor(toConvert) % 60,//seconds
                    Mathf.Floor((toConvert * 10) % 10));//miliseconds
                break;
            case "00.00":
                return string.Format("{0:00}:{1:00}",
                    Mathf.Floor(toConvert) % 60,//seconds
                    Mathf.Floor((toConvert * 100) % 100));//miliseconds
                break;
            case "00.000":
                return string.Format("{0:00}:{1:000}",
                    Mathf.Floor(toConvert) % 60,//seconds
                    Mathf.Floor((toConvert * 1000) % 1000));//miliseconds
                break;
            case "#00.000":
                return string.Format("{0:#00}:{1:000}",
                    Mathf.Floor(toConvert) % 60,//seconds
                    Mathf.Floor((toConvert * 1000) % 1000));//miliseconds
                break;
            case "#0:00":
                return string.Format("{0:#0}:{1:00}",
                    Mathf.Floor(toConvert / 60),//minutes
                    Mathf.Floor(toConvert) % 60);//seconds
                break;
            case "#00:00":
                return string.Format("{0:#00}:{1:00}",
                    Mathf.Floor(toConvert / 60),//minutes
                    Mathf.Floor(toConvert) % 60);//seconds
                break;
            case "0:00.0":
                return string.Format("{0:0}:{1:00}.{2:0}",
                    Mathf.Floor(toConvert / 60),//minutes
                    Mathf.Floor(toConvert) % 60,//seconds
                    Mathf.Floor((toConvert * 10) % 10));//miliseconds
                break;
            case "#0:00.0":
                return string.Format("{0:#0}:{1:00}.{2:0}",
                    Mathf.Floor(toConvert / 60),//minutes
                    Mathf.Floor(toConvert) % 60,//seconds
                    Mathf.Floor((toConvert * 10) % 10));//miliseconds
                break;
            case "0:00.00":
                return string.Format("{0:0}:{1:00}.{2:00}",
                    Mathf.Floor(toConvert / 60),//minutes
                    Mathf.Floor(toConvert) % 60,//seconds
                    Mathf.Floor((toConvert * 100) % 100));//miliseconds
                break;
            case "#0:00.00":
                return string.Format("{0:#0}:{1:00}.{2:00}",
                    Mathf.Floor(toConvert / 60),//minutes
                    Mathf.Floor(toConvert) % 60,//seconds
                    Mathf.Floor((toConvert * 100) % 100));//miliseconds
                break;
            case "0:00.000":
                return string.Format("{0:0}:{1:00}.{2:000}",
                    Mathf.Floor(toConvert / 60),//minutes
                    Mathf.Floor(toConvert) % 60,//seconds
                    Mathf.Floor((toConvert * 1000) % 1000));//miliseconds
                break;
            case "#0:00.000":
                return string.Format("{0:#0}:{1:00}.{2:000}",
                    Mathf.Floor(toConvert / 60),//minutes
                    Mathf.Floor(toConvert) % 60,//seconds
                    Mathf.Floor((toConvert * 1000) % 1000));//miliseconds
                break;
        }
        return "error";
    }

    /// <summary>
    /// How to use it ? Exemple : 
    /// IEnumarator m_count = CustomMethod.Count(m_currentValue, p_targetValue, m_incrementDuration, m_incrementCurve,l_returnValue =>
    ///  {
    ///        UpdateText(l_returnValue);
    ///        m_currentValue = l_returnValue;
    ///  }); 
    /// </summary>
    /// <param name="p_valueToUpdate"></param>
    /// <param name="p_targetValue"></param>
    /// <param name="p_duration"></param>
    /// <param name="p_valueEvolutionCurve"></param>
    /// <param name="callback"></param>
    /// <returns></returns>
    public static IEnumerator Count(float p_valueToUpdate, float p_targetValue, float p_duration, AnimationCurve p_valueEvolutionCurve, System.Action<float> callback)
    {
        float l_startValue = p_valueToUpdate;

        for (float l_timer = 0; l_timer < p_duration; l_timer += Time.deltaTime)
        {
            float l_progress = l_timer / p_duration;
            p_valueToUpdate = Mathf.Lerp(l_startValue, p_targetValue, p_valueEvolutionCurve.Evaluate(l_progress));
            callback(p_valueToUpdate);
            yield return p_valueToUpdate;
        }
        p_valueToUpdate = p_targetValue;
        callback(p_valueToUpdate);
        yield return p_valueToUpdate;
    }

    public static bool IsBetween(float p_valueToCheck,float p_minRange,float p_maxRange)
    {
        return p_valueToCheck > p_minRange && p_valueToCheck < p_maxRange;
    }

    public static bool IsBetweenOrEqual(float p_valueToCheck, float p_minRange, float p_maxRange)
    {
        return p_valueToCheck >= p_minRange && p_valueToCheck <= p_maxRange;
    }
}

