using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Text.RegularExpressions;

namespace Notepad___Clone
{
    internal class SearchService
    {
        public void ExecuteSearch(ItemCollection tabs, TabItem selectedTab, string text, bool allTabs)
        {
            if (string.IsNullOrEmpty(text))
                return;
            if (allTabs)
            {
                foreach (TabItem tab in tabs)
                {
                    if (tab.Content is RichTextBox rtb)
                    {
                        HighlightAllOccurrences(rtb, text);
                    }
                }
                MessageBox.Show($"Cautarea a fost finalizata in toate fisierele deschise.");
            }
            else if (selectedTab?.Content is RichTextBox rtb)
            {
                TextRange fullDoc = new TextRange(rtb.Document.ContentStart, rtb.Document.ContentEnd);
                fullDoc.ApplyPropertyValue(TextElement.BackgroundProperty, Brushes.Transparent);

                TextPointer startPointer = rtb.Selection.End;
                TextRange foundRange = FindTextInRange(startPointer, rtb.Document.ContentEnd, text);

                if (foundRange == null)
                {
                    MessageBoxResult result = MessageBox.Show("Am ajuns la final. Reluam?", "Cautare", MessageBoxButton.YesNo);
                    if (result == MessageBoxResult.Yes)
                        foundRange = FindTextInRange(rtb.Document.ContentStart, rtb.Document.ContentEnd, text);
                }

                if (foundRange != null)
                {
                    rtb.Selection.Select(foundRange.Start, foundRange.End);
                    rtb.Focus();
                    (foundRange.Start.Parent as FrameworkElement)?.BringIntoView();
                }
            }
        }

        public void ExecuteReplaceSingle(ItemCollection tabs, TabItem selectedTab, string search, string replace, bool allTabs)
        {
            if (string.IsNullOrEmpty(search)) return;

            if (allTabs)
            {
                ReplaceAll(tabs, search, replace, true, selectedTab);
            }
            else if (selectedTab?.Content is RichTextBox rtb)
            {
                TextRange foundRange = FindTextInRange(rtb.Selection.Start, rtb.Document.ContentEnd, search);

                if (foundRange == null)
                    foundRange = FindTextInRange(rtb.Document.ContentStart, rtb.Document.ContentEnd, search);

                if (foundRange != null)
                {
                    rtb.Selection.Select(foundRange.Start, foundRange.End);
                    rtb.Selection.Text = replace;
                    rtb.Focus();

                    ExecuteSearch(tabs, selectedTab, search, false);
                }
            }
        }

        public void ReplaceAll(ItemCollection tabs, string search, string replace, bool allTabs, TabItem selectedTab)
        {
            if (string.IsNullOrEmpty(search))
                return;
            string pattern = $@"\b{Regex.Escape(search)}\b";

            if (allTabs)
            {
                foreach (TabItem tab in tabs)
                {
                    if (tab.Content is RichTextBox rtb)
                        PerformReplaceAllInRTB(rtb, pattern, replace);
                }
            }
            else if (selectedTab?.Content is RichTextBox rtb)
            {
                PerformReplaceAllInRTB(rtb, pattern, replace);
            }
        }

        private void PerformReplaceAllInRTB(RichTextBox rtb, string pattern, string replace)
        {
            TextRange range = new TextRange(rtb.Document.ContentStart, rtb.Document.ContentEnd);
            range.Text = Regex.Replace(range.Text, pattern, replace, RegexOptions.IgnoreCase);
        }

        public void HighlightAllOccurrences(RichTextBox rtb, string text)
        {
            TextRange fullRange = new TextRange(rtb.Document.ContentStart, rtb.Document.ContentEnd);
            fullRange.ApplyPropertyValue(TextElement.BackgroundProperty, Brushes.Transparent);

            string pattern = $@"\b{Regex.Escape(text)}\b";

            TextPointer position = rtb.Document.ContentStart;
            while (position != null)
            {
                if (position.GetPointerContext(LogicalDirection.Forward) == TextPointerContext.Text)
                {
                    string textRun = position.GetTextInRun(LogicalDirection.Forward);
                    MatchCollection matches = Regex.Matches(textRun, pattern, RegexOptions.IgnoreCase);

                    foreach (Match match in matches)
                    {
                        TextPointer start = position.GetPositionAtOffset(match.Index);
                        TextPointer end = start.GetPositionAtOffset(match.Length);
                        TextRange range = new TextRange(start, end);
                        range.ApplyPropertyValue(TextElement.BackgroundProperty, Brushes.Yellow);
                    }
                }
                position = position.GetNextContextPosition(LogicalDirection.Forward);
            }
        }

        private TextRange FindTextInRange(TextPointer start, TextPointer end, string searchText)
        {
            string pattern = $@"\b{Regex.Escape(searchText)}\b";

            while (start != null && start.CompareTo(end) < 0)
            {
                if (start.GetPointerContext(LogicalDirection.Forward) == TextPointerContext.Text)
                {
                    string textRun = start.GetTextInRun(LogicalDirection.Forward);
                    Match match = Regex.Match(textRun, pattern, RegexOptions.IgnoreCase);

                    if (match.Success)
                    {
                        TextPointer startPos = start.GetPositionAtOffset(match.Index);
                        TextPointer endPos = startPos.GetPositionAtOffset(match.Length);
                        return new TextRange(startPos, endPos);
                    }
                }
                start = start.GetNextContextPosition(LogicalDirection.Forward);
            }
            return null;
        }
    }

}
