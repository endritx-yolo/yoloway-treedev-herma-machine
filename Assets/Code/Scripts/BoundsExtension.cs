using UnityEngine;

public static class BoundsExtension
{
    public static Bounds GrowBounds( this Bounds a, Bounds b )
    {
       Vector3 max = Vector3.Max ( a.max, b.max );
       Vector3 min = Vector3.Min( a.min, b.min );
 
       a = new Bounds( (max + min)*0.5f, max - min );
       return a;
    }
}