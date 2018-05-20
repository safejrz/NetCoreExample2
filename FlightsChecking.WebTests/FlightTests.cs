using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using FlightsChecking.CommonLibrary.Models;
using FlightsChecking.WebApi.Controllers;
using FlightsChecking.WebApi.Services;

namespace FlightsChecking.WebTests
{
  public class FlightTests
  {
    private readonly FlightRepository _FlightRepository = new FlightRepository();
    private FlightController FlightController { get; set; }

    [Fact]
    public void GetFlightTest()
    {
      Initialize();
      try
      {
        if (GetAllFlights() == null)
          Assert.True(false, "Getting all Flights failed. Verify you have writting permissions in the WebApi folder.");
      }
      catch
      {
        Assert.True(false, "Getting all Flights succeed.");
        throw;
      }
    }

    [Fact]
    public void CreateFlightTest()
    {
      Initialize();
      Flight Flight = CreateFlight();
      var testFlight = GetFlight(Flight);

      if (testFlight == null) Assert.True(false, "Creating new Flight failed. The Flight cannot be found.");
      Assert.Equal(Flight.Id, testFlight.Id);
      Assert.Equal(Flight.Description, testFlight.Description);      
      Assert.Equal(Flight.Company, testFlight.Company);
      Assert.Equal(Flight.Price, testFlight.Price);
    }

    [Fact]
    public void UpdateFlightTest()
    {
      Initialize();
      var Flight = CreateFlight();

      //update Flight            
      Flight.Description = "Updated Mockup Description";      
      Flight.Company = "Updated Company";
      Flight.Price = Convert.ToDecimal(1.00);
      FlightController.Update(Flight);

      //Verify all values were changed for the selected Flight.
      var testFlight = GetFlight(Flight);      
      Assert.True(testFlight.Description.Contains("Updated"), "Success, Flight Descrition was updated.");      
      Assert.True(testFlight.Company.Contains("Updated"), "Success, Flight's Company was updated.");
      Assert.Equal(testFlight.Price, 1);
    }

    [Fact]
    public void DeleteFlightTest()
    {
      Initialize();
      var Flight = CreateFlight();
      FlightController.Delete(Flight.Id);
      var testFlight = GetFlight(Flight);

      //if testFlight is null, it means it was deleted succesfully.
      Assert.True(testFlight == null, "Success the Flight was not found because it was deleted.");
    }

    [Fact]
    public void FlightValidationsTest()
    {
      Initialize();
      var Flight = CreateFlight();
      ModelValidator modelvalidator = new ModelValidator();
      modelvalidator.ValidateModel(Flight);
      Assert.True(modelvalidator.ModelState.IsValid, "A valid model is recognized.");

      Flight = CreateFlight(true);
      modelvalidator.ValidateModel(Flight);
      Assert.False(modelvalidator.ModelState.IsValid, $"Validation is working fine and encountered {modelvalidator.validationResults.Count} errors.");
    }

    private Flight CreateFlight(bool testingValidation = false)
    {
      Flight Flight = new Flight()
      {        
        Description = "Mockup Description.",        
        Company = "Mock Company",
        Price = Convert.ToDecimal(new Random().Next(0, 100))
      };

      if (testingValidation)
      {       
        Flight.Description = "Mockup Description Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Aenean commodo ligula eget dolor.";       
        Flight.Company = null;
        Flight.Price = 5000;
      }

      if (!testingValidation)
      {
        FlightController.Create(Flight);
      }

      return Flight;
    }

    private Flight GetFlight(Flight Flight)
    {
      List<Flight> Flights = GetAllFlights();
      var testFlight = Flights.Find(p => p.Id == Flight.Id);

      return testFlight;
    }

    private List<Flight> GetAllFlights()
    {
      return FlightController.Get().ToList();
    }

    private void Initialize()
    {
      if (FlightController != null) return;
      FlightController = new FlightController(_FlightRepository);
    }
  }
}