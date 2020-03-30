using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;
using GMCBookApp.Models;
using Plugin.FilePicker.Abstractions;
using Plugin.FilePicker;
using System.IO;
using GMCBookApp.Views;

namespace GMCBookApp
{
    public partial class AddBook : ContentPage
    {
        List<string> _images = new List<string>();
        Book book = new Book();
        public bool pdf_clicked = false;
        public AddBook()
        {
            InitializeComponent();
        }
        private void AddBookButton_Clicked(object sender, EventArgs e)
        {
        }

        private async void BooksButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new MainPage(),false);
        }

        private async void BackButton_Clicked(object sender, EventArgs e)
        {
            if (Navigation.NavigationStack.Count > 0)
            {   //pops last page
                await Navigation.PopAsync(false).ConfigureAwait(false);
            }
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            try
            {       //upload the data into database
                if (pdf_clicked && bookName.Text != null && priceOfBook.Text != null && writerName.Text != null && yearPublished.Text != null)
                {
                    book.BookName = bookName.Text;
                    book.Price = Convert.ToInt32(priceOfBook.Text);
                    book.WriterName = writerName.Text;
                    book.YearPublished = Convert.ToInt32(yearPublished.Text);
                    await App.Database.SaveBookAsync(book);
                    await Navigation.PushAsync(new AddBook(), false);
                }
                else
                {
                    await DisplayAlert("Alert", "Please fill all the necesarry inputs in order to add book", "OK");
                }
            }
            catch
            {
                await DisplayAlert("Alert", "Please fill all the necesarry inputs correctly in order to add book", "OK");
            }

        }

        private async void PDFImport_Clicked(object sender, EventArgs e)
        {
            if(book.PDF != null)
            {
                bool answer = await DisplayAlert("PDf Warning", "PDF is already created, do you want to continue?","accept","cancel");
                if (!answer) return; 
            }
            await PDFImport_ClickedAsync();
        }


        private async Task PDFImport_ClickedAsync()
        {
            try
            {
                pdf_clicked = true;
                FileData fileData = await CrossFilePicker.Current.PickFile();
                if (fileData == null)
                {
                    pdf_clicked = false;
                    return; // user canceled file picking
                }
                var memoryStream = new MemoryStream();
                fileData.GetStream().CopyTo(memoryStream);
                fileData.Dispose();
                book.PDF = memoryStream.ToArray();
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("Exception choosing file: " + ex.ToString());
            }
        }


        private async void PDFCreate_Clicked(object sender, EventArgs e)
        {
            if (book.PDF != null)
            {
                bool answer = await DisplayAlert("PDf Warning", "PDF is already created, do you want to continue?", "accept", "cancel");
                if (!answer) return;
            }
            await PDFCreate_ClickedAsync();
        } 

        private async Task PDFCreate_ClickedAsync()
        {
            try
            {
                pdf_clicked = true;
                await Navigation.PushAsync(new GeneratePDF(book), false);
            }
            catch (Exception ex)
            {
                pdf_clicked = false;
                System.Console.WriteLine("Exception generating file: " + ex.ToString());
            }
        }

    }
}
