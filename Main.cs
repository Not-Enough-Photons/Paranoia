using MelonLoader;

namespace Paranoia
{
    public class Main : MelonMod
    {
        internal const string Name = "Paranoia"; // Required
        internal const string Description = "Keep your clones close."; // Required
        internal const string Author = "Not Enough Photons, adamdev, SoulWithMae"; // Required
        internal const string Company = "Not Enough Photons";
        internal const string Version = "1.0.0";
        internal const string DownloadLink = "null";
        
        public override void OnInitializeMelon()
        {
            FieldInjection.Inject();
        }
    }
}