using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Institute
{
   
    internal class Program
    {
        //делегат для работы с группой
        public delegate void GroupWorkDelegate(ref Group group);

        
        static void AddStudent(ref Group group)
        {
            Console.Clear();
            Console.WriteLine("Введите Фамилию студента");
            string lastName = Console.ReadLine();
            
            Console.WriteLine("Введите Имя студента");
            string firstName = Console.ReadLine();

            Console.WriteLine("Введите Отчество студента");
            string middleName = Console.ReadLine();

            Console.WriteLine("Введите дату рождения студента");
            DateTime dateOfBirth = DateTime.Parse(Console.ReadLine());

            Console.WriteLine("Введите адрес студента");
            string address = Console.ReadLine();

            Console.WriteLine("Введите номер телефона студента");
            string telephoneNumber = Console.ReadLine(); 

            group.AddStudent(new Student(new Person(lastName, firstName, middleName, dateOfBirth, address, telephoneNumber)));

            Console.WriteLine("Студент добавлен");
            
            Console.ReadLine();
        }

        static void RemoveStudent(ref Group group)
        {
            Console.Clear();
            Console.WriteLine("Введите Фамилию студента которого вы хотите удалить");
            string lastName = Console.ReadLine();
            group.RemoveStudent(group.FindStudent(lastName));
            Console.WriteLine("Студент удален");
            
            Console.ReadLine();
        }

        static void ShowStudentsWithExcellentMark(in Group group)
        {
            Console.Clear();
            group.ShowExcellentStudent();
            Console.ReadLine();
        }

        static void ShowStudentsWithBadMark(in Group group)
        {
            Console.Clear();
            group.ShowStudentWithMark(3);
        }

        //Проверка студентов 
        static void ShowStudents(in List<Student> students)
        {
            int counter = 0;
            foreach (Student student in students)
            {
                ++counter;
                Console.Write($"Студент №" + counter + student.ToString() + "\n");
            }
        }

        static void ExamMark(in Student student, in int mark)
        {
            student.AddExam(mark);
        }

        static void CourseMark(in Student student, in int mark)
        {
            student.AddCourseWork(mark);
        }

        static void StudentWorkMark(in Student student, in int mark)
        {
            student.AddStudentWork(mark);
        }

        static void Main(string[] args)
        {
           
            Person person = new Person("Иванов", "Иван", "Иванович", new DateTime(2000, 1, 20), "Odessa, Jukova, 15", "0931233212");
            Person person1 = new Person("Яковлев", "Александр", "Ибрагимович", new DateTime(2001, 3, 10), "Odessa, prospect Shevchenko, 12", "0932222222");
            Person person2 = new Person("Демидов", "Василий", "Петрович", new DateTime(1975, 8, 12), "Odessa, Tolbuhino, 10", "0933333333");
            Person person3 = new Person("Авокадо", "Анатолий", "Святославович", new DateTime(1980, 11, 1), "Odessa, Tamojennaya square, 5", "0934444444");
           
                
           
            //Console.Write(person + "\n");


            //проверка работы конструкторов 
            //Student student = new Student(person);
            //student.Person = person;
            //student.Person.LastName = "Петров";

            //Console.Write(student.Person+"\n");
            //Console.Write(person + "\n");
            
            List<Student> students = new List<Student>();
            students.Add(new Student(person));
            students.Add(new Student(person1));
            students.Add(new Student(person2));
            students.Add(new Student(person3));
            
            Group group1 = new Group();
            group1.Name = "ПВП-11";
            group1.Speciality = "Програмирование";
            group1.Course = 1;

            foreach (Student student in students)
            {
                group1.AddStudent(student);
            }

            group1.ShowStudents();
            group1.RemoveStudent(group1.FindStudent("Иванов"));

            //проверка добавления студента в группу
            students.Add(new Student(new Person("Роде", "Евгений", "Юрьевич", new DateTime(1985, 1, 22), "Odessa, Bazarnaya 45", "0937332233")));
            group1.AddStudent(students[students.Count - 1]);
            group1.ShowStudents();
            
            Console.ReadLine();
            Console.Clear();
            

            Random random = new Random();

            foreach (Student student in group1.Students)
            {
                for (int i = 0; i < 4; ++i)
                {
                    ExamMark(student, random.Next(3, 13));
                    CourseMark(student, random.Next(9, 13));
                    StudentWorkMark(student, random.Next(9, 13));

                }
            }
            
            group1.ShowStudents();
            
            Console.ReadLine();
            Console.Clear();

            try
            {
                Console.WriteLine(group1[10].Person.ToString());
            }
            catch (System.ArgumentOutOfRangeException e)
            {
                Console.WriteLine(e.Message);
            }
            Console.ReadLine();
            Console.Clear();

            //проверка отчисления студентов
            //group1.DismissAllWithAvg(7);
            //group1.ShowStudents();

            //проверка перевода студента в другую группу
            //Group group2 = new Group();
            //group1.TransferStudent("ковлев", ref group2);
            //group1.TransferStudent("Яковлев", ref group2);
            //group1.ShowStudents();
            //group2.ShowStudents();

            ////тест Clone()
            //Person clonePerson = (Person)person.Clone();
            //clonePerson.LastName = "Клонированный";
            //Console.ForegroundColor = ConsoleColor.Green;
            //Console.WriteLine($"Оригинал\n" + person);
            //Console.ForegroundColor = ConsoleColor.Yellow;
            //Console.WriteLine($"Клон\n" + clonePerson+'\n');

            //Console.ReadLine();
            //Console.Clear();

            //Student cloneStudent = (Student)students[1].Clone();
            //cloneStudent.Person.LastName = "Клонированный";
            //Console.ForegroundColor = ConsoleColor.Green;
            //Console.WriteLine($"Оригинал\n" + students[1]);
            //Console.ForegroundColor = ConsoleColor.Yellow;
            //Console.WriteLine($"Клон\n" + cloneStudent + '\n');

            //Console.ReadLine();
            //Console.Clear();

            //Group cloneGroup = (Group)group1;
            //cloneGroup.Speciality += "Клонированная";
            //Console.ForegroundColor = ConsoleColor.Green;
            //Console.WriteLine($"Оригинал");
            //group1.ShowStudents();
            //Console.ForegroundColor = ConsoleColor.Yellow;
            //Console.WriteLine($"Клон\n");
            //cloneGroup.ShowStudents();                        
            //Console.ForegroundColor = ConsoleColor.Gray;
                        
            //Console.ReadLine();
            //Console.Clear();

            ////тест перечисления
            //foreach (Student student in group1)
            //{
            //    Console.WriteLine(student.Person.ToString());
            //}

            ////сортирую студентов по среднему баллу
            //Console.WriteLine("\nСортировка по среднему баллу");
            //group1.SortGroup(new Student.AvgMarkComparer());
            //group1.ShowStudents();

            //Console.ReadLine();
            //Console.Clear();
            

            ////сортирую студентов по фамилии
            //Console.WriteLine("\nСортировка по фамилии");
            //group1.SortGroup(new Student.LastNameASCComparer());
            //group1.ShowStudents();

            //Console.ReadLine();
            //Console.Clear();
            

            ////сортирую студентов по имени
            //Console.WriteLine("\nСортировка по имени");
            //group1.SortGroup(new Student.FirstNameASCComparer());
            //group1.ShowStudents();

            //Console.ReadLine();
            //Console.Clear();
            

            //работа с делегатами
            GroupWorkDelegate groupWorkDelegate = AddStudent;
            
            groupWorkDelegate += RemoveStudent;
            groupWorkDelegate += ShowStudentsWithExcellentMark;
            groupWorkDelegate += ShowStudentsWithBadMark;
                        
            Console.Clear();
            //groupWorkDelegate(ref group1);
            bool isRunning = true;

            while (isRunning) 
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("1. Добавить студента");
                Console.WriteLine("2. Удалить студента");
                Console.WriteLine("3. Показать студентов с отличными оценками");
                Console.WriteLine("4. Показать студентов с 3-ми за экзамен");
                Console.WriteLine("5. Показать студентов");
                Console.WriteLine("6. Выход");
                Console.ForegroundColor = ConsoleColor.Gray;

                Console.Write("Выберите действие: ");
                int choice = int.Parse(Console.ReadLine());

                switch (choice)
                {
                    case 1:
                        ((GroupWorkDelegate)groupWorkDelegate.GetInvocationList()[0])(ref group1);

                        break;
                    case 2:
                        ((GroupWorkDelegate)groupWorkDelegate.GetInvocationList()[1])(ref group1);
                        break;
                    case 3:
                        ((GroupWorkDelegate)groupWorkDelegate.GetInvocationList()[2])(ref group1);
                        break;
                    case 4:
                        ((GroupWorkDelegate)groupWorkDelegate.GetInvocationList()[3])(ref group1);
                        break;
                    case 5:
                        Console.Clear();
                        group1.ShowStudents();
                        break;
                    case 6:
                        isRunning = false;
                        break;
                    default:
                        Console.WriteLine("Неверный ввод");
                        break;
                }
            }
        }
    }
}
