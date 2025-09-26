using ECPAPI.Data;

namespace ECPAPI.Data
{
    public class Role
    {
        public int Id { get; set; }
        public string RoleName { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }    // განმარტება იხილეთ ქვედა ველში
        public bool IsDeleted { get; set; }   //// განმარტება იხილეთ ქვედა ველში
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public virtual ICollection<RolePrivilege> RolePrivileges { get; set; }
        public virtual ICollection<UserRoleMapping> UserRoleMappings { get; set; }
    }
}

//////////////////////// განმარტება IsActive-ს და IsDeleted-ს შორის:
///თუ IsActive = false - როლი დროებით გამორთულია, მაგრამ არ არის წაშლილი
///IsActive = true - როლი აქტიურია და შეიძლება მიენიჭოს მომხმარებლებს 
///IsActive = false - როლი დეაქტივირებულია(დროებით გამორთული)
///თუ IsDeleted = true - როლი მონიშნულია როგორც წაშლილი (არ ჩანს აპლიკაციაში)
///IsDeleted = true - როლი "წაშლილია" (არ ჩანს UI-ზე) 
///IsDeleted = false - როლი არ არის წაშლილი
///
// აქტიური და არაწაშლილი როლი
//var role1 = new Role
//{
//    IsActive = true,
//    IsDeleted = false
//};
//// ✅ გამოიყენება აპლიკაციაში

//// დეაქტივირებული, მაგრამ არაწაშლილი როლი
//var role2 = new Role
//{
//    IsActive = false,
//    IsDeleted = false
//};
//// ⚠️ დროებით გამორთულია, მაგრამ შეიძლება კვლავ აქტიური გახდეს

//// წაშლილი როლი (soft delete)
//var role3 = new Role
//{
//    IsActive = false,
//    IsDeleted = true
//};
//// X "წაშლილია", არ ჩანს აპლიკაციაში

//SQL query-ებში გამოყენება:
//--მხოლოდ აქტიური და არაწაშლილი როლები
//SELECT * FROM Roles WHERE IsActive = 1 AND IsDeleted = 0

//-- ყველა როლი (წაშლილების ჩათვლით)
//SELECT * FROM Roles WHERE IsDeleted = 0

//-- დეაქტივირებული, მაგრამ არაწაშლილი როლები
//SELECT * FROM Roles WHERE IsActive = 0 AND IsDeleted = 0