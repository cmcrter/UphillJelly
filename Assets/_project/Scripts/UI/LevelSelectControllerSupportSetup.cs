

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class LevelSelectControllerSupport
{
    public static void Setup(List<Toggle> levelSelectionToggles, Selectable listTopExitSelectable, Selectable listBottomExitSelectable)
    {
        List<Toggle> activelevelSelectionToggles = new List<Toggle>(levelSelectionToggles);
        for (int i = 0; i < activelevelSelectionToggles.Count; ++i)
        {
            if (!activelevelSelectionToggles[i].gameObject.activeInHierarchy)
            {
                activelevelSelectionToggles.RemoveAt(i);
                --i;
            }
        }

        if (activelevelSelectionToggles.Count == 1)
        {
            SetUpNaivationForSelectable(listBottomExitSelectable, activelevelSelectionToggles[0], listBottomExitSelectable.navigation.selectOnDown);
            SetUpNaivationForSelectable(listTopExitSelectable, listTopExitSelectable.navigation.selectOnUp, activelevelSelectionToggles[0]);

            if (activelevelSelectionToggles[0].gameObject.activeInHierarchy)
            {
                Navigation newNavigation = new Navigation();
                newNavigation.mode = Navigation.Mode.Explicit;
                activelevelSelectionToggles[0].navigation = newNavigation;
            }
        }
        else if (activelevelSelectionToggles.Count > 0)
        {
            SetUpNaivationForSelectable(listBottomExitSelectable, activelevelSelectionToggles[activelevelSelectionToggles.Count - 1], listBottomExitSelectable.navigation.selectOnDown);
            SetUpNaivationForSelectable(listTopExitSelectable, listTopExitSelectable.navigation.selectOnUp, activelevelSelectionToggles[0]);

            SetUpNaivationForSelectable(activelevelSelectionToggles[0], listTopExitSelectable, activelevelSelectionToggles[1]);
            for (int i = 1; i < activelevelSelectionToggles.Count - 1; ++i)
            {
                SetUpNaivationForSelectable(activelevelSelectionToggles[i], activelevelSelectionToggles[i - 1], activelevelSelectionToggles[i + 1]);
            }
            SetUpNaivationForSelectable(activelevelSelectionToggles[activelevelSelectionToggles.Count - 1], activelevelSelectionToggles[activelevelSelectionToggles.Count - 2], listBottomExitSelectable);
        }
    }

    private static void SetUpNaivationForSelectable(Selectable selectableToAlter, Selectable upSelectable, Selectable downSelectable)
    {
        Navigation newNavigation = new Navigation();
        newNavigation.mode = Navigation.Mode.Explicit;
        newNavigation.selectOnDown = downSelectable;
        newNavigation.selectOnUp = upSelectable;
        newNavigation.selectOnRight = selectableToAlter.navigation.selectOnRight;
        newNavigation.selectOnLeft = selectableToAlter.navigation.selectOnLeft;
        selectableToAlter.navigation = newNavigation;
    }
}
