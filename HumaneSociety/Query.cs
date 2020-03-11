﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumaneSociety
{
    public static class Query
    {        
        static HumaneSocietyDataContext db;

        static Query()
        {
            db = new HumaneSocietyDataContext();
        }

        internal static List<USState> GetStates()
        {
            List<USState> allStates = db.USStates.ToList();       

            return allStates;
        }
            
        internal static Client GetClient(string userName, string password)
        {
            Client client = db.Clients.Where(c => c.UserName == userName && c.Password == password).Single();

            return client;
        }

        internal static List<Client> GetClients()
        {
            List<Client> allClients = db.Clients.ToList();

            return allClients;
        }

        internal static void AddNewClient(string firstName, string lastName, string username, string password, string email, string streetAddress, int zipCode, int stateId)
        {
            Client newClient = new Client();

            newClient.FirstName = firstName;
            newClient.LastName = lastName;
            newClient.UserName = username;
            newClient.Password = password;
            newClient.Email = email;

            Address addressFromDb = db.Addresses.Where(a => a.AddressLine1 == streetAddress && a.Zipcode == zipCode && a.USStateId == stateId).FirstOrDefault();

            // if the address isn't found in the Db, create and insert it
            if (addressFromDb == null)
            {
                Address newAddress = new Address();
                newAddress.AddressLine1 = streetAddress;
                newAddress.City = null;
                newAddress.USStateId = stateId;
                newAddress.Zipcode = zipCode;                

                db.Addresses.InsertOnSubmit(newAddress);
                db.SubmitChanges();

                addressFromDb = newAddress;
            }

            // attach AddressId to clientFromDb.AddressId
            newClient.AddressId = addressFromDb.AddressId;

            db.Clients.InsertOnSubmit(newClient);

            db.SubmitChanges();
        }

        internal static void UpdateClient(Client clientWithUpdates)
        {
            // find corresponding Client from Db
            Client clientFromDb = null;

            try
            {
                clientFromDb = db.Clients.Where(c => c.ClientId == clientWithUpdates.ClientId).Single();
            }
            catch(InvalidOperationException e)
            {
                Console.WriteLine("No clients have a ClientId that matches the Client passed in.");
                Console.WriteLine("No update have been made.");
                return;
            }
            
            // update clientFromDb information with the values on clientWithUpdates (aside from address)
            clientFromDb.FirstName = clientWithUpdates.FirstName;
            clientFromDb.LastName = clientWithUpdates.LastName;
            clientFromDb.UserName = clientWithUpdates.UserName;
            clientFromDb.Password = clientWithUpdates.Password;
            clientFromDb.Email = clientWithUpdates.Email;

            // get address object from clientWithUpdates
            Address clientAddress = clientWithUpdates.Address;

            // look for existing Address in Db (null will be returned if the address isn't already in the Db
            Address updatedAddress = db.Addresses.Where(a => a.AddressLine1 == clientAddress.AddressLine1 && a.USStateId == clientAddress.USStateId && a.Zipcode == clientAddress.Zipcode).FirstOrDefault();

            // if the address isn't found in the Db, create and insert it
            if(updatedAddress == null)
            {
                Address newAddress = new Address();
                newAddress.AddressLine1 = clientAddress.AddressLine1;
                newAddress.City = null;
                newAddress.USStateId = clientAddress.USStateId;
                newAddress.Zipcode = clientAddress.Zipcode;                

                db.Addresses.InsertOnSubmit(newAddress);
                db.SubmitChanges();

                updatedAddress = newAddress;
            }

            // attach AddressId to clientFromDb.AddressId
            clientFromDb.AddressId = updatedAddress.AddressId;
            
            // submit changes
            db.SubmitChanges();
        }
        
        internal static void AddUsernameAndPassword(Employee employee)
        {
            Employee employeeFromDb = db.Employees.Where(e => e.EmployeeId == employee.EmployeeId).FirstOrDefault();

            employeeFromDb.UserName = employee.UserName;
            employeeFromDb.Password = employee.Password;

            db.SubmitChanges();
        }

        internal static Employee RetrieveEmployeeUser(string email, int employeeNumber)
        {
            Employee employeeFromDb = db.Employees.Where(e => e.Email == email && e.EmployeeNumber == employeeNumber).FirstOrDefault();

            if (employeeFromDb == null)
            {
                throw new NullReferenceException();
            }
            else
            {
                return employeeFromDb;
            }
        }

        internal static Employee EmployeeLogin(string userName, string password)
        {
            Employee employeeFromDb = db.Employees.Where(e => e.UserName == userName && e.Password == password).FirstOrDefault();

            return employeeFromDb;
        }

        internal static bool CheckEmployeeUserNameExist(string userName)
        {
            Employee employeeWithUserName = db.Employees.Where(e => e.UserName == userName).FirstOrDefault();

            return employeeWithUserName != null;
        }


        //// TODO Items: ////
        
        // TODO: Allow any of the CRUD operations to occur here
        internal static void RunEmployeeQueries(Employee employee, string crudOperation)
        {
            //throw new NotImplementedException();
            switch (crudOperation)
            {
                case "create":
                    CreateEmployee(employee);
                    break;
                case "read":
                    ReadEmployee(employee);
                    break;
                case "update":
                    UpdateEmployee(employee);
                    break;
                case "delete":
                    DeleteEmployee(employee);
                    break;
                default:
                    break;
            }
        }
        private static void CreateEmployee(Employee employee)
        {
            db.Employees.InsertOnSubmit(employee);
            db.SubmitChanges();
        }
        private static void ReadEmployee(Employee employee)
        {
            Employee foundEmployee = db.Employees.FirstOrDefault(a => a.EmployeeNumber == employee.EmployeeNumber);
            
            UserInterface.DisplayEmployeeInfo(foundEmployee);
        }
        private static void UpdateEmployee(Employee employee)
        {
            try
            {
                Employee employeeToUpdate = db.Employees.FirstOrDefault(e => e.FirstName == employee.FirstName && e.LastName == employee.LastName && e.EmployeeNumber == employee.EmployeeNumber);
                List<string> options = new List<string>() { "What would you like to update? Please enter number of option. When done updating.", "1: First Name", "2: Last Name", "3: Employee Number", "4: Email", "5: Username", "6: Password", "7: Submit Changes" };
                Dictionary<string, string> valuesToUpdate = new Dictionary<string, string>();

                int input = 0;
                var employees = db.Employees.ToList<Employee>();
                List<int?> employeeNumbers = employees.Select(a => a.EmployeeNumber).ToList<int?>();
                List<string> usernames = employees.Select(a => a.UserName).ToList<string>();
                GetNewValuesForDictionary(input, options, valuesToUpdate, employeeNumbers, usernames);
                SubmitDictionaryToDB(valuesToUpdate, employeeToUpdate);
                UserInterface.DisplayEmployeeInfo(employeeToUpdate);
            }
            catch (Exception)
            {
                UserInterface.DisplayUserOptions("Employee not found. Please try again.");
                UserInterface.Pause();
            }
        }
        private static void GetNewValuesForDictionary(int input, List<string> options, Dictionary<string, string> valuesToUpdate, List<int?> employeeNumbers, List<string> usernames)
        {
            while (input != 7)
            {
                UserInterface.DisplayUserOptions(options);
                input = UserInterface.GetIntegerData();
                bool validResponse = false;
                switch (input)
                {
                    case 1:
                        UserInterface.DisplayUserOptions("New First Name:");
                        valuesToUpdate["FirstName"] = UserInterface.GetUserInput();
                        break;
                    case 2:
                        UserInterface.DisplayUserOptions("New Last Name:");
                        valuesToUpdate["LastName"] = UserInterface.GetUserInput();
                        break;
                    case 3:
                        ValidateEmployeeNumber(valuesToUpdate,employeeNumbers,validResponse);
                        do
                        {
                            UserInterface.DisplayUserOptions("New Employee Number:");
                            valuesToUpdate["EmployeeNumber"] = UserInterface.GetUserInput();
                            validResponse = int.TryParse(valuesToUpdate["EmployeeNumber"], out int num) && !employeeNumbers.Contains(num);
                        } while (!validResponse);
                        break;
                    case 4:
                        UserInterface.DisplayUserOptions("New Email:");
                        valuesToUpdate["Email"] = UserInterface.GetEmail();
                        break;
                    case 5:
                        UserInterface.DisplayUserOptions("New Username:");
                        string username = UserInterface.GetUserInput();
                        if (!usernames.Contains(username))
                        {
                            valuesToUpdate["Username"] = username;
                        }
                        break;
                    case 6:
                        UserInterface.DisplayUserOptions("New Password:");
                        valuesToUpdate["Password"] = UserInterface.GetUserInput();
                        break;
                    case 7:
                        break;
                }
            }
        }
        private static void SubmitDictionaryToDB(Dictionary<string,string> keyValues, Employee employeeToUpdate)
        {
            if(keyValues.ContainsKey("FirstName"))
            {
                employeeToUpdate.FirstName = keyValues["FirstName"];
            }
            if (keyValues.ContainsKey("LastName"))
            {
                employeeToUpdate.LastName = keyValues["LastName"];
            }
            if (keyValues.ContainsKey("EmployeeNumber"))
            {
                employeeToUpdate.EmployeeNumber = int.Parse(keyValues["EmployeeNumber"]);
            }
            if (keyValues.ContainsKey("Email"))
            {
                employeeToUpdate.Email = keyValues["Email"];
            }
            if (keyValues.ContainsKey("Username"))
            {
                employeeToUpdate.UserName = keyValues["Username"];
            }
            if (keyValues.ContainsKey("Password"))
            {
                employeeToUpdate.Password = keyValues["Password"];
            }
            db.SubmitChanges();
        }
        private static void DeleteEmployee(Employee employee)
        {
            Employee foundEmployee = db.Employees.FirstOrDefault(a => a.EmployeeNumber == employee.EmployeeNumber);
            db.Employees.DeleteOnSubmit(foundEmployee);
            db.SubmitChanges();
        }

        // TODO: Animal CRUD Operations
        internal static void AddAnimal(Animal animal)
        {
            db.Animals.InsertOnSubmit(animal);
            db.SubmitChanges();
        }

        internal static Animal GetAnimalByID(int id)
        {
            throw new NotImplementedException();
        }

        internal static void UpdateAnimal(int animalId, Dictionary<int, string> updates)
        {            
            throw new NotImplementedException();
        }

        internal static void RemoveAnimal(Animal animal)
        {
            throw new NotImplementedException();
        }
        
        // TODO: Animal Multi-Trait Search
        internal static IQueryable<Animal> SearchForAnimalsByMultipleTraits(Dictionary<int, string> updates) // parameter(s)?
        {
            throw new NotImplementedException();
        }
         
        // TODO: Misc Animal Things
        internal static int GetCategoryId(string categoryName)
        {
            return db.Categories.FirstOrDefault(a => a.Name == categoryName) != null ? db.Categories.FirstOrDefault(a => a.Name == categoryName).CategoryId : 0;
        }
        
        internal static Room GetRoom(int animalId)
        {
            throw new NotImplementedException();
        }
        
        internal static int GetDietPlanId(string dietPlanName)
        {
            throw new NotImplementedException();
        }

        // TODO: Adoption CRUD Operations
        internal static void Adopt(Animal animal, Client client)
        {
            throw new NotImplementedException();
        }

        internal static IQueryable<Adoption> GetPendingAdoptions()
        {
            throw new NotImplementedException();
        }

        internal static void UpdateAdoption(bool isAdopted, Adoption adoption)
        {
            throw new NotImplementedException();
        }

        internal static void RemoveAdoption(int animalId, int clientId)
        {
            throw new NotImplementedException();
        }

        // TODO: Shots Stuff
        internal static IQueryable<AnimalShot> GetShots(Animal animal)
        {
            throw new NotImplementedException();
        }

        internal static void UpdateShot(string shotName, Animal animal)
        {
            throw new NotImplementedException();
        }
    }
}