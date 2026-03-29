using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace Notepad___Clone
{
    internal class TabService
    {
        private int _fileCounter = 1;
        public TabItem CreateNewTab(string header = null, string content = "", string path = null)
        {
            RichTextBox rtb = new RichTextBox
            {
                VerticalScrollBarVisibility = ScrollBarVisibility.Auto,
                HorizontalScrollBarVisibility = ScrollBarVisibility.Auto,
                AcceptsReturn = true
            };

            Style noMarginStyle = new Style(typeof(Paragraph));
            noMarginStyle.Setters.Add(new Setter(Paragraph.MarginProperty, new Thickness(0)));

            rtb.Resources.Add(typeof(Paragraph), noMarginStyle);

            rtb.Document.Blocks.Clear();
            rtb.Document.Blocks.Add(new Paragraph(new Run(content)));

            TabItem newTab = new TabItem
            {
                Header = header ?? $"File {_fileCounter++}",
                Content = rtb,
                Tag = path
            };

            rtb.TextChanged += (s, e) =>
            {
                if (newTab.Header is string h && !h.EndsWith("*"))
                    newTab.Header += "*";
            };

            return newTab;
        }
    }
}
