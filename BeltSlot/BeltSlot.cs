using System.Reflection;
using BepInEx;
using EFT.InventoryLogic;
using EFT.UI;
using SPT.Reflection.Patching;

namespace BeltSlot;

[BepInPlugin("com.ozen.beltslot", "Belt Slot", "1.0.0")]
[BepInDependency("com.wtt.packnstrap")]
public class BeltSlot : BaseUnityPlugin
{
    public static string ArmbandText { get; } = "ARMBAND".Localized().ToUpper();

    protected void Awake()
    {
        var patchManager = new PatchManager(this, true);
        patchManager.EnablePatches();

        typeof(ContainersPanel)
            .GetField("equipmentSlot_0", BindingFlags.Static | BindingFlags.NonPublic)
            ?.SetValue(null, _equipmentSlotsWithArmband);
    }

    private static readonly EquipmentSlot[] _equipmentSlotsWithArmband =
    [
        EquipmentSlot.TacticalVest,
        EquipmentSlot.Pockets,
        EquipmentSlot.ArmBand,
        EquipmentSlot.Backpack,
        EquipmentSlot.SecuredContainer,
        EquipmentSlot.Dogtag,
    ];
}
