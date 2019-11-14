using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipViewManager : MonoBehaviour
{
    [Header("Parallax")]
    [SerializeField] private RectTransform[] backgrounds;
    [SerializeField] private float speed_parallax = 1.0f;

    private Vector2 size_background;
    
    void Start()
    {
        size_background = backgrounds[0].sizeDelta;
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
        foreach ( RectTransform back in backgrounds )
        {
            back.Translate( speed_parallax * Time.deltaTime * Vector2.left );

            if ( back.localPosition.x < -1600 )
                back.localPosition = new Vector2(back.localPosition.x + (size_background.x * 2), back.localPosition.y );
        }
    }
}
