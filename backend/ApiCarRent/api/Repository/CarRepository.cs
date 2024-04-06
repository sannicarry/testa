using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Helpers;
using api.Interfaces;
using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Repository
{
    public class CarRepository : ICarRepository
    {
        private readonly ApplicationDBContext _context;
        public CarRepository(ApplicationDBContext context)
        {
            _context = context;
        }
        public async Task<Car> CreateAsync(Car carModel)
        {
            await _context.AddAsync(carModel);
            await _context.SaveChangesAsync();
            return carModel;
        }

        public async Task<Car?> UpdateAsync(Car carModel)
        {
            await _context.SaveChangesAsync();
            return carModel;
        }

        public async Task<List<Car>> GetAllAsync(QueryObject query)
        {
            return await _context.Car.AsQueryable().ToListAsync();
        }

        public async Task<Car?> GetByIdAsync(int id)
        {
            return await _context.Car.FirstOrDefaultAsync(x => x.CarId == id);
        }

        public async Task<List<string>> GetCarImageByIdAsync(int id)
        {
            return await _context.CarImage
                               .Where(x => x.CarId == id)
                               .Select(x => x.ImageUrl)
                               .ToListAsync();
        }
    }
}