﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cw3.DAL;
using Cw3.Exceptions;
using Cw3.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Cw3.Controllers
{
    [ApiController]
    [Route("api/students")]
    public class StudentsController : ControllerBase
    {
        private readonly IDbService _dbService;

        public StudentsController(IDbService dbService)
        {
            _dbService = dbService;
        }

        [HttpGet]
        public IActionResult GetStudents(string orderBy)
        {
            return Ok(_dbService.GetStudents());
        }

        [HttpGet("secured")]
        public IActionResult GetStudentsSecured(string orderBy)
        {
            return Ok(_dbService.GetStudents());
        }
        //  [HttpGet]
        //  public string GetStudents(string orderBy)
        //  {
        //      return $"Kowalski, Malewski, Andrzejewski sortowanie={orderBy}";
        //  }

        [HttpGet("{id}")]
        public IActionResult GetStudent(String id)
        {
            //if (id == 1) {
            //    return Ok("Kowalski");
            //} else if (id == 2)
            //{
            //    return Ok("Malewski");
            //}
            //return NotFound("Nie znaleziono studenta");
            try
            {
                return Ok(_dbService.GetStudent(id));
            }catch (StudentNotFoundException ex)
            {
                return NotFound(ex.Message);
            }catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("secured/{index}")]
        public IActionResult GetStudentSecured(String index)
        {
            //if (id == 1) {
            //    return Ok("Kowalski");
            //} else if (id == 2)
            //{
            //    return Ok("Malewski");
            //}
            //return NotFound("Nie znaleziono studenta");
            try
            {
                return Ok(_dbService.GetStudent(index));
            }
            catch (StudentNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,ex.Message);
            }
        }

        [HttpPost]
        public IActionResult CreateStudent(Student student)
        {
            //... add to database
            //... generating index number
            student.IndexNumber = $"s{new Random().Next(1, 20000)}";
            _dbService.GetStudents().ToList().Add(student);
            return Ok(student);
        }

        //Zadanie 7
        [HttpPut("{id}")]
        public IActionResult updateStudent(String id)
        {
            Student student = _dbService.GetStudents().ToList().Find(i => i.IndexNumber == id);
            if (student == null)
                return BadRequest($"Student o id {id} nie został znaleziony");
            else 
            {
                student.IndexNumber = $"s{new Random().Next(1, 20000)}";
                return Ok($"Student o id {id} zaktualizowany pomyślnie");
            }
        }

        [HttpDelete("{id}")]
        public IActionResult deleteStudent(String id)
        {
            Student student = _dbService.GetStudents().ToList().Find(i => i.IndexNumber == id);
            if (student == null)
                return BadRequest($"Student o id {id} nie został znaleziony");
            //Linijka poniżej nic nie robi, ale chciałem "zasymulować" usuwanie
             _dbService.GetStudents().ToList().Remove(student);
            return Ok($"Student {id} usunięty pomyślnie");
        }

        [HttpGet("{idStudenta}/wpis")]
        public IActionResult getEnrollment(String idStudenta)
        {
            try
            {
                SQLServerDbService sqlServer = (SQLServerDbService) _dbService;
                return Ok(sqlServer.GetEnrollment(idStudenta));
            }
            catch (ArgumentException)
            {
                return NotFound($"Could not find student with id = {idStudenta}");
            }
        }
    }

}