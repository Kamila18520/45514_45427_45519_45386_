using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class AnimationUIPanels : MonoBehaviour
{
    [SerializeField] RectTransform creditPanel;
    float panelHeight;
    float panelWidth;

    public void CreditsButton()
    {
        panelHeight = creditPanel.rect.height;
        panelWidth = creditPanel.rect.width;

        AnimatePanel(panelHeight, panelWidth);
    }

    private void AnimatePanel(float height, float width)
    {
        Debug.Log("Animated Credits Panel; " + height  +" : "+ width);
    }
  
}
