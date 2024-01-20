namespace NEP.Paranoia.Scripts.Helpers;

/// <summary>
/// A collection of helper methods for Barcode.
/// </summary>
public static class Utilities
{
    /// <summary>
    /// Finds all NPCs within the scene.
    /// </summary>
    /// <returns>An array of all NPCs within the scene.</returns>
    public static Il2CppArrayBase<AIBrain> FindAIBrains()
    {
        return Object.FindObjectsOfType<AIBrain>();
    }
        
    /// <summary>
    /// Similar to Il2CppArayBase FindAIBrains, but also returns the BehaviourBaseNav array.
    /// </summary>
    /// <returns>An array of all NPCs within the scene and their BehaviourBaseNavs.</returns>
    public static AIBrain[] FindAIBrains(out BehaviourBaseNav[] navs)
    {
        AIBrain[] result = Object.FindObjectsOfType<AIBrain>();
        navs = FindBaseNavs(result);

        return result;
    }

    private static BehaviourBaseNav[] FindBaseNavs(AIBrain[] brains)
    {
        var baseNavs = new List<BehaviourBaseNav>();

        brains.ToList().ForEach((brain) =>
        {
            baseNavs.Add(brain != null ? brain.behaviour : null);
        });

        return baseNavs.ToArray();
    }
        
    /// <summary>
    /// Moves all NPCs to a given point.
    /// </summary>
    /// <param name="behaviour">The AIBrain to be moved</param>
    /// <param name="point">The position to move the AIBrain to</param>
    public static void MoveAIToPoint(BehaviourBaseNav behaviour, Vector3 point)
    {
        behaviour.sensors.hearingSensitivity = 0f;
        behaviour.SetHomePosition(point, true, true);
        behaviour.Investigate(point, true, 120f);
    }
        
    /// <summary>
    /// Freezes the player.
    /// <br/>Used in Paralyzer.<see cref="Entities.Paralyzer.Start"/>
    /// </summary>
    /// <param name="freeze">Whether to freeze or unfreeze the player.</param>
    public static void FreezePlayer(bool freeze)
    {
        var physRig = Player.physicsRig;
        physRig.rbFeet.isKinematic = freeze;
        physRig.m_pelvis.GetComponent<Rigidbody>().isKinematic = freeze;
        physRig.leftHand.GetComponent<Rigidbody>().isKinematic = freeze;
        physRig.rightHand.GetComponent<Rigidbody>().isKinematic = freeze;
    }
        
    /// <summary>
    /// Checks the date to see if it is a given month and day, then returns as true if it is that day.
    /// </summary>
    /// <param name="month">The number of the month to use</param>
    /// <param name="day">The number of the day to use</param>
    /// <returns>True if it is the given month and day, false if not.</returns>
    public static bool CheckDate(int month, int day)
    {
        var currentDate = DateTime.Now;
        return currentDate.Month == month && currentDate.Day == day;
    }
        
    /// <summary>
    /// Crashes the game in a truely Unity fashion: Access Violation
    /// <br/>Used in Crasher.<see cref="Entities.Crasher.OnTriggerEnter"/>
    /// </summary>
    public static void CrashGame()
    {
        ModConsole.Warning("THIS IS THAT LATER WARNING. THIS WAS CAUSED BY INTENTIONAL MOD DESIGN. THIS IS NOT A BUG. THIS LOG IS VOID!");
        Utils.ForceCrash(ForcedCrashCategory.AccessViolation);
    }

    /// <summary>
    /// Checks if a common recording software is running
    /// </summary>
    /// <returns>True if a recording program is running, false if not.</returns>
    public static bool CheckIfRecording()
    {
        var obs = Process.GetProcessesByName("obs64");
        var fraps = Process.GetProcessesByName("Fraps");
        var bandicam = Process.GetProcessesByName("bdcam");
        var xsplit = Process.GetProcessesByName("XSplit.Core");
        if (obs.Length > 0)
        {
            ModConsole.Msg("OBS is running!", LoggingMode.Debug);
            return true;
        }
        if (fraps.Length > 0)
        {
            ModConsole.Msg("Fraps is running!", LoggingMode.Debug);
            return true;
        }
        if (bandicam.Length > 0)
        {
            ModConsole.Msg("Bandicam is running!", LoggingMode.Debug);
            return true;
        }
        if (xsplit.Length > 0)
        {
            ModConsole.Msg("Xsplit is running!", LoggingMode.Debug);
            return true;
        }
        ModConsole.Msg("No recording software detected.", LoggingMode.Debug);
        return false;
    }
        
    public static SpawnableCrateReference GetRandomEntity(List<SpawnableCrateReference> entities)
    {
        var validEntities = entities.Where(entity => int.Parse(entity.Crate.Description) > 0).ToList();

        if (validEntities.Count == 0)
        {
            ModConsole.Error("No valid entities to spawn!");
            return null;
        }
            
        int totalPercentage = validEntities.Sum(entity => int.Parse(entity.Crate.Description));

        int randomValue = Random.Range(0, totalPercentage);
            
        int cumulativePercentage = 0;
        foreach (SpawnableCrateReference entity in validEntities)
        {
            cumulativePercentage += int.Parse(entity.Crate.Description);
            if (randomValue < cumulativePercentage)
            {
                return entity;
            }
        }
            
        return null;
    }
}