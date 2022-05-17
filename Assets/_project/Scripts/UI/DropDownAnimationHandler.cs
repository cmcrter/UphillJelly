using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DropDownAnimationHandler : MonoBehaviour
{
    [SerializeField]
    private Animator buttonAnimator;

    [SerializeField]
    private TMPro.TMP_Dropdown dropdown;

    [SerializeField]
    private int expetedChildCount = 0;

    private ScrollRect scrollRect;

    private EventSystem eventSystem;

    private bool isDroppedDown = false;

    private void Awake()
    {
        eventSystem = FindObjectOfType<EventSystem>();
    }

    private void Update()
    {
        if (expetedChildCount < dropdown.transform.childCount)
        {
            buttonAnimator.SetBool("DropDownOpened", true);
            isDroppedDown = true;
            // The last child will be the new drop down
            if (dropdown.transform.GetChild(dropdown.transform.childCount - 1).TryGetComponent<ScrollRect>(out ScrollRect newScrollRect))
            {
                scrollRect = newScrollRect;
            }
        }
        else
        {
            buttonAnimator.SetBool("DropDownOpened", false);
            isDroppedDown = false;
        }

        if (isDroppedDown)
        {
            SetPositonBasedOnSelectedItem();
        }
    }

    private void SetPositonBasedOnSelectedItem()
    {
        if (scrollRect == null)
        {
            Debug.LogError("Scroll Rect was not found on a drop down, dropped down list", this);
            return;
        }
        // Ignore the first child because it is a template 
        for (int i = 1; i < scrollRect.content.childCount; ++i)
        {
            if (scrollRect.content.GetChild(i).gameObject == eventSystem.currentSelectedGameObject)
            {
                Debug.Log(scrollRect.content.GetChild(i).GetComponent<RectTransform>().anchoredPosition.y / scrollRect.content.rect.height);
                RectTransform rectTransform = scrollRect.content.GetChild(i).GetComponent<RectTransform>();
                scrollRect.verticalNormalizedPosition = (rectTransform.anchoredPosition.y + rectTransform.rect.height) / scrollRect.content.rect.height;
                //scrollRect.verticalNormalizedPosition = 1f - ((float)i + 1f)/ ((float)scrollRect.content.childCount - 1f);
                //scrollRect.
                break;
            }
        }
    }
}
