using UnityEngine;

public class Constants : MonoBehaviour
{   
    public static float DEFAULT_WAITING_SECONDS = 0.05f;
    public static int DEFAULT_START_LINE = 1;
    public static int DEFAULT_UNEXiST_NUMBER = -1;

    public static string STORY_PATH = "Assets/Resources/story/";
    public static string DEFAULT_STORY_FILE_NAME = "1";
    public static string DEFAULT_FILE_EXTENSION = ".xlsx";

    public static string BACKGROUND_PATH = "image/background/";
    public static string AVATAR_PATH = "image/avatar/";
    public static string CHARACTER_PATH = "image/character/";
    public static string VOCAL_PATH = "audio/vocal/";
    public static string MUSIC_PATH = "audio/music/";
    
    
    public static string AUDIO_LOAD_FAILED = "Failed to load audio file: ";
    public static string MUSIC_LOAD_FAILED = "Failed to load music file: ";
    public static string IMAGE_LOAD_FAILED = "Failed to load image file: ";

    public static string NO_DATA_FOUND = "NO data found";
    public static string END_OF_STORY = "End of story";
    public static string CHOICE = "choice";

    public static string CHARACTERACTION_APPEARAT = "appearAt";
    public static int DEFAULT_APPEARAT_START_POSITION = 9;          //appearAt( �ӵ�9��λ�ö�ȡ��ֵ
    public static int DEFAULT_APPEARAT_IRRELEVANT_CHAR = 10;   //�޹��ַ�����Ϊ10��ֻ��ȡ��x��y���ĳ���
    public static string CHARACTERACTION_DISAPPEAR = "disappear";
    
    public static string CHARACTERACTION_MOVETO = "moveTo";
    public static int DEFAULT_MOVETO_START_POSITION = 7;          //moveTo( �ӵ�7��λ�ö�ȡ��ֵ
    public static int DEFAULT_MOVETO_IRRELEVANT_CHAR = 8;       //�޹��ַ�����Ϊ8��ֻ��ȡ��x��y���ĳ���
    public static int DEFAULT_DURATION_TIME = 1;                    //��������ʱ��Ϊ1��
    public static string COORDINATE_MISSING = "Coordinate missing";
}
