////////////////////////////////////////////////////////////
// File: LoadingScreenUI.cs
// Author: Charles Carter
// Date Created: 04/02/22
// Last Edited By: Charles Carter
// Date Last Edited: 04/02/22
// Brief: A script to control the UI during the loading screen
//////////////////////////////////////////////////////////// 

using TMPro;
using UnityEngine;

namespace L7.Loading
{
    public class LoadingScreenUI : MonoBehaviour
    {
        #region Variables

        [SerializeField]
        GameObject CenterCircle;
        [SerializeField]
        GameObject SmallerCircle;

        [SerializeField]
        GameObject pressAnythingText;

        private float circleDist;
        private float RotateSpeed = 5f;
        private float _angle;

        #endregion

        #region Unity Methods

        void Start()
        {
            circleDist = Vector2.Distance(SmallerCircle.transform.position, CenterCircle.transform.position);
        }

        void Update()
        {
            _angle += RotateSpeed * Time.deltaTime;

            var offset = new Vector3(Mathf.Sin(_angle), Mathf.Cos(_angle), 0) * circleDist;
            SmallerCircle.transform.position = CenterCircle.transform.position + offset;
        }

        #endregion

        #region Public Methods

        public void TurnOnPressText()
        {
            pressAnythingText.SetActive(true);
        }

        #endregion

        #region Private Methods


        #endregion
    }
}