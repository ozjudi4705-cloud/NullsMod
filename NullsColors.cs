using MiraAPI.Utilities;
using TownOfUs;
using UnityEngine;

namespace NullsMod;

public static class NullsColors
{
    // Crew Colors
    public static Color Micromanager => TownOfUsColors.UseBasic ? Palette.CrewmateBlue : new Color32(38, 104, 148, 255);
    // Neutral Colors
    public static Color Workaholic => new Color32(124, 142, 158, 255);
}