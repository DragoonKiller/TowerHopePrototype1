using System;
using UnityEngine;

public static partial class Util
{
    public static void DebugDrawRect(Vector2 pos, Vector2 size, Color color, float duration = 0f)
        => DebugDrawRect(pos, size, color, duration, Quaternion.identity);
    
    public static void DebugDrawRect(Vector2 pos, Vector2 size, Color color, float duration, Quaternion rotation)
    {
        size /= 2.0f;
        Vector2 lb = pos + Vector2.left * size.x + Vector2.down * size.y;
        Vector2 rb = pos + Vector2.right * size.x + Vector2.down * size.y;
        Vector2 lt = pos + Vector2.left * size.x + Vector2.up * size.y;
        Vector2 rt = pos + Vector2.right * size.x + Vector2.up * size.y;
        Debug.DrawLine(lb, rb, color, duration);
        Debug.DrawLine(lt, rt, color, duration);
        Debug.DrawLine(lb, lt, color, duration);
        Debug.DrawLine(rb, rt, color, duration);
    }
    
    public static void DebugDrawCircle(Vector2 pos, float radius, int edgeCount, Color color, float duraction = 0f)
    {
        float angle = 2.0f * Mathf.PI / edgeCount;
        for(int i = 0; i < edgeCount; i++)
        {
            float curAngle = i * angle;
            Vector2 curPoint = Vector2.right.Rot(curAngle) * radius;
            Vector2 nxtPoint = Vector2.right.Rot(curAngle + angle) * radius;
            Debug.DrawLine(pos + curPoint, pos + nxtPoint, color, duraction);
        }
    }
}
