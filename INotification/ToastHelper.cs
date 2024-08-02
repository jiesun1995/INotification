using Microsoft.Toolkit.Uwp.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace INotificationClient
{
    public static class ToastHelper
    {
        public static void ShowToastByText(string message)
        {
            new ToastContentBuilder()
            .AddText($"{message}")
            .Show();
        }


        //public void ShowToast()
        //{
        //    var toastContentBuilder = new ToastContentBuilder()
        //           .AddText($"{message}")
        //            .AddToastInput(new ToastSelectionBox("time")
        //            {
        //                DefaultSelectionBoxItemId = "0",
        //                Items =
        //                       {
        //                            new ToastSelectionBoxItem("0", "现在完成"),
        //                            new ToastSelectionBoxItem("1", "推迟1分钟"),
        //                            new ToastSelectionBoxItem("5", "推迟5分钟"),
        //                            new ToastSelectionBoxItem("10", "推迟10分钟")
        //                       }
        //            })
        //           .AddButton(new ToastButton()
        //           .SetContent("提交"));
        //    toastContentBuilder
        //    .Show();
        //}

        public static void ShowToast(string? message=null, string? iconAddress=null)
        {
            var toastContentBuilder = new ToastContentBuilder();
            var imgFileFullPath = Path.GetFullPath($"Images\\MarketeqNotificationBell.png");
            var imgUri = new Uri($"file:///{imgFileFullPath}");
            if (!string.IsNullOrEmpty(message)) toastContentBuilder.AddText($"{message}");
            if (!string.IsNullOrEmpty(iconAddress))
            {
                var fileFullName = DownloadImg(iconAddress);
                imgFileFullPath = Path.GetFullPath($"{fileFullName}");
                var fileUriString = $"file:///{imgFileFullPath}";
                imgUri = new Uri(fileUriString);
            }
            toastContentBuilder.AddAppLogoOverride(imgUri, ToastGenericAppLogoCrop.Circle);
            //.AddText($"{message}")
            //.AddAppLogoOverride(imgUri, ToastGenericAppLogoCrop.Circle);
            toastContentBuilder.Show();
        }

        public static void ShowToastByIcon(string message,string iconAddress)
        {
            var fileFullName = DownloadImg(iconAddress);
            var imgFileFullPath = Path.GetFullPath($"{fileFullName}");
            var fileUriString = $"file:///{imgFileFullPath}";
            var imgUri = new Uri(fileUriString);

            var toastContentBuilder = new ToastContentBuilder()
                    .AddText($"{message}")
                    .AddAppLogoOverride(imgUri, ToastGenericAppLogoCrop.Circle);
                    ;
            toastContentBuilder
            .Show();
        }


        private static string DownloadImg(string? imgAddress)
        {
            if(string.IsNullOrEmpty(imgAddress))
                return "";
            Uri uri = new Uri(imgAddress);
            string fileName = System.IO.Path.GetFileName(uri.LocalPath);
            var fileFullName = $"Images/{fileName}";
            if (!File.Exists(fileFullName))
            {
                HttpClient httpClient = new HttpClient();
                var response = httpClient.GetAsync(imgAddress).Result;
                if (response.IsSuccessStatusCode)
                {
                    if (!Directory.Exists("Images"))
                        Directory.CreateDirectory("Images");
                    using (var fileStream = System.IO.File.Create($"Images/{fileName}"))
                        response.Content.CopyToAsync(fileStream).Wait();
                }
            }
            return fileFullName;
        }
    }
}
