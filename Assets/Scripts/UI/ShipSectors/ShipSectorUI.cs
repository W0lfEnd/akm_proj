using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class ShipSectorUI : MonoBehaviourBase, IPointerClickHandler
{
  public event Action<Sector> onClick = delegate {};

  [SerializeField] private Image sector_filter_image;

  [SerializeField] private Transform fire_ui;
  //[SerializeField] private Image     sector_in_fire;
  [SerializeField] private Button    btn_turn_off_fire;

  [SerializeField] private Transform       repair_ui;
  [SerializeField] private Slider          hp_slider;
  [SerializeField] private TextMeshProUGUI hp_text;


  protected override void initMyComponents()
  {
    btn_turn_off_fire.onClick.AddListener( turnOffFire );
    hp_slider.minValue = 0;
    hp_slider.maxValue = 100;
  }

  private Sector sector;
  public Sector Sector => sector;

  public void init( Sector sector )
  {
    initComponents();

    this.sector = sector;
  }

  private void Update()
  {
    if ( was_inited_components )
      updateSectorUI();
  }

  public void OnPointerClick( PointerEventData eventData )
  {
    onClick( Sector );
  }

  private void turnOffFire()
  {
    Client.client.Send( new PlayerInput { actionType = PlayerInput.ActionType.FightFire, ownerId = Client.client.Id, sectorPosition = sector.position } );
  }

  private Color getSectorColor()
  {
    if( sector.health == 100 )
      return Color.clear;

    if ( sector.health >= 90 )
      return new Color( 0.04f, 1f, 0.02f, 0.53f );

    if ( sector.health >= 30 )
      return new Color( 1f, 0.76f, 0f, 0.53f );

    if ( sector.health > 0 )
      return new Color( 1f, 0f, 0.08f, 0.53f );

    return new Color( 0f, 0f, 0f, 0.73f );
  }

  public void updateSectorUI()
  {
    fire_ui.gameObject.SetActive( sector.isFire );
    repair_ui.gameObject.SetActive( sector.isRepairing );
    sector_filter_image.color = getSectorColor();
    hp_slider.value = sector.health;
    hp_text.text = sector.health + "%";
  }
}
