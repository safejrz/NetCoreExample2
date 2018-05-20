using System;
using System.Collections.Generic;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using FlightsChecking.CommonLibrary.Models;

namespace FlightsChecking.WebMVC.Controllers
{
  public class ProductController : Controller
  {
    private AppSettings AppSettings { get; set; }
    private HttpClient WebClient { get; set; }

    public ProductController(IOptions<AppSettings> settings)
    {
      AppSettings = settings.Value;

      WebClient = new HttpClient { BaseAddress = new Uri(AppSettings.FlightsCheckingWebApiUrl) };
      WebClient.DefaultRequestHeaders.Accept.Clear();
      WebClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
    }

    public IActionResult Index()
    {
      var products = GetProducts();

      return View(products);
    }

    public IActionResult Edit(int id)
    {
      var product = GetProduct(id);
      return View(product);
    }

    [HttpPost, ActionName("Edit")]
    [ValidateAntiForgeryToken]
    public ActionResult Edit(Flight flight)
    {
      if (ModelState.IsValid)
      {
        UpdateProduct(flight);
        return RedirectToAction("Index");
      }

      return View(flight);
    }

    public IActionResult Details(int id)
    {
      var product = GetProduct(id);

      return View(product);
    }

    public IActionResult Delete(int id)
    {
      var product = GetProduct(id);
      if (product == null)
      {
        return Error();
      }

      return View(product);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public ActionResult DeleteConfirmed(int id)
    {
      DeleteProduct(id);
      return RedirectToAction("Index");
    }


    public IActionResult Create(Flight flight)
    {
      if (ModelState.IsValid)
      {
        AddProduct(flight);
        return RedirectToAction("Index");
      }

      return View();
    }

    public IActionResult Error()
    {
      return View();
    }

    private List<Flight> GetProducts()
    {
      HttpResponseMessage response = WebClient.GetAsync("/api/product").Result;
      string data = response.Content.ReadAsStringAsync().Result;
      List<Flight> flights = JsonConvert.DeserializeObject<List<Flight>>(data);
      return flights;
    }

    private Flight GetProduct(int id)
    {
      HttpResponseMessage response = WebClient.GetAsync($"/api/product/{id}").Result;
      string data = response.Content.ReadAsStringAsync().Result;
      Flight flight = JsonConvert.DeserializeObject<Flight>(data);
      return flight;
    }

    private void UpdateProduct(Flight flight)
    {
      string data = JsonConvert.SerializeObject(flight);
      var contentData = new StringContent(data, System.Text.Encoding.UTF8, "application/json");
      HttpResponseMessage response = WebClient.PutAsync("/api/product", contentData).Result;
    }

    private void AddProduct(Flight flight)
    {
      string data = JsonConvert.SerializeObject(flight);
      var contentData = new StringContent(data, System.Text.Encoding.UTF8, "application/json");
      HttpResponseMessage response = WebClient.PostAsync("/api/product", contentData).Result;
    }

    private void DeleteProduct(int id)
    {
      HttpResponseMessage response = WebClient.DeleteAsync($"/api/product/{id}").Result;
    }
  }
}
