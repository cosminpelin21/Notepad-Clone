using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Notepad___Clone
{
    internal class DirectoryService
    {
        public TreeViewItem CreateTreeItem(DirectoryInfo directoryInfo)
        {
            TreeViewItem item = new TreeViewItem
            {
                Header = directoryInfo.Name,
                Tag = directoryInfo.FullName
            };

            try
            {
                foreach (var directory in directoryInfo.GetDirectories())
                    item.Items.Add(CreateTreeItem(directory));

                foreach (var file in directoryInfo.GetFiles())
                {
                    item.Items.Add(new TreeViewItem
                    {
                        Header = file.Name,
                        Tag = file.FullName
                    });
                }
            }
            catch (UnauthorizedAccessException)
            {

            }

            return item;
        }
        public void CopyDirectory(string sourceDir, string destinationDir)
        {
            Directory.CreateDirectory(destinationDir);

            foreach (var file in Directory.GetFiles(sourceDir))
                File.Copy(file, Path.Combine(destinationDir, Path.GetFileName(file)), true);

            foreach (var directory in Directory.GetDirectories(sourceDir))
                CopyDirectory(directory, Path.Combine(destinationDir, Path.GetFileName(directory)));
        }

        public bool IsFolderAlreadyInTree(ItemCollection items, string path)
        {
            foreach (TreeViewItem item in items)
            {
                if (item.Tag.ToString().Equals(path, StringComparison.OrdinalIgnoreCase))
                    return true;
            }
            return false;
        }

        public string FolderToCopy { get; set; } = string.Empty;

        public void CopyPathToClipboard(TreeViewItem selectedItem)
        {
            if (selectedItem?.Tag != null)
            {
                System.Windows.Clipboard.SetText(selectedItem.Tag.ToString());
            }
        }

        public void SetFolderToCopy(string path)
        {
            if (System.IO.Directory.Exists(path))
            {
                FolderToCopy = path;
            }
        }
        public void PasteDirectory(string targetPath)
        {
            if (string.IsNullOrEmpty(FolderToCopy))
                return;
            if (targetPath.StartsWith(FolderToCopy, StringComparison.OrdinalIgnoreCase))
            {
                throw new InvalidOperationException("Cannot copy a folder into itself!");
            }

            if (Directory.Exists(targetPath))
            {
                string destination = Path.Combine(targetPath, Path.GetFileName(FolderToCopy));
                CopyDirectory(FolderToCopy, destination);
            }
        }
    }
}
