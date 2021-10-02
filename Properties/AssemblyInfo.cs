using System.Reflection;
using MelonLoader;

[assembly: AssemblyTitle(NEP.Paranoia.BuildInfo.Description)]
[assembly: AssemblyDescription(NEP.Paranoia.BuildInfo.Description)]
[assembly: AssemblyCompany(NEP.Paranoia.BuildInfo.Company)]
[assembly: AssemblyProduct(NEP.Paranoia.BuildInfo.Name)]
[assembly: AssemblyCopyright("Created by " + NEP.Paranoia.BuildInfo.Author)]
[assembly: AssemblyTrademark(NEP.Paranoia.BuildInfo.Company)]
[assembly: AssemblyVersion(NEP.Paranoia.BuildInfo.Version)]
[assembly: AssemblyFileVersion(NEP.Paranoia.BuildInfo.Version)]
[assembly: MelonInfo(typeof(NEP.Paranoia.Paranoia), NEP.Paranoia.BuildInfo.Name, NEP.Paranoia.BuildInfo.Version, NEP.Paranoia.BuildInfo.Author, NEP.Paranoia.BuildInfo.DownloadLink)]
[assembly: MelonColor(System.ConsoleColor.Gray)]

// Create and Setup a MelonGame Attribute to mark a Melon as Universal or Compatible with specific Games.
// If no MelonGame Attribute is found or any of the Values for any MelonGame Attribute on the Melon is null or empty it will be assumed the Melon is Universal.
// Values for MelonGame Attribute can be found in the Game's app.info file or printed at the top of every log directly beneath the Unity version.
[assembly: MelonGame("Stress Level Zero", "BONEWORKS")]