using MiraAPI.Utilities;
using TownOfUs;
using UnityEngine;

namespace NullsMod;

public static class NullsColors
{
    // Crew Colors
    public static Color Micromanager => TownOfUsColors.UseBasic ? Palette.CrewmateBlue : new Color32(38, 104, 148, 255);
    public static Color Shackled => TownOfUsColors.UseBasic ? Palette.CrewmateBlue : new Color32(145, 155, 155, 255);
    public static Color Mortician => TownOfUsColors.UseBasic ? Palette.CrewmateBlue : new Color32(0, 68, 218, 255);


    // Neutral Colors
    public static Color Workaholic => new Color32(124, 142, 158, 255);
}