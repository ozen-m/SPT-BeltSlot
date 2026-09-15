using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using EFT.InventoryLogic;
using SPT.Reflection.Patching;

namespace BeltSlot.Patches;

public class ContainerPriorityPatch : ModulePatch
{
    protected override MethodBase GetTargetMethod()
    {
        return typeof(InventoryEquipmentExtension).GetMethod(nameof(InventoryEquipmentExtension.GetPrioritizedContainersForLoot));
    }

    [PatchPrefix]
    protected static bool Prefix(InventoryEquipment equipment, Item item, ref IEnumerable<IContainer> __result)
    {
        var vestContainers = (equipment.GetSlot(EquipmentSlot.TacticalVest).ContainedItem as ContainerCollection)?.Containers ?? [];
        var backpackContainers = (equipment.GetSlot(EquipmentSlot.Backpack).ContainedItem as ContainerCollection)?.Containers ?? [];
        var pocketsContainers = (equipment.GetSlot(EquipmentSlot.Pockets).ContainedItem as ContainerCollection)?.Containers ?? [];
        var securedContainers = (equipment.GetSlot(EquipmentSlot.SecuredContainer).ContainedItem as ContainerCollection)?.Containers ?? [];

        // Pack n Strap
        var armbandContainers = (equipment.GetSlot(EquipmentSlot.ArmBand).ContainedItem as ContainerCollection)?.Containers ?? [];

        __result = item switch
        {
            Ammo _ => armbandContainers
                .Concat(vestContainers)
                .Concat(pocketsContainers)
                .Concat(backpackContainers)
                .Concat(securedContainers),
            Magazine _ => vestContainers
                .Concat(armbandContainers)
                .Concat(pocketsContainers)
                .Concat(backpackContainers)
                .Concat(securedContainers),
            Money _ => securedContainers
                .Concat(backpackContainers)
                .Concat(armbandContainers)
                .Concat(vestContainers)
                .Concat(pocketsContainers),
            ThrowWeap _ => pocketsContainers
                .Concat(armbandContainers)
                .Concat(vestContainers)
                .Concat(backpackContainers)
                .Concat(securedContainers),
            _ => backpackContainers.Concat(vestContainers).Concat(armbandContainers).Concat(pocketsContainers).Concat(securedContainers),
        };

        return false;
    }
}
