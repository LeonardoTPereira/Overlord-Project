using System;
using MyBox;
using UnityEngine;

namespace Overlord.ProfileAnalyst
{
    public class PlayerProfileManager : MonoBehaviour
    {
        public bool GetRandomProfile = false;

        [DisplayInspector]
        public YeePlayerProfileSO playerProfileSO;

        public static event Action<IPlayerProfile> ProfileSelected;

        protected IPlayerProfileCalculator _profileCalculator = new YeeProfileCalculator();        // Change it with another player profile calculator if needed

        protected void SetPlayerProfileFromManualPlayerProfileSO()
        {
            var playerProfile = _profileCalculator.CreateProfileFromPlayerProfileSO(playerProfileSO);
            InvokeEventOnSelectedProfile(playerProfile);            
        }

        protected void SetRandomPlayerProfile()
        {

        }

        protected virtual void InvokeEventOnSelectedProfile(IPlayerProfile profile)
        {
            ProfileSelected?.Invoke(profile);
        }
    }
}