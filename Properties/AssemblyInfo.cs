[assembly: AssemblyTitle(NEP.Paranoia.Paranoia.Description)]
[assembly: AssemblyDescription(NEP.Paranoia.Paranoia.Description)]
[assembly: AssemblyCompany(NEP.Paranoia.Paranoia.Company)]
[assembly: AssemblyProduct(NEP.Paranoia.Paranoia.Name)]
[assembly: AssemblyCopyright("Developed by " + NEP.Paranoia.Paranoia.Author)]
[assembly: AssemblyTrademark(NEP.Paranoia.Paranoia.Company)]
[assembly: AssemblyVersion(NEP.Paranoia.Paranoia.Version)]
[assembly: AssemblyFileVersion(NEP.Paranoia.Paranoia.Version)]
[assembly:
    MelonInfo(typeof(NEP.Paranoia.Paranoia), NEP.Paranoia.Paranoia.Name, NEP.Paranoia.Paranoia.Version,
        NEP.Paranoia.Paranoia.Author, NEP.Paranoia.Paranoia.DownloadLink)]
[assembly: MelonColor(System.ConsoleColor.White)]

// Create and Setup a MelonGame Attribute to mark a Melon as Universal or Compatible with specific Games.
// If no MelonGame Attribute is found or any of the Values for any MelonGame Attribute on the Melon is null or empty it will be assumed the Melon is Universal.
// Values for MelonGame Attribute can be found in the Game's app.info file or printed at the top of every log directly beneath the Unity version.
[assembly: MelonGame("Stress Level Zero", "BONELAB")]