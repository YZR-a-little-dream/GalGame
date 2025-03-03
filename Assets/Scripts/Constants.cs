using UnityEngine;

public class Constants : MonoBehaviour
{   
    public static float DEFAULT_WAITING_SECONDS = 0.05f;
    public static int DEFAULT_START_LINE = 1;
    public static int DEFAULT_UNEXiST_NUMBER = -1;

    public static string STORY_PATH = "Assets/Resources/story/";
    public static string DEFAULT_STORY_FILR_NAME = "1.xlsx";

    public static string AVATAR_PATH = "image/avatar/";
    public static string VOCAL_PATH = "audio/vocal/";
    public static string BACKGROUND_PATH = "image/background/";
    public static string MUSIC_PATH = "audio/music/";
    public static string CHARACTER_PATH = "image/character/";
    
    public static string AUDIO_LOAD_FAILED = "Failed to load audio file: ";
    public static string MUSIC_LOAD_FAILED = "Failed to load MUSIC file: ";
    public static string IMAGE_LOAD_FAILED = "Failed to load image file: ";

    public static string NO_DATA_FOUND = "NO data found";
    public static string END_OF_STORY = "End of story";

    public static string CHARACTERACTION_APPEARAT = "appearAt";
    public static string CHARACTERACTION_DISAPPEAR = "disappear";
    public static string CHARACTERACTION_MOVETO = "moveTo";
}
