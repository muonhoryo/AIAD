using AIAD.SL;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace AIAD
{
    public sealed class MainMenuButton : MonoBehaviour,IPointerUpHandler,IPointerDownHandler,IPointerEnterHandler,IPointerExitHandler
    {
        [SerializeField] private Image ButtonImage;
        [SerializeField] private TextMeshProUGUI Text;

        [SerializeField] private Sprite NormalSprite;
        [SerializeField] private Sprite PressedSprite;

        [SerializeField] private Color NormalColor;
        [SerializeField] private Color SelectedColor;

        [SerializeField] private string SLScripts_Click;

        void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
        {
            ButtonImage.sprite = PressedSprite;
            if (!string.IsNullOrEmpty(SLScripts_Click))
                SLScripts_Click.RunSLScripts();
        }
        void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
        {
            ButtonImage.sprite = NormalSprite;
        }
        void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
        {
            Text.color = SelectedColor;
        }
        void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
        {
            Text.color = NormalColor;
        }
    }

}
