using UnityEngine;


public static class RotationHelper
{
  public static Quaternion lookAtIn2D( this Vector3 subject, Vector3 look_at )
  {
    Vector2 rotate_vector = look_at - subject;
    return Quaternion.Euler( 0, 0, Mathf.Atan2( rotate_vector.y, rotate_vector.x ) * Mathf.Rad2Deg );
  }
}