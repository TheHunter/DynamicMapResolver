namespace DynamicMapResolver.Test.Pocos
{
    /// <summary>
    /// 
    /// </summary>
    public class Student
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public int AnnoNascita { get; set; }
        public Person Father { get; set; }
    }

    public class StudentDetails
        : Student
    {
        public string CF { get; set; }
    }

}
