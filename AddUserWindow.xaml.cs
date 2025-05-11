using System;
using System.Linq;
using System.Windows;

namespace WpfApp1
{
    public partial class AddUserWindow : Window
    {
        public Osoba NowyUczen { get; private set; }

        public AddUserWindow()
        {
            InitializeComponent();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            string pesel = PESELBox.Text;
            string data = DateBox.Text;

            if (pesel.Length != 11 || !pesel.All(char.IsDigit))
            {
                MessageBox.Show("PESEL musi mieć dokładnie 11 cyfr.");
                return;
            }

            if (!IsValidPeselChecksum(pesel))
            {
                MessageBox.Show("Nieprawidłowa suma kontrolna PESEL.");
                return;
            }

            if (!IsValidDateFormat(data))
            {
                MessageBox.Show("Data urodzenia powinna być w formacie dd.mm.rrrr.");
                return;
            }

            if (!DoesPeselMatchBirthDate(pesel, data))
            {
                MessageBox.Show("Data urodzenia nie zgadza się z numerem PESEL.");
                return;
            }

            if (string.IsNullOrWhiteSpace(NameBox.Text))
            {
                MessageBox.Show("Imię jest wymagane.");
                return;
            }

            if (string.IsNullOrWhiteSpace(SurnameBox.Text))
            {
                MessageBox.Show("Nazwisko jest wymagane.");
                return;
            }

            NowyUczen = new Osoba
            {
                m_strPESEL = pesel,
                m_strName = FormatText(NameBox.Text),
                m_strSecName = FormatText(SecNameBox.Text),
                m_strSurname = FormatText(SurnameBox.Text),
                m_strDateOfBirth = data,
                m_strPhoneNumber = PhoneBox.Text.Trim(),
                m_strAddres = FormatText(AddressBox.Text),
                m_strCity = FormatText(CityBox.Text),
                m_strPostalCode = PostalBox.Text.Trim()
            };


            DialogResult = true;
            Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private bool IsValidPeselChecksum(string pesel)
        {
            int[] weights = { 1, 3, 7, 9, 1, 3, 7, 9, 1, 3 };
            int sum = 0;

            for (int i = 0; i < 10; i++)
                sum += int.Parse(pesel[i].ToString()) * weights[i];

            int control = (10 - (sum % 10)) % 10;
            return control == int.Parse(pesel[10].ToString());
        }

        private bool IsValidDateFormat(string date)
        {
            if (string.IsNullOrWhiteSpace(date)) return false;
            string[] parts = date.Split('.');
            return parts.Length == 3 &&
                   parts[0].Length == 2 &&
                   parts[1].Length == 2 &&
                   parts[2].Length == 4 &&
                   parts.All(p => p.All(char.IsDigit));
        }

        private bool DoesPeselMatchBirthDate(string pesel, string date)
        {
            string[] parts = date.Split('.');
            int day = int.Parse(parts[0]);
            int month = int.Parse(parts[1]);
            int year = int.Parse(parts[2]);

            int peselYear = int.Parse(pesel.Substring(0, 2));
            int peselMonth = int.Parse(pesel.Substring(2, 2));
            int peselDay = int.Parse(pesel.Substring(4, 2));

            int fullYear;

            if (peselMonth >= 81 && peselMonth <= 92) fullYear = 1800 + peselYear;
            else if (peselMonth >= 1 && peselMonth <= 12) fullYear = 1900 + peselYear;
            else if (peselMonth >= 21 && peselMonth <= 32) fullYear = 2000 + peselYear;
            else if (peselMonth >= 41 && peselMonth <= 52) fullYear = 2100 + peselYear;
            else if (peselMonth >= 61 && peselMonth <= 72) fullYear = 2200 + peselYear;
            else return false;

            int trueMonth = peselMonth % 20;

            return day == peselDay && month == trueMonth && year == fullYear;
        }

        private string FormatText(string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return "";

            input = input.Trim();
            return char.ToUpper(input[0]) + input.Substring(1).ToLower();
        }

    }

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

}
