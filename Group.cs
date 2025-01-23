using System;
using System.Collections.Generic;
using System.Diagnostics;

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

        public delegate bool FilterStudentDelegate(Student student);

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

        //средний балл группы
        public double AvgGroupMark()
        {
            double sum = 0;
            for (int i = 0; i < m_studentsCount; ++i)
            {
                sum += m_students[i].AvgMark;
            }
            return sum / m_studentsCount;
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

        public void ShowStudentWithMark(int mark)
        {
            Console.WriteLine($"Студенты у которых есть оценка {mark} за экзамен");
            for (int i = 0; i < m_students.Count; ++i)
            {
                for (int j = 0; j < m_students[i].Exam.Count; ++j)
                {
                    if (m_students[i].Exam[j] <= mark)
                    {
                        Console.WriteLine(m_students[i].ToString());
                        break;
                    }
                }
            }
            
        }

        //одинаковые оценки за курсовую или зачет или экзамен
        public bool HaveSameMarks(in Student student)
        {   
            bool allSame = false;
            var studMarks = new List<int>(student.CourseWork);
            studMarks.Sort();

            //беру массивы оценок сортирую и сравниваю
            for (int i = 0; i < m_students.Count; ++i)
            {
                if (student.Person.FirstName != m_students[i].Person.FirstName && student.Person.LastName != m_students[i].Person.LastName)
                {
                    var marks = new List<int>(m_students[i].CourseWork);
                    marks.Sort();
                    int counter = 0;

                    for (int j = 0; j < studMarks.Count; ++j)
                    {
                        
                        if (studMarks[j] == marks[j])
                        {
                            ++counter;
                        }

                        if (counter == studMarks.Count)
                        {
                            allSame = true;
                            return allSame;
                        }
                    }
                }

                
            }
                ////Алгоритм сравнения оценок студентов // не сработал надо будет доработать
                //for (int i = 0; i < m_students.Count; ++i)
                //{
                //    int counter = 0;

                //    //проверка на сравнение с самим собой
                //    if (student.Person.FirstName != m_students[i].Person.FirstName && student.Person.LastName != m_students[i].Person.LastName)
                //    {
                //        if (student.CourseWork.Count == m_students[i].CourseWork.Count)
                //        { 
                //            //проверяю массив оценок студента
                //            for (int j = 0; j < student.CourseWork.Count; ++j)
                //            {
                //                for (int k = 0; k < m_students[i].CourseWork.Count; ++k)
                //                {
                //                    //если оценки совпадают увеличиваю счетчик
                //                    if (student.CourseWork[j] == m_students[i].CourseWork[k])
                //                    {
                //                        ++counter;
                //                        break;
                //                    }

                //                }

                //            }
                //        }
                //    }

                //    //если счетчик равен количеству оценок у студента тогда есть студент с такими же оценками
                //    if (counter == student.CourseWork.Count)
                //    {
                //        allSame = true;
                //        break;
                //    }

                //    counter = 0;                
                //}



                return allSame;
        }

        public void ShowExcellentStudent()
        {
            Console.WriteLine("Отличники:");
            for (int i = 0; i < m_students.Count; ++i)
            {
                if (m_students[i].AverageMark() >= 9)
                {
                    Console.WriteLine(m_students[i].ToString());
                }
            }
        }

        //метод фильтрации студентов
        public List<Student> FilterStudents(FilterStudentDelegate filter)
        {
            List<Student> group = new List<Student>();
            for (int i = 0; i < m_students.Count; ++i)
            {
                if (filter(m_students[i]))
                {
                    group.Add(m_students[i]);
                }
            }
            return group;
        }
    }


}
