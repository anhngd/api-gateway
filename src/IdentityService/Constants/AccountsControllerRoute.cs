namespace IdentityService.Constants
{
    public static class AccountsControllerRoute
    {
        public const string GetAllAccount = ControllerName.Account + nameof(GetAllAccount);
        public const string PostAccount = ControllerName.Account + nameof(PostAccount);
        public const string GetAccountRole = ControllerName.Account + nameof(GetAccountRole);

        public const string GetMyAccount = ControllerName.Account + nameof(GetMyAccount);
        public const string PutMyAccount = ControllerName.Account + nameof(PutMyAccount);

        public const string GetAccount = ControllerName.Account + nameof(GetAccount);
        public const string PutAccount = ControllerName.Account + nameof(PutAccount);
        public const string DeleteAccount = ControllerName.Account + nameof(DeleteAccount);

        public const string PostAccountNote = ControllerName.Account + nameof(PostAccountNote);
        public const string PostAccountStatus = ControllerName.Account + nameof(PostAccountStatus);
        public const string PutAccountUser = ControllerName.Account + nameof(PutAccountUser);
        public const string DeleteAccountUser = ControllerName.Account + nameof(DeleteAccountUser);
        public const string PutAccountLock = ControllerName.Account + nameof(PutAccountLock);
        public const string PutAccountUnLock = ControllerName.Account + nameof(PutAccountUnLock);
    }
}
