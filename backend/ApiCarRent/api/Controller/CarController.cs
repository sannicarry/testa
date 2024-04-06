using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Dtos.Car;
using api.Helpers;
using api.Interfaces;
using api.Mappers;
using api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api.Controller
{
    [Route("api/car")]
    [ApiController]
    public class CarController : ControllerBase
    {
        private readonly ICarRepository _carRepo;
        private readonly IBrandRepository _brandRepo;
        private readonly ICarImageRepository _carImageRepo;
        private readonly string _imageUploadDirectory = "Uploads";
        public CarController(ICarRepository carRepo, IBrandRepository brandRepo, ICarImageRepository carImageRepo)
        {
            _carRepo = carRepo;
            _brandRepo = brandRepo;
            _carImageRepo = carImageRepo;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAll([FromQuery] QueryObject query)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var cars = await _carRepo.GetAllAsync(query);
            var carsDto = cars.Select(c => c.ToCarDto()).ToList();
            return Ok(carsDto);
        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var carModel = await _carRepo.GetByIdAsync(id);
            if (carModel == null)
            {
                return NotFound();
            }
            return Ok(carModel.ToCarDto());
        }

        [HttpPost("{BrandId:int}")]
        [ProducesResponseType(typeof(CarDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Create([FromRoute] int BrandId, [FromForm] CreateCarDto carDto, List<IFormFile> images)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!await _brandRepo.BrandExists(BrandId))
            {
                return BadRequest("Brand does not exist");
            }

            var carModel = carDto.ToCarFromCreateDTO(BrandId);

            var carImages = new List<CarImage>();

            var uploadDirectory = Path.Combine(Directory.GetCurrentDirectory(), _imageUploadDirectory);
            if (!Directory.Exists(uploadDirectory))
            {
                Directory.CreateDirectory(uploadDirectory);
            }

            foreach (var image in images)
            {
                var imageName = $"{Guid.NewGuid()}{Path.GetExtension(image.FileName)}";
                var imagePath = Path.Combine(uploadDirectory, imageName);

                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    await image.CopyToAsync(stream);
                }

                var imageUrl = $"{Request.Scheme}://{Request.Host}/{_imageUploadDirectory}/{imageName}";
                carImages.Add(new CarImage { ImageUrl = imageUrl });
            }


            carModel.CarImage = carImages;

            await _carRepo.CreateAsync(carModel);
            return CreatedAtAction(nameof(GetById), new { id = carModel.CarId }, carModel.ToCarDto());
        }

        [HttpPut]
        // [Authorize]
        [Route("{CarId:int}")]
        public async Task<IActionResult> Update([FromRoute] int CarId, [FromForm] UpdateCarDto carDto, List<IFormFile> images)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var carModel = await _carRepo.GetByIdAsync(CarId);
            if (carModel == null)
            {
                return NotFound($"Car not found {CarId}");
            }

            carModel.Make = carDto.Make;
            carModel.Model = carDto.Model;
            carModel.Type = carDto.Type;
            carModel.Gasoline = carDto.Gasoline;
            carModel.Capacity = carDto.Capacity;
            carModel.Year = carDto.Year;
            carModel.CityMpg = carDto.CityMpg;
            carModel.Fuel = carDto.Fuel;
            carModel.Transmission = carDto.Transmission;

            if (images != null && images.Any())
            {

                var uploadDirectory = Path.Combine(Directory.GetCurrentDirectory(), _imageUploadDirectory);
                if (!Directory.Exists(uploadDirectory))
                {
                    Directory.CreateDirectory(uploadDirectory);
                }

                for (int i = 0; i < images.Count; i++)
                {
                    var imageName = $"{Guid.NewGuid()}{Path.GetExtension(images[i].FileName)}";
                    var imagePath = Path.Combine(uploadDirectory, imageName);
                    using (var stream = new FileStream(imagePath, FileMode.Create))
                    {
                        await images[i].CopyToAsync(stream);
                    }

                    var imageUrl = $"{Request.Scheme}://{Request.Host}/{_imageUploadDirectory}/{imageName}";
                    await _carImageRepo.UpdateImageAsync(CarId, imageUrl, i);
                }
            }
            await _carRepo.UpdateAsync(carModel);

            return Ok(carModel.ToCarDto());
        }

        [HttpGet("image/{id:int}")]
        public async Task<IActionResult> GetImageById(int id)
        {
            var carImage = await _carRepo.GetCarImageByIdAsync(id);
            if (carImage == null)
            {
                return NotFound();
            }
            return Ok(carImage);
        }

    }
}