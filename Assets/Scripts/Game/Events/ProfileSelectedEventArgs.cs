using System;
using Game.NarrativeGenerator;

namespace Game.Events
{
    public delegate void ProfileSelectedEvent(object sender, ProfileSelectedEventArgs e);

    public class ProfileSelectedEventArgs : EventArgs
    {
        private PlayerProfile playerProfile;

        public ProfileSelectedEventArgs(PlayerProfile playerProfile)
        {
            PlayerProfile = playerProfile;
        }

        public PlayerProfile PlayerProfile { get => playerProfile; set => playerProfile = value; }
    }
}