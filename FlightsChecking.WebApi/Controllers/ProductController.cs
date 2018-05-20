using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using FlightsChecking.CommonLibrary.Contracts;
using FlightsChecking.CommonLibrary.Models;


namespace ProductChecking.WebApi.Controllers
{
  [Route("api/[controller]")]
  public class ProductController : Controller
  {
    public IRepository<Product> Products { get; set; }

    public ProductController(IRepository<Product> ProductRepository)
    {
      Products = ProductRepository;
    }

    // GET api/values
    [HttpGet]
    public IEnumerable<Product> Get()
    {
      return Products.GetAll();
    }

    [HttpGet("{id}", Name = "GetProduct")]
    public IActionResult GetById(int id)
    {
      var item = Products.GetById(id);
      if (item == null)
      {
        return NotFound();
      }
      return new ObjectResult(item);
    }

    [HttpPost]
    public IActionResult Create([FromBody] Product Product)
    {
      if (Product == null)
      {
        return BadRequest();
      }

      Products.Add(Product);
      return CreatedAtRoute("GetProduct", new { id = Product.Id }, Product);
    }

    [HttpPut]
    public IActionResult Update([FromBody] Product item)
    {
      Products.Update(item);
      return new NoContentResult();
    }


    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
      Products.Delete(id);
      return new NoContentResult();
    }
  }
}
