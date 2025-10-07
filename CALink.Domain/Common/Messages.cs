using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CALink.Domain.Common
{
    public class Messages
    {
        //AuthServices Messages
        public const string IncorrectEmailPass = "Incorrect email or password.";
        public const string LoginSuccessful = "Login Successfully.";
        public const string Unauthorized = "You are not authorized to access this resource.";        
        public const string UnActive = "Your account is not active";

        //General Messages
        public const string NotFound = "Not found";
        public const string BadRequest = "Bad request";
        public const string Error = "Error";
        public const string InvalidData = "Invalid request data";
        public const string EmailAlreadyExists = "Email already exists";

        //AppUserService Messages
        public const string AppUserCreate = "App User created successfully";
        public const string AppUserUpdated = "App User updated successfully";
        public const string AppUserRetrievedSuccess = "App User retrieved successfully";
        public const string AppUsersRetrievedSuccess = "App Users retrieved successfully.";
        public const string AppUserDeleted = "App User deleted successfully";
        public const string AppUserNotFound = "App User not found";

        //CompanyService Messages
        public static string CompanyCreate(string name) => $"{name} created successfully";       
        public static string CompanyUpdated(string name) => "{name} updated successfully";
        public const string CompanyRetrievedSuccess = "Company retrieved successfully";
        public const string CompaniesRetrievedSuccess = "Companies retrieved successfully.";
        public static string CompanyDeleted(string name) => $"{name} Deleted successfully";
        public const string CompanyNotFound = "Company not found";    
        public const string CompanyUnActive = "Company is not active";
        public const string IncorrectSecratCode = "Incorrect secrat code";

        //Category Service
        public static string CategoryCreate(string name) => $"{name} create successfully";
        public static string CategoryUpdate(string name) => "{name} updated successfully";
        public const string categoryRetriveedSuccess = "Category retrived successfully";
        public const string CategoryRetrievedSuccess = "Categories retrived successfully";

        public static string CategoryDeleted(string name) => $"{name} Deleted successfully";
        public const string CategoryNotFound = "Category Not Found";
        public const string CategoryUnActive = "Category is not active";
        public const string IncorrectSecretCode = "Incorrect secret code";
    }
}
