using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using GMCBookApp.Models;
using System.IO;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Graphics;


namespace GMCBookApp.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class GeneratePDF : ContentPage
	{
        Book book;
        List<MediaFile> images = new List<MediaFile>();
        public GeneratePDF(Book thebook)
        {
            book = thebook;
            InitializeComponent();
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            updatePage();
        }
        public void updatePage()
        {
            //foreach(var image in images) checkRotation(image);            
            listView.ItemsSource = null;
            listView.ItemsSource = images;
            if (images.Count == 0) gallerylabel.Text = "No image is added yet";
            else gallerylabel.Text = "Click on images to remove";
        }
        //public void checkRotation(MediaFile image)
        //{
        //    var fixImage = new Image();
        //    fixImage.Source = ImageSource.FromFile(image.Path);
        //    fixImage.RotateTo(90);
        //}
        public void Item_Selected(object sender, SelectionChangedEventArgs e)
        {
            var selectedItem = (e.CurrentSelection.FirstOrDefault() as MediaFile);
            if (selectedItem != null)
            {
                images.Remove(selectedItem);
                listView.ItemsSource = null;
                listView.ItemsSource = images;
            }
        }
        private async void Generate_and_Publish_Clicked(object sender, EventArgs e)
        {
            try
            {
                PdfDocument document = new PdfDocument();
                if (images != null)
                {
                    foreach (MediaFile current_page in images)
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
                }
                await Navigation.PopAsync(false).ConfigureAwait(false);
            }
            catch
            {
                DisplayAlert("Document Error", ":( An error occured. Please re-import or re-generate pdf", "OK");
                return;
            }
        }

        private async void Go_Back_Clicked(object sender, EventArgs e)
        {
            if (Navigation.NavigationStack.Count > 0) await Navigation.PopAsync(false).ConfigureAwait(false);
        }

        private async void Add_via_Gallery_Clicked(object sender, EventArgs e)
        {
            try {if (!CrossMedia.Current.IsPickPhotoSupported)
            {
                    DisplayAlert("Photos Not Supported", ":( Permission not granted to photos.", "OK");
                    return;
                }
                await GaleryAsync();
            }
            catch
            {
                DisplayAlert("Image error", ":( An error occured. No valid pictures", "OK");
                return;
            }
            
        }

        private async void Add_via_Camera_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
                {
                    DisplayAlert("No Camera", ":( No camera available.", "OK");
                    return;
                }
                await CameraAsync();
            }
            catch
            {
                DisplayAlert("Camera error", ":( An error occured. No valid pictures", "OK");
                return;
            }

        }

        private async Task GaleryAsync()
        {

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
            try
            {
                List<MediaFile> file = await CrossMedia.Current.PickPhotosAsync(new PickMediaOptions
                {
                    PhotoSize = PhotoSize.MaxWidthHeight,
                    MaxWidthHeight = 650
                });
                //burada kaldım

                if (file != null)
                    for(int i = 0; i< file.Count; i++)
                    {
                        //Console.WriteLine("file" + file[i].Path);
                        if (file[i].Path != null) images.Add(file[i]);
                    }
                updatePage();
            }
            catch
            {
                DisplayAlert("Image error", ":( An error occured. No valid pictures", "OK");
                return;
            }
        }

        private async Task CameraAsync()
        {
            try
            {
                bool take_picture = true;
                while (take_picture)
                {
                    MediaFile file = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
                    {
                        PhotoSize = PhotoSize.MaxWidthHeight,
                        AllowCropping = true,
                        MaxWidthHeight = 700,
                        SaveToAlbum = false,
                        DefaultCamera = CameraDevice.Rear
                    });
                    if (file == null) take_picture = false;
                    else images.Add(file);
                }
                updatePage();
            }
            catch
            {
                DisplayAlert("Image error", ":( An error occured. No valid pictures", "OK");
                return;
            }
        }

    }
}

