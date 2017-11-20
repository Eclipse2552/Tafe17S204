using SQLite.Net;
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
    public sealed partial class PersonalInfoPage : Page
    {
        SQLiteConnection conn; // adding an SQLite connection
        string path = Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, "Findata.sqlite");

        public PersonalInfoPage()
        {
            this.InitializeComponent();
            NavigationCacheMode = Windows.UI.Xaml.Navigation.NavigationCacheMode.Enabled;
            /// Initializing a database
            conn = new SQLite.Net.SQLiteConnection(new SQLite.Net.Platform.WinRT.SQLitePlatformWinRT(), path);
            // Creating table
            Results();
        }

        public void Results()
        {
            conn.CreateTable<PersonalInfo>();
            var query1 = conn.Table<PersonalInfo>();
            PersonalInfoView.ItemsSource = query1.ToList();
        }

        private async void AddWish_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (tbEmail.Text.ToString() == "")
                {
                    MessageDialog dialog = new MessageDialog("No value entered", "Oops..!");
                    await dialog.ShowAsync();
                }
                else
                {
                    conn.CreateTable<PersonalInfo>();
                    conn.Insert(new PersonalInfo
                    {
                        FirstName = tbFirstName.Text,
                        LastName = tbLastName.Text,
                        DOB = tbDoB.Text,
                        Gender = tbGender.Text,
                        Email = tbEmail.Text,
                        Phone = tbPhone.Text
                    });
                    // Creating table
                    Results();
                }
            }
            catch (Exception ex)
            {
                if (ex is SQLiteException)
                {
                    MessageDialog dialog = new MessageDialog("An entry with this email already exist, Try Different a Email", "Oops..!");
                    await dialog.ShowAsync();
                }
            }


        }

        private async void DeleteItem_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string AccSelection = ((PersonalInfo)PersonalInfoView.SelectedItem).Email;
                if (AccSelection == "")
                {
                    MessageDialog dialog = new MessageDialog("No item selected", "Oops..!");
                    await dialog.ShowAsync();
                }
                else
                {
                    conn.CreateTable<PersonalInfo>();
                    var query1 = conn.Table<PersonalInfo>();
                    var query3 = conn.Query<PersonalInfo>("DELETE FROM PersonalInfo WHERE Email ='" + AccSelection + "'");
                    PersonalInfoView.ItemsSource = query1.ToList();
                }
            }
            catch (NullReferenceException)
            {
                MessageDialog dialog = new MessageDialog("No item selected", "Oops..!");
                await dialog.ShowAsync();
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Results();
        }

        private void AppBarButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private async void EditInfo_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string AccSelection = ((PersonalInfo)PersonalInfoView.SelectedItem).Email;
                if (AccSelection == "")
                {
                    MessageDialog dialog = new MessageDialog("No item selected", "Oops..!");
                    await dialog.ShowAsync();
                }
                else
                {
                    conn.CreateTable<PersonalInfo>();
                    var query1 = conn.Table<PersonalInfo>();
                    var query3 = conn.Query<PersonalInfo>("UPDATE PersonalInfo SET FirstName='"+tbFirstName.Text
                        +"',LastName='"+tbLastName.Text
                        +"',DOB ='"+tbDoB.Text
                        +"',Gender='"+tbGender.Text
                        +"',Email='"+tbEmail.Text
                        +"',Phone='"+tbPhone.Text
                        +"' WHERE Email='"+AccSelection+"'");
                    PersonalInfoView.ItemsSource = query1.ToList();
                }
            }
            catch (NullReferenceException)
            {
                MessageDialog dialog = new MessageDialog("No item selected", "Oops..!");
                await dialog.ShowAsync();
            }
        }

        private void PersonalInfoView_ItemClick(object sender, ItemClickEventArgs e)
        {
             
        }
    }
}