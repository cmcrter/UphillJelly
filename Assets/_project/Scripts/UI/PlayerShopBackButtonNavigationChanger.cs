using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerShopBackButtonNavigationChanger : MonoBehaviour
{
    public Selectable hatNavItem;
    public Selectable skinNavItem;
    public Selectable BoardNavItem;

    public Button buttonToChange;

    private Navigation currentNaviagtion;

    private void Start()
    {
        currentNaviagtion = new Navigation();
        currentNaviagtion.mode = Navigation.Mode.Explicit;
        currentNaviagtion.selectOnDown = buttonToChange.navigation.selectOnDown;
        currentNaviagtion.selectOnUp = buttonToChange.navigation.selectOnUp;
        currentNaviagtion.selectOnLeft = buttonToChange.navigation.selectOnLeft;
        currentNaviagtion.selectOnRight = buttonToChange.navigation.selectOnRight;
    }

    public void Update()
    {
        if (hatNavItem.gameObject.activeInHierarchy)
        {
            currentNaviagtion.selectOnDown = hatNavItem;
            buttonToChange.navigation = currentNaviagtion;
        }
        else if (skinNavItem.gameObject.activeInHierarchy)
        {
            currentNaviagtion.selectOnDown = skinNavItem;
            buttonToChange.navigation = currentNaviagtion;
        }
        else if (BoardNavItem.gameObject.activeInHierarchy)
        {
            currentNaviagtion.selectOnDown = BoardNavItem;
            buttonToChange.navigation = currentNaviagtion;
        }
    }
}
