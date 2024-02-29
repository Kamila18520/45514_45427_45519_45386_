using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiMenuButtonPanel : MonoBehaviour
{
    [SerializeField] RectTransform[] buttonRectTransform;
    [SerializeField] RectTransform[] panelRectTransform;
    [SerializeField] Vector2[] panelVector2;
    [SerializeField] Vector2[] buttonVector2;

    float duration = 5f;
    public bool isOpen= false;


    private void Awake()
    {
        buttonVector2 = new Vector2[buttonRectTransform.Length];
        panelVector2 = new Vector2[panelRectTransform.Length];

        for (int i = 0; i < buttonRectTransform.Length; i++)
        {
            buttonVector2[i] = buttonRectTransform[i].sizeDelta;
            panelVector2[i] = panelRectTransform[i].sizeDelta;
            panelRectTransform[i].transform.position = buttonRectTransform[i].transform.position;
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
            if(isOpen) 
            { 
            DebugEnabledPanels();
            }

            isOpen = false;
            currentState = State.Off;
          
            panelRectTransform[i].gameObject.SetActive(false);

        }
        else if (currentState == State.Off)
        {

            isOpen= true;
            currentState = State.On;
            
            panelRectTransform[i].gameObject.SetActive(true);

        }

    }

    private void DebugEnabledPanels()
    {
      
        for (int z = 0; z < panelRectTransform.Length; z++)
        {
            panelRectTransform[z].gameObject.SetActive(false);
        }
    }




}

