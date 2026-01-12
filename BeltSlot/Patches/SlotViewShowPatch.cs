using System;
using System.Reflection;
using EFT.UI.DragAndDrop;
using PackNStrap.Core.Items;
using SPT.Reflection.Patching;
using TMPro;

namespace BeltSlot.Patches;

public class SlotViewShowPatch : ModulePatch
{
    protected override MethodBase GetTargetMethod()
    {
        return typeof(SlotView).GetMethod(nameof(SlotView.Show));
    }

    [PatchPostfix]
    public static void Postfix(SlotView __instance)
    {
        var slot = __instance.Slot;
        if (slot is null) return;
        if (!slot.ID.StartsWith("ArmBand", StringComparison.Ordinal)) return;

        // Conflicts with SearchableSlotView, need to check for childCount
        var transform = __instance.transform;
        if (transform.childCount < 8) return;

        var unsubAction = slot.ReactiveContainedItem.Bind((item) =>
        {
            var textMesh = transform.GetChild(3).GetChild(2).GetComponent<TextMeshProUGUI>();
            textMesh.text = item switch
            {
                CustomBeltItemClass => "BELT",
                _ => BeltSlot.ArmbandText
            };
        });

        __instance.AddDisposable(unsubAction);
    }
}
