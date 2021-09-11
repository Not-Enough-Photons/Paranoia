using System.Reflection;
using MelonLoader;

[assembly: AssemblyTitle(NotEnoughPhotons.Paranoia.BuildInfo.Description)]
[assembly: AssemblyDescription(NotEnoughPhotons.Paranoia.BuildInfo.Description)]
[assembly: AssemblyCompany(NotEnoughPhotons.Paranoia.BuildInfo.Company)]
[assembly: AssemblyProduct(NotEnoughPhotons.Paranoia.BuildInfo.Name)]
[assembly: AssemblyCopyright("Created by " + NotEnoughPhotons.Paranoia.BuildInfo.Author)]
[assembly: AssemblyTrademark(NotEnoughPhotons.Paranoia.BuildInfo.Company)]
[assembly: AssemblyVersion(NotEnoughPhotons.Paranoia.BuildInfo.Version)]
[assembly: AssemblyFileVersion(NotEnoughPhotons.Paranoia.BuildInfo.Version)]
[assembly: MelonInfo(typeof(NotEnoughPhotons.Paranoia.Paranoia), NotEnoughPhotons.Paranoia.BuildInfo.Name, NotEnoughPhotons.Paranoia.BuildInfo.Version, NotEnoughPhotons.Paranoia.BuildInfo.Author, NotEnoughPhotons.Paranoia.BuildInfo.DownloadLink)]
[assembly: MelonColor(System.ConsoleColor.DarkGray)]

// Create and Setup a MelonGame Attribute to mark a Melon as Universal or Compatible with specific Games.
// If no MelonGame Attribute is found or any of the Values for any MelonGame Attribute on the Melon is null or empty it will be assumed the Melon is Universal.
// Values for MelonGame Attribute can be found in the Game's app.info file or printed at the top of every log directly beneath the Unity version.
[assembly: MelonGame("Stress Level Zero", "BONEWORKS")]