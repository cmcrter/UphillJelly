using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.InputSystem;

[RequireComponent(typeof(ScrollRect))]
public class ScrollRectAutoScroll : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public float scrollSpeed = 10f;

    public int expectedColumns = 6;

    private bool mouseOver = false;

    private List<Selectable> m_Selectables = new List<Selectable>();
    private ScrollRect m_ScrollRect;

    private Vector2 m_NextScrollPosition = Vector2.up;



    private GameObject lastSelectedGameObject;
    void OnEnable() {
        if (m_ScrollRect)
        {
            m_ScrollRect.content.GetComponentsInChildren(false, m_Selectables);
        }
    }
    void Awake() {
        m_ScrollRect = GetComponent<ScrollRect>();
    }
    void Start() {
        if (m_ScrollRect)
        {
            m_ScrollRect.content.GetComponentsInChildren(false, m_Selectables);
        }
        ScrollToSelected(true);
    }
    void Update() {
        // If we are on mobile and we do not have a gamepad connected, do not do anything.
        if (SystemInfo.deviceType == DeviceType.Handheld && Gamepad.all.Count <= 1) {
            return;
        }

        // Scroll via input.
        //InputScroll();
        if (lastSelectedGameObject != EventSystem.current.currentSelectedGameObject)
        {
            if (m_ScrollRect)
            {
                m_ScrollRect.content.GetComponentsInChildren(false, m_Selectables);
            }
            ScrollToSelected(true);
        }

        if (!mouseOver) {
            // Lerp scrolling code.
            m_ScrollRect.normalizedPosition = Vector2.Lerp(m_ScrollRect.normalizedPosition, m_NextScrollPosition, Mathf.Clamp01(scrollSpeed * Time.unscaledDeltaTime));
        } else {
            m_NextScrollPosition = m_ScrollRect.normalizedPosition;
        }
        lastSelectedGameObject = EventSystem.current.currentSelectedGameObject ? EventSystem.current.currentSelectedGameObject : null;

    }

#nullable enable
    void InputScroll() {
        if (m_ScrollRect)
        {
            m_ScrollRect.content.GetComponentsInChildren(false, m_Selectables);
        }
        if (m_Selectables.Count > 0) {
            Keyboard? currentKeyboard = Keyboard.current;
            Gamepad? currentGamepad = Gamepad.current;

            if (currentKeyboard != null) {
                if (Keyboard.current.upArrowKey.wasPressedThisFrame || Keyboard.current.downArrowKey.wasPressedThisFrame) {
                    ScrollToSelected(false);
                }
            }

            if (currentGamepad != null) {
                if (Gamepad.current.dpad.up.wasPressedThisFrame || Gamepad.current.dpad.down.wasPressedThisFrame) {
                    ScrollToSelected(false);
                }
            }
        }
    }
#nullable disable
    void ScrollToSelected(bool quickScroll) {
        if (m_Selectables.Count <= 0)
        {
            m_ScrollRect.normalizedPosition = new Vector2(0, 1f);
            m_NextScrollPosition = m_ScrollRect.normalizedPosition;
        }

        int selectedIndex = -1;
        Selectable selectedElement = EventSystem.current.currentSelectedGameObject ? EventSystem.current.currentSelectedGameObject.GetComponent<Selectable>() : null;

        if (selectedElement) {
            selectedIndex = m_Selectables.IndexOf(selectedElement);
        }


        if (selectedIndex > -1) {
            int numberOfRows = Mathf.CeilToInt(((float)m_Selectables.Count) / (float)expectedColumns);
            if (m_ScrollRect.verticalScrollbar != null)
            {
                m_ScrollRect.verticalScrollbar.numberOfSteps = numberOfRows;
            }

            int currentRow = Mathf.FloorToInt((float)selectedIndex / (float)expectedColumns);
            Debug.Log("Number Of Rows: " + numberOfRows, this);
            float newSelectionYPos = 1f - ((float)currentRow / ((float)numberOfRows - 1f));

            if (quickScroll) {
                m_ScrollRect.normalizedPosition = new Vector2(0, newSelectionYPos);
                m_NextScrollPosition = m_ScrollRect.normalizedPosition;
            } else {
                m_NextScrollPosition = new Vector2(0, newSelectionYPos);
            }
        }
    }
    public void OnPointerEnter(PointerEventData eventData) {
        mouseOver = true;
    }
    public void OnPointerExit(PointerEventData eventData) {
        mouseOver = false;
        ScrollToSelected(false);
    }
}