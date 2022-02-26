using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TimeSheets.Models;
using TimeSheets.Repositories;
using TimeSheets.Requests;

namespace TimeSheets.Controllers
{
    [ApiController]
    [Route("api/persons")]
    public class PersonsController : ControllerBase
    {
        private readonly IRepository _repository;
        private readonly ILogger<PersonsController> _logger;

        public PersonsController(IRepository repository, ILogger<PersonsController> logger)
        {
            _repository = repository;
            _logger = logger;
            _logger.LogDebug(1, "PersonsController created");
        }

        [HttpGet]
        [Route("{id}")]
        public ActionResult<Person> GetPersonById(int id)
        {
            Person person = _repository.GetPersonById(id);
            _logger.LogTrace(1, $"Query GetPersonById with param id = {id}");
            return person;
        }

        [HttpGet]
        [Route("searchTerm={term}")]
        public ActionResult<Person> GetPersonByName(string term)
        {
            Person person = _repository.GetPersonByName(term);
            _logger.LogTrace(1, $"Query GetPersonByName with param term = {term}");
            return person;
        }

        [HttpGet]
        [Route("skip={skip}&take={take}")]
        public ActionResult<List<Person>> GetPersons(int skip, int take)
        {
            if (take < skip)
            {
                _logger.LogError("take < skip");
                return BadRequest();
            }

            try
            {
                List<Person> persons = _repository.GetPersons(skip, take);
                _logger.LogTrace(1, $"Query GetPersons with params skip = {skip} and take = {take}");
                return persons;
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.ToString());
                return BadRequest(); 
            }
        }

        [HttpPost]
        public ActionResult AddPerson([FromBody] PersonCreateRequest request)
        {
            List<Person> persons = _repository.GetAllPersons();
            List<int> ids = persons.Select(x => x.Id).ToList();
            int id = 1;
            while (ids.Contains(id))
            {
                id++;
            }

            _repository.AddPerson(new Person
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Age = request.Age,
                Company = request.Company,
                Email = request.Email,
                Id = id
            });
            _logger.LogTrace(1, $"Query AddPerson with params request.Age = {request.Age}, request.Company = {request.Company}, " +
                                $"request.Email = {request.Email}, request.FirstName = {request.FirstName} and request.LastName = {request.LastName}");
            return NoContent();
        }

        [HttpPut]
        public ActionResult UpdatePerson([FromBody] PersonUpdateRequest request)
        {
            List<Person> persons = _repository.GetAllPersons();
            List<int> ids = persons.Select(x => x.Id).ToList();
            if (!ids.Contains(request.Id))
            {
                _logger.LogError("Not found");
                return BadRequest();
            }

            _repository.UpdatePerson(new Person
            {
                Id = request.Id,
                FirstName = request.FirstName,
                LastName = request.LastName,
                Age = request.Age,
                Company = request.Company,
                Email = request.Email
            });
            _logger.LogTrace(1, $"Query UpdatePerson with params request.Id = {request.Id}, request.Age = {request.Age}," +
                                $" request.Company = {request.Company}, request.Email = {request.Email}, request.FirstName = {request.FirstName} " +
                                $"and request.LastName = {request.LastName}");
            return NoContent();
        }

        [HttpDelete]
        [Route("{id}")]
        public ActionResult DeletePerson(int id)
        {
            List<Person> persons = _repository.GetAllPersons();
            List<int> ids = persons.Select(x => x.Id).ToList();
            if (!ids.Contains(id))
            {
                _logger.LogError("Not found");
                return BadRequest();
            }

            _repository.Delete(id);
            _logger.LogTrace(1, $"Query DeletePerson with param id = {id}");
            return NoContent();
        }
    }
}
