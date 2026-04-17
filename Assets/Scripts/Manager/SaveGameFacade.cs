public static class SaveGameFacade
{
    public static void SaveAllPersistentStates()
    {
        if (PointManager.Instance != null)
        {
            PointManager.Instance.SaveToDisk();
        }

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