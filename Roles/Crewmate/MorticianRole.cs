using UnityEngine;
using System.Text;
using TMPro;
using MiraAPI.Modifiers;
using MiraAPI.Patches.Stubs;
using MiraAPI.Roles;
using MiraAPI.Utilities;
using MiraAPI.Utilities.Assets;
using MiraAPI.GameOptions;
using TownOfUs.Modifiers.Crewmate;
using TownOfUs.Utilities;
using TownOfUs.Modules.Wiki;
using TownOfUs.Extensions;
using TownOfUs.Assets;
using TownOfUs.Roles;
using TownOfUs;
using NullsMod.Assets;
using NullsMod.Options.Roles.Crewmate;
using NullsMod.Modifiers.Hidden;

namespace NullsMod.Roles.Crewmate;

public sealed class MorticianRole(IntPtr cppPtr) : CrewmateRole(cppPtr), ITownOfUsRole, IWikiDiscoverable, IDoomable
{
    public DoomableType DoomHintType => DoomableType.Perception;
    public string IdPart => "Mortician";
    public string RoleName => "Mortician";
    public Color RoleColor => NullsColors.Mortician;
    public ModdedRoleTeams Team => ModdedRoleTeams.Crewmate;
    public RoleAlignment RoleAlignment => RoleAlignment.CrewmateSupport;
    public int AbilityUses { get; set; } 
    public float TaskProgress { get; set; }

    public CustomRoleConfiguration Configuration => new(this)
    {
        IconTmp = TmpSpriteUtils.CreateSpriteAsset(NullsIcons.Mortician.LoadAsset(), "Mortician", 1.45f),
        Icon = NullsIcons.Mortician,
        OptionsScreenshot = TouBanners.CrewmateRoleBanner,
        IntroSound = TouAudio.ScientistIntroSound
    };

    public string RoleDescription => "Reveal the dead to gain extra info!";
    public string RoleMedDescriptionLocale => "";
    public string RoleLongDescription => 
        $"Reveal dead roles to everyone, and report bodies to figure out the killer.\n" +
        $"Autopsy Uses: <color=#{ColorUtility.ToHtmlStringRGBA(NullsColors.Mortician)}>{AbilityUses}</color>";
    public string GetAdvancedDescription()
    {
        return
            "The Mortician is a Crewmate Support that can perform an autopsy during the meeting " +
            "to reveal a dead players role to everyone. Reporting bodies also gives the Mortician " +
            "the Killers Role." + 
            MiscUtils.AppendOptionsText(GetType());
    }

    public override void Initialize(PlayerControl player)
    {
        RoleBehaviourStubs.Initialize(this, player);

        AbilityUses = (int)OptionGroupSingleton<MorticianOptions>.Instance.InitialAbilityUses;
        TaskProgress = 0;

        if (!player.HasModifier<MorticianCacheModifier>())
        {
            player.AddModifier<MorticianCacheModifier>();
        }
    }

    // public bool CanShowSecondTab => true;

    // public float ShowAbilitiesTab(Transform abilityTemplate, Transform abilityTemplateLong, Transform abilityScroller)
    // {
    //     var listOfAbilities = new List<GameObject>();
    //         var newAbility = Instantiate(abilityTemplate, abilityScroller);
    //         var icon = newAbility.GetChild(0).GetChild(0).GetComponent<SpriteRenderer>();
    //         var text = newAbility.GetChild(1).GetComponent<TextMeshPro>();
    //         var desc = newAbility.GetChild(2).GetComponent<TextMeshPro>();

    //         icon.sprite = TouCrewAssets.InspectSprite.LoadAsset();
    //         icon.size = new Vector2(0.8f, 0.8f * icon.sprite.bounds.size.y / icon.sprite.bounds.size.x);
    //         // icon.tileMode = SpriteTileMode.Adaptive;

    //         text.text =
    //             $"<font=\"LiberationSans SDF\" material=\"LiberationSans SDF - Chat Message Masked\">{"Crewmate Imitation"}</font>";
    //         desc.text =
    //             $"<font=\"LiberationSans SDF\" material=\"LiberationSans SDF - Chat Message Masked\">{"All Roles can be revealed except already Revealed players"}</font>";
    //         newAbility.gameObject.SetActive(true);
    //         listOfAbilities.Add(newAbility.gameObject);
    //         var variantRoles = MiscUtils.AllRoles.Where(x => x is ICrewVariant).OrderBy(x => x.GetRoleName()).ToList();
    //         var neutEquivalents = new Dictionary<RoleBehaviour, RoleBehaviour>();
    //         var impEquivalents = new Dictionary<RoleBehaviour, RoleBehaviour>();
    //         foreach (var role in variantRoles)
    //         {
    //             var crewVariant = role as ICrewVariant;
    //             if (role.IsNeutral())
    //             {
    //                 neutEquivalents.Add(role, crewVariant!.CrewVariant);
    //             }
    //             else
    //             {
    //                 impEquivalents.Add(role, crewVariant!.CrewVariant);
    //             }
    //         }

    //         var newSubObject = new GameObject("NewContainer");
    //         newSubObject.layer = newAbility.gameObject.layer;
    //         newSubObject.transform.SetParent(abilityScroller);
    //         var newTransform = newSubObject.AddComponent<RectTransform>();
    //         newTransform.offsetMax = new Vector2(5.5f, -2.4f);
    //         newTransform.offsetMin = new Vector2(3f, 0f);
    //         newTransform.transform.localPosition = new Vector3(0, 0f, -10f);

    //     return Mathf.Max(0f, 3.4f);
    // }

    // public static GameObject AddTabInfo(string title, Dictionary<RoleBehaviour, RoleBehaviour> equivalentRoles, Transform abilityTemplate, float xOffset, Transform abilityScroller)
    // {
    //     var newAbility = Instantiate(abilityTemplate, abilityScroller);
    //     newAbility.localPosition = new Vector3(xOffset, -3.44f, 0f);
    //     var text = newAbility.GetChild(0).GetComponent<TextMeshPro>();
    //     var desc = newAbility.GetChild(1).GetComponent<TextMeshPro>();

    //     text.text =
    //         $"<font=\"LiberationSans SDF\" material=\"LiberationSans SDF - Chat Message Masked\">{title}</font>";
    //     var description = new StringBuilder();
    //     foreach (var rolePair in equivalentRoles)
    //     {
    //         var ogRole = rolePair.Key;
    //         var newRole = rolePair.Value;
    //         description.AppendLine(TownOfUsPlugin.Culture,
    //             $"{ogRole.GetRoleName()} ⇨ {newRole.GetRoleName()}");
    //     }
    //     desc.text =
    //         $"<font=\"LiberationSans SDF\" material=\"LiberationSans SDF - Chat Message Masked\">{description}</font>";
    //     newAbility.gameObject.SetActive(true);
    //     return newAbility.gameObject;
    // }
    // public string SecondTabName => "Role Guide";
}