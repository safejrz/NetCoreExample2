using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using FlightsChecking.CommonLibrary.Contracts;
using FlightsChecking.CommonLibrary.Models;

namespace FlightsChecking.WebApi.Controllers
{
  [Route("api/[controller]")]
  public class FlightController : Controller
  {
    public IRepository<Flight> Flights { get; set; }

    public FlightController(IRepository<Flight> flightsRepository)
    {
      Flights = flightsRepository;
    }

    // GET api/values
    [HttpGet]
    public IEnumerable<Flight> Get()
    {
      return Flights.GetAll();
    }

    [HttpGet("{id}", Name = "GetFlight")]
    public IActionResult GetById(int id)
    {
      var item = Flights.GetById(id);
      if (item == null)
      {
        return NotFound();
      }
      return new ObjectResult(item);
    }

    [HttpPost]
    public IActionResult Create([FromBody] Flight flight)
    {
      if (flight == null)
      {
        return BadRequest();
      }

      Flights.Add(flight);
      return CreatedAtRoute("GetFlight", new { id = flight.Id }, Flights);
    }

    [HttpPut]
    public IActionResult Update([FromBody] Flight item)
    {
      Flights.Update(item);
      return new NoContentResult();
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
      Flights.Delete(id);
      return new NoContentResult();
    }
  }
}
