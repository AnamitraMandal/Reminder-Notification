using System;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using NotificationReminder.Model;
using Newtonsoft.Json;
using NotificationReminder.Repository;
using NotificationReminder.BroadcastNotification;
using ReminderApp;

namespace NotificationReminder
{
    [Activity(Label = "NotificationReminder", MainLauncher = true)]
    public class MainActivity : Activity
    {
        public Reminder reminder;
        EditText _dateDisplay;
        EditText _timeDisplay;
        EditText _txtNote;
        Button _saveButton;
        Button _btnList;

        #region DateOperation  
        void DateSelect_OnClick(object sender, EventArgs eventArgs)
        {
            DatePickerFragment frag = DatePickerFragment.NewInstance(delegate (DateTime time)
            {
                _dateDisplay.Text = time.ToString().Split(' ')[0];
                reminder.Date = _dateDisplay.Text;
            });
            frag.Show(FragmentManager, DatePickerFragment.TAG);
        }
        #endregion

        #region TimeOperation  
        void TimeSelectOnClick(object sender, EventArgs eventArgs)
        {
            TimePickerFragment frag = TimePickerFragment.NewInstance(
                delegate (DateTime time)
                {
                    _timeDisplay.Text = time.ToShortTimeString();
                    reminder.Time = _timeDisplay.Text;
                });

            frag.Show(FragmentManager, TimePickerFragment.TAG);
        }
        #endregion

        #region SaveDetails  
        void SaveRecords(Object sender, EventArgs eventArgs)
        {
            reminder.Note = _txtNote.Text;
            if (Vaidate())
            {
                DateTime currentDT = DateTime.Now;
                DateTime selectedDT = Convert.ToDateTime(reminder.Date + " " + reminder.Time);

                // Reminder should not be applied on past date time  
                if (selectedDT > currentDT)
                {
                    ReminderHelper.InsertReminderData(this, reminder);
                    ScheduleReminder(reminder);
                    var reminderAdded = new Intent(this, typeof(ReminderAdded));
                    reminderAdded.PutExtra("reminder", JsonConvert.SerializeObject(reminder));
                    StartActivity(reminderAdded);
                }
                else
                {
                    Toast.MakeText(this, "This is invalid selelction of Date, Time!", ToastLength.Short).Show();
                }
            }

        }

        bool Vaidate()
        {
            if (reminder.Date == string.Empty || reminder.Time == string.Empty || reminder.Note == string.Empty)
            {
                Toast.MakeText(this, "Enter the details of all fields!", ToastLength.Short).Show();
                return false;
            }
            return true;
        }
        #endregion

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Main);
            // Create your application here  
            reminder = new Reminder();
            _dateDisplay = FindViewById<EditText>(Resource.Id.date_display);
            _timeDisplay = FindViewById<EditText>(Resource.Id.time_display);
            _txtNote = FindViewById<EditText>(Resource.Id.txtNote);

            _saveButton = FindViewById<Button>(Resource.Id.save);
            _btnList = FindViewById<Button>(Resource.Id.btnList);

            _dateDisplay.Click += DateSelect_OnClick;
            _timeDisplay.Click += TimeSelectOnClick;
            _saveButton.Click += SaveRecords;
            _btnList.Click += (sender, e) => {
                StartActivity(new Intent(this, typeof(ListReminder)));
            };
        }

        #region ReminderNotification  
        public void ScheduleReminder(Reminder reminder)
        {
            AlarmManager manager = (AlarmManager)GetSystemService(Context.AlarmService);
            Intent myIntent;
            PendingIntent pendingIntent;
            myIntent = new Intent(this, typeof(ReminderNotification));

            //Date and time should be in the format 11/1/1970 and 12:00 AM  
            // get the time parameters hrr,min, am/pm  
            var t = reminder.Time.Split(':');
            var ampm = t[1].Split(' ')[1];
            var hrr = Convert.ToDouble(t[0]);
            var min = Convert.ToDouble(t[1].Split(' ')[0]);

            string dateString = Convert.ToString(reminder.Date + " " + hrr + ":" + min + ":00 " + ampm);

            DateTimeOffset dateOffsetValue = DateTimeOffset.Parse(dateString);
            var millisec = dateOffsetValue.ToUnixTimeMilliseconds();

            pendingIntent = PendingIntent.GetBroadcast(this, 0, myIntent, 0);
            manager.Set(AlarmType.RtcWakeup, millisec, pendingIntent);
        }
        #endregion
    }
}