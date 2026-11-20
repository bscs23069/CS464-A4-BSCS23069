using UnityEngine;

// The only script that spells a PlayerPrefs key. Everything else asks through here,
// so the goal that writes a best time and the menu that reads it can't disagree on a name.
public static class SaveData
{
    public const int LevelCount = 5;

    const string UnlockedKey = "unlocked";
    const string BestPrefix = "best";

    // one function builds the per-level key for the writer and the reader
    static string BestKey(int level) => BestPrefix + level;

    // a new player has Level 1 open and nothing else
    public static int Unlocked => PlayerPrefs.GetInt(UnlockedKey, 1);

    public static bool IsUnlocked(int level) => level <= Unlocked;

    public static void Unlock(int level)
    {
        level = Mathf.Min(level, LevelCount); // winning Level 5 has nothing left to open
        if (level <= Unlocked)
            return; // unlocking never goes backwards
        PlayerPrefs.SetInt(UnlockedKey, level);
        PlayerPrefs.Save();
    }

    // HasKey first: a best time of 0 would beat every real time
    public static bool HasBest(int level) => PlayerPrefs.HasKey(BestKey(level));

    public static float Best(int level) => PlayerPrefs.GetFloat(BestKey(level), 0f);

    // true when this time is a new best
    public static bool Record(int level, float time)
    {
        if (HasBest(level) && time >= Best(level))
            return false;
        PlayerPrefs.SetFloat(BestKey(level), time);
        PlayerPrefs.Save();
        return true;
    }

    // volumes are saved under the mixer parameter's own name, MusicVolume or SfxVolume, full volume by default
    public static float Volume(string parameter) => PlayerPrefs.GetFloat(parameter, 1f);

    public static void SetVolume(string parameter, float value)
    {
        PlayerPrefs.SetFloat(parameter, value);
        PlayerPrefs.Save();
    }
}
