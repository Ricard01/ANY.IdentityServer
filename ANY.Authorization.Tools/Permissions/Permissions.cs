using System.ComponentModel.DataAnnotations;

namespace ANY.Authorization.Tools.Permissions;

    public enum Permissions : short
    {
        NotSet = 0, //error condition

    //Here is an example of very detailed control over something
    [Display(GroupName = "UserAdmin", Name = "Read users", Description = "Can list User")]
    UserRead = 10,
    //This is an example of grouping multiple actions under one permission
    [Display(GroupName = "UserAdmin", Name = "Alter user", Description = "Can do anything to the User")]
    UserChange = 11,

    [Display(GroupName = "UserAdmin", Name = "Read Roles", Description = "Can list Role")]
    RoleRead = 20,
    [Display(GroupName = "UserAdmin", Name = "Change Role", Description = "Can create, update or delete a Role")]
    RoleChange = 21,


    //This is an example of what to do with permission you don't used anymore.
    //You don't want its number to be reused as it could cause problems 
    //Just mark it as obsolete and the PermissionDisplay code won't show it
    [Obsolete]
    [Display(GroupName = "Old", Name = "Not used", Description = "example of old permission")]
    OldPermissionNotUsed = 100,

    //This is an example of a permission linked to a optional (paid for?) feature
    //The code that turns roles to permissions can
    //remove this permission if the user isn't allowed to access this feature
    //[LinkedToModule(PaidForModules.Feature1)]
    //[Display(GroupName = "Features", Name = "Feature1", Description = "Can access feature1")]
    //Feature1Access = 1000,
    //[LinkedToModule(PaidForModules.Feature2)]
    //[Display(GroupName = "Features", Name = "Feature2", Description = "Can access feature2")]
    //Feature2Access = 1001,

    [Display(GroupName = "SuperAdmin", Name = "AccessAll", Description = "This allows the user to access every feature")]
    AccessAll = Int16.MaxValue,
    }
