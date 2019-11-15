namespace IdentityService.Constants
{
    public static class EmailTemplatesControllerRoute
    {
        public const string GetConfirmNewAccount = ControllerName.EmailTemplate + nameof(GetConfirmNewAccount);
        public const string PutConfirmNewAccount = ControllerName.EmailTemplate + nameof(PutConfirmNewAccount);
        public const string GetResetPassword = ControllerName.EmailTemplate + nameof(GetResetPassword);
        public const string PutResetPassword = ControllerName.EmailTemplate + nameof(PutResetPassword);
    }
}
