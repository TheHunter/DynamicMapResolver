namespace DynamicMapResolver.Test.Pocos
{
    /// <summary>
    /// 
    /// </summary>
    public class Person
        : IPersonHeader
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public double? AnnoNascita { get; set; }
        public Person Parent { get; set; }
    }
}
