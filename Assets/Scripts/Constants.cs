using System;
using JetBrains.Annotations;
using UnityEngine;

public class Constants : MonoBehaviour
{
    public static float DEFAULT_TYPING_SPEED = 0.05f;
    public static float DEFAULT_SKIP_MODE_TYPING_SPEED = 0.01f;
    public static int DEFAULT_START_LINE = 1;
    public static int DEFAULT_UNEXiST_NUMBER = -1;

    //public static string STORY_PATH = "Assets/Resources/story/";
    public static string DEFAULT_STORY_FILE_NAME = "1";
    public static string DEFAULT_FILE_EXTENSION = ".xlsx";

    public static string BACKGROUND_PATH = "image/background/";
    public static string AVATAR_PATH = "image/avatar/";
    public static string CHARACTER_PATH = "image/character/";
    public static string THUMBNATL_PATH = "image/thumbnail/";        //缩略图
    public static string BUTTON_PATH = "image/button/";
    public static string VOCAL_PATH = "audio/vocal/";
    public static string MUSIC_PATH = "audio/music/";


    public static string AUDIO_LOAD_FAILED = "Failed to load audio file: ";
    public static string MUSIC_LOAD_FAILED = "Failed to load music file: ";
    public static string IMAGE_LOAD_FAILED = "Failed to load image file: ";
    public static string BIG_IMAGE_LOAD_FAILED = "Failed to load big image : ";

    public static string NO_DATA_FOUND = "NO data found";
    public static string END_OF_STORY = "End of story";
    public static string CHOICE = "choice";

    public static string CHARACTERACTION_APPEARAT = "appearAt";
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

    public static string CAMERA_NOT_FOUND = "Main camera  notfound.";

    public static string SAVE_FILE_PATH = "saves";
    public static string SAVE_FILE_EXTENSION = ".json";

    public static int DEFAULT_MAX_LENGTH = 50;

    public static string RESOLUTION = "Resolution";
    public static string FULLSCREEN = "Fullscreen";
    public static string WINDIW = "Window";

    public static int GALLERY_SLOTS_PER_PAGE = 9;

    //其他背景文件名
    public static String[] ALL_BACKGROUNDS =
    {
        "1","2","3","4","c_青空","c_青空2","c_青空3","c_月01","c_月03","c_月04","c_月05"
    };
    public static string GALLERY = "Gallery";
    public static string GALLERY_PACEHOLDER = "gallery_placeholder";

    public static string CONFIRM = "确认名字";
    public static string PROMPT_TEXT = "Please Enter Your Name";
    //public static string DEAFULT_PLAYERNAME = "Player";

    public static string NAME_PLACEHOLDER = "[Name]";

    public static string GOTO = "goto";
    public static string APPEARAT_INSTANTLY = "appearAtInstantly";

    //Localization
    public static string DEFAULT_LANGUAGE = "zh";
    public static string LANGUAGE_PATH = "languages";
    public static string JSON_FILE_EXTENSION = ".json";
    public static string LOCALIZATION_LOAD_FAILED = "Failed to load localization file:";
    public static int DEFAULE_LANGUAGE_INDEX = 0;

    public static string CHINESE = "中文";
    public static string ENGLISH = "English";
    public static string JAPANESE = "日本語";
    public static string[] LANGUAGES = { "zh", "en", "ja" };

    #region  IntroManger
    public static string videoPath = "video";
    public static string VIDEO_FILE_EXTENSION = ".mp4";
    public static string MENU_SCENE = "MenuScene";
    #endregion
}
