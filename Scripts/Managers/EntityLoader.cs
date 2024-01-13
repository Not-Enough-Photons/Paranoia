namespace NEP.Paranoia.Scripts.Managers;

internal static class EntityLoader
{
    public static readonly List<SpawnableCrateReference> EntityCrates = new();
    private static readonly List<SLZ.Marrow.Warehouse.Pallet> SupportedPallets = new();
    
    public static void CheckEntities()
    {
        var pallets = AssetWarehouse.Instance.GetPallets();
        foreach (var pallet in pallets)
        {
            if (pallet.Description.Contains("[ParanoiaExtension]"))
            {
                SupportedPallets.Add(pallet);
            }
        }
        
        if (SupportedPallets.Count == 0) return;

        foreach (var pallet in SupportedPallets)
        {
            var crates = pallet.Crates;
            foreach (var crate in crates)
            {
                if (crate.Tags.Contains("ParanoiaEntity"))
                {
                    EntityCrates.Add(new SpawnableCrateReference(crate.Barcode));
                }
            }
        }
    }
}