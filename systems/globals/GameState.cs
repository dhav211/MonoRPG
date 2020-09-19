namespace MonoRPG
{
    public static class GameState
    {
        enum State { GAMEPLAY, MENU_OPEN, CUTSCENE, GAME_OVER }
        static State currentState = State.GAMEPLAY;

        public static bool CanPlayerMove()
        {
            if (currentState == State.GAMEPLAY)
                return true;
            
            return false;
        }

        public static void OpenMenu()
        {
            currentState = State.MENU_OPEN;
        }

        public static void CloseMenu()
        {
            currentState = State.GAMEPLAY;
        }
    }
}