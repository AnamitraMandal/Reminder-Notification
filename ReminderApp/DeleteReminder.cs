using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using ReminderApp;

namespace NotificationReminder
{
    [Activity(Label = "DeleteReminder")]
    public class DeleteReminder : Activity
    {
        TextView _txtLabel;
        Button _btn_back;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.DeleteReminder);
            // Create your application here  
            _txtLabel = FindViewById<TextView>(Resource.Id.txt_label);
            _btn_back = FindViewById<Button>(Resource.Id.btn_back);
            _txtLabel.Text = "Deleted!!";
            _btn_back.Click += (sender, e) => {
                StartActivity(new Intent(this, typeof(ListReminder)));
            };
        }
    }
}