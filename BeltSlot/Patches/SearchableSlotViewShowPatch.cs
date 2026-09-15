using System;
using System.Reflection;
using EFT;
using EFT.UI.DragAndDrop;
using PackNStrap.Core.Items;
using SPT.Reflection.Patching;
using TMPro;

namespace BeltSlot.Patches;

public class SearchableSlotViewShowPatch : ModulePatch
{
    protected override MethodBase GetTargetMethod()
    {
        return typeof(SearchableSlotView).GetMethod(nameof(SearchableSlotView.Show));
    }

    [PatchPostfix]
    public static void Postfix(SearchableSlotView __instance)
    {
        var slot = __instance.Slot;
        if (slot is null)
        {
            return;
        }
        if (!slot.ID.StartsWith("ArmBand", StringComparison.Ordinal))
        {
            return;
        }

        var unsubAction = slot.ReactiveContainedItem.Bind(
            (item) =>
            {
                var transform = __instance.transform;
                var slotPanel = transform.GetChild(1).gameObject;
                var textMesh = transform.GetChild(0).GetChild(1).GetChild(2).GetComponent<TextMeshProUGUI>();
                switch (item)
                {
                    case CustomBeltItemClass:
                        slotPanel.SetActive(true);
                        textMesh.text = "BELT";
                        break;
                    default:
                        slotPanel.SetActive(false);
                        textMesh.text = BeltSlot.ArmbandText;
                        break;
                }
            }
        );

        __instance.AddDisposable(unsubAction);
    }
}
