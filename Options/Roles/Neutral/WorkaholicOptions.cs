using MiraAPI.GameOptions;
using MiraAPI.GameOptions.Attributes;
using MiraAPI.GameOptions.OptionTypes;
using MiraAPI.Utilities;
using TownOfUs.Roles.Neutral;

namespace TownOfUs.Options.Roles.Neutral;

public sealed class WorkaholicOptions : AbstractRoleOptionGroup<WorkaholicRole>
{
    public override string GroupName => "Workaholic";

    [ModdedToggleOption("Workaholic Is Revealed")]
    public bool WorkRevealed { get; set; } = true;

    [ModdedNumberOption("Extra Common Tasks", 0, 8)]
    public float ExtraCommonTasks { get; set; } = 1f;

    [ModdedNumberOption("Extra Short Tasks", 0, 10)]
    public float ExtraShortTasks { get; set; } = 3f;

    [ModdedNumberOption("Extra Long Tasks", 0, 8)]
    public float ExtraLongTasks { get; set; } = 2f;

    // public ModdedEnumOption<WorkWinOptions> WorkWin { get; } = new("Workaholic Win", WorkWinOptions.EndsGame, ["Ends Game", "Haunts", "Nothing"])
    // {
    //     Visible = () => !OptionGroupSingleton<WorkaholicOptions>.Instance.WorkRevealed
    // };

    // public enum WorkWinOptions
    // {
    //     EndsGame,
    //     Haunts,
    //     Nothing
    // }
}