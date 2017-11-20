using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using SQLite;
using StartFinance.Models;
using Windows.UI.Popups;
using SQLite.Net;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace StartFinance.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ContactDetailsPage : Page
    {
        SQLiteConnection conn; // adding an SQLite connection
        string path = Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, "Findata.sqlite");
        ContactDetails tempContactDetails = null;
        
        public ContactDetailsPage()
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
            conn.CreateTable<ContactDetails>();
            var query1 = conn.Table<ContactDetails>();
            ContactsView.ItemsSource = query1.ToList();
        }

        private async void AddContact_Click(object sender, RoutedEventArgs e)
        {
            String tempFirstName, tempLastName, tempPhone;

            // Check all fields are entered, spit dummy and return if not

            if (_FirstName.Text.ToString() == "")
            {
                MessageDialog dialog = new MessageDialog("No first name entered", "Oops..!");
                await dialog.ShowAsync();
                return;
            }
            else
                tempFirstName = _FirstName.Text.ToString();

            if (_LastName.Text.ToString() == "")
            {
                MessageDialog dialog = new MessageDialog("No last name entered", "Oops..!");
                await dialog.ShowAsync();
                return;
            }
            else
                tempLastName = _LastName.Text.ToString();

            if (_Phone.Text.ToString() == "")
            {
                MessageDialog dialog = new MessageDialog("No phone number entered", "Oops..!");
                await dialog.ShowAsync();
                return;
            }
            else
                tempPhone = _Phone.Text.ToString();

            // insert potentially correct details into database

            conn.Insert(new ContactDetails
            {
                FirstName = tempFirstName,
                LastName = tempLastName,
                Phone = tempPhone
            });

            Results();
            
        }

        private async void DeleteContact_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string AccSelection = (((ContactDetails)ContactsView.SelectedItem).ID).ToString();
                if (AccSelection == "")
                {
                    MessageDialog dialog = new MessageDialog("Not selected the Item", "Oops..!");
                    await dialog.ShowAsync();
                }
                else
                {
                    conn.CreateTable<ContactDetails>();
                    var query1 = conn.Table<ContactDetails>();
                    var query3 = conn.Query<ContactDetails>("DELETE FROM ContactDetails WHERE ID ='" + AccSelection + "'");
                    ContactsView.ItemsSource = query1.ToList();
                }
            }
            catch (NullReferenceException)
            {
                MessageDialog dialog = new MessageDialog("Not selected the Item", "Oops..!");
                await dialog.ShowAsync();
            }

            // clear fields and whatnot when done to avoid duplicate entries
            _FirstName.Text = "";
            _LastName.Text = "";
            _Phone.Text = "";
            tempContactDetails = null;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Results();
        }

        private void ContactsView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            // get details from contactDetailsList into tempContactDetails and display onscreen
            if (ContactsView.SelectedIndex != -1)
            {
                
                tempContactDetails = ((ContactDetails)ContactsView.SelectedItem); // retrieve contact from list and place into temp for modification
                _FirstName.Text = tempContactDetails.FirstName;
                _LastName.Text = tempContactDetails.LastName;
                _Phone.Text = tempContactDetails.Phone;
            }

            
        }

        private async void ModifyContact_Click(object sender, RoutedEventArgs e)
        {
            // the magic begins here
            // I hope this is what KT had in mind...
            
            if (tempContactDetails == null) // if null, then no record selected etc... spit dummy and return
            {
                MessageDialog dialog = new MessageDialog("No customer has been selected from list!", "Oops..!");
                await dialog.ShowAsync();
                return;
            }

            try
            {
                String tempFirstName, tempLastName, tempPhone;

                if (_FirstName.Text.ToString() == "")
                {
                    MessageDialog dialog = new MessageDialog("First name empty", "Oops..!");
                    await dialog.ShowAsync();
                    return;
                }
                else
                    tempFirstName = _FirstName.Text.ToString();

                if (_LastName.Text.ToString() == "")
                {
                    MessageDialog dialog = new MessageDialog("Last name empty", "Oops..!");
                    await dialog.ShowAsync();
                    return;
                }
                else
                    tempLastName = _LastName.Text.ToString();

                if (_Phone.Text.ToString() == "")
                {
                    MessageDialog dialog = new MessageDialog("Telephone is empty", "Oops..!");
                    await dialog.ShowAsync();
                    return;
                }
                else
                    tempPhone = _Phone.Text.ToString();

                tempContactDetails.FirstName = tempFirstName;
                tempContactDetails.LastName = tempLastName;
                tempContactDetails.Phone = tempPhone;

                conn.Update(tempContactDetails); // no need to change ID, because already present in tempContactDetails
            }
            catch (NullReferenceException) // in case code chucks exception
            {
                MessageDialog dialog = new MessageDialog("No customer has been selected from list!", "Oops..!");
                await dialog.ShowAsync();
                return;
            }

            // clear fields and whatnot when done to avoid duplicate entries
            _FirstName.Text = "";
            _LastName.Text = "";
            _Phone.Text = "";
            tempContactDetails = null;

            Results(); // create and display list

        }
    }
}
