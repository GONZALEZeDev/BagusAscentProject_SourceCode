public class WorldsManager
{
    private static WorldsManager Instance;

    public static WorldsManager instance
    {
        get
        {
            if (Instance == null)
            {
                Instance = new WorldsManager();
            }
            return Instance;
        }
    }

    public bool[] world1ChestsOpened;
    public bool[] world2ChestsOpened;
    public bool[] world3ChestsOpened;
    public bool[] world4ChestsOpened;

    public WorldsManager()
    {
        world1ChestsOpened = new bool[6] { false, false, false, false ,false, false };
        world2ChestsOpened = new bool[6] { false, false, false, false, false, false };
        world3ChestsOpened = new bool[5] { false, false, false, false, false };
        world4ChestsOpened = new bool[4] { false, false, false, false };
    }
}
