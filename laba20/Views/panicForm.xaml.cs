using laba20.ViewModels;
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

namespace laba20.Views
{
    /// <summary>
    /// Логика взаимодействия для panicForm.xaml
    /// </summary>
    public partial class panicForm : Window
    {
        public static panicForm formView;
        private MainViewModel viewModel;
        public panicForm()
        {
            formView = this;
            viewModel = new MainViewModel();
            DataContext = viewModel;
            InitializeComponent();
            cmbBox.SelectionChanged += StatusChange;
        }

        private void StatusChange(object sender, SelectionChangedEventArgs e)
        {
            ComboBoxItem selectedItem = (ComboBoxItem)cmbBox.SelectedItem;
            string status = selectedItem.Content.ToString();
            bool state;
            if (status == "сдал")
            {
                state = true;
            }
            else
            {
                state = false;
            }
            viewModel.NewStatusDiscipline = state;
        }
    }
}
