using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    class Student
    {

        public string name { get; set; }
        public string lastname { get; set; }
        public string degree { get; set; }
        public string university { get; set; }
        public string email { get; set; }
        public string username { get; set; }

        public Student(string name, string lastname, string degree, string university, string email, string username)
        {
            this.name = name;
            this.lastname = lastname;
            this.degree = degree;
            this.university = university;
            this.email = email;
            this.username = username;
        }

        public Student()
        {

        }

    }
}
