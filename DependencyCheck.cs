namespace Paranoia;

public static class DependencyCheck
{
    private static bool _hasFieldInjector;
    private static bool _hasJevilib;
    private static bool _hasBonelib;
        
    public static bool CheckForDependencies()
    {
        var assemblies = AppDomain.CurrentDomain.GetAssemblies();
        foreach (var asm in assemblies)
        {
            if (asm.GetName().Name.ToLower().Contains("fieldinjector"))
            {
                _hasFieldInjector = true;
            }
            if (asm.GetName().Name.ToLower().Contains("jevilib"))
            {
                _hasJevilib = true;
            }
            if (asm.GetName().Name.ToLower().Contains("bonelib"))
            {
                _hasBonelib = true;
            }
        }
        return _hasFieldInjector && _hasJevilib && _hasBonelib;
    }

    public static string GetMissingDependency()
    {
        if (!_hasFieldInjector)
        {
            return "FieldInjector";
        }
        if (!_hasJevilib)
        {
            return "JevilLib";
        }
        if (!_hasBonelib)
        {
            return "BoneLib";
        }
        return "None";
    }
}