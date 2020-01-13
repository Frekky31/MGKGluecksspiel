using MGKGluecksspiel.Serializable;
using MGKGluecksspiel.Viewmodel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MGKGluecksspiel
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private WindowsViewmodel viewmodel;
        public MainWindow()
        {
            InitializeComponent();
            viewmodel = new WindowsViewmodel();
            this.DataContext = viewmodel;
        }


        public static bool IsValidDouble(string str, double min, double max)
        {
            return double.TryParse(str, out double i) && i >= min && i <= max;
        }

        public static bool IsValidInt(string str, int min, int max)
        {
            return int.TryParse(str, out int i) && i >= min && i <= max;
        }

        #region
        private void NbrShowRange_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !IsValidInt(((TextBox)sender).Text + e.Text, 0, 9999);
        }

        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !IsValidDouble(((TextBox)sender).Text + e.Text, 0, 999999999);
        }

        private void TxtNumber_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !IsValidDouble(((TextBox)sender).Text + e.Text, 0, 999999999);
        }
        #endregion Handle inputs

        private void BtnInsert_Click(object sender, RoutedEventArgs e)
        {
            Insert();
        }

        private void Insert() {
            if (!string.IsNullOrWhiteSpace(txtName.Text) && !string.IsNullOrWhiteSpace(txtNumber.Text) && double.TryParse(txtNumber.Text, out double number))
            {
                viewmodel.Inputs.Add(new InputViewmodel(txtName.Text, number));
            }
        }

        private void BtnEvaluate_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                viewmodel.Outputs.Clear();
                foreach (var input in viewmodel.Inputs)
                {
                    double guessNumber = double.Parse(txtGuessNumber.Text);
                    var output = new OutputViewmodel
                    {
                        Difference = Math.Abs(guessNumber - input.Number),
                        Name = input.Name,
                        Number = input.Number
                    };
                    viewmodel.Outputs.Add(output);
                }
                List<OutputViewmodel> outputViewmodels = viewmodel.Outputs.OrderBy(x => x.Difference).ToList();

                for (int i = 0; i < outputViewmodels.Count; i++)
                    outputViewmodels.ElementAt(i).Place = i + 1;

                if (rdoOnly.IsChecked == true)
                    outputViewmodels.RemoveAll(x => x.Place > int.Parse(txtShowRange.Text));

                viewmodel.Outputs = new ObservableCollection<OutputViewmodel>(outputViewmodels);
                lstOutputs.ItemsSource = viewmodel.Outputs;
                lstOutputs.Items.SortDescriptions.Add(new SortDescription("Place", ListSortDirection.Ascending));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void MniDelete_Click(object sender, RoutedEventArgs e)
        {
            DeleteSelected();
        }

        private void DeleteSelected()
        {
            try
            {
                List<InputViewmodel> list = lstInputs.SelectedItems.Cast<InputViewmodel>().ToList();
                foreach (InputViewmodel eachItem in list)
                {
                    viewmodel.Inputs.Remove(eachItem);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void MniDeleteAll_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                viewmodel.Inputs.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void MniExport_Click(object sender, RoutedEventArgs e)
        {
            ExportToExcel();
        }
        private void mniExportExcel_Click(object sender, RoutedEventArgs e)
        {
            ExportToExcel();
        }

        private void ExportToExcel()
        {
            try
            {
                var excel = new Microsoft.Office.Interop.Excel.Application(); 
                excel.Visible = false;
                excel.DisplayAlerts = false;
                var workbook = excel.Workbooks.Add(Type.Missing);
                var worksheet = (Microsoft.Office.Interop.Excel.Worksheet)workbook.ActiveSheet;
                DateTime dateTime = DateTime.Today;
                worksheet.Name = $"Jahreskonzert_{dateTime.ToString("yyyyMMdd")}";

                worksheet.Cells[1, 1] = "Platz";
                worksheet.Cells[1, 2] = "Name";
                worksheet.Cells[1, 3] = "Nummer";
                worksheet.Cells[1, 4] = "Differenz";

                int rowcount = 2;

                foreach (OutputViewmodel datarow in viewmodel.Outputs.ToList())
                {
                    worksheet.Cells[rowcount, 1] = datarow.Place;
                    worksheet.Cells[rowcount, 2] = datarow.Name;
                    worksheet.Cells[rowcount, 3] = datarow.Number;
                    worksheet.Cells[rowcount, 4] = datarow.Difference;
                    rowcount += 1;
                }

                Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog
                {
                    FileName = $"Jahreskonzert_{dateTime.ToString("yyyyMMdd")}", // Default file name
                    DefaultExt = ".xlsx", // Default file extension
                    Filter = "Excel Dokument (.xlsx)|*.xlsx" // Filter files by extension
                };

                bool? result = dlg.ShowDialog();

                if (result == true)
                {
                    string filename = dlg.FileName;
                    workbook.SaveAs(filename);
                }

                workbook.Close();
                excel.Quit();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void MniSave_Click(object sender, RoutedEventArgs e)
        {
            Save();
        }

        private void Save()
        {
            try
            {
                DateTime dateTime = DateTime.Today;
                Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog
                {
                    FileName = $"Jahreskonzert_{dateTime.ToString("yyyyMMdd")}", // Default file name
                    DefaultExt = ".dat", // Default file extension
                    Filter = "Datei Dokument (.dat)|*.dat" // Filter files by extension
                };

                bool? result = dlg.ShowDialog();

                if (result == true)
                {
                    string filename = dlg.FileName;

                    BinaryFormatter formatter = new BinaryFormatter();
                    FileStream writerFileStream =
                            new FileStream(filename, FileMode.Create, FileAccess.Write);
                    formatter.Serialize(writerFileStream, ToSaveOject());
                    writerFileStream.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void MniOpen_Click(object sender, RoutedEventArgs e)
        {
            Open();
        }

        private void Open()
        {
            try
            {
                Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
                dlg.FileName = ""; // Default file name
                dlg.DefaultExt = ".dat"; // Default file extension
                dlg.Filter = "Datei Dokument (.dat)|*.dat"; // Filter files by extension

                bool? result = dlg.ShowDialog();

                if (result == true)
                {
                    string filename = dlg.FileName;

                    BinaryFormatter formatter = new BinaryFormatter();
                    FileStream readerFileStream = new FileStream(filename,
                    FileMode.Open, FileAccess.Read);
                    SaveObject saveObject = (SaveObject)formatter.Deserialize(readerFileStream);
                    FromSaveObject(saveObject);
                    readerFileStream.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private SaveObject ToSaveOject()
        {
            SaveObject saveObject = new SaveObject();
            try
            {
                saveObject.Only = int.Parse(txtShowRange.Text);
            }
            catch (Exception)
            {
                saveObject.Only = 1;
            }
            try
            {
                saveObject.GuessNumber = double.Parse(txtGuessNumber.Text);
            }
            catch (Exception)
            {
                saveObject.GuessNumber = 0;
            }
            foreach (var input in viewmodel.Inputs)
                saveObject.Inputs.Add(new Input(input.Name, input.Number));
            foreach (var output in viewmodel.Outputs)
                saveObject.Outputs.Add(new Output(output.Place, output.Name, output.Number, output.Difference));

            return saveObject;
        }

        private void FromSaveObject(SaveObject saveObject)
        {
            viewmodel.Inputs.Clear();
            viewmodel.Outputs.Clear();

            txtGuessNumber.Text = saveObject.GuessNumber.ToString();
            txtShowRange.Text = saveObject.Only.ToString();

            foreach (var input in saveObject.Inputs)
                viewmodel.Inputs.Add(new InputViewmodel(input.Name, input.Number));
            foreach (var output in saveObject.Outputs)
                viewmodel.Outputs.Add(new OutputViewmodel(output.Place, output.Name, output.Number, output.Difference));
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.S && Keyboard.IsKeyDown(Key.LeftCtrl))
            {
                Save();
            }
            else if (e.Key == Key.O && Keyboard.IsKeyDown(Key.LeftCtrl))
            {
                Open();
            }
            else if (e.Key == Key.Delete)
            {
                DeleteSelected();
            }
        }

        private void txtNumber_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter) {
                Insert();
            }
        }
    }
}
