using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Plugin.Permissions;
using Xamarin.Forms;
using GMCBookApp.Models;

namespace GMCBookApp
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private async void AddBookButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AddBook(), false);     //Düzenlenecek.
        }

        private void BooksButton_Clicked(object sender, EventArgs e)
        {
        }

        private async void BackButton_Clicked(object sender, EventArgs e)
        {
            if (Navigation.NavigationStack.Count > 0)
            {
                await Navigation.PopAsync(false).ConfigureAwait(false);
            }
        }
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            listView.ItemsSource = await App.Database.GetBooksAsync();
        }
        async void OnListViewItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem != null)
            {
                Book book = new Book(e.SelectedItem as Book);
                await Navigation.PushAsync(new BookDetail(book),false);
            }
        }
    }
}
