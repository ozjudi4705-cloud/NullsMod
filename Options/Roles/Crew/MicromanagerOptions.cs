using MiraAPI.GameOptions;
using MiraAPI.GameOptions.Attributes;
using TownOfUs.Roles.Crewmate;

namespace TownOfUs.Options.Roles.Crewmate;

public sealed class MicromanagerOptions : AbstractRoleOptionGroup<MicromanagerRole>
{
    public override string GroupName => "Micromanager";

    [ModdedNumberOption("Tasks Left Before Clickable", 0f, 8f)]
    public float NumTasksLeftBeforeClickable { get; set; } = 5f;

    [ModdedNumberOption("Tasks per Managed Task", 1f, 3f)]
    public float NumTasksPerManagedTask { get; set; } = 1f;

    [ModdedEnumOption("Can Manage", typeof(MicromanagerRoleCrewType), ["Alive Crew", "Dead Crew", "Any Crew"])]
    public MicromanagerRoleCrewType MicromanagerManagesCrewTasks { get; set; } = MicromanagerRoleCrewType.AliveCrew;

    [ModdedEnumOption("Can Be Clicked By", typeof(MicromanagerRoleClickableType),["Non Crew", "Imps Only"])]
    public MicromanagerRoleClickableType MicromanagerCanBeClickedBy { get; set; } = MicromanagerRoleClickableType.NonCrew;

    [ModdedNumberOption("Extra Common Tasks", 0, 8)]
    public float ExtraCommonTasks { get; set; } = 1f;

    [ModdedNumberOption("Extra Short Tasks", 0, 10)]
    public float ExtraShortTasks { get; set; } = 3f;

    [ModdedNumberOption("Extra Long Tasks", 0, 8)]
    public float ExtraLongTasks { get; set; } = 2f;
}

public enum MicromanagerRoleClickableType
{
    NonCrew,
    ImpsOnly
}

public enum MicromanagerRoleCrewType
{
    AliveCrew,
    DeadCrew,
    AnyCrew
}