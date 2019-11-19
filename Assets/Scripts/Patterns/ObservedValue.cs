using System;


public class ObservedValue<T>
{
  private T cur_value;
  public  T last_value { get; private set; }

  public event Action<T> onValueChange;


  public ObservedValue( T init_value )
  {
    cur_value = init_value;
  }
  
  public ObservedValue()
  {
    cur_value = default;
  }

  public T Value
  {
    get => cur_value;
    set
    {
      if ( cur_value.Equals( value ) )
        return;

      last_value = cur_value;
      cur_value = value;

      onValueChange?.Invoke( cur_value );
    }
  }

  public void setSilently( T value )
  {
    cur_value = value;
  }
}