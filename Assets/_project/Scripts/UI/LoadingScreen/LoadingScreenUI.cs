////////////////////////////////////////////////////////////
// File: LoadingScreenUI.cs
// Author: Charles Carter
// Date Created: 04/02/22
// Last Edited By: Charles Carter
// Date Last Edited: 13/04/22
// Brief: A script to control the UI during the loading screen
//////////////////////////////////////////////////////////// 

using System.Collections.Generic;
using TMPro;
using UnityEngine;
using L7Games;
using System.Collections;
using UnityEngine.UI;

namespace L7Games.Loading
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

        [SerializeField]
        private TextMeshProUGUI tooltipText;
        [SerializeField]
        private Image tooltipImage;

        [SerializeField]
        private List<ScriptableTooltips> toolTips = new List<ScriptableTooltips>();
        private ScriptableTooltips currentTooltip;

        [SerializeField]
        private float TooltipFadeTime = 1f;
        [SerializeField]
        private float TooltipStayTime = 5f;

        private float weighting = 0f;

        #endregion

        #region Unity Methods

        void Start()
        {
            circleDist = Vector2.Distance(SmallerCircle.transform.position, CenterCircle.transform.position);

            StartCoroutine(Co_TooltipDisplay());
        }

        void Update()
        {
            _angle += RotateSpeed * Time.deltaTime;

            var offset = new Vector3(Mathf.Sin(_angle), Mathf.Cos(_angle), 0) * circleDist;
            SmallerCircle.transform.position = CenterCircle.transform.position + offset;
            SmallerCircle.transform.Rotate(0, 0, (RotateSpeed * Time.deltaTime * 20f) *-1f);
        }

        #endregion

        #region Public Methods

        public void TurnOnPressText()
        {
            pressAnythingText.SetActive(true);
        }

        public void TurnOffPressText()
        {
            pressAnythingText.SetActive(false);
        }

        #endregion

        #region Private Methods

        private void SelectNewTooltip()
        {
            if(currentTooltip)
            {
                toolTips.Add(currentTooltip);
            }

            currentTooltip = GetRandom();
            toolTips.Remove(currentTooltip);

            tooltipText.text = currentTooltip.text;

            if(currentTooltip.texture != null)
            {
                float boundsX = Screen.width * 0.25f;
                float randScreenPos = Random.Range(-boundsX, boundsX);

                tooltipImage.gameObject.SetActive(true);
                tooltipImage.transform.localPosition = new Vector3(randScreenPos, tooltipImage.transform.localPosition.y, 0);
                tooltipImage.sprite = currentTooltip.texture;
            }
            else
            {
                tooltipImage.gameObject.SetActive(true);
            }
        }

        private ScriptableTooltips GetRandom()
        {
            if(toolTips.Count == 1)
            {
                return toolTips[0];
            }

            foreach (ScriptableTooltips tooltip in toolTips)
            {
                float r = Random.Range(0f, 1f) * weighting;

                if(tooltip.priority >= r)
                {
                    return tooltip;
                }

                weighting += tooltip.priority;
            }

            return toolTips[0];
        }

        private IEnumerator Co_TooltipDisplay()
        {
            while(enabled)
            {
                yield return Co_FadeTooltipIn();

                yield return new WaitForSeconds(TooltipStayTime);

                yield return Co_FadeTooltipOut();
            }
        }

        private IEnumerator Co_FadeTooltipIn()
        {
            //Select new tooltip and change 
            SelectNewTooltip();

            //Loop to fade in text / image
            for(float t = 0; t <= 1; t += Time.deltaTime / TooltipFadeTime)
            {
                tooltipText.alpha = t;

                if(currentTooltip.texture != null)
                {
                    tooltipImage.color = new Color(tooltipImage.color.r, tooltipImage.color.g, tooltipImage.color.b, t);
                }

                yield return null;
            }
        }

        private IEnumerator Co_FadeTooltipOut()
        {
            //Loop to fade out text / image
            for(float t = 1; t >= 0; t -= Time.deltaTime / TooltipFadeTime)
            {
                tooltipText.alpha = t;

                if(currentTooltip.texture != null)
                {
                    tooltipImage.color = new Color(tooltipImage.color.r, tooltipImage.color.g, tooltipImage.color.b, t);
                }

                yield return null;
            }
        }

        #endregion
    }
}