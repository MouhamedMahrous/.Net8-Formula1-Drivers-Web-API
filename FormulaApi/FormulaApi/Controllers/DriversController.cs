using FormulaApi.Core;
using FormulaApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace FormulaApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DriversController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        public DriversController(IUnitOfWork unitOfWork) 
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _unitOfWork.Drivers.All());
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var driver = await _unitOfWork.Drivers.GetById(id);
            return driver == null ? NotFound() : Ok(driver);
        }

        [HttpPost]
        public async Task<IActionResult> Post(Driver driver)
        {
            driver.Id = 0;
            await _unitOfWork.Drivers.Add(driver);

            await _unitOfWork.CompleteAsync();
            return Ok();
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var driver = await _unitOfWork.Drivers.GetById(id);
            if (driver == null)
                return NotFound();

            await _unitOfWork.Drivers.Delete(driver);
            await _unitOfWork.CompleteAsync();
            return NoContent();
        }

        [HttpPut]
        public async Task<IActionResult> Update(Driver driver)
        {
            var updatedDriver = await _unitOfWork.Drivers.GetById(driver.Id);

            if (updatedDriver == null)
                return NotFound();

            updatedDriver.Name = driver.Name;
            updatedDriver.DriverNumber = driver.DriverNumber;
            updatedDriver.Team = driver.Team;

            await _unitOfWork.Drivers.Update(updatedDriver);
            await _unitOfWork.CompleteAsync();
            
            return NoContent();
        }
    }
}
