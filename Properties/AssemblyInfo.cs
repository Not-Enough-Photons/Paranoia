[assembly: AssemblyTitle(NEP.Paranoia.Main.Description)]
[assembly: AssemblyDescription(NEP.Paranoia.Main.Description)]
[assembly: AssemblyCompany(NEP.Paranoia.Main.Company)]
[assembly: AssemblyProduct(NEP.Paranoia.Main.Name)]
[assembly: AssemblyCopyright("Developed by " + NEP.Paranoia.Main.Author)]
[assembly: AssemblyTrademark(NEP.Paranoia.Main.Company)]
[assembly: AssemblyVersion(NEP.Paranoia.Main.Version)]
[assembly: AssemblyFileVersion(NEP.Paranoia.Main.Version)]
[assembly: MelonInfo(typeof(NEP.Paranoia.Main), NEP.Paranoia.Main.Name, NEP.Paranoia.Main.Version, NEP.Paranoia.Main.Author, NEP.Paranoia.Main.DownloadLink)]
[assembly: MelonColor(ConsoleColor.White)]

// Create and Setup a MelonGame Attribute to mark a Melon as Universal or Compatible with specific Games.
// If no MelonGame Attribute is found or any of the Values for any MelonGame Attribute on the Melon is null or empty it will be assumed the Melon is Universal.
// Values for MelonGame Attribute can be found in the Game's app.info file or printed at the top of every log directly beneath the Unity version.
[assembly: MelonGame("Stress Level Zero", "BONELAB")]