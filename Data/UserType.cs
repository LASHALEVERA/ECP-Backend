using ECPAPI.Data;
using System.Runtime.InteropServices;

namespace ECPAPI.Data
{
    public class UserType
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public virtual ICollection<User> Users { get; set; }
    }
}
//Role - სა და UserType-ს შორის განსხვავება ეს არის სხვადასხვა დონის ავტორიზაციის კონცეფციები როლებზე დაფუძნებული სისტემაში:

//UserType(მომხმარებლის ტიპი) - Coarse - grained(უხეში) კონტროლი
//რა არის: მომხმარებლის ძირითადი კატეგორია/ტიპი
//დანიშნულება: განსაზღვრავს მომხმარებლის ძირითად სტატუსს სისტემაში
//მაგალითები: Admin, Customer, Moderator, Vendor
//"დამოკიდებულება: ერთი მომხმარებელი → ერთი UserType"
//Role (როლი) - Fine-grained (ნაზი) კონტროლი
//რა არის: ნებართვების / პრივილეგიების ნაკრები
//დანიშნულება: განსაზღვრავს კონკრეტულ ქმედებებს, რომელთა შესრულებაც შეუძლია
//მაგალითები: CanEditProducts, CanDeleteUsers, CanViewReports
//"დამოკიდებულება: ერთ მომხმარებელს → მრავალი Role"

// UserType - ძირითადი კატეგორია
//public class UserType
//{
//    public int Id = 1;
//    public string Name = "Administrator";
//}

///Roles - კონკრეტული ნებართვები
//public class Role
//{
//    public int Id = 1;
//    public string RoleName = "CanManageUsers";

//    public int Id = 2;
//    public string RoleName = "CanEditProducts";

//    public int Id = 3;
//    public string RoleName = "CanViewReports";
//}

// მომხმარებელი
//var user = new User
//{
//    UserTypeId = 1, // Administrator
//    UserRoleMappings = new List<UserRoleMapping> {
//        new() { RoleId = 1 }, // CanManageUsers
//        new() { RoleId = 2 }, // CanEditProducts
//        new() { RoleId = 3 }  // CanViewReports
//    }
//};

//UserType გამოვიყენოთ, როცა გვინდა:
//* მომხმარებლის ძირითადი კატეგორიის განსაზღვრა
//* UI/UX განსხვავებები (სხვადასხვა დაფა სხვადასხვა ტიპის მომხმარებლისთვის)
//* სისტემური პრივილეგიების განსაზღვრა

//Role გამოვიყენოთ, როცა გვინდა:
//* კონკრეტული ქმედებების ავტორიზაცია
//* დინამიური ნებართვების მენეჯმენტი
//* მრავალჯერადი პრივილეგიების კომბინაცია

//ტიპიური არქიტექტურა:

//User(მომხმარებელი)
//│
//├── UserType(ტიპი): "Admin", "Customer", "Moderator"
//│   └── განსაზღვრავს ძირითად დონეს/კატეგორიას
//│
//└── Roles (როლები): 
//    ├── "CanCreateProducts"
//    ├── "CanDeleteUsers"
//    ├── "CanViewReports"
//    └── განსაზღვრავს კონკრეტულ ნებართვებს