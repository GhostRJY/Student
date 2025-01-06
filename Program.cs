using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Institute
{
    internal class Program
    {
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

            Random random = new Random();

            foreach (Student student in group1.Students)
            {
                for (int i = 0; i < 4; ++i)
                {
                    ExamMark(student, random.Next(3, 13));
                    CourseMark(student, random.Next(3, 13));
                    StudentWorkMark(student, random.Next(3, 13));

                }
            }
            group1.ShowStudents();

            group1.DismissBadStudent();
            group1.ShowStudents();
            
            try
            {
                Console.WriteLine(group1[10].Person.ToString());
            }
            catch (System.ArgumentOutOfRangeException e)
            {
                Console.WriteLine(e.Message);
            }

            
            //проверка отчисления студентов
            //group1.DismissAllWithAvg(7);
            //group1.ShowStudents();

            //проверка перевода студента в другую группу
            //Group group2 = new Group();
            //group1.TransferStudent("ковлев", ref group2);
            //group1.TransferStudent("Яковлев", ref group2);
            //group1.ShowStudents();
            //group2.ShowStudents();
        }
    }
}
