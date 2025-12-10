using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MoreMountains.NiceVibrations
{
    public class RegularPresetsDemoManager : DemoManager
    {

      

        public virtual void SelectionButton()
        {
            MMVibrationManager.Haptic(HapticTypes.Selection, false, true, this);
            
        }

        public virtual void SuccessButton()
        {
            MMVibrationManager.Haptic(HapticTypes.Success, false, true, this);
           
        }

        public virtual void WarningButton()
        {
            MMVibrationManager.Haptic(HapticTypes.Warning, false, true, this);
            
        }

        public virtual void FailureButton()
        {
            MMVibrationManager.Haptic(HapticTypes.Failure, false, true, this);
           
        }

        public virtual void RigidButton()
        {
            MMVibrationManager.Haptic(HapticTypes.RigidImpact, false, true, this);
           
        }

        public virtual void SoftButton()
        {
            MMVibrationManager.Haptic(HapticTypes.SoftImpact, false, true, this);
            
        }

        public virtual void LightButton()
        {
            MMVibrationManager.Haptic(HapticTypes.LightImpact, false, true, this);
        }

        public virtual void MediumButton()
        {
            MMVibrationManager.Haptic(HapticTypes.MediumImpact, false, true, this);
           
        }

        public virtual void HeavyButton()
        {
            MMVibrationManager.Haptic(HapticTypes.HeavyImpact, false, true, this);
           
        }
    }
}