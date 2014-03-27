using DynamicMapResolver.Test.Pocos;

namespace DynamicMapResolver.Test
{
    /// <summary>
    /// 
    /// </summary>
    public class CustomTransformer
        : IUserTransformer<Student, Person>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="destination"></param>
        public void Transform(Student source, Person destination)
        {
            if (destination != null)
            {
                destination.Name = source.Name;
                destination.Surname = source.Surname;

                if (source.AnnoNascita != 0)
                    destination.AnnoNascita = source.AnnoNascita;
            }
        }
    }
}
