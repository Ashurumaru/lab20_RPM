using laba20.Models;
using laba20.Views;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace laba20.ViewModels
{
    internal class MainViewModel : BaseViewModel
    {
        private string nameDiscipline;
        private bool statusDiscipline;
        private string newNameDiscipline;
        private bool newStatusDiscipline;
        private bool statusFilterAll;
        private bool statusFilterTrue;
        private bool statusFilterFalse; 
        private ObservableCollection<DisciplineModel> allDisciplines;
        private ObservableCollection<DisciplineModel> showDiscipline;
        private DisciplineModel selectedDiscipline;

        public string NewNameDiscipline
        {
            get => newNameDiscipline;
            set
            {
                newNameDiscipline = value;
                OnPropertyChanged(nameof(NewNameDiscipline));
            }
        }

        public bool NewStatusDiscipline
        {
            get => newStatusDiscipline;
            set
            {
                newStatusDiscipline = value;
                OnPropertyChanged(nameof(NewStatusDiscipline));
            }
        }

        public string NameDiscipline
        {
            get => nameDiscipline;
            set
            {
                nameDiscipline = value;
                OnPropertyChanged(nameof(NameDiscipline));
            }
        }

        public bool StatusDiscipline
        {
            get => statusDiscipline;
            set
            {
                statusDiscipline = value;
                OnPropertyChanged(nameof(StatusDiscipline));
            }
        }

        public ObservableCollection<DisciplineModel> AllDisciplines
        {
            get => allDisciplines;
            set
            {
                allDisciplines = value;
                OnPropertyChanged(nameof(AllDisciplines));
            }
        }

        public ObservableCollection<DisciplineModel> ShowDiscipline
        {
            get => showDiscipline;
            set
            {
                showDiscipline = value;
                OnPropertyChanged(nameof(ShowDiscipline));
            }
        }

        public DisciplineModel SelectedDiscipline
        {
            get => selectedDiscipline;
            set
            {
                selectedDiscipline = value;
                OnPropertyChanged(nameof(SelectedDiscipline));
            }
        }

        public bool StatusFilterAll
        {
            get => statusFilterAll;
            set
            {
                statusFilterAll = value;
                OnPropertyChanged(nameof(StatusFilterAll));
            }
        }

        public bool StatusFilterTrue
        {
            get => statusFilterTrue;
            set
            {
                statusFilterTrue = value;
                OnPropertyChanged(nameof(StatusFilterTrue));
            }
        }

        public bool StatusFilterFalse
        {
            get => statusFilterFalse;
            set
            {
                statusFilterFalse = value;
                OnPropertyChanged(nameof(StatusFilterFalse));
            }
        }


        public ICommand ChangeStatusDiciplineCommand { get; private set; }
        public ICommand DeleteDisciplineCommand { get; private set; }
        public ICommand AddDisciplineCommand { get; private set; }
        public ICommand SaveCommand { get; private set; }
        public ICommand FilterShowCommand { get; private set; }


        public MainViewModel()
        {
            AllDisciplines = new ObservableCollection<DisciplineModel>
            {
                new DisciplineModel { NameDiscipline = "ТРЗБД", StatusDiscipline = true },
                new DisciplineModel { NameDiscipline = "Английский язык", StatusDiscipline = false },
                new DisciplineModel { NameDiscipline = "Теория вероятностей", StatusDiscipline = false }
            };
            UpdateListView();

            ChangeStatusDiciplineCommand = new RelayCommand(ChangeStatusDiscipline);
            DeleteDisciplineCommand = new RelayCommand(DeleteDiscipline);
            AddDisciplineCommand = new RelayCommand(AddDiscipline, CanAddDiscipline);
            SaveCommand = new RelayCommand(Save);
            FilterShowCommand = new RelayCommand(FilterShow, CanFilterShow);
        }
        private void UpdateListView()
        {
            ShowDiscipline = allDisciplines;
        }
        private void ChangeStatusDiscipline(object parameter)
        {
            if (SelectedDiscipline != null)
            {
                SelectedDiscipline.StatusDiscipline = !SelectedDiscipline.StatusDiscipline;
                AllDisciplines.Add(SelectedDiscipline);
                AllDisciplines.Remove(SelectedDiscipline);
                UpdateListView();
            }
            else
            {
                MessageBox.Show("Выберете дисциплину.");
            }
        }

        private void DeleteDiscipline(object parameter)
        {
            if (SelectedDiscipline != null)
            {
                AllDisciplines.Remove(SelectedDiscipline);
                UpdateListView();
            }
            else
            {
                MessageBox.Show("Выберете дисциплину.");
            }
        }

        private void AddDiscipline(object parameter)
        {
            if (AllDisciplines.Any(d => d.NameDiscipline.Equals(NewNameDiscipline, StringComparison.OrdinalIgnoreCase)))
            {
                MessageBox.Show("Дисциплина с таким названием уже существует.");
                return;
            }

            DisciplineModel newDiscipline = new DisciplineModel
            {
                NameDiscipline = NewNameDiscipline,
                StatusDiscipline = NewStatusDiscipline
            };

            AllDisciplines.Add(newDiscipline);
            UpdateListView();
        }

        private bool CanAddDiscipline(object parameter)
        {
            return !string.IsNullOrWhiteSpace(NewNameDiscipline);
        }

        private void Save(object parameter)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Текстовые файлы (*.txt)|*.txt|Все файлы (*.*)|*.*";

            if (saveFileDialog.ShowDialog() == true)
            {
                string filePath = saveFileDialog.FileName;

                try
                {
                    File.WriteAllLines(filePath, ShowDiscipline.Select(d => $"{d.NameDiscipline}:\t{d.StatusDiscipline}"));
                    MessageBox.Show($"Список дисциплин сохранен в файл по пути {filePath}.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при сохранении файла: {ex.Message}");
                }
            }
        }


        private bool CanFilterShow(object parameter)
        {
            return StatusFilterAll == true || StatusFilterTrue == true || StatusFilterFalse == true;
        }
        private void FilterShow(object parameter)
        {
            List<DisciplineModel> filteredDisciplines = new List<DisciplineModel>();

            if (StatusFilterAll)
            {
                filteredDisciplines.AddRange(AllDisciplines);
            }
            if (StatusFilterTrue)
            {
                filteredDisciplines.AddRange(AllDisciplines.Where(d => d.StatusDiscipline == true));
            }
            if (StatusFilterFalse)
            {
                filteredDisciplines.AddRange(AllDisciplines.Where(d => d.StatusDiscipline == false));
            }
            ShowDiscipline = new ObservableCollection<DisciplineModel>(filteredDisciplines);
        }
    }
}
