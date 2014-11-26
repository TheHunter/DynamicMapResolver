namespace DynamicMapResolver.Test.Pocos
{
    public interface IUser
    {
        /// <summary>
        /// 
        /// </summary>
        string Name { get; }

        /// <summary>
        /// 
        /// </summary>
        string Surname { get; }

        /// <summary>
        /// 
        /// </summary>
        User Parent { get; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class User
        : IUser
    {
        /// <summary>
        /// 
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Surname { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public User Parent { get; set; }
    }

    public class UserDto
    {
        /// <summary>
        /// 
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Surname { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public UserDto Parent { get; set; }
    }
}
