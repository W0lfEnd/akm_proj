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
  public Vector2Int pos { get; private set; }

  private bool is_selected;
  public bool isSelected
  {
    get => is_selected;
    set
    {
      is_selected = value;
      img_point_background.color = is_selected ? Color.green : Color.red;
    }
  }

  protected override void initMyComponents()
  {
    locationID = ++locationsCounter;
  }

  public void init( Vector2Int pos, bool is_selected )
  {
    initComponents();

    this.pos = pos;
    isSelected = is_selected;
  }

  public void OnPointerClick( PointerEventData eventData )
  {
    onLocationClicked( this );
  }
}