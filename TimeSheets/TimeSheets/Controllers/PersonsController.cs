using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TimeSheets.Models;
using TimeSheets.Repositories;
using TimeSheets.Requests;

namespace TimeSheets.Controllers
{
    [Route("api/persons")]
    [ApiController]
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
        public IActionResult GetPersonById([FromRoute]int id)
        {
            Person person = _repository.GetPersonById(id);
            _logger.LogTrace(1, $"Query GetPersonById with param id = {id}");
            return Ok(person);
        }

        [HttpGet]
        [Route("searchTerm={term}")]
        public IActionResult GetPersonByName([FromRoute] string term)
        {
            Person person = _repository.GetPersonByName(term);
            _logger.LogTrace(1, $"Query GetPersonByName with param term = {term}");
            return Ok(person);
        }

        [HttpGet]
        [Route("skip={skip}&take={take}")]
        public IActionResult GetPersons([FromRoute] int skip, [FromRoute] int take)
        {
            List<Person> persons = _repository.GetPersons(skip, take);
            _logger.LogTrace(1, $"Query GetPersons with params skip = {skip} and take = {take}");
            return Ok(persons);
        }

        [HttpPost]
        public IActionResult AddPerson([FromBody] PersonCreateRequest request)
        {
            _repository.AddPerson(new Person
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Age = request.Age,
                Company = request.Company,
                Email = request.Email
            });
            _logger.LogTrace(1, $"Query AddPerson with params request.Age = {request.Age}, request.Company = {request.Company}, " +
                                $"request.Email = {request.Email}, request.FirstName = {request.FirstName} and request.LastName = {request.LastName}");
            return Ok();
        }

        [HttpPost]
        public IActionResult UpdatePerson([FromBody] PersonUpdateRequest request)
        {
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
            return Ok();
        }

        [HttpDelete]
        [Route("{id}")]
        public IActionResult DeletePerson([FromRoute] int id)
        {
            _repository.Delete(id);
            _logger.LogTrace(1, $"Query DeletePerson with param id = {id}");
            return Ok();
        }
    }
}
