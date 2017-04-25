using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using GenericCloudant.Services;
using GenericCloudant.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace GenericCloudant.Controllers
{
    [Route("api/[controller]")]
    public class StudentController : Controller
    {
        private readonly GenericCloudantService<Student> _cloudantService;

        public StudentController()
        {
            _cloudantService = new GenericCloudantService<Student>();
        }

        [HttpPost]
        public async Task<dynamic> Post(Student item)
        {
            return await _cloudantService.CreateAsync(item);
        }

        [HttpGet]
        public async Task<dynamic> Get()
        {
            return await _cloudantService.GetAllAsync("Student");
        }

        [HttpPut]
        public async Task<string> Update(Student item)
        {
            return await _cloudantService.UpdateAsync(item);
        }

        [HttpDelete]
        public async Task<dynamic> Delete(Student item)
        {
            return await _cloudantService.DeleteAsync(item);
        }

    }
}
