using System;
using MyBox;
using UnityEngine;

namespace Overlord.ProfileAnalyst
{
    public class PlayerProfileManager : MonoBehaviour
    {
        public static bool GetRandomProfile = false;

        [DisplayInspector]
        public YeePlayerProfileSO playerProfileSO;

        public static event Action<IPlayerProfile> ProfileSelected;

        // Change it with another player profile calculator if needed
        // However, for different calculators, you may need to override methods in this class
        // to adapt the input for Content Generators
        protected IPlayerProfileCalculator _profileCalculator = new YeeProfileCalculator();

        public void SetPlayerProfileFromManualPlayerProfileSO()
        {
            var playerProfile = _profileCalculator.CreateProfileFromPlayerProfileSO(playerProfileSO);
            InvokeEventOnSelectedProfile(playerProfile);            
        }

        public void SetRandomPlayerProfile()
        {
            var playerProfile = _profileCalculator.GetRandomPlayerProfile();
            InvokeEventOnSelectedProfile(playerProfile);
        }

        protected virtual void InvokeEventOnSelectedProfile(IPlayerProfile profile)
        {
            ProfileSelected?.Invoke(profile);
        }
    }
}