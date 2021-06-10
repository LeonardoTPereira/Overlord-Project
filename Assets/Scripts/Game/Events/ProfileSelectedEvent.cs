using System;
using static Enums;

public delegate void ProfileSelectedEvent(object sender, ProfileSelectedEventArgs e);

public class ProfileSelectedEventArgs : EventArgs
{
    private PlayerProfileEnum playerProfile;

    public ProfileSelectedEventArgs(PlayerProfileEnum playerProfile)
    {
        PlayerProfile = playerProfile;
    }

    public PlayerProfileEnum PlayerProfile { get => playerProfile; set => playerProfile = value; }
}