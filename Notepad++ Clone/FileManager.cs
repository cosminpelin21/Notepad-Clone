using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;

namespace Notepad___Clone
{
    internal class FileManager
    {
        public void SaveToDisk(string filePath, TabItem tab)
        {
            if (tab.Content is RichTextBox rtb)
            {
                TextRange range = new TextRange(rtb.Document.ContentStart, rtb.Document.ContentEnd);
                File.WriteAllText(filePath, range.Text);

                tab.Tag = filePath;
                tab.Header = System.IO.Path.GetFileName(filePath);
            }
        }
        public string OpenFile()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Text files (*.txt)|*.txt|All files(*.*)|*.*",
                Title = "Open Text File"
            };

            return openFileDialog.ShowDialog() == true ? openFileDialog.FileName : null;
        }

        public string SaveAs(string currentName)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "Text files (*.txt)|*.txt|All files(*.*)|*.*",
                FileName = currentName.Replace("*", "")
            };

            return saveFileDialog.ShowDialog() == true ? saveFileDialog.FileName : null;
        }

        public bool ConfirmSave(TabItem tab)
        {
            if (tab.Header.ToString().EndsWith("*"))
            {
                var result = MessageBox.Show($"Do you want to save the changes for {tab.Header}?",
                    "Save file", MessageBoxButton.YesNoCancel);

                if (result == MessageBoxResult.Yes)
                {
                    if (tab.Tag != null && !string.IsNullOrEmpty(tab.Tag.ToString()))
                        SaveToDisk(tab.Tag.ToString(), tab);
                    else
                    {
                        string newPath = SaveAs(tab.Header.ToString());
                        if (!string.IsNullOrEmpty(newPath)) SaveToDisk(newPath, tab);
                    }
                    return true;
                }
                return result == MessageBoxResult.No;
            }
            return true;
        }
    }
}
