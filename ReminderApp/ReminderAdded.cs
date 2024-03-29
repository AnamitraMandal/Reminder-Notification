﻿using System;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using Newtonsoft.Json;
using NotificationReminder.Model;
using ReminderApp;

namespace NotificationReminder
{
    [Activity(Label = "ReminderAdded")]
    public class ReminderAdded : Activity
    {
        TextView _reminderLabel;
        TextView _dateDisplay;
        TextView _timeDisplay;
        TextView _txtNote;
        Button _backButton;
        Reminder reminder;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.ReminderAdded);
            // Create your application here  
            reminder = JsonConvert.DeserializeObject<Reminder>(Intent.GetStringExtra("reminder"));

            _reminderLabel = FindViewById<TextView>(Resource.Id.reminder_label);
            _dateDisplay = FindViewById<TextView>(Resource.Id.date_display);
            _timeDisplay = FindViewById<TextView>(Resource.Id.time_display);
            _txtNote = FindViewById<TextView>(Resource.Id.txt_note);
            _backButton = FindViewById<Button>(Resource.Id.btn_back);

            _reminderLabel.Text = "Reminder added successfully!!";
            _dateDisplay.Text = "Date : " + reminder.Date;
            _timeDisplay.Text = "Time : " + reminder.Time;
            _txtNote.Text = "Content: " + reminder.Note;

            _backButton.Click += (object obj, EventArgs e) => {
                StartActivity(new Intent(this, typeof(ListReminder)));
            };
        }
    }
}