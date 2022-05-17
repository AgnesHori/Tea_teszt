using Microsoft.Win32;
using Solution_Tea.Modells;
using Solution_Tea.Repository;
using Solution_Tea.UI.TeaUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;


namespace Solution_Tea
{

    public partial class MainWindow : Window
    {
        private readonly TeaRepository _teaRepository = new TeaRepository();

        private ObservableCollection<Tea> _teas = new ObservableCollection<Tea>();


        public MainWindow()
        {
            InitializeComponent();
            TeaAddOrUpdate.TeaCreatedEvent += TeaCreatedCreatedEventHandler;
            TeaAddOrUpdate.TeaUpdatedEvent += TeaUpdatedCreatedEventHandler;
        }

        #region eseménykezelők
        private void OnLoad(object sender, RoutedEventArgs e)
        {
            _teas = _teaRepository.GetAll();
            dataGrid.ItemsSource = _teas;
        }

        private void OnAddClick(object sender, RoutedEventArgs e)
        {
            TeaAddOrUpdate form = new TeaAddOrUpdate();
            form.ShowDialog();
        }

        private void OnUpdateClick(object sender, RoutedEventArgs e)
        {
            Tea tea = (Tea)dataGrid.SelectedItem;

            if (tea == null)
            {
                MessageBox.Show("No tea selected!", "", MessageBoxButton.OK);
                return;
            }

            TeaAddOrUpdate form = new TeaAddOrUpdate(tea);
            form.ShowDialog();
        }

        private void OnDeleteClick(object sender, RoutedEventArgs e)
        {
            Tea tea = (Tea)dataGrid.SelectedItem;

            if (tea == null)
            {
                MessageBox.Show("No tea selected!", "", MessageBoxButton.OK);
                return;
            }

            MessageBoxResult dialogResult = MessageBox.Show($"Are you sure that you wan't to delete tea named {tea.Name}?", "", MessageBoxButton.YesNo);

            if (dialogResult == MessageBoxResult.No)
            {
                return;
            }

            try
            {
                _teas.Remove(tea);
                _teaRepository.Remove(tea);
                MessageBox.Show($"{tea.Name} deleted.", "", MessageBoxButton.OK);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{tea.Name} not deleted.", "Error", MessageBoxButton.OK);
            }
        }

        private void OnExportClick(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "TextFile (*.txt)|*.txt";
            saveFileDialog.Title = "Save file";
            saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            if (saveFileDialog.ShowDialog() == false)
            {
                return;
            }

            List<string> output = new List<string>();
            string line = string.Empty;

            foreach (Tea tea in _teas)
            {
                line = $"{tea.Id}\t{tea.Name}\t{tea.Type.Name}\t{tea.Debut.ToString("yyyy-MM-dd")}";
                output.Add(line);
            }

            File.WriteAllLines(saveFileDialog.FileName, output);
        }

        private void TeaCreatedCreatedEventHandler(Tea tea)
        {
            _teas.Add(tea);
        }

        private void TeaUpdatedCreatedEventHandler(Tea tea)
        {
            Tea selected = (Tea)dataGrid.SelectedItem;

            if (selected == null)
            {
                return;
            }

            int index = _teas.IndexOf(selected);
            _teas.RemoveAt(index);
            _teas.Insert(index, tea);
        }
        #endregion
    }
}
