using Solution_Tea.Modells;
using Solution_Tea.Repository;
using System;
using System.Windows;

namespace Solution_Tea.UI.TeaUI
{
    /// <summary>
    /// Interaction logic for TeaAddOrUpdate.xaml
    /// </summary>
    public partial class TeaAddOrUpdate : Window
    {
        private delegate void Action();
        private Action _action;

        private readonly TeaRepository _teaRepository;
        private readonly TypeRepository _typeRepository;

        public delegate void TeaDelegate(Tea tea);
        public static event TeaDelegate TeaCreatedEvent;
        public static event TeaDelegate TeaUpdatedEvent;

        private Tea _tea = null;

        public TeaAddOrUpdate()
        {
            InitializeComponent();

            _typeRepository = new TypeRepository();
            _teaRepository = new TeaRepository();

            _action = CreateNewTea;
        }

        public TeaAddOrUpdate(Tea tea)
        {
            InitializeComponent();

            _typeRepository = new TypeRepository();
            _teaRepository = new TeaRepository();

            _tea = tea;
            _action = UpdateTea;

            DataContext = tea;
        }

        #region fuggvenyek
        private void CreateNewTea()
        {
            try
            {
                Tea data = (Tea)DataContext;
                _tea = new Tea(data);
                _teaRepository.Create(_tea);

                MessageBox.Show("New data saved successfully", "", MessageBoxButton.OK);

                TeaCreatedEvent(data);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error occoured while saving", "", MessageBoxButton.OK);
                return;
            }
        }

        private void UpdateTea()
        {
            try
            {
                Tea data = (Tea)DataContext;
                _tea = new Tea(data);
                _teaRepository.Update(_tea, _tea.Id);

                MessageBox.Show("New data saved successfully", "", MessageBoxButton.OK);
                TeaUpdatedEvent(data);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error occoured while saving", "", MessageBoxButton.OK);
                return;
            }
        }
        #endregion

        #region esemenykezelok
        private void OnLoad(object sender, RoutedEventArgs e)
        {
            cbType.ItemsSource = _typeRepository.GetAll();
        }

        private void OnClick(object sender, RoutedEventArgs e)
        {
            _action();
            this.Close();
        }
        #endregion
    }
}