using System;
using System.Collections.Generic;

namespace Institute
{
    internal class Group
    {
        /// <summary>
        /// Поля класса 
        /// </summary>
        private string m_name;
        public string Name
        {
            get { return m_name; }
            set { m_name = value; }
        }
        private string m_speciality;
        public string Speciality
        {
            get { return m_speciality; }
            set { m_speciality = value; }
        }
        private int m_course;
        public int Course
        {
            get { return m_course; }
            set
            {
                if (value >= 0)
                {
                    m_course = value;
                }
                else
                {
                    Console.WriteLine("Недопустимое значение для номера групппы!");
                }
            }
        }

        private List<Student> m_students;
        public List<Student> Students
        {
            get { return m_students; }
            set { m_students = value; }
        }
        private int m_studentsCount;

        /// <summary>
        /// Методы класса
        /// </summary>

        public Group(string name = "", string speciality = "", int course = 0)
        {
            Name = name;
            Speciality = speciality;
            Course = course;
            m_studentsCount = 0;
            m_students = new List<Student>();
        }
        private void SortGroup()
        {
            //сортировка студентов по фамилии
            if (m_studentsCount > 1)
                m_students.Sort((x, y) => x.Person.LastName.CompareTo(y.Person.LastName));
        }
        public void AddStudent(in Student student)
        {
            
                m_students.Add(student);
                ++m_studentsCount;
                SortGroup();
            
        }
        public int FindStudent(in string lastName)
        {
            //проверка пустая ли группа
            if (m_studentsCount == 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("В группе нет студентов!");
                Console.ForegroundColor = ConsoleColor.Gray;
                
                return -1;
            }

            for (int i = 0; i < m_students.Count; ++i)
            {
                if (m_students[i].Person.LastName == lastName)
                {                    
                    return i;
                }
            }

            return -2;
        }
        public void RemoveStudent(in int index)
        {
            if (index >= 0)
            {
                m_students.RemoveAt(index);
                --m_studentsCount;
            }
        }
        public void TransferStudent(in string studentName, ref Group toGroup)
        {
            int index = FindStudent(studentName);
            if (index >= 0)
            {
                toGroup.AddStudent(m_students[index]);
                RemoveStudent(index);
            }
            else 
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Студент не найден!\n");
                Console.ForegroundColor = ConsoleColor.Gray;
            }
        }
        //отчисление студентов с низким средним баллом
        public void DismissAllWithAvg(in double avgMark)
        { 
            
            for (int i = 0; i < m_studentsCount;)
            {
                if (m_students[i].AverageMark() < avgMark)
                {
                    Console.WriteLine($"Студент {m_students[i].Person.LastName} отчислен!");
                    RemoveStudent(i);
                    continue;
                }
                ++i;
            }
        }
        public void DismissBadStudent()
        {
            double minAvg = 12;

            for (int i = 0; i < m_studentsCount; ++i)
            {
                if(minAvg > m_students[i].AverageMark())
                    minAvg = m_students[i].AverageMark();
            }
            
            for (int i = 0; i < m_studentsCount; ++i)
            {
                if (m_students[i].AverageMark() == minAvg)
                {
                    Console.WriteLine($"Студент {m_students[i].Person.LastName} отчислен!");
                    RemoveStudent(i);
                }
            }
                        
        }
        public void ShowStudents()
        {
            Console.WriteLine($"\nГруппа \"{Name}\"\n" +
                $"специализация - {Speciality}\n" +
                $" курс №{Course}\n" +
                $"========================================");
            if (m_students.Count > 0)
            {
                int counter = 0;
                foreach (Student student in m_students)
                {
                    ++counter;
                    Console.Write($"{counter}|{student.Person.LastName}");
                    
                    //выравниваю ввывод строки
                    Console.CursorLeft += 10 - student.Person.LastName.Length;
                    Console.Write($" |{student.Person.FirstName}");

                    Console.CursorLeft += 10 - student.Person.FirstName.Length;
                    Console.Write("|");
                    student.ShowMarks();
                    Console.Write('|');
                    student.ShowAverageMark();
                    Console.WriteLine();
                }

                Console.WriteLine("========================================\n");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("В группе нет студентов");
                Console.ForegroundColor = ConsoleColor.Gray;
            }
        }

        public bool IsEmpty()
        {
            return m_studentsCount == 0;
        }

        //Критерии
        public static bool operator true(Group group)
        {
            return !group.IsEmpty();
        }

        public static bool operator false(Group group)
        {
            return group.IsEmpty();
        }

        //операторы сравнения
        public static bool operator <(Group left, Group right)
        {
            return left.m_studentsCount < right.m_studentsCount;
        }

        public static bool operator >(Group left, Group right)
        {
            return left.m_studentsCount > right.m_studentsCount;
        }

        public static bool operator ==(Group left, Group right)
        {
            return left.m_studentsCount == right.m_studentsCount;
        }

        public static bool operator !=(Group left, Group right)
        {
            return left.m_studentsCount != right.m_studentsCount;
        }

        //индексатор
        public Student this[int index]
        {
            get
            {  
                if (index < 0 && index >= m_studentsCount)
                    throw new ArgumentOutOfRangeException("index");
                
                    return m_students[index]; 
                
            }
            
        }

        
    }    
}
