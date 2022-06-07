using L7Games.Movement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace L7Games
{
    public class SelectLeaderBoardTrigger : MonoBehaviour, ITriggerable
    {
        public GameObject selectableToSelect;
        public EventSystem eventSystemsInScene;

        public GameObject ReturnGameObject()
        {
            return gameObject;
        }

        public void Trigger(PlayerController player)
        {
            Debug.Log(gameObject.name + " is triggered");
            StartCoroutine(WaitAndSetSelected());
        }

        public void UnTrigger(PlayerController player)
        {
            
        }

        public IEnumerator WaitAndSetSelected()
        {
            Debug.Log(gameObject.name + " is co start");
            yield return new WaitUntil(SelectableEnabled);
            eventSystemsInScene.SetSelectedGameObject(selectableToSelect.gameObject);
        }

        public bool SelectableEnabled()
        {
            Debug.Log(selectableToSelect.name + " is Selected");
            return selectableToSelect.gameObject.activeInHierarchy;
        }
    }
}
