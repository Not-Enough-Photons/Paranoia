using System.Reflection;
using MelonLoader;

[assembly: AssemblyTitle(Paranoia.Main.Description)]
[assembly: AssemblyDescription(Paranoia.Main.Description)]
[assembly: AssemblyCompany(Paranoia.Main.Company)]
[assembly: AssemblyProduct(Paranoia.Main.Name)]
[assembly: AssemblyCopyright("Developed by " + Paranoia.Main.Author)]
[assembly: AssemblyTrademark(Paranoia.Main.Company)]
[assembly: AssemblyVersion(Paranoia.Main.Version)]
[assembly: AssemblyFileVersion(Paranoia.Main.Version)]
[assembly:
    MelonInfo(typeof(Paranoia.Main), Paranoia.Main.Name, Paranoia.Main.Version,
        Paranoia.Main.Author, Paranoia.Main.DownloadLink)]
[assembly: MelonColor(System.ConsoleColor.White)]

// Create and Setup a MelonGame Attribute to mark a Melon as Universal or Compatible with specific Games.
// If no MelonGame Attribute is found or any of the Values for any MelonGame Attribute on the Melon is null or empty it will be assumed the Melon is Universal.
// Values for MelonGame Attribute can be found in the Game's app.info file or printed at the top of every log directly beneath the Unity version.
[assembly: MelonGame("Stress Level Zero", "BONELAB")]