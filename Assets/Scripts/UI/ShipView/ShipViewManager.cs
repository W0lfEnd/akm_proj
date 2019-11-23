using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipViewManager : MonoBehaviour
{
  [Header("Parallax")]
  [SerializeField] private Transform controller = null;
  [SerializeField] private Transform targe_test = null;
  [SerializeField] private RectTransform[] backgrounds;
  [SerializeField] public float speed_parallax = 1.0f;

  private Vector2 size_background;

  void Start()
  {
    size_background = backgrounds[0].sizeDelta;
    setRotation( targe_test );
  }

  void Update()
  {
    parallaxEffext();
  }

  void parallaxEffext()
  {
    slowMoveBackground();
  }

  private void slowMoveBackground()
  {
    if ( Client.client != null && Client.client.Model != null )
      speed_parallax = Client.client.Model.speed.Value / 10f;

    foreach ( RectTransform back in backgrounds )
    {
      back.Translate( speed_parallax * Time.deltaTime * Vector2.left );

      if ( back.localPosition.x < -1600 )
        back.localPosition = new Vector2( back.localPosition.x + (size_background.x * 2), back.localPosition.y );
    }
  }

  public void setRotation( Transform target )
  {
    Vector3 vectorToTarget = target.position - controller.position;
    float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;
    Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
    transform.rotation = q;
  }
}
