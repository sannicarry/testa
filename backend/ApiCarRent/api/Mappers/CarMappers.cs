using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos.Car;
using api.Models;

namespace api.Mappers
{
    public static class CarMappers
    {
        public static CarDto ToCarDto(this Car car)
        {
            return new CarDto
            {
                CarId = car.CarId,
                Make = car.Make,
                Model = car.Model,
                Type = car.Type,
                Gasoline = car.Gasoline,
                Capacity = car.Capacity,
                Year = car.Year,
                CityMpg = car.CityMpg,
                Fuel = car.Fuel,
                Transmission = car.Transmission,
                BrandId = car.BrandId,
                CarImages = car.CarImages.Select(c => c.ToCarImageDto()).ToList(),
            };
        }
        public static Car ToCarFromCreateDTO(this CreateCarDto car, int BrandId)
        {
            return new Car
            {
                Make = car.Make,
                Model = car.Model,
                Type = car.Type,
                Gasoline = car.Gasoline,
                Capacity = car.Capacity,
                Year = car.Year,
                CityMpg = car.CityMpg,
                Fuel = car.Fuel,
                Transmission = car.Transmission,
                BrandId = BrandId
            };
        }
    }
}