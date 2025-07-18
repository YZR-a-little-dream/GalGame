using System;
using JetBrains.Annotations;
using UnityEngine;

public class Constants : MonoBehaviour
{
    public static float DEFAULT_TYPING_SPEED = 0.05f;
    public static float DEFAULT_SKIP_MODE_TYPING_SPEED = 0.01f;
    public static int DEFAULT_STORY_START_LINE = 1;
    public static int DEFAULT_UNEXiST_NUMBER = -1;

    public static string STORY_PATH = "story";
    public static string DEFAULT_STORY_FILE_NAME = "1";
    public static string STORY_FILE_EXTENSION = ".xlsx";

    public static string BACKGROUND_PATH = "image/background/";
    public static string AVATAR_PATH = "image/avatar/";
    public static string CHARACTER_PATH = "image/character/";
    public static string THUMBNATL_PATH = "image/thumbnail/";        //缩略图
    public static string BUTTON_PATH = "image/button/";
    public static string VOCAL_PATH = "audio/vocal/";
    public static string MUSIC_PATH = "audio/music/";

    public static string VOCAL_LOAD_FAILED = "Failed to load music";
    public static string MENU_MUSIC_FILE_NAME = "1";
    public static string CREDITS_MUSIC_FILE_NAME = "4";


    public static string AUDIO_LOAD_FAILED = "Failed to load audio file: ";
    public static string MUSIC_LOAD_FAILED = "Failed to load music file: ";
    public static string IMAGE_LOAD_FAILED = "Failed to load image file: ";
    public static string BIG_IMAGE_LOAD_FAILED = "Failed to load big image : ";

    public static string NO_DATA_FOUND = "NO data found";
    public static string END_OF_STORY = "End of story";
    public static string CHOICE = "Choice";

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
    public static string SAVE_GAME = "save_game";
    public static string LOAD_GAME = "load_game";
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
    public static string GALLERY = "gallery";
    public static string GALLERY_PACEHOLDER = "gallery_placeholder";


    public static string PROMPT_TEXT = "Please Enter Your Name";
    //public static string DEAFULT_PLAYERNAME = "Player";

    public static string NAME_PLACEHOLDER = "[Name]";

    public static string GOTO = "goto";
    public static string GAME = "game";
    public static string APPEARAT_INSTANTLY = "appearAtInstantly";

    //Localization
    public static string DEFAULT_LANGUAGE = "zh";
    public static string LANGUAGE_PATH = "languages";
    public static string JSON_FILE_EXTENSION = ".json";
    public static string LOCALIZATION_LOAD_FAILED = "Failed to load localization file:";
    public static int DEFAULT_LANGUAGE_INDEX = 0;

    public static string CHINESE = "中文";
    public static string ENGLISH = "English";
    public static string JAPANESE = "日本語";
    public static string[] LANGUAGES = { "zh", "en", "ja" };

    #region  IntroManger
    public static string videoPath = "video";
    public static string VIDEO_FILE_EXTENSION = ".mp4";

    #endregion

    #region  Scenes

    public static string MENU_SCENE = "MenuScene";

    public static string GAME_SCENE = "GameScene";

    public static string INPUT_SCENE = "InputScene";

    public static string SAVE_LOAD_SCENE = "SaveLoadScene";

    public static string Gallery_SCENE = "GalleryScene";
    public static string Setting_SCENE = "SettingScene";
    public static string HISTORY_SCENE = "HistoryScene";
    public static string CREDITS_SCENE = "CreditsScene";

    #endregion

    #region  本地化
    public static string PREV_PAGE = "previous_page";
    public static string NEXT_PAGE = "next_page";
    public static string BACK = "back";
    public static string CLOSE = "close";
    public static string CONFIRM = "confirm";
    public static string RESET = "reset";
    #endregion

    public static string MASTER_VOLUME = "MasterVolume";
    public static string MUSIC_VOLUME = "MusicVolume";
    public static string VOICE_VOLUME = "VoiceVolume";

    public static float DEFAULT_VOLUME = 0.8f;

    public static string CREDITS_PATH = "credits";
    public static string CREDITS_FILE_EXTENSION = ".txt";
    public static string CREDITS_SCROLL_END = "Credits scrolling ended";
    public static float CREDITS_SCROLL_END_Y = 2000f;
    public static string CREDITS_LOAD_FILED = "failed to load credits file:";
    public static float DEFAULT_MULTIPLIER = 1f;
    public static float DEFAULT_FAST_MULTIPLIER = 3f;

    public static char SHOICEDELIMITER = '\n';
    public static string CONFIRM_COVER_SAVE_FILE = "override_sav";
    public static string CONFIRM_DELETE_SAVE_FILE = "delete_sav";

    public static int QUICK_SAVE_SLOT = 0; // 快速保存槽位
    public static string CHARACTERCOMMANDS = "CharacterCommands"; // 角色指令块标识符
}
