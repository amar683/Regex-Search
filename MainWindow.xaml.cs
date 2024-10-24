using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace RegexSearch
{
    public partial class MainWindow : Window
    {
        private List<string> ListA;
        private List<string> ListB;

        private string selectedList = "All Lists"; 

        public MainWindow()
        {
            InitializeComponent();

            ListA = new List<string> { "Apple", "Banana", "Cherry", "Date", "Elderberry" };
            ListB = new List<string> { "Apricot", "Blueberry", "Coconut", "Dragonfruit", "Fig" };

            PopulateTreeView();
        }

        private void PopulateTreeView()
        {
            ItemTreeView.Items.Clear();

            var rootItem = new TreeViewItem { Header = "All Lists" };

            var listAItem = new TreeViewItem { Header = "List A" };
            var listBItem = new TreeViewItem { Header = "List B" };

            rootItem.Items.Add(listAItem);
            rootItem.Items.Add(listBItem);

            ItemTreeView.Items.Add(rootItem);

            rootItem.IsExpanded = true;
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            PerformSearch();
        }

        private void SearchTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                PerformSearch();
                e.Handled = true; 
            }
        }

        private void PerformSearch()
        {
            string searchText = SearchTextBox.Text;
            bool useRegex = RegexCheckBox.IsChecked == true;
            ContentTextBox.Clear();

            if (string.IsNullOrWhiteSpace(searchText))
            {
                return;
            }

            List<string> FilterList(List<string> list)
            {
                if (useRegex)
                {
                    try
                    {
                        Regex regex = new Regex(searchText, RegexOptions.IgnoreCase);
                        return list.Where(item => regex.IsMatch(item)).ToList();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Invalid regular expression!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        return new List<string>(); 
                    }
                }
                else
                {
                    return list.Where(item => item.ToLower().StartsWith(searchText.ToLower())).ToList();
                }
            }

            if (selectedList == "All Lists")
            {
                var filteredListA = FilterList(ListA);
                var filteredListB = FilterList(ListB);

                ContentTextBox.AppendText("List A:\n");
                ContentTextBox.AppendText(string.Join("\n", filteredListA));
                ContentTextBox.AppendText("\n\nList B:\n");
                ContentTextBox.AppendText(string.Join("\n", filteredListB));
            }
            else if (selectedList == "List A")
            {
                var filteredListA = FilterList(ListA);
                ContentTextBox.AppendText(string.Join("\n", filteredListA));
            }
            else if (selectedList == "List B")
            {
                var filteredListB = FilterList(ListB);
                ContentTextBox.AppendText(string.Join("\n", filteredListB));
            }
        }

        private void ItemTreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (ItemTreeView.SelectedItem is TreeViewItem selectedItem)
            {
                ContentTextBox.Clear();
                selectedList = selectedItem.Header.ToString(); 

                SearchTextBox.Text = string.Empty;

                if (selectedList == "All Lists")
                {
                    ContentTextBox.AppendText("List A:\n");
                    ContentTextBox.AppendText(string.Join("\n", ListA));
                    ContentTextBox.AppendText("\n\nList B:\n");
                    ContentTextBox.AppendText(string.Join("\n", ListB));
                }
                else if (selectedList == "List A")
                {
                    ContentTextBox.AppendText(string.Join("\n", ListA));
                }
                else if (selectedList == "List B")
                {
                    ContentTextBox.AppendText(string.Join("\n", ListB));
                }
            }
        }
    }
}
