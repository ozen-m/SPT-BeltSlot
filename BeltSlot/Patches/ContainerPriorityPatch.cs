using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using EFT.InventoryLogic;
using PackNStrap.Core.Items;
using SPT.Reflection.Patching;

namespace BeltSlot.Patches;

public class ContainerPriorityPatch : ModulePatch
{
    protected override MethodBase GetTargetMethod()
    {
        return typeof(GClass3372).GetMethod(nameof(GClass3372.GetPrioritizedContainersForLoot));
    }

    [PatchPrefix]
    protected static bool Prefix(GClass3372 __instance, InventoryEquipment equipment, Item item, ref IEnumerable<EFT.InventoryLogic.IContainer> __result)
    {
        var slotVest = equipment.GetSlot(EquipmentSlot.TacticalVest);
        var slotBackpack = equipment.GetSlot(EquipmentSlot.Backpack);
        var slotPockets = equipment.GetSlot(EquipmentSlot.Pockets);
        var slotSecureContainer = equipment.GetSlot(EquipmentSlot.SecuredContainer);

        // Pack n Strap
        var slotArmBand = equipment.GetSlot(EquipmentSlot.ArmBand);

        var containers2 = slotVest.ContainedItem is VestItemClass vest ? vest.Containers : [];
        var containers4 = slotBackpack.ContainedItem is BackpackItemClass backpack ? backpack.Containers : [];
        var containers6 = slotPockets.ContainedItem is PocketsItemClass pockets ? pockets.Containers : [];
        var containers8 = slotSecureContainer.ContainedItem is MobContainerItemClass mobContainer ? mobContainer.Containers : [];

        // Pack n Strap
        var containers10 = slotArmBand.ContainedItem is CustomBeltItemClass belt ? belt.Containers : [];
        var containers12 = slotArmBand.ContainedItem is VestItemClass vestArmband ? vestArmband.Containers : [];

        __result = item switch
        {
            AmmoItemClass _ => containers10.Concat(containers12).Concat(containers2).Concat(containers6).Concat(containers4).Concat(containers8),
            MagazineItemClass _ => containers2.Concat(containers10).Concat(containers12).Concat(containers6).Concat(containers4).Concat(containers8),
            MoneyItemClass _ => containers8.Concat(containers4).Concat(containers2).Concat(containers10).Concat(containers12).Concat(containers6),
            ThrowWeapItemClass _ => containers6.Concat(containers10).Concat(containers12).Concat(containers2).Concat(containers4).Concat(containers8),
            _ => containers4.Concat(containers2).Concat(containers10).Concat(containers12).Concat(containers6).Concat(containers8)
        };

        return false;
    }
}
