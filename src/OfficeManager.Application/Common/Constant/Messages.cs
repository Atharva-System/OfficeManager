using System.Net;

namespace OfficeManager.Application.Common.Constant
{
    public static class Messages
    {
        public static string Success => "Success";
        public static string RegisteredSuccesfully => "Registered Successfully";
        public static string AddedSuccesfully => "Added Successfully";
        public static string UpdatedSuccessfully => "Updated Successfully";
        public static string DeletedSuccessfully => "Deleted Successfully";
        public static string InvalidCredentials => "Invalid credentials endtered.";
        public static string NotFound => "Entity not found";
        public static string DataFound => "Data found";
        public static string NoDataFound => "No Data found";
        public static string IssueWithData => "There is some issue with the data";
        public static string CheckCredentials => "Please check login credentials";
        public static string UserNameOrPasswordIsIncorrect => "Username or password is incorrect";
        public static string ConfirmYourEmail => "Please confirm your email";
        public static string EmailIsAlreadyExist => "Email is already exist";
        public static string UsernameIsAlreadyExist => "Username is already exist";
        public static string PasswordDontMatchWithConfirmation => "Password doesn't match its confirmation";
        public static string RegisterSuccessfully => "Register successfuly please look at your mail box for account confirmation.";
        public static string UserNotFound => "User not found";
        public static string TokenOrUserNotFound => "Token or User Not Found";
        public static string RefreshTokenNotFound => "Refresh Token Not Found";
        public static string AlreadyEmailConfirmed => "Already your email confirmed";
        public static string SuccessfullyEmailConfirmed => "Email confirmed successfully.You can login now";
        public static string RefreshTokenExpired => "Refresh Token Expired";
        public static string RoleNameAlreadyExist => "Role Name Already Exist";
        public static string UserRolesUpdatedSuccessfully => "User Roles Updated Successfully";
        public static string PasswordChangedSuccessfully => "Password changed successfully";
        public static string CurrentPasswordIsFalse => "Current Password is false";
        public static string IfEmailTrue => "If your email address is entered correctly, you will receive a link to reset your password.";
        public static string PasswordSuccessfullyReset => "Your password has been successfully reset.Your new password has been sent to your email address.We recommend that you change your password.";
        public static string ResetPasswordCodeInvalid => "Your Reset Password Code is invalid";
        public static string EmailSuccessfullyChangedConfirmYourEmail => "Email Successfully Changed.Please confirm your email";
    }

    public static class StatusCodes
    {
        //200
        public static string Accepted => HttpStatusCode.Accepted.ToString();

        //400
        public static string BadRequest => HttpStatusCode.BadRequest.ToString();

        //404
        public static string NotFound => HttpStatusCode.NotFound.ToString();

        //500
        public static string InternalServerError => HttpStatusCode.InternalServerError.ToString();

        //401
        public static string Forbidden => HttpStatusCode.Forbidden.ToString();
    }
}
