using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Notepad___Clone
{
    public partial class MainWindow : Window
    {
        private FileManager _fileManager = new FileManager();
        private SearchService _searchService = new SearchService();
        private DirectoryService _directoryService = new DirectoryService();
        private TabService _tabService = new TabService();

        public MainWindow()
        {
            InitializeComponent();
            FileTabs.Items.Add(_tabService.CreateNewTab());
        }

        private void NewFile_Click(object sender, ExecutedRoutedEventArgs e)
        {
            FileTabs.Items.Add(_tabService.CreateNewTab());
        }

        private void OpenFile_Click(object sender, ExecutedRoutedEventArgs e)
        {
            string filePath = _fileManager.OpenFile();
            if (!string.IsNullOrEmpty(filePath))
            {
                var tab = _tabService.CreateNewTab(System.IO.Path.GetFileName(filePath), File.ReadAllText(filePath), filePath);
                FileTabs.Items.Add(tab);
                FileTabs.SelectedItem = tab;
            }
        }

        private void SaveFile_Click(object sender, ExecutedRoutedEventArgs e)
        {
            if (FileTabs.SelectedItem is TabItem currentTab)
            {
                if (currentTab.Tag is string filePath)
                    _fileManager.SaveToDisk(filePath, currentTab);
                else SaveAsFile_Click(sender, e);
            }
        }

        private void SaveAsFile_Click(object sender, RoutedEventArgs e)
        {
            if (FileTabs.SelectedItem is TabItem currentTab)
            {
                string currentHeader = currentTab.Header.ToString();

                string newPath = _fileManager.SaveAs(currentHeader);

                if (!string.IsNullOrEmpty(newPath))
                {
                    _fileManager.SaveToDisk(newPath, currentTab);
                }
            }
        }

        private void CloseAllFiles_Click(object sender, RoutedEventArgs e)
        {
            for (int i = FileTabs.Items.Count - 1; i >= 0; i--)
            {
                TabItem tab = (TabItem)FileTabs.Items[i];
                if (_fileManager.ConfirmSave(tab))
                    FileTabs.Items.Remove(tab);
                else break;
            }
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void SelectFolder_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new Microsoft.Win32.OpenFolderDialog();
            if (dialog.ShowDialog() == true)
            {
                string rootPath = dialog.FolderName;
                if (!_directoryService.IsFolderAlreadyInTree(FolderTree.Items, rootPath))
                {
                    FolderTree.Items.Add(_directoryService.CreateTreeItem(new DirectoryInfo(rootPath)));
                }
                else
                {
                    MessageBox.Show("This folder is already added to the explorer.");
                }
            }
        }
        private void FolderTree_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (FolderTree.SelectedItem is TreeViewItem selectedItem)
            {
                string path = selectedItem.Tag.ToString();
                if (File.Exists(path))
                {
                    OpenFileFromPath(path);
                }
            }
        }

        private void OpenFileFromPath(string filePath)
        {
            foreach (TabItem tab in FileTabs.Items)
            {
                if (tab.Tag != null && tab.Tag.ToString().Equals(filePath, StringComparison.OrdinalIgnoreCase))
                {
                    if (tab.Content is RichTextBox rtb)
                    {
                        TextRange range = new TextRange(rtb.Document.ContentStart, rtb.Document.ContentEnd);
                        range.Text = File.ReadAllText(filePath);
                    }
                    FileTabs.SelectedItem = tab;
                    return;
                }
            }
            var newTab = _tabService.CreateNewTab(System.IO.Path.GetFileName(filePath), File.ReadAllText(filePath), filePath);
            FileTabs.Items.Add(newTab);
            FileTabs.SelectedItem = newTab;
        }

        private void ContextMenuNewFile_Click(object sender, RoutedEventArgs e)
        {
            if (FolderTree.SelectedItem is TreeViewItem selectedItem)
            {
                string path = selectedItem.Tag.ToString();
                if (Directory.Exists(path))
                {
                    string newFilePath = System.IO.Path.Combine(path, "NewFile.txt");
                    File.WriteAllText(newFilePath, "");
                    selectedItem.Items.Add(new TreeViewItem { Header = "NewFile.txt", Tag = newFilePath });
                }
            }
        }

        private void CopyPath_Click(object sender, RoutedEventArgs e)
        {
            _directoryService.CopyPathToClipboard(FolderTree.SelectedItem as TreeViewItem);
        }
        private void CopyFolder_Click(object sender, RoutedEventArgs e)
        {
            if (FolderTree.SelectedItem is TreeViewItem selectedItem)
                _directoryService.SetFolderToCopy(selectedItem.Tag.ToString());
        }

        private void PasteFolder_Click(object sender, RoutedEventArgs e)
        {
            if (FolderTree.SelectedItem is TreeViewItem selectedItem)
            {
                try
                {
                    _directoryService.PasteDirectory(selectedItem.Tag.ToString());
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void CloseCurrentTab_Click(object sender, RoutedEventArgs e)
        {
            if (FileTabs.SelectedItem is TabItem currentTab)
            {
                if (_fileManager.ConfirmSave(currentTab))
                    FileTabs.Items.Remove(currentTab);
            }
        }

        private void FindReplace_Click(object sender, RoutedEventArgs e)
        {
            FindWindow frWin = new FindWindow();
            frWin.Owner = this;

            if (frWin.ShowDialog() == true)
            {
                string search = frWin.SearchText;
                string replace = frWin.ReplaceText;
                bool searchInAll = frWin.SearchInAllTabs;

                if (frWin.Tag.ToString() == "ReplaceAll")
                {
                    _searchService.ReplaceAll(FileTabs.Items, search, replace, searchInAll, FileTabs.SelectedItem as TabItem);
                    MessageBox.Show("The operation has been completed.");
                }
                else if (frWin.Tag.ToString() == "Replace")
                {
                    _searchService.ExecuteReplaceSingle(FileTabs.Items, FileTabs.SelectedItem as TabItem, search, replace, searchInAll);
                }
                else if (frWin.Tag.ToString() == "Find")
                {
                    _searchService.ExecuteSearch(FileTabs.Items, FileTabs.SelectedItem as TabItem, search, searchInAll);
                }
            }
        }

        private void ViewStandard_Click(object sender, RoutedEventArgs e)
        {
            ExplorerColumn.Width = new GridLength(0);
            SplitterColumn.Width = new GridLength(0);
            ExplorerPanel.Visibility = Visibility.Collapsed;
            MainSplitter.Visibility = Visibility.Collapsed;
        }

        private void ViewExplorer_Click(object sender, RoutedEventArgs e)
        {
            ExplorerColumn.Width = new GridLength(200);
            SplitterColumn.Width = new GridLength(5);
            ExplorerPanel.Visibility = Visibility.Visible;
            MainSplitter.Visibility = Visibility.Visible;
        }

        private void About_Click(object sender, RoutedEventArgs e)
        {
            About aboutWindow = new About();
            aboutWindow.ShowDialog();
        }
    }
}