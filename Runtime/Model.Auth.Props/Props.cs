using System;

namespace GameStack.Model.Auth.Props
{
    [Serializable]
    public class CreateGameStackUserProps
    {
        /// <summary>
        /// Username is the alias the user wants.
        /// </summary>
        public string Username;
        /// <summary>
        /// Email is the users email address.
        /// </summary>
        public string Email;
        /// <summary>
        /// Password is the users password.
        /// </summary>
        public string Password;
        /// <summary>
        /// Name is the users real name.
        /// </summary>
        public string Name;

        public CreateGameStackUserProps()
        {
            Username = null;
            Email = null;
            Password = null;
            Name = null;
        }

        public CreateGameStackUserProps(string username, string email,
            string password, string name)
        {
            Username = username;
            Email = email;
            Password = password;
            Name = name;
        }

        public override string ToString()
        {
            return typeof(CreateGameStackUserProps).FullName + ":: Username: " +
                Username + " Email: " + Email + " Password: " + Password +
                " Name: " + Name;
        }
    }

    /// <summary>
    /// CreateGameStackUserPropsBuilder is an object builder for the
    /// CreateGameStackUserProps class.
    /// </summary>
    public class CreateGameStackUserPropsBuilder
    {
        private CreateGameStackUserProps props = new CreateGameStackUserProps();

        /// <summary>
        /// Username sets the username that will be used in the built result.
        /// </summary>
        public CreateGameStackUserPropsBuilder Username(string username)
        {
            props.Username = username;
            return this;
        }

        /// <summary>
        /// Email sets the email that will be used in the built
        /// result.
        /// </summary>
        public CreateGameStackUserPropsBuilder Email(string email)
        {
            props.Email = email;
            return this;
        }

        /// <summary>
        /// Password sets the password that will be used in the built result.
        /// </summary>
        public CreateGameStackUserPropsBuilder Password(string password)
        {
            props.Password = password;
            return this;
        }

        /// <summary>
        /// name sets the name that will be used in the built result.
        /// </summary>
        public CreateGameStackUserPropsBuilder Name(string name)
        {
            props.Name = name;
            return this;
        }

        /// <summary>
        /// Build creates a new CreateGameStackUserProps object with the
        /// configured settings.
        /// </summary>
        public CreateGameStackUserProps Build()
        {
            return new CreateGameStackUserProps(props.Username, props.Email,
                props.Password, props.Name);
        }
    }
}
