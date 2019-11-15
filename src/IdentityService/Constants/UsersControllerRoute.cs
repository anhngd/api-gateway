namespace IdentityService.Constants
{
    public static class UsersControllerRoute
    {
        public const string GetMyUser = ControllerName.User + nameof(GetMyUser);
        public const string PutMyUser = ControllerName.User + nameof(PutMyUser);
        public const string PutMyPassUser = ControllerName.User + nameof(PutMyPassUser);

        public const string PostUser = ControllerName.User + nameof(PostUser);

        public const string GetAllUser = ControllerName.User + nameof(GetAllUser);
        public const string GetUser = ControllerName.User + nameof(GetUser);

        public const string PutUser = ControllerName.User + nameof(PutUser);
        public const string PutPassUser = ControllerName.User + nameof(PutPassUser);
        public const string PutLockUser = ControllerName.User + nameof(PutLockUser);
        public const string PutUnLockUser = ControllerName.User + nameof(PutUnLockUser);

        public const string DeleteUser = ControllerName.User + nameof(DeleteUser);

        public const string PostForgotPassword = ControllerName.User + nameof(PostForgotPassword);
        public const string GetResetPassword = ControllerName.User + nameof(GetResetPassword);
        public const string PostResetPassword = ControllerName.User + nameof(PostResetPassword);
    }
}
