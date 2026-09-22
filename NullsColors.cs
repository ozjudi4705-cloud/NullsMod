using MiraAPI.Utilities;
using TownOfUs;
using UnityEngine;

namespace NullsMod;

public static class NullsColors
{
    public static Color Micromanager => TownOfUsColors.UseBasic ? Palette.CrewmateBlue : new Color32(38, 104, 148, 255);
    public static Color Mortician => TownOfUsColors.UseBasic ? Palette.CrewmateBlue : new Color32(0, 68, 218, 255);
    public static Color Shackled => new Color32(145, 155, 155, 255);
    public static Color Exposed => new Color32(220, 175, 51, 255);
    public static Color Battery => new Color32(255, 255, 255, 255);
    public static Color Workaholic => new Color32(124, 142, 158, 255);
}