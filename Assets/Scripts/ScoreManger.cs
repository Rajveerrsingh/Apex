public static class ScoreManager
{
    public static int Score { get; private set; }

    public static void Add(int points)
    {
        Score += points;
    }

    public static void Reset()
    {
        Score = 0;
    }
}