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
using Windows.Storage;
using Windows.Storage.Pickers;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

// Документацию по шаблону элемента "Пустая страница" см. по адресу https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x419

namespace ExhibitionApp1
{
    /// <summary>
    /// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
    /// </summary>
    public sealed partial class MainPage : Page
    {

        public static StorageFolder currentFolder;

        private static string currentPath;//current folder full path

        private IReadOnlyCollection<StorageFolder> temp;

        private List<StorageFile> Pictures; // contains a list of pictures for view

        private Stack<ObservableCollection<StorageFolder>> CallOutStack = new Stack<ObservableCollection<StorageFolder>>();

        private int i = 0;

        public MainPage()
        {
            this.InitializeComponent();

        }
        
        public async void RefillCollection(StorageFolder x)
        {
            IReadOnlyList<StorageFolder> temp = await x.GetFoldersAsync();

            if (temp.Count == 0)
            {
                DisplayNotify("There are no folders. Navigating to PicturesView...");
                currentFolder = x;
                NavigateToPicturesView();
                return;
            }

            ObservableCollection<StorageFolder> temp1 = new ObservableCollection<StorageFolder>();
            foreach (StorageFolder sf in temp)
            {
                temp1.Add(sf);
            }
            CallOutStack.Push(temp1);
            this.FoldersGridView.ItemsSource = CallOutStack.First();
        }

        private async void FoldersGridView_ItemClick(object sender, ItemClickEventArgs e)
        {
           RefillCollection((StorageFolder)e.ClickedItem);
        }

        private async void DisplayNotify()
        {
            ContentDialog noWifiDialog = new ContentDialog
            {
                Title = "Notification",
                Content = "The Folder is EMPTY!",
                CloseButtonText = "Ok"
            };

            ContentDialogResult result = await noWifiDialog.ShowAsync();
        }

        private async void DisplayNotify(string text)
        {
            ContentDialog noWifiDialog = new ContentDialog
            {
                Title = "Notification",
                Content = text,
                CloseButtonText = "Ok"
            };

            ContentDialogResult result = await noWifiDialog.ShowAsync();
        }

        public async void FolderPicking()
        {
            FolderPicker folderPicker = new FolderPicker();
            folderPicker.ViewMode = PickerViewMode.Thumbnail;
            folderPicker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
            folderPicker.FileTypeFilter.Add("*");

            StorageFolder storageFolder = await folderPicker.PickSingleFolderAsync();

            RefillCollection(storageFolder);
            PathBox.Text = storageFolder.Path;


        }

        private void ChangeFolderButton_Click(object sender, RoutedEventArgs e)
        {
            FolderPicking();
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            if (CallOutStack.Count > 1)
            {
                CallOutStack.Pop();
                this.FoldersGridView.ItemsSource = CallOutStack.First();
            }
            else
            {
                DisplayNotify("Cant go back");
            }
        }

        private void NavigateToPicturesView()
        {

            this.Frame.Navigate(typeof(PicturesView));

        }

        public void NavigatedTo()
        {
            this.FoldersGridView.ItemsSource = CallOutStack.First();
        }

    }
}
