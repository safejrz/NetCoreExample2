using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FlightsChecking.WebTests
{
  public class ModelValidator : Controller
  {
    public ValidationContext validationContext { get; private set; }
    public List<ValidationResult> validationResults { get; private set; }

    public void ValidateModel(object Model)
    {
      validationContext = new ValidationContext(Model, null, null);
      validationResults = new List<ValidationResult>();
      Validator.TryValidateObject(Model, validationContext, validationResults, true);
      foreach (ValidationResult validationResult in validationResults)
      {
        this.ModelState.AddModelError(string.Join(", ", validationResult.MemberNames), validationResult.ErrorMessage);
      }
    }
  }
}