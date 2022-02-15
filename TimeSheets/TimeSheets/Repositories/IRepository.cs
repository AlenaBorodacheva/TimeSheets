using System.Collections.Generic;
using TimeSheets.Models;

namespace TimeSheets.Repositories
{
    public interface IRepository
    {
        Person GetPersonById(int id);
        Person GetPersonByName(string name);
        List<Person> GetPersons(int from, int to);
        void AddPerson(Person person);
        void UpdatePerson(Person person);
        void Delete(int id);
    }
}
