using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TouchInputButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] InputType inputType;
    public void OnPointerDown(PointerEventData pointer) {
        InputManager.Instance.ActiveInputs[inputType] = true;
    }

    public void OnPointerUp(PointerEventData pointer) {
        InputManager.Instance.ActiveInputs[inputType] = false;
    }
}
