namespace IdentityService.Constants
{
    public static class ActivityHistoriesControllerRoute
    {
        public const string GetAllActivityHistory = ControllerName.User + nameof(GetAllActivityHistory);
        public const string GetMyActivityHistory = ControllerName.User + nameof(GetMyActivityHistory);
        public const string GetActivityHistoryById = ControllerName.User + nameof(GetActivityHistoryById);
        public const string GetActivityHistoryByUserId = ControllerName.User + nameof(GetActivityHistoryByUserId);
        public const string PostActivityHistory = ControllerName.User + nameof(PostActivityHistory);
    }
}
