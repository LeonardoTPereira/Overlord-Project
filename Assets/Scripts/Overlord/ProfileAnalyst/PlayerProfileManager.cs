using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Overlord.ProfileAnalyst
{
    public class PlayerProfileManager : MonoBehaviour
    {
        public static event Action<IPlayerProfile> ProfileSelected;

        protected virtual void InvokeEventOnSelectedProfile(IPlayerProfile profile)
        {
            ProfileSelected?.Invoke(profile);
        }
    }
}