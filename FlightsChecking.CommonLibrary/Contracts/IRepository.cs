using System.Collections.Generic;

namespace FlightsChecking.CommonLibrary.Contracts
{
  public interface IRepository<T>
  {
    //TODO: Update, make it more generic
    void Add(T product);
    IEnumerable<T> GetAll();
    T GetById(int id);
    void Delete(int id);
    void Update(T product);
  }
}
