using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using System.Collections.Specialized;

namespace ExhibitionApp1
{

    public sealed partial class PicturesView : Page
    {
        private static List<StorageFile> Pictures = new List<StorageFile>();

        private static StorageFolder currenFolder = MainPage.currentFolder;

        public PicturesView()
        {
            this.InitializeComponent();
            FillCollection();
        }

        private async void FillCollection()
        {
            IReadOnlyList <StorageFile> xFiles = await currenFolder.GetFilesAsync();

            foreach (StorageFile a in xFiles)
            {
                if (a.FileType == ".png" || a.FileType == ".jpg")
                {
                    Pictures.Add(a);
                }
            }

            ImagesFlipView.ItemsSource = Pictures;
        }

        private void OnMainPageButton_OnClick(object sender, RoutedEventArgs e)
        {
            Pictures.Clear();
            if (Frame.CanGoBack)
            {
                Frame.GoBack();
            }
            this.Frame.Navigate(typeof(MainPage));
        }
    }
}2