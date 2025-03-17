using JetBrains.Annotations;
using UnityEngine;

public class Constants : MonoBehaviour
{   
    public static float DEFAULT_TYPING_SPEED = 0.05f;
    public static float DEFAULT_SKIP_MODE_TYPING_SPEED = 0.01f;
    public static int DEFAULT_START_LINE = 1;
    public static int DEFAULT_UNEXiST_NUMBER = -1;

    public static string STORY_PATH = "Assets/Resources/story/";
    public static string DEFAULT_STORY_FILE_NAME = "1";
    public static string DEFAULT_FILE_EXTENSION = ".xlsx";

    public static string BACKGROUND_PATH = "image/background/";
    public static string AVATAR_PATH = "image/avatar/";
    public static string CHARACTER_PATH = "image/character/";
    public static string BUTTON_PATH = "image/button/";
    public static string VOCAL_PATH = "audio/vocal/";
    public static string MUSIC_PATH = "audio/music/";
    
    
    public static string AUDIO_LOAD_FAILED = "Failed to load audio file: ";
    public static string MUSIC_LOAD_FAILED = "Failed to load music file: ";
    public static string IMAGE_LOAD_FAILED = "Failed to load image file: ";

    public static string NO_DATA_FOUND = "NO data found";
    public static string END_OF_STORY = "End of story";
    public static string CHOICE = "choice";

    public static string CHARACTERACTION_APPEARAT = "appearAt";
    public static int DEFAULT_APPEARAT_START_POSITION = 9;          //appearAt( 从第9个位置读取数值
    public static int DEFAULT_APPEARAT_IRRELEVANT_CHAR = 10;   //无关字符数量为10，只读取（x，y）的长度
    public static string CHARACTERACTION_DISAPPEAR = "disappear";
    
    public static string CHARACTERACTION_MOVETO = "moveTo";
    public static int DEFAULT_MOVETO_START_POSITION = 7;          //moveTo( 从第7个位置读取数值
    public static int DEFAULT_MOVETO_IRRELEVANT_CHAR = 8;       //无关字符数量为8，只读取（x，y）的长度
    public static int DEFAULT_DURATION_TIME = 1;                    //动画持续时间为1秒
    public static string COORDINATE_MISSING = "Coordinate missing";

    public static string AUTO_ON = "autoplayon";
    public static string AUTO_OFF = "autoplayoff";
    public static float DEFAULT_AUTO_WAITING_SECONDS = 0.1f;
    
    public static string SKIP_ON = "skipon";
    public static string SKIP_OFF = "skioff";
    public static float DEFAULT_SKIP_WAITING_SECONDS = 0.02f;

    public static int DEFAULT_START_INDEX = 0;
    public static int SLOTS_PER_PAGE = 8;                         //每页显示8个栏位
    public static int TOTAL_SLOTS = 40;                           //总共40个栏位
    public static string COLON = ": ";
    public static string SAVE_GAME = "Save_game";
    public static string LOAD_GAME = "Load_game";
    public static string Empty_SLOT = "Empty_slot";

}
