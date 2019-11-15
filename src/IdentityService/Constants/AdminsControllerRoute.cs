namespace IdentityService.Constants
{
    public static class AdminsControllerRoute
    {
        public const string GetAllAdmin = ControllerName.Admin + nameof(GetAllAdmin);
        public const string PostAdmin = ControllerName.Admin + nameof(PostAdmin);
        public const string GetAdmin = ControllerName.Admin + nameof(GetAdmin);
        public const string PutAdmin = ControllerName.Admin + nameof(PutAdmin);
        public const string DeleteAdmin = ControllerName.Admin + nameof(DeleteAdmin);

        public const string PutAdminGroup = ControllerName.Admin + nameof(PutAdminGroup);
        public const string PutAdminPermission = ControllerName.Admin + nameof(PutAdminPermission);
        public const string PutAdminUser = ControllerName.Admin + nameof(PutAdminUser);
        public const string DeleteAdminUser = ControllerName.Admin + nameof(DeleteAdminUser);
        public const string PutAdminLock = ControllerName.Admin + nameof(PutAdminLock);
        public const string PutAdminUnLock = ControllerName.Admin + nameof(PutAdminUnLock);
    }
}
