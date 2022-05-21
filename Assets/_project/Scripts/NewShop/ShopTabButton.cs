////////////////////////////////////////////////////////////
// File: ShopTabButton.cs
// Author: Jack Peedle
// Date Created: 10/04/22
// Last Edited By: Jack Peedle
// Date Last Edited: 10/04/22
// Brief: 
//////////////////////////////////////////////////////////// 

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace SleepyCat
{
    [RequireComponent(typeof(Image))]
    public class ShopTabButton : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler //IPointerDownHandler
    {
        public TabGroup tabGroup;

        public Image backgroundTabImage;

        // Start is called before the first frame update
        void Start() {

            backgroundTabImage = GetComponent<Image>();
            tabGroup.Subscribe(this);

        }
        
        public void OnPointerClick(PointerEventData eventData) {
            tabGroup.OnTabSelected(this);
        }

        public void OnPointerEnter(PointerEventData eventData) {
            tabGroup.OnTabHover(this);
        }

        public void OnPointerExit(PointerEventData eventData) {
            tabGroup.OnTabExit(this);
        }
        

        /*
        public void OnPointerEnter(PointerEventData eventData) {
            //Output the name of the GameObject that is being clicked
            Debug.Log(name + "Game Object Click in Progress");
            tabGroup.OnTabHover(this);
        }

        //Detect current clicks on the GameObject (the one with the script attached)
        public void OnPointerDown(PointerEventData pointerEventData) {

            //Output the name of the GameObject that is being clicked
            Debug.Log(name + "Game Object Click in Progress");
            tabGroup.OnTabSelected(this);
        }

        //Detect if clicks are no longer registering
        public void OnPointerUp(PointerEventData pointerEventData) {

            Debug.Log(name + "No longer being clicked");
            tabGroup.OnTabExit(this);
        }
        */
    }

    [SerializeField]
    [RequireComponent(typeof(Image))]
    public class ShopItemButton : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler //IPointerDownHandler
    {
        public ShopItemGroup shopItemGroup;

        //public Image backgroundTabImage;

        // Start is called before the first frame update
        void Start() {

            //backgroundTabImage = GetComponent<Image>();
            shopItemGroup.Subscribe(this);

        }

        public void OnPointerClick(PointerEventData eventData) {
            shopItemGroup.OnTabSelected(this);
        }

        public void OnPointerEnter(PointerEventData eventData) {
            shopItemGroup.OnTabHover(this);
        }

        public void OnPointerExit(PointerEventData eventData) {
            shopItemGroup.OnTabExit(this);
        }

    }





}
