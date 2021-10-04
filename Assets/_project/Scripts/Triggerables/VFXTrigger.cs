////////////////////////////////////////////////////////////
// File: VFXTrigger.cs
// Author: Charles Carter
// Date Created: 04/10/21
// Last Edited By: Charles Carter
// Date Last Edited: 04/10/21
// Brief: A triggerable to specifically activate VFX attached
//////////////////////////////////////////////////////////// 

using UnityEngine;
using System.Collections.Generic;

namespace SleepyCat.Triggerables
{
    public class VFXTrigger : MonoBehaviour, ITriggerable
    {
        #region Interface Contracts

        //The interfaces have specific functions that need to be fulfilled within this script and any child of the script
        GameObject ITriggerable.ReturnGameObject() => gameObject;

        void ITriggerable.Trigger() => ActivateVFX();
        void ITriggerable.UnTrigger() => DeActivateVFX();

        #endregion

        #region Variables

        public List<ParticleSystem> VFXToPlayOnEnter = new List<ParticleSystem>();
        public List<ParticleSystem> VFXToPlayOnExit = new List<ParticleSystem>();

        [SerializeField]
        private bool bTurnOffVFXonExit = false;

        #endregion

        #region Public Methods

        public void ActivateVFX()
        {
            //Playing all the VFXs selected in inspector
            foreach (ParticleSystem vfx in VFXToPlayOnEnter)
            {
                vfx.Play();
            }
        }

        public void DeActivateVFX()
        {
            if (bTurnOffVFXonExit)
            {
                //Stopping all the VFXs selected in inspector
                foreach (ParticleSystem vfx in VFXToPlayOnEnter)
                {
                    if (vfx.isPlaying)
                    {
                        vfx.Stop();
                    }
                }
            }

            //Playing all the VFXs selected in inspector
            foreach (ParticleSystem vfx in VFXToPlayOnExit)
            {
                vfx.Play();
            }
        }

        #endregion
    }
}