using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using FlightsChecking.CommonLibrary.Contracts;
using FlightsChecking.CommonLibrary.Models;

namespace FlightsChecking.WebApi.Services
{
  public class FlightRepository : IRepository<Flight>
  {
    private List<Flight> _flights;
    private static readonly string ProductFile = Path.Combine(AppContext.BaseDirectory, "flights.json");

    public void Add(Flight flight)
    {
      List<Flight> flights = GetAll().ToList();

      int nextIndex = 1;

      if (flights != null && flights.Count > 0)
      {
        int last = flights.FindLastIndex(i => i != null);
        nextIndex = flights[last].Id + 1;
      }

      flight.Id = nextIndex;
      flights.Add(flight);

      SaveFlights(flights);
    }

    private void SaveFlights(List<Flight> flights)
    {
      if (File.Exists(ProductFile))
        File.Delete(ProductFile);

      using (FileStream fs = File.Open(ProductFile, FileMode.Create))
      using (StreamWriter sw = new StreamWriter(fs))
      using (JsonWriter jw = new JsonTextWriter(sw))
      {
        jw.Formatting = Formatting.Indented;

        JsonSerializer serializer = new JsonSerializer();
        serializer.Serialize(jw, flights);
      }
    }

    public IEnumerable<Flight> GetAll()
    {
      if (!File.Exists(ProductFile))
      {
        SaveFlights(new List<Flight>());
      }

      return _flights = JsonConvert.DeserializeObject<List<Flight>>(File.ReadAllText(ProductFile));
    }

    public Flight GetById(int id)
    {
      return GetAll().First(x => x.Id.Equals(id));
    }


    public void Delete(int flightId)
    {
      List<Flight> flights = GetAll().ToList();
      Flight result = flights.Find(x => x.Id.Equals(flightId));
      flights.Remove(result);

      SaveFlights(flights);
    }

    public void Update(Flight flight)
    {
      List<Flight> flights = GetAll().ToList();

      var data = flights.Find(x => x.Id.Equals(flight.Id));

      data.Departure = flight.Departure;
      data.Company = flight.Company;
      data.Description = flight.Description;
      data.Price = flight.Price;

      SaveFlights(flights);
    }
  }
}
