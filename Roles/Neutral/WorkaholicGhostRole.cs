using System;
using System.Text;
using AmongUs.GameOptions;
using MiraAPI.GameOptions;
using MiraAPI.Patches.Stubs;
using MiraAPI.Roles;
using MiraAPI.Utilities;
using TownOfUs.Utilities;
using TownOfUs.Interfaces;
using TownOfUs.Modules;
using TownOfUs.Options.Roles.Neutral;
using Il2CppInterop.Runtime.Attributes;
using UnityEngine;


namespace TownOfUs.Roles.Neutral;

public sealed class WorkaholicGhostRole(IntPtr cppPtr)
    : NeutralGhostRole(cppPtr), IProgressTally
{
    public WorkaholicOptions options = OptionGroupSingleton<WorkaholicOptions>.Instance;
    public bool FinishedTasks { get; private set; }

    public override void Initialize(PlayerControl player)
    {
        RoleBehaviourStubs.Initialize(this, player);

        FinishedTasks = false;
    }

    public override CustomRoleConfiguration Configuration => new(this)
    {
        TasksCountForProgress = false,
        HideSettings = true,
        CanModifyChance = false,
        DefaultChance = 0,
        DefaultRoleCount = 0,
        MaxRoleCount = 0,
        ShowInFreeplay = false,
    };

    private void FixedUpdate()
    {
        if (!Player || Player.Data.Role is not WorkaholicGhostRole)
        {
            return;
        }
        
        CheckTaskCompletion();
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

            return;
        }
    }

    public bool ProgressOnName(bool localDead, bool inMeeting, bool amOwner, out string progress)
    {
        progress = string.Empty;
        if(options.WorkRevealed)
        {
            progress = GetTaskTally();
            return true;
        }
        else if(amOwner && localDead)
        {
            progress = GetTaskTally();
            return true;
        }
        return false;
    }

    public string ProgressOnSummaryNormal => string.Empty;

    public string ProgressOnSummaryDetailed =>
        string.Empty;

    private void CheckTaskCompletion()
    {
        GetTaskCounts(Player, out var completed, out var total);

        if (completed == total && total > 0)
        {
            FinishedTasks = true;
        }
    }

    public override bool WinConditionMet()
    {
        // if (options.WorkWin is not WorkWinOptions.Endsgame)
        // {
        //     return false;
        // }

        if (!options.WorkRevealed && Player.HasDied())
        {
            return false;
        }

        return FinishedTasks;
    }

    public override bool DidWin(GameOverReason gameOverReason)
    {
        return FinishedTasks;
    }

    public override bool CanUse(IUsable console)
    {
        if (!GameManager.Instance.LogicUsables.CanUse(console, Player))
        {
            return false;
        }

        var console2 = console.TryCast<Console>();

        return console2 != null;
    }

    public override void Deinitialize(PlayerControl targetPlayer)
    {
        RoleBehaviourStubs.Deinitialize(this, targetPlayer);
        TouRoleUtils.ClearTaskHeader(Player);
    }
}