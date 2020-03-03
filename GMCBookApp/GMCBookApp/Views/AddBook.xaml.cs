using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using GMCBookApp.Models;
using Plugin.FilePicker.Abstractions;
using Plugin.FilePicker;
using Plugin.Media;
using System.IO;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Graphics;
using Syncfusion.Drawing;
using Plugin.Media.Abstractions;

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

        private async void PDFCreateviaGalery_Clicked(object sender, EventArgs e)
        {
            await GaleryAsync();
        }
        private async void PDFCreateviaCamera_Clicked(object sender, EventArgs e)
        {
            await CameraAsync();
        }
        private async Task GaleryAsync()
        {
            if (!CrossMedia.Current.IsPickPhotoSupported)
            {
                DisplayAlert("Photos Not Supported", ":( Permission not granted to photos.", "OK");
                return;
            }
            PdfDocument document = new PdfDocument();
            //bool take_picture = true;
            //while(take_picture)
            //{
                //var file = await MediaService.Instance.OpenMediaPickerAsync(MediaType.Image);
                //var file = await Plugin.Media.CrossMedia.Current.PickPhotoAsync(new Plugin.Media.Abstractions.PickMediaOptions
                //{
                //    PhotoSize = Plugin.Media.Abstractions.PhotoSize.Medium,

                //});
                //if (file == null) take_picture = false;
                //else
                //{
                //    PdfPage page = document.Pages.Add();
                //    PdfGraphics graphics = page.Graphics;
                //    var imageStream = new MemoryStream();
                //    file.GetStream().CopyTo(imageStream);
                //    PdfBitmap image = new PdfBitmap(imageStream);
                //    graphics.DrawImage(image, 0, 0);
                //}
            //}

            List<MediaFile> file = await CrossMedia.Current.PickPhotosAsync(new PickMediaOptions
            {
                PhotoSize = PhotoSize.Medium
            });
            if(file != null)
            {
                foreach (MediaFile current_page in file)
                {
                    PdfPage page = document.Pages.Add();
                    PdfGraphics graphics = page.Graphics;
                    var imageStream = new MemoryStream();
                    current_page.GetStream().CopyTo(imageStream);
                    PdfBitmap image = new PdfBitmap(imageStream);
                    graphics.DrawImage(image, 0, 0);
                }
            }
            if (document.PageCount > 0)
            {
                MemoryStream stream = new MemoryStream();
                document.Save(stream);
                document.Close(true);
                book.PDF = stream.ToArray();
                if (book.PDF != null) pdf_clicked = true;
            }
        }

        private async Task CameraAsync()
        {
            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                DisplayAlert("No Camera", ":( No camera available.", "OK");
                return;
            }
            PdfDocument document = new PdfDocument();
            bool take_picture = true;
            while (take_picture)
            {
                var file = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions {
                    PhotoSize = PhotoSize.MaxWidthHeight,
                    AllowCropping = true,
                    MaxWidthHeight = 700,
                    SaveToAlbum = false,
                    DefaultCamera = CameraDevice.Rear});

                if (file == null)   take_picture = false;
                else
                {
                    PdfPage page = document.Pages.Add();
                    PdfGraphics graphics = page.Graphics;
                    var imageStream = new MemoryStream();
                    file.GetStream().CopyTo(imageStream);
                    PdfBitmap image = new PdfBitmap(imageStream);
                    graphics.DrawImage(image, 0, 0);
                }
            }
            if (document.PageCount > 0)
            {
                MemoryStream stream = new MemoryStream();
                document.Save(stream);
                document.Close(true);
                book.PDF = stream.ToArray();
                if (book.PDF != null) pdf_clicked = true;
            }
        }

    }
}
