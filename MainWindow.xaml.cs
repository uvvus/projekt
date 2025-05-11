using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows;

namespace WpfApp1
{
    public partial class MainWindow : Window
    {
        public class Osoba
        {
            public string m_strPESEL { get; set; }
            public string m_strName { get; set; }
            public string m_strSecName { get; set; }
            public string m_strSurname { get; set; }
            public string m_strDateOfBirth { get; set; }
            public string m_strPhoneNumber { get; set; }
            public string m_strAddres { get; set; }
            public string m_strCity { get; set; }
            public string m_strPostalCode { get; set; }
        }

        public MainWindow()
        {
            InitializeComponent();
        }

        private void MenuItem_Load_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Pliki CSV|*.csv";
            dialog.Title = "Wczytaj listę uczniów";

            if (dialog.ShowDialog() == true)
            {
                mainList.Items.Clear();
                string[] linie = File.ReadAllLines(dialog.FileName, Encoding.UTF8);

                for (int i = 1; i < linie.Length; i++)
                {
                    string[] dane = linie[i].Split(';');
                    if (dane.Length >= 9)
                    {
                        Osoba o = new Osoba();
                        o.m_strPESEL = dane[0];
                        o.m_strName = dane[1];
                        o.m_strSecName = dane[2];
                        o.m_strSurname = dane[3];
                        o.m_strDateOfBirth = dane[4];
                        o.m_strPhoneNumber = dane[5];
                        o.m_strAddres = dane[6];
                        o.m_strCity = dane[7];
                        o.m_strPostalCode = dane[8];

                        mainList.Items.Add(o);
                    }
                }
            }
        }
        private void MenuItem_Save_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = "Pliki CSV|*.csv";
            dialog.Title = "Zapisz listę uczniów";

            if (dialog.ShowDialog() == true)
            {
                var linie = new List<string>();

                linie.Add("PESEL;Imię;Drugie imię;Nazwisko;Data urodzenia;Numer telefonu;Adres zamieszkania;Miejscowość;Kod pocztowy");

                foreach (var item in mainList.Items)
                {
                    if (item is Osoba o)
                    {
                        string linia = o.m_strPESEL + ";" + o.m_strName + ";" + o.m_strSecName + ";" + o.m_strSurname + ";" +
                                       o.m_strDateOfBirth + ";" + o.m_strPhoneNumber + ";" + o.m_strAddres + ";" +
                                       o.m_strCity + ";" + o.m_strPostalCode;
                        linie.Add(linia);
                    }
                }

                File.WriteAllLines(dialog.FileName, linie, Encoding.UTF8);
                MessageBox.Show("Dane zostały zapisane.", "OK", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
        private void MenuItem_Exit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void MenuItem_RemoveSelected_Click(object sender, RoutedEventArgs e)
        {
            var selectedItems = new List<Osoba>();

            foreach (var item in mainList.SelectedItems)
            {
                if (item is Osoba osoba)
                {
                    selectedItems.Add(osoba);
                }
            }

            foreach (var osoba in selectedItems)
            {
                mainList.Items.Remove(osoba);
            }
        }
        private void MenuItem_AddUser_Click(object sender, RoutedEventArgs e)
        {
            AddUserWindow okno = new AddUserWindow();
            if (okno.ShowDialog() == true)
            {
                mainList.Items.Add(okno.NowyUczen);
            }
        }
    }
}
