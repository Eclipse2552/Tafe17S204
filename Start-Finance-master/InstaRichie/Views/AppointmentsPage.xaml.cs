﻿using SQLite.Net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using StartFinance.Models;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace StartFinance.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AppointmentsPage : Page
    {

        SQLiteConnection conn; // adding an SQLite connection
        string path = Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, "Findata.sqlite");

        public AppointmentsPage()
        {
            this.InitializeComponent();

            NavigationCacheMode = Windows.UI.Xaml.Navigation.NavigationCacheMode.Enabled;

            // Initial database

            conn = new SQLite.Net.SQLiteConnection(new SQLite.Net.Platform.WinRT.SQLitePlatformWinRT(), path);

            // Creating Tables

             Result();

        }

        public void Result()
        {
            conn.CreateTable<Appointments>();
            var query1 = conn.Table<Appointments>();
            AppointmentDetailView.ItemsSource = query1.ToList();


        }

        private async void AddWish_Click(object sender, RoutedEventArgs e)
        {


            try
            {
                if (tbAppointmentName.Text)
            }

            conn.CreateTable<Appointments>();
            conn.Insert(new Appointments
            {
                ID = tbAppointmentID.Text,
                appointmentName = tbAppointmentName.Text,
                EndTime = appointmentEndTime.Text,
                StartTime = appointmentStartTime.Text,
                Date = appointmentDate.Text




            });

            Result();


        }


        private async void DeleteItem_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string AceSelection = ((Appointments)AppointmentDetailView.SelectedItem).appointmentName;
                if (AceSelection == "")
                {
                    MessageDialog dialog = new MessageDialog("not selected the item", "ooops..!");
                    await dialog.ShowAsync();
                }
                else
                {
                    conn.CreateTable<Appointments>();
                    var query1 = conn.Table<Appointments>();
                    var query3 = conn.Query<Appointments>("DELETE FROM PersonalInfo Where Appointment name ='" + AceSelection + "'");
                    AppointmentDetailView.ItemsSource = query1.ToList();
                }
            }

            catch (NullReferenceException)
            {
                MessageDialog dialogue = new MessageDialog("Not Selected the Item", "Opps...");
                await dialogue.ShowAsync();
            }
        }


        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Result();
        }


private async void AppBarButton_Click(object sender, RoutedEventArgs e)
        {
            string Date = appointmentDate.ToString();

            try
            {
                if (appointmentDate.ToString() == "" || tbAppointmentName.ToString() == "" || appointmentStartTime.ToString() == "" || appointmentEndTime.ToString() == "")
                {
                    MessageDialog dialog = new MessageDialog("Value(s) not entered, Opps");
                    await dialog.ShowAsync();
                }

                else
                {
                    conn.Insert(new Appointments()
                    {
                        ID = tbAppointmentID.Text,
                        appointmentName = tbAppointmentName.Text,
                        EndTime = appointmentEndTime.Text,
                        StartTime = appointmentStartTime.Text,
                        Date = appointmentDate.Text
                    });

                    Result();

                }
            }

            catch (Exception ex)
            {
                if (ex is FormatException)
                {
                    MessageDialog dialog = new MessageDialog("You fogot to include some date ");
                    await dialog.ShowAsync();
                }
            }
            }
        }

        private void AppBarButton_Click_1(object sender, RoutedEventArgs e)
        {

        }

        private void AppBarButton_Click_2(object sender, RoutedEventArgs e)
        {

        }
    }
}
