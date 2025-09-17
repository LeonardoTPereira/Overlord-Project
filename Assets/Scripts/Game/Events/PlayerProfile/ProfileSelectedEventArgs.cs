using Overlord.ProfileAnalyst;
using System;

namespace Game.Events
{
    public delegate void ProfileSelectedEvent(object sender, ProfileSelectedEventArgs e);

    public class ProfileSelectedEventArgs : EventArgs
    {
        private YeePlayerProfile playerProfile;

        public ProfileSelectedEventArgs(YeePlayerProfile playerProfile)
        {
            PlayerProfile = playerProfile;
        }

        public YeePlayerProfile PlayerProfile { get => playerProfile; set => playerProfile = value; }
    }
}