using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class MapSpaceLocationPointUI : MonoBehaviourBase, IPointerClickHandler
{
  public event Action<MapSpaceLocationPointUI> onLocationClicked = delegate { };

  private static int locationsCounter = 0;

  [SerializeField] private Image img_point_background;

  public int locationID { get; private set; }

  protected override void initMyComponents()
  {
    locationID = ++locationsCounter;
  }

  public void init( bool is_selected = false )
  {
    initComponents();

    img_point_background.color = is_selected ? Color.black : Color.red;
  }

  public void OnPointerClick( PointerEventData eventData )
  {
    onLocationClicked( this );
  }
}