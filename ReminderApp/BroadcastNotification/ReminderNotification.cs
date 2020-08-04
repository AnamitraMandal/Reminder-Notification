using System;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.V4.App;
using Newtonsoft.Json;
using NotificationReminder.Model;
using NotificationReminder.Repository;
using ReminderApp;

namespace NotificationReminder.BroadcastNotification
{
    [BroadcastReceiver(Enabled = true)]
    public class ReminderNotification : BroadcastReceiver
    {
        Reminder reminder;
        public ReminderNotification()
        {
        }

        [Obsolete]
        public override void OnReceive(Context context, Intent intent)
        {
            reminder = ReminderHelper.SelectReminder(context);
            if (reminder != null)
            {
                
                Intent newIntent = new Intent(context, typeof(ReminderContent));
                newIntent.PutExtra("reminder", JsonConvert.SerializeObject(reminder));

                //Add the next task to the stack  
                Android.Support.V4.App.TaskStackBuilder stackBuilder = Android.Support.V4.App.TaskStackBuilder.Create(context);
                stackBuilder.AddParentStack(Java.Lang.Class.FromType(typeof(ReminderContent)));
                stackBuilder.AddNextIntent(newIntent);

                // set the intent which will open when one click on notification  
                PendingIntent resultPendingIntent = stackBuilder.GetPendingIntent(0, (int)PendingIntentFlags.UpdateCurrent);

                NotificationCompat.Builder builder = new NotificationCompat.Builder(context).SetAutoCancel(true)
                    .SetDefaults((int)NotificationDefaults.All)
                    .SetContentIntent(resultPendingIntent).SetContentTitle("Reminder!!")
                    .SetSmallIcon(Resource.Drawable.notify_panel_notification_icon_bg).SetContentText("Click for details..")
                    .SetContentInfo("Start");
                NotificationManager notificationManager = (NotificationManager)context.GetSystemService(Context.NotificationService);
                notificationManager.Notify(2, builder.Build());
            }
        }
    }
}