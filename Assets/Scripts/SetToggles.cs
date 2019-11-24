using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetToggles : MonoBehaviour
{
  private List<int> cur_combo = new List<int>();
  
  public void onToggleChange( int id_tog )
  {
    cur_combo.Add( id_tog );

    if ( cur_combo.Count >= 4 )
    {
      reset();
    }
  }

  private void reset()
  {
    cur_combo.Clear();
  }
}
