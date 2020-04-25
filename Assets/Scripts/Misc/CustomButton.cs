using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.UI;
public class CustomButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler
{


    public Graphic[] graphics = new Graphic[0];

    public UnityEvent enter = new UnityEvent();
    public UnityEvent leftClick = new UnityEvent();
    public UnityEvent rightClick = new UnityEvent();
    public UnityEvent down  = new UnityEvent();
    public UnityEvent up    = new UnityEvent();
    public UnityEvent exit  = new UnityEvent();

    
    public bool hover = false;

    public void OnPointerEnter(PointerEventData eventData)
    {
        hover = true;
        enter.Invoke();
        
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            leftClick.Invoke();
        }
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            rightClick.Invoke();
        }

    }

    public void OnPointerDown(PointerEventData eventData)
    {
        down.Invoke();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        up.Invoke();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        hover = false;
        exit.Invoke();
    }



}
