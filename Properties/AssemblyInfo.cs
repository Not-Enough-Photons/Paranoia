using System.Reflection;
using MelonLoader;

[assembly: AssemblyTitle(Paranoia.Paranoia.Description)]
[assembly: AssemblyDescription(Paranoia.Paranoia.Description)]
[assembly: AssemblyCompany(Paranoia.Paranoia.Company)]
[assembly: AssemblyProduct(Paranoia.Paranoia.Name)]
[assembly: AssemblyCopyright("Developed by " + Paranoia.Paranoia.Author)]
[assembly: AssemblyTrademark(Paranoia.Paranoia.Company)]
[assembly: AssemblyVersion(Paranoia.Paranoia.Version)]
[assembly: AssemblyFileVersion(Paranoia.Paranoia.Version)]
[assembly:
    MelonInfo(typeof(Paranoia.Paranoia), Paranoia.Paranoia.Name, Paranoia.Paranoia.Version,
        Paranoia.Paranoia.Author, Paranoia.Paranoia.DownloadLink)]
[assembly: MelonColor(System.ConsoleColor.White)]

// Create and Setup a MelonGame Attribute to mark a Melon as Universal or Compatible with specific Games.
// If no MelonGame Attribute is found or any of the Values for any MelonGame Attribute on the Melon is null or empty it will be assumed the Melon is Universal.
// Values for MelonGame Attribute can be found in the Game's app.info file or printed at the top of every log directly beneath the Unity version.
[assembly: MelonGame("Stress Level Zero", "BONELAB")]