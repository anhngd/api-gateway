namespace IdentityServer.Common
{
    using System.Net;
    using System.Net.Mail;
    public static class EmailExtension
    {
        public static void SendEmail(string emailToSend, string recvName, string linkConfirm, string type)
        {
            var appSettingValue = ConfigurationExtension.AppSetting;

            var smtp = new SmtpClient
            {
                Host = appSettingValue.GetSection("SMTPServer").GetSection("HostAddress").Value,
                Port = 587,
                EnableSsl = true,
                UseDefaultCredentials = false
            };
            var credentials = new NetworkCredential
            {
                UserName = appSettingValue.GetSection("SMTPServer").GetSection("UserName").Value,
                Password = appSettingValue.GetSection("SMTPServer").GetSection("Password").Value,
            };
            smtp.Credentials = credentials;

            var addressFrom = new MailAddress(appSettingValue.GetSection("SMTPServer").GetSection("MailAddress").Value, "Tripbricks Account");
            var addressTo = new MailAddress(emailToSend);

            linkConfirm = appSettingValue.GetSection("BaseUrl").Value + linkConfirm;

            switch (type)
            {
                case "REGISTER":
                {
                    var msg = new MailMessage(addressFrom, addressTo)
                    {
                        Subject = "Verify your email address: " + emailToSend,
                        IsBodyHtml = true,
                        Body = GetEmailRegisterBody(emailToSend, recvName, linkConfirm),
                    };
                    smtp.Send(msg);
                    break;
                }
                case "FORGOT_PASSWORD":
                {
                    var msg = new MailMessage(addressFrom, addressTo)
                    {
                        Subject = "Request to reset your password",
                        IsBodyHtml = true,
                        Body = GetEmailForgotPasswordBody(emailToSend, recvName, linkConfirm),
                    };
                    smtp.Send(msg);
                    break;
                }
                case "UPDATE_PASSWORD":
                {
                    var msg = new MailMessage(addressFrom, addressTo)
                    {
                        Subject = "Account updated!",
                        IsBodyHtml = true,
                        Body = GetEmailUpdatePasswordBody(emailToSend, recvName),
                    };
                    smtp.Send(msg);
                    break;
                }
            }
        }

        public static string GetEmailRegisterBody(string emailToSend, string recvName, string linkConfirm)
        {
            var htmlString =
                "<html>" +
                "<head>" +
                "</head>" +
                "<body>" +
                    "<div style='margin:0px;padding:0px;border:0px;background-color:#ffffff;font-family:BlinkMacSystemFont,-apple-system,Segoe UI,Roboto,Helvetica,Arial,sans-serif;font-size:14px;line-height:20px;color:#383838'>" +
                        "<div style='padding: height: 50px;'>" +
                            "<div style='width: 560px; margin:auto; padding: 20px; text-align: center;'>" +
                                "<img src='https://account.tripbricks.com/img/header-logo.png' style='width: 220px;'/>" +
                            "</div>" +
                        "</div>" +
                        "<table style='width: 600px; margin:auto;'>" +
                            "<tbody>" +
                                "<tr>" +
                                    "<td style='padding: 20px; color:#000000;'>" +
                                        "<p style='font-size: 16px; font-family:BlinkMacSystemFont,-apple-system,Segoe UI,Roboto,Helvetica,Arial,sans-serif;'>" +
                                            "Hi " + recvName + "," +
                                        "</p>" +
                                        "<h2>" + "Verify your email address" + "</h2>" +
                                        "<span  style='font-size: 16px; font-family:BlinkMacSystemFont,-apple-system,Segoe UI,Roboto,Helvetica,Arial,sans-serif;'>" +
                                            "You just create an account with your email address: " + emailToSend +
                                        "</span>" +
                                        "<br />" +
                                        "<span  style='font-size: 16px; font-family:BlinkMacSystemFont,-apple-system,Segoe UI,Roboto,Helvetica,Arial,sans-serif;'>" +
                                            "Press 'Confirm' to validate email address and unlock for the entire account." +
                                        "</span>" +
                                        "<br />" +
                                        "<span  style='font-size: 16px; font-family:BlinkMacSystemFont,-apple-system,Segoe UI,Roboto,Helvetica,Arial,sans-serif;'>" +
                                            "We will also enter the reservation you made with this email address." +
                                        "</span>" +
                                    "</td>" +
                                "</tr>" +
                                "<tr>" +
                                    "<td style='text-align: center;'>" +
                                        "<a style='font-family:BlinkMacSystemFont,-apple-system,Segoe UI,Roboto,Helvetica,Arial,sans-serif;margin:0px;padding:0px;border:0px;font-size:14px;line-height:20px;text-align:center;color:#ffffff;background:#5E194F;border-radius:3px;border:1px solid #5E194F;background:#5E194F;padding-top:8px;padding-right:16px;padding-bottom:8px;padding-left:16px; width: 80%; display: block; text-decoration: none; margin: auto;' valign='middle'" +
                                        "href='" + linkConfirm + "'" +
                                        "target='_blank'>" +
                                            "Confirm" +
                                        "</a>" +
                                    "</td>" +
                                "</tr>" +
                                "<tr>" +
                                    "<td style='text-align: center;'>" +
                                        "<p style='font-family:BlinkMacSystemFont,-apple-system,Segoe UI,Roboto,Helvetica,Arial,sans-serif;margin:0px;padding:0px;border:0px;font-size:12px;line-height:18px;color:#707070;margin-bottom:8px; margin-top: 50px;'>" +
                                            "Copyright © 2019 Tripbricks.com. All rights reserved. <br>" +
                                            "This email was sent by Tripbricks.com, Hong Ha Road 33/301, Phuc Tan, Hoan Hiem, Ha Noi, Viet Nam" +
                                        "</p>" +
                                        "<span>" +
                                            "<a target='_blank' href='https://www.tripbricks.com/terms-and-conditions' style='font-size: 12px; color: #5E194F;'>" + "Terms and Conditions" + "</a>" +
                                            "<span style='color:#C3C3D4;'> | </span>" +
                                            "<a target='_blank' href='https://www.tripbricks.com/privacy' style='font-size: 12px; color: #5E194F;'>" + "Privacy Policy" + "</a>" +
                                            "<span style='color:#C3C3D4;'> | </span>" +
                                            "<a target='_blank' href='https://www.tripbricks.com/about-us' style='font-size: 12px; color: #5E194F;'>" + "About us" + "</a>" +
                                        "</span>" +
                                    "</td>" +
                                "</tr>" +
                            "</tbody>" +
                        "<table>" +
                    "</div>" +
                "</body>" +
                "</html>";
            return htmlString;
        }

        public static string GetEmailForgotPasswordBody(string emailToSend, string recvName, string linkConfirm)
        {
            var htmlString =
                "<html>" +
                "<head>" + "</head>" +
                "<body>" +
                    "<div style='margin:0px;padding:0px;border:0px;background-color:#ffffff;font-family:BlinkMacSystemFont,-apple-system,Segoe UI,Roboto,Helvetica,Arial,sans-serif;font-size:14px;line-height:20px;color:#383838'>" +
                        "<div style='padding: height: 50px;'>" +
                            "<div style='width: 560px; margin:auto; padding: 20px; text-align: center;'>" +
                                "<img src='https://account.tripbricks.com/img/header-logo.png' style='width: 220px;'/>" +
                                "<br />" +
                                "<span>" + emailToSend + "</span>" +
                            "</div>" +
                        "</div>" +
                        "<table style='width: 600px; margin:auto;'>" +
                            "<tbody>" +
                                "<tr>" +
                                    "<td style='padding: 20px; color:#000000;'>" +
                                        "<p style='font-size: 16px; font-family:BlinkMacSystemFont,-apple-system,Segoe UI,Roboto,Helvetica,Arial,sans-serif;'>" +
                                            "Hi " + recvName + "," +
                                        "</p>" +
                                        "<h2>" + "Forgot your password?" + "</h2>" +
                                        "<span style='font-size: 16px; font-family:BlinkMacSystemFont,-apple-system,Segoe UI,Roboto,Helvetica,Arial,sans-serif;'>" + "Just click on the button below to choose a new one." + "</span>" +
                                    "</td>" +
                                "</tr>" +
                                "<tr>" +
                                    "<td style='text-align: center;'>" +
                                        "<a style='font-family:BlinkMacSystemFont,-apple-system,Segoe UI,Roboto,Helvetica,Arial,sans-serif;margin:0px;padding:0px;border:0px;font-size:14px;line-height:20px;text-align:center;color:#ffffff;background:#5E194F;border-radius:3px;border:1px solid #5E194F;background:#5E194F;padding-top:8px;padding-right:16px;padding-bottom:8px;padding-left:16px; width: 80%; display: block; text-decoration: none; margin: auto;' valign='middle'" +
                                        " href='" + linkConfirm + "'" + "target='_blank'>" +
                                            "Reset password" +
                                        "</a>" +
                                    "</td>" +
                                "</tr>" +
                                "<tr>" +
                                    "<td style='text-align: center;'>" +
                                        "<p style='font-family:BlinkMacSystemFont,-apple-system,Segoe UI,Roboto,Helvetica,Arial,sans-serif;margin:0px;padding:0px;border:0px;font-size:12px;line-height:18px;color:#707070;margin-bottom:8px; margin-top: 50px;'>" +
                                            "Copyright © 2019 Tripbricks.com. All rights reserved.<br>" +
                                            "This email was sent by Tripbricks.com, Hong Ha Road 33/301, Phuc Tan, Hoan Hiem, Ha Noi, Viet Nam" +
                                        "</p>" +
                                        "<span>" +
                                            "<a target='_blank' href='https://www.tripbricks.com/terms-and-conditions' style='font-size: 12px; color: #5E194F;'>" + "Terms and Conditions" + "</a>" +
                                            "<span style='color:#C3C3D4;'> | </span>" +
                                            "<a target='_blank' href='https://www.tripbricks.com/privacy' style='font-size: 12px; color: #5E194F;'>" + "Privacy Policy" + "</a>" +
                                            "<span style='color:#C3C3D4;'> | </span>" +
                                            "<a target='_blank' href='https://www.tripbricks.com/about-us' style='font-size: 12px; color: #5E194F;'>" + "About us" + "</a>" +
                                        "</span>" +
                                    "</td>" +
                                "</tr>" +
                            "</tbody>" +
                        "<table>" +
                    "</div>" +
                "</body>" +
                "</html>";
            return htmlString;
        }

        public static string GetEmailUpdatePasswordBody(string emailToSend, string recvName)
        {
            var htmlString =
                "<html>" +
                "<head>" + "</head>" +
                "<body>" +
                    "<div style='margin:0px;padding:0px;border:0px;background-color:#ffffff;font-family:BlinkMacSystemFont,-apple-system,Segoe UI,Roboto,Helvetica,Arial,sans-serif;font-size:14px;line-height:20px;color:#383838'>" +
                        "<div style='padding: height: 50px;'>" +
                            "<div style='width: 560px; margin:auto; padding: 20px; text-align: center;'>" +
                                "<img src='https://account.tripbricks.com/img/header-logo.png' style='width: 220px;'/>" +
                                "<br />" +
                                "<span>" + emailToSend + "</span>" +
                            "</div>" +
                        "</div>" +
                        "<table style='width: 600px; margin:auto;'>" +
                            "<tbody>" +
                                "<tr>" +
                                    "<td style='padding: 20px; color:#000000;'>" +
                                        "<p style='font-size: 16px; font-family:BlinkMacSystemFont,-apple-system,Segoe UI,Roboto,Helvetica,Arial,sans-serif;'>" +
                                            "Hello " + recvName + "," +
                                        "</p>" +
                                        "<h2>" + "There has been an update to your account" + "</h2>" +
                                        "<span style='font-size: 16px; font-family:BlinkMacSystemFont,-apple-system,Segoe UI,Roboto,Helvetica,Arial,sans-serif;'>" + "Your Tripbricks.com password for " + emailToSend + " was just changed" + "</span>" +
                                    "</td>" +
                                "</tr>" +
                                "<tr>" +
                                    "<td style='text-align: center;'>" +
                                        "<p style='font-family:BlinkMacSystemFont,-apple-system,Segoe UI,Roboto,Helvetica,Arial,sans-serif;margin:0px;padding:0px;border:0px;font-size:12px;line-height:18px;color:#707070;margin-bottom:8px; margin-top: 50px;'>" +
                                            "Copyright © 2019 Tripbricks.com. All rights reserved.<br>" +
                                            "This email was sent by Tripbricks.com, Hong Ha Road 33 / 301, Phuc Tan, Hoan Hiem, Ha Noi, Viet Nam" +
                                        "</p>" +
                                        "<span>" +
                                            "<a target='_blank' href='https://www.tripbricks.com/terms-and-conditions' style='font-size: 12px; color: #5E194F;'>Terms and Conditions</a>" +
                                            "<span style='color:#C3C3D4;'> | </span>" +
                                            "<a target='_blank' href='https://www.tripbricks.com/privacy' style='font-size: 12px; color: #5E194F;'>Privacy Policy</a>" +
                                            "<span style='color:#C3C3D4;'> | </span>" +
                                            "<a target='_blank' href='https://www.tripbricks.com/about-us' style='font-size: 12px; color: #5E194F;'>About us</a>" +
                                        "</span>" +
                                    "</td>" +
                                "</tr>" +
                            "</tbody>" +
                        "<table>" +
                    "</div>" +
                "</body>" +
                "</html>";
            return htmlString;
        }
    }
}
