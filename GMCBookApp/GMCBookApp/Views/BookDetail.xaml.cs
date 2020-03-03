using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using GMCBookApp.Models;
using System.IO;

namespace GMCBookApp
{
    public partial class BookDetail : ContentPage
    {
        public byte[] pdf_array;
        Book thebook;
        public BookDetail(Book book)
        {
            thebook = book;
            InitializeComponent();
            bookName.Text = book.BookName;
            writerName.Text = book.WriterName;
            year.Text = Convert.ToString(book.YearPublished) ;
            price.Text = Convert.ToString(book.Price);
            pdf_array = book.PDF;
        }

        private async void AddBookButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AddBook(), false);
        }

        private async void BooksButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new MainPage(), false);
        }

        private async void BackButton_Clicked(object sender, EventArgs e)
        {
            if (Navigation.NavigationStack.Count > 0) await Navigation.PopAsync(false).ConfigureAwait(false);
        }

        private async void OpenPDF_Clicked(object sender, EventArgs e)
        {
            await DependencyService.Get<ISave>().SaveAndView("Output.pdf", "application / pdf", new MemoryStream(pdf_array));
        }

        private async void DeleteBook_Clicked(object sender, EventArgs e)
        {
            await App.Database.DeleteBookAsync(thebook);
            await Navigation.PopAsync(false).ConfigureAwait(false);
        }
    }
}
