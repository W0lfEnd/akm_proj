using System;
using UnityEngine;
using UnityEngine.UI;


public class SpaceShipPlacesManager : MonoBehaviour
{
  [SerializeField] private Button[] btn_places;

  private void Start()
  {
    setAllPlacesGreen();
  }


  public void changePlace( int idx_of_place )
  {
    Debug.Log( $"Ви сіли на {idx_of_place} місце" );

    changeColorPlace( idx_of_place );
    gameObject.SetActive( false );// Тушим панель
  }

  private void changeColorPlace( int idx_of_place )
  {
    setAllPlacesGreen();
    btn_places[idx_of_place].GetComponent<Image>().color = Color.black;
  }

  private void setAllPlacesGreen()
  {
    foreach ( Button btn in btn_places )
      btn.GetComponent<Image>().color = Color.green;
  }
}
