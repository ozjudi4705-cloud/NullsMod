using System.Text;
using AmongUs.GameOptions;
using HarmonyLib;
using Il2CppInterop.Runtime.Attributes;
using MiraAPI.GameOptions;
using MiraAPI.Modifiers;
using MiraAPI.Networking;
using MiraAPI.Patches.Stubs;
using MiraAPI.Roles;
using MiraAPI.Utilities;
using Reactor.Networking.Attributes;
using Reactor.Utilities;
using Reactor.Utilities.Extensions;
using TownOfUs.Interfaces;
using TownOfUs.Modifiers;
using TownOfUs.Modifiers.Crewmate;
using TownOfUs.Modifiers.Neutral;
using TownOfUs.Modules;
using TownOfUs.Modules.Components;
using TownOfUs.Modules.Wiki;
using TownOfUs.Extensions;
using TownOfUs.Networking;
using TownOfUs.Utilities;
using MiraAPI.Utilities.Assets;
using TownOfUs.Assets;
using TownOfUs.Options.Roles.Neutral;
using TownOfUs.Roles.Crewmate;
using UnityEngine;
using Il2CppInterop.Runtime.InteropTypes.Arrays;

namespace TownOfUs.Roles.Neutral;

public sealed class WorkaholicRole(IntPtr cppPtr)
    : NeutralRole(cppPtr), ITownOfUsRole, IWikiDiscoverable, IProgressTally, IUnlovable, IUnguessable
{

    public string LocaleKey => "Workaholic";
    public string RoleName => "Workaholic";
    public bool IsUnlovable => true;
    public bool IsDraftable => true;
    public bool IsGuessable => false;
    public bool FinishedTasks { get; private set; }
    public bool MetWinCon => FinishedTasks;

    public Color RoleColor => TownOfUsColors.Neutral;
    public ModdedRoleTeams Team => ModdedRoleTeams.Custom;
    public RoleAlignment RoleAlignment => RoleAlignment.NeutralEvil;
    public RoleBehaviour AppearAs => RoleManager.Instance.GetRole((RoleTypes)RoleId.Get<WorkaholicRole>());
    public WorkaholicOptions options = OptionGroupSingleton<WorkaholicOptions>.Instance;
    public static LoadableAsset<AudioClip> WorkahoilcIntro { get; } = new LoadableAudioResourceAsset($"NullsMod.Resources.Audio.WorkaholicIntro.wav");

    private bool _tasksAdded;
    private bool _revealSent;

    public string RoleDescription => "Finish your tasks to steal the win!";
    public string RoleLongDescription => options.WorkRevealed
        ? "You are revealed. Finish your extra tasks while alive and/or dead to win !"
        : "You are hidden. Finish your extra tasks while alive to win !";

    public string GetAdvancedDescription() =>(options.WorkRevealed
        ? "You are revealed, everyone will know your role and task progression, and " +
        "when you finish your tasks you end the game, even after you die."
        : "You are hidden, nobody knows your role and you can only win while alive.") +
        MiscUtils.AppendOptionsText(GetType());

    public CustomRoleConfiguration Configuration => new(this)
    {
        IntroSound = WorkahoilcIntro,
        Icon = TouRoleIcons.Shifter,
        OptionsScreenshot = TouBanners.NeutralRoleBanner,
        GhostRole = (RoleTypes)RoleId.Get<WorkaholicGhostRole>(),
        MaxRoleCount = 1,
    };

    public bool WinConditionMet()
    {
        // if (options.WorkWin is not WorkWinOptions.Endsgame)
        // {
        //     return false;
        // }
        if(!options.WorkRevealed && Player.HasDied())
        {
            return false;
        }

        return FinishedTasks;
    }

    public string GetTaskTally()
    {
        GetTaskCounts(Player, out var completed, out var total);
        var colorbase = Color.yellow;
        var color = Color.yellow;
        if (completed <= 0)
        {
            color = TownOfUsColors.ImpSoft;
        }
        else if (completed >= total)
        {
            color = TownOfUsColors.Doomsayer;
        }
        else if (completed > total / 2)
        {
            var fraction = ((completed * 0.4f) / total);
            Color color2 = TownOfUsColors.Doomsayer;
            color = new
            ((color2.r * fraction + colorbase.r * (1 - fraction)),
                (color2.g * fraction + colorbase.g * (1 - fraction)),
                (color2.b * fraction + colorbase.b * (1 - fraction)));
        }
        else if (completed < total / 2)
        {
            var fraction = ((completed * 0.9f) / total);
            Color color2 = TownOfUsColors.ImpSoft;
            color = new
            ((colorbase.r * fraction + color2.r * (1 - fraction)),
                (colorbase.g * fraction + color2.g * (1 - fraction)),
                (colorbase.b * fraction + color2.b * (1 - fraction)));
        }
        return $"{color.ToTextColor()}({completed}/{total})</color>";
    }

    public bool ProgressOnName(bool localDead, bool inMeeting, bool amOwner, out string progress)
    {
        progress = string.Empty;
        if(options.WorkRevealed)
        {
            progress = GetTaskTally();
            return true;
        }
        else if(amOwner || localDead)
        {
            progress = GetTaskTally();
            return true;
        }
        return false;
    }

    public string ProgressOnSummaryNormal => string.Empty;

    public string ProgressOnSummaryDetailed =>
        string.Empty;

    public override void Initialize(PlayerControl player)
    {
        RoleBehaviourStubs.Initialize(this, player);
    }

    public void LobbyStart()
    {
        _tasksAdded = false;
        FinishedTasks = false;
        _revealSent = false;
    }


    public void FixedUpdate()
    {
        if (!Player || Player.Data.Role is not WorkaholicRole)
        {
            return;
        }
        if (!_revealSent && options.WorkRevealed && AmongUsClient.Instance != null)
        {
            Player.RpcAddModifier<WorkaholicRevealModifier>();
            _revealSent = true;
        }
        if (!_tasksAdded && Player.myTasks != null && Player.myTasks.Count > 0)
        {
            AddExtraTasks();
            _tasksAdded = true;
        }

        CheckTaskCompletion();
    }

    private void CheckTaskCompletion()
    {
        GetTaskCounts(Player, out var completed, out var total);

        if (completed == total && total > 0)
        {
            FinishedTasks = true;
        }
    }

    private void AddExtraTasks()
    {
        if (Player?.Data?.Tasks == null)
            return;

        var tasks = Player.Data.Tasks;

        var commonTasks = ShipStatus.Instance.CommonTasks;
        var shortTasks  = ShipStatus.Instance.ShortTasks;
        var longTasks   = ShipStatus.Instance.LongTasks;

        var toAdd = new List<NormalPlayerTask>();

        // Temporary changes
        for (int i = 0; i < (int)options.ExtraCommonTasks; i++)
            toAdd.Add(commonTasks[UnityEngine.Random.Range(0, commonTasks.Length)]);

        for (int i = 0; i < (int)options.ExtraShortTasks; i++)
            toAdd.Add(shortTasks[UnityEngine.Random.Range(0, shortTasks.Length)]);

        for (int i = 0; i < (int)options.ExtraLongTasks; i++)
            toAdd.Add(longTasks[UnityEngine.Random.Range(0, longTasks.Length)]);

        foreach (var prefab in toAdd)
        {
            var task = UnityEngine.Object.Instantiate(prefab, Player.transform);

            task.Id = (uint)tasks.Count;
            task.Owner = Player;
            task.Initialize();

            Player.myTasks.Add(task);
            tasks.Add(CreateTaskInfo(task.Id));
        }
    }

    private static NetworkedPlayerInfo.TaskInfo CreateTaskInfo(uint id)
    {
        var ptr = Il2CppInterop.Runtime.IL2CPP.il2cpp_object_new(
            Il2CppInterop.Runtime.Il2CppClassPointerStore<NetworkedPlayerInfo.TaskInfo>.NativeClassPtr);

        var taskInfo = new NetworkedPlayerInfo.TaskInfo(ptr);

        taskInfo.Id = id;
        taskInfo.Complete = false;

        return taskInfo;
    }

    private static void GetTaskCounts(PlayerControl player, out int completed, out int total)
    {
        completed = 0;
        total = 0;

        if (player == null || player.Data == null)
        {
            return;
        }

        if (player.myTasks != null && player.myTasks.Count > 0)
        {
            var tasks = player.myTasks.ToArray().Where(x => !PlayerTask.TaskIsEmergency(x) && !x.TryCast<ImportantTextTask>());
            foreach (var t in tasks)
            {
                total++;
                var taskInfo = player.Data.FindTaskById(t.Id);
                var isComplete = taskInfo != null ? taskInfo.Complete : t.IsComplete;
                if (isComplete)
                {
                    completed++;
                }
            }
        }
    }

    public override void Deinitialize(PlayerControl targetPlayer)
    {
        RoleBehaviourStubs.Deinitialize(this, targetPlayer);
        TouRoleUtils.ClearTaskHeader(Player);
        if(options.WorkRevealed)
        {
            targetPlayer.RpcAddModifier<BasicGhostModifier>();
        }
    }

    public override bool DidWin(GameOverReason gameOverReason)
    {
        return FinishedTasks;
    }
}