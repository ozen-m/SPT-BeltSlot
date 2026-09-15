using System.Reflection;
using EFT.InventoryLogic;
using EFT.UI;
using EFT.UI.DragAndDrop;
using SPT.Reflection.Patching;
using UnityEngine;

namespace BeltSlot.Patches;

public class ContainersPanelGetTemplatePatch : ModulePatch
{
    protected override MethodBase GetTargetMethod()
    {
        return typeof(ContainersPanel).GetMethod(nameof(ContainersPanel.InstantiateSlotView));
    }

    [PatchPrefix]
    public static bool Prefix(ContainersPanel __instance, EquipmentSlot slotName, ref SlotView __result)
    {
        __result = Object.Instantiate(slotName is EquipmentSlot.Dogtag ? __instance._dogtagTemplate : __instance._defaultSlotTemplate);
        return false;
    }
}
