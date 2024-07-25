using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class HoverButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler 
{
    public Transform border;
    private float opacity = 0;

    public void OnPointerEnter(PointerEventData eventData)
    {
        AudioManager.Instance.SoundEffect("ButtonHover");
        opacity = 0.7f;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        opacity = 0f;
    }

    void Update()
    {
        Image image = border.GetComponent<Image>();
        var tempColor = image.color;
        tempColor.a = Mathf.Lerp(tempColor.a, opacity, 20f * Time.deltaTime);
        image.color = tempColor;
    }
}
