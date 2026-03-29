using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Notepad___Clone
{
    /// <summary>
    /// Interaction logic for FindWindow.xaml
    /// </summary>
    public partial class FindWindow : Window
    {
        public FindWindow()
        {
            InitializeComponent();
        }
        public string SearchText => SearchTabs.SelectedIndex == 0 ? txtSearchFind.Text : txtSearchReplace.Text;
        public string ReplaceText => txtReplaceWith.Text;
        public bool SearchInAllTabs => chkAllTabs.IsChecked ?? false;

        private void ReplaceAll_Click(object sender, RoutedEventArgs e)
        {
            this.Tag = "ReplaceAll";
            this.DialogResult = true;
        }

        private void Replace_Click(object sender, RoutedEventArgs e)
        {
            this.Tag = "Replace";
            this.DialogResult = true;
        }

        private void FindNext_Click(object sender, RoutedEventArgs e)
        {
            this.Tag = "Find";
            this.DialogResult = true;
        }
    }
}
