using Android.App;
using Android.Content;
using Android.OS;
using OscVrcMaui.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
[assembly: Dependency(typeof(IForegroundWorker))]
namespace OscVrcMaui
{
    [Service]
    internal class ForegroundWorker : Service, IForegroundWorker
    {
        private OSCMainService oscOmni = DependencyService.Get<OSCMainService>();
        public override IBinder OnBind(Intent intent)
        {
            return null;
        }
        public const int SERVICE_RUNNING_NOTIFICATION_ID = 228228;
        public void StartWorker()
        {
            var intent = new Intent(Android.App.Application.Context, typeof(ForegroundWorker));
            if (Android.OS.Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.O)
            {
                Android.App.Application.Context.StartForegroundService(intent);

            }
            else
            {
                Android.App.Application.Context.StartService(intent);
            }
        }
        public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
        {

            CreateNotificationChannel();
            string messageBody = "SendingData to host";
            var notification = new Notification.Builder(this, "228")
            .SetContentTitle("Osc worker service")
            .SetContentText(messageBody)
            .SetSmallIcon(Resource.Drawable.abc_switch_track_mtrl_alpha)
            .SetOngoing(true)
            .Build();
            StartForeground(SERVICE_RUNNING_NOTIFICATION_ID, notification);
            //do you work
            oscOmni.StartSendingBandDate();

            return StartCommandResult.Sticky;
        }
        void CreateNotificationChannel()
        {
            if (Build.VERSION.SdkInt < BuildVersionCodes.O)
            {
                // Notification channels are new in API 26 (and not a part of the
                // support library). There is no need to create a notification
                // channel on older versions of Android.
                return;
            }

            var channel = new NotificationChannel("228", "OscService", NotificationImportance.Default)
            {
                Description = "Osc worker service"
            };
            var notificationManager = (NotificationManager)GetSystemService(NotificationService);
            notificationManager.CreateNotificationChannel(channel);
        }

        public void StopWorker()
        {
         
            var intent = new Intent(Android.App.Application.Context, typeof(ForegroundWorker));
            Android.App.Application.Context.StopService(intent);
        }

        public void RestartWorker()
        {
            var intent = new Intent(Android.App.Application.Context, typeof(ForegroundWorker));
            Android.App.Application.Context.StopService(intent);
            StartWorker();
        }
    }
}
