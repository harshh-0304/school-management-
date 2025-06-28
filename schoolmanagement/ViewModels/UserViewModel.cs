    using System.Collections.Generic;

    namespace schoolmanagement.ViewModels // <--- THIS IS THE CORRECT NAMESPACE
    {
        public class UserViewModel
        {
            public string Id { get; set; }
            public string Email { get; set; }
            public List<string> Roles { get; set; } = new List<string>();
        }
    }
    