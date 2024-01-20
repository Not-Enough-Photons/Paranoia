namespace NEP.Paranoia.Scripts.Managers;

internal static class EntityLoader
{
    public static readonly List<SpawnableCrateReference> EntityCrates = new();
    private static readonly List<SLZ.Marrow.Warehouse.Pallet> SupportedPallets = new();
    
    public static void CheckEntities()
    {
        var pallets = AssetWarehouse.Instance.GetPallets();

        if (pallets.Count == 0) return; // how tf would someone not be installing mods in a modding based game?? just in case though
        
        foreach (var pallet in pallets)
        {
            if (pallet.Description.Contains("[ParanoiaExtension]"))
            {
                ModConsole.Msg("Found Paranoia Extension Pallet: " + pallet.Barcode, LoggingMode.Debug);
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
                    ModConsole.Msg("Found Paranoia Extension Entity: " + crate.Barcode, LoggingMode.Debug);
                    EntityCrates.Add(new SpawnableCrateReference(crate.Barcode));
                }
            }
        }
    }
}