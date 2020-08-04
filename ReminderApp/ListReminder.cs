using System;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using NotificationReminder.Model;
using NotificationReminder.Repository;
using ReminderApp;

namespace NotificationReminder
{
    [Activity(Label = "ReminderList")]
    public class ListReminder : Activity
    {
        TextView _txtLabel;
        ListView list;
        Reminder reminder;
        Reminder[] listitem;
        Button addNew;
        GridReminder adapter;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here  
            SetContentView(Resource.Layout.ListReminder);
            _txtLabel = FindViewById<TextView>(Resource.Id.txt_label);
            list = (ListView)FindViewById(Resource.Id.listReminder);
            _txtLabel.Visibility = Android.Views.ViewStates.Invisible;
            BindData();
            addNew = FindViewById<Button>(Resource.Id.addNew);
            addNew.Click += (sender, e) => {
                StartActivity(typeof(MainActivity));
            };
        }
        private void List_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            Android.App.AlertDialog.Builder dialog = new AlertDialog.Builder(this);
            AlertDialog alert = dialog.Create();
            alert.SetTitle("Delete");
            alert.SetMessage("Are you sure!");
            alert.SetIcon(Resource.Drawable.notification_template_icon_low_bg);
            alert.SetButton("yes", (c, ev) =>
            {
                reminder = listitem[e.Position];
                ReminderHelper.DeleteReminder(this, reminder);
                StartActivity(new Intent(this, typeof(DeleteReminder)));
                GC.Collect();
            });
            alert.SetButton2("no", (c, ev) => { });
            alert.Show();
        }
        private void BindData()
        {
            listitem = ReminderHelper.GetReminderList(this).ToArray();
            if (listitem.Length > 0)
            {
                adapter = new GridReminder(this, listitem);
                list.Adapter = adapter;
                list.ItemClick += List_ItemClick;
            }
            else
            {
                list.Visibility = Android.Views.ViewStates.Invisible;
                _txtLabel.Visibility = Android.Views.ViewStates.Visible;
                _txtLabel.Text = "No upcoming reminders!!";
            }


        }
    }
}