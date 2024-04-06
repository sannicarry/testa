using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos.Car;
using api.Helpers;
using api.Models;

namespace api.Interfaces
{
    public interface ICarRepository
    {
        Task<Car?> GetByIdAsync(int id);
        Task<Car> CreateAsync(Car carModel);
        Task<Car?> UpdateAsync(Car carModel);
        Task<List<string>> GetCarImageByIdAsync(int id);
        Task<List<Car>> GetAllAsync(QueryObject query);
    }
}