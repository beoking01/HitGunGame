public static class SaveGameFacade
{
    public static void SaveAllPersistentStates()
    {
        if (TruckStateManager.Instance != null)
        {
            TruckStateManager.Instance.SaveToDisk();
        }

        if (RoomStateManager.Instance != null)
        {
            RoomStateManager.Instance.SaveToDisk();
        }
    }
}