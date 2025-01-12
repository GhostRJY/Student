using System;
using System.Collections.Generic;

namespace Institute
{
    internal class Group : ICloneable
    {
        //поля
        private string m_name;
        private string m_speciality;
        private int m_course;
        private List<Student> m_students;
        private int m_studentsCount;
        
        //свойства
        public string Name
        {
            get { return m_name; }
            set { m_name = value; }
        }
        public string Speciality
        {
            get { return m_speciality; }
            set { m_speciality = value; }
        }
        public int Course
        {
            get { return m_course; }
            set
            {
                if (value < 0)
                    throw new ArgumentException("Недопустимое значение для курса!");

                m_course = value;

            }
        }
        public List<Student> Students
        {
            get { return m_students; }
            set { m_students = value; }
        }

        internal class GroupEnumerator : IEnumerator<Student>
        {
            private Group m_group;
            private int m_index;

            //класс реализует интерфейс IEnumerator
            public GroupEnumerator(in Group group)
            {
                m_group = group;
                m_index = -1;
            }
            public Student Current
            {
                get
                {
                    if (m_index == -1 || m_index >= m_group.m_studentsCount)
                        throw new Exception("не верный индекс колекции");
                    
                    return m_group.m_students[m_index];
                }
            }
            object System.Collections.IEnumerator.Current
            {
                get { return Current; }
            }
            public void Dispose()
            {
                m_group = null;
            }
            public bool MoveNext()
            {
                return ++m_index < m_group.m_studentsCount;
            }
            public void Reset()
            {
                m_index = -1;
            }
        }

        //получаю перечислитель
        public GroupEnumerator GetEnumerator()
        {
            return new GroupEnumerator(this);
        }

        // конструктор
        public Group(string name = "", string speciality = "", int course = 0)
        {
            Name = name;
            Speciality = speciality;
            Course = course;
            m_studentsCount = 0;
            m_students = new List<Student>();
        }

        //конструктор копирования
        public Group(in Group other)
        {
            Name = other.Name;
            Speciality = other.Speciality;
            Course = other.Course;
            m_studentsCount = other.m_studentsCount;
            m_students = new List<Student>(other.m_students);
        }

        //сортировка студентов (по компаратору)
        public void SortGroup(IComparer<Student> comparer)
        {

            if (m_studentsCount > 1)
                m_students.Sort(comparer);
        }

        //добавление студента
        public void AddStudent(in Student student)
        {

            m_students.Add(student);
            ++m_studentsCount;
            SortGroup(new Student.LastNameASCComparer());

        }

        //поиск студента по фамилии
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

        //удаление студента
        public void RemoveStudent(in int index)
        {
            if (index >= 0)
            {
                m_students.RemoveAt(index);
                --m_studentsCount;
            }
        }

        //перевод студента в другую группу
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
                if (minAvg > m_students[i].AverageMark())
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

        //показать студентов
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

        private bool IsEmpty()
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

        //реализую интерфейс ICloneable
        public Object Clone()
        {
            return new Group(this);
        }
    }


}
