using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiMenuButtonPanel : MonoBehaviour
{
    [SerializeField] RectTransform[] buttonRectTransform;
    [SerializeField] RectTransform[] panelRectTransform;
    [SerializeField] Vector2[] panelVector2;
    [SerializeField] Vector2[] buttonVector2;

    float duration = 5f;



    private void Awake()
    {
        buttonVector2 = new Vector2[buttonRectTransform.Length];
        panelVector2 = new Vector2[panelRectTransform.Length];

        for (int i = 0; i < buttonRectTransform.Length; i++)
        {
            buttonVector2[i] = buttonRectTransform[i].sizeDelta;
        }

        for (int i = 0; i < panelRectTransform.Length; i++)
        {
            panelVector2[i] = panelRectTransform[i].sizeDelta;
        }
    }

    enum State { On, Off }
    State currentState = State.Off;

    public void CreditsButtons(int i)
    {

        SetRectParameters(i);
    }

    public void OptionsButton(int i)
    {

        SetRectParameters(i);
    }

    void SetRectParameters(int i)
    {
        int number = i;
        if (currentState == State.On)
        {

            currentState = State.Off;
            AnimatePanel(panelVector2[i], buttonVector2[i], number);

        }
        else if (currentState == State.Off)
        {
            currentState = State.On;
            AnimatePanel(buttonVector2[i], panelVector2[i], number);
        }

    }

    private void AnimatePanel(Vector2 startVector2, Vector2 targetVector2, int number)
    {
        buttonRectTransform[number].sizeDelta = targetVector2;
    }


}

