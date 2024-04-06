using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Models;

namespace api.Interfaces
{
    public interface ICarImageRepository
    {
        Task<List<string>> GetUrlByCarId(int carId);
        Task<CarImage?> UpdateImageAsync(int CarId, string url, int index);
        Task<List<CarImage>> GetCarImageByCarId(int carId);
    }
}