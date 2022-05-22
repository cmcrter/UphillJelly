using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using L7Games.Triggerables;
using L7Games;
using L7Games.Movement;

public class TutoialPopUpTrigger : MonoBehaviour, ITriggerable
{
    [SerializeField]
    private GameObject uiPanelToSpawn;

    [SerializeField]
    private Canvas canvasToOverlay;

    public GameObject ReturnGameObject()
    {
        return this.ReturnGameObject();
    }

    public void Trigger(PlayerController player)
    {
        GameObject.Instantiate(uiPanelToSpawn, canvasToOverlay.transform);
        GameObject.Destroy(gameObject);
    }

    public void UnTrigger(PlayerController player)
    {
        throw new System.NotImplementedException();
    }
}
