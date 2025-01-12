using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Institute
{
    class Person : ICloneable
    {
        private string m_lastName;
        public string LastName
        {
            get { return m_lastName; }
            set { m_lastName = value; }
        }

        private string m_middleName;
        public string MiddleName
        {
            get { return m_middleName; }
            set { m_middleName = value; }
        }

        private string m_firstName;
        public string FirstName
        {
            get { return m_firstName; }
            set { m_firstName = value; }
        }

        private DateTime m_dateOfBirth;
        public DateTime DateOfBirth
        {
            get { return m_dateOfBirth; }
            set
            {
                if (value < DateTime.MinValue && value > DateTime.MaxValue)
                    throw new ArgumentException("Недопустимое значение для даты рождения!");

                m_dateOfBirth = value;
            }
        }

        private string m_address;
        public string Address
        {
            get { return m_address; }
            set { m_address = value; }
        }

        private string m_telephoneNumber;
        private string TelephoneNumber
        {
            get { return m_telephoneNumber; }
            set { m_telephoneNumber = value; }
        }
        
        //конструкторы
        public Person(string lastName,
                      string firstName,
                      string middleName,
                      DateTime dateOfBirth,
                      string address,
                      string telephoneNumber)
        {
            LastName = lastName;
            FirstName = firstName;
            MiddleName = middleName;
            DateOfBirth = dateOfBirth;
            Address = address;
            TelephoneNumber = telephoneNumber;
        }

        //конструктор копирования
        public Person(in Person other)
        {
            LastName = other.LastName;
            FirstName = other.FirstName;
            MiddleName = other.MiddleName;
            DateOfBirth = other.DateOfBirth;
            Address = other.Address;
            TelephoneNumber = other.TelephoneNumber;
        }
        public override string ToString()
        {
            return $"Фамилия: {LastName}\n" +
                   $"Имя: {FirstName}\n" +
                   $"Отчество: {MiddleName}\n" +
                   $"Дата рождения: {DateOfBirth.ToShortDateString()}\n" +
                   $"Адрес: {Address}\n" +
                   $"Телефон: {TelephoneNumber}";
        }

        public Object Clone()
        {
            return new Person(this);
        }
    }
}
