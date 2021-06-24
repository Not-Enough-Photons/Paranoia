using System.Reflection;
using MelonLoader;

[assembly: AssemblyTitle(NotEnoughPhotons.paranoia.BuildInfo.Description)]
[assembly: AssemblyDescription(NotEnoughPhotons.paranoia.BuildInfo.Description)]
[assembly: AssemblyCompany(NotEnoughPhotons.paranoia.BuildInfo.Company)]
[assembly: AssemblyProduct(NotEnoughPhotons.paranoia.BuildInfo.Name)]
[assembly: AssemblyCopyright("Created by " + NotEnoughPhotons.paranoia.BuildInfo.Author)]
[assembly: AssemblyTrademark(NotEnoughPhotons.paranoia.BuildInfo.Company)]
[assembly: AssemblyVersion(NotEnoughPhotons.paranoia.BuildInfo.Version)]
[assembly: AssemblyFileVersion(NotEnoughPhotons.paranoia.BuildInfo.Version)]
[assembly: MelonInfo(typeof(NotEnoughPhotons.paranoia.Paranoia), NotEnoughPhotons.paranoia.BuildInfo.Name, NotEnoughPhotons.paranoia.BuildInfo.Version, NotEnoughPhotons.paranoia.BuildInfo.Author, NotEnoughPhotons.paranoia.BuildInfo.DownloadLink)]
[assembly: MelonColor(System.ConsoleColor.DarkGray)]

// Create and Setup a MelonGame Attribute to mark a Melon as Universal or Compatible with specific Games.
// If no MelonGame Attribute is found or any of the Values for any MelonGame Attribute on the Melon is null or empty it will be assumed the Melon is Universal.
// Values for MelonGame Attribute can be found in the Game's app.info file or printed at the top of every log directly beneath the Unity version.
[assembly: MelonGame("Stress Level Zero", "BONEWORKS")]