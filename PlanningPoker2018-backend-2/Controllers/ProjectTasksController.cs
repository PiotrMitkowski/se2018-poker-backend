﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PlanningPoker2018_backend_2.Models;

namespace PlanningPoker2018_backend_2.Controllers
{
    [Produces("application/json")]
    [Route("api/tasks")]
    public class ProjectTasksController : Controller
    {
        private readonly DatabaseContext _context;

        public ProjectTasksController(DatabaseContext context)
        {
            _context = context;
        }

        //GET: api/tasks
        [HttpGet]
        public IEnumerable<ProjectTask> GetProjectTasks()
        {
            return _context.ProjectTask;
        }

        // GET: api/tasks/{roomId}
        [HttpGet]
        public ProjectTask GetProjectTask(int id)
        {
            return _context.ProjectTask.First(t => t.id == id);
        }


        // PUT: api/tasks
        [HttpPut]
        public async Task<IActionResult> PostProjectTask([FromBody] ProjectTask projectTask)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.ProjectTask.Add(projectTask);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProjectTask", new {id = projectTask.id}, projectTask);
        }

        // Patch: api/tasks/{taskId}
        [HttpPatch("{taskId}")]
        public async Task<IActionResult> ChangeProjectTaskEstimate(int taskId, int estimate)
        {
            Console.WriteLine("START");
            Console.Write(ModelState);
            Console.WriteLine("END");
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            ProjectTask projectTask = new ProjectTask() {id = taskId, estimate = estimate};
            _context.ProjectTask.Attach(projectTask);
            _context.Entry(projectTask).Property(t => t.estimate).IsModified = true;
            await _context.SaveChangesAsync();

            return AcceptedAtAction("ChangeProjectTaskEstimate", new
            {
                id = projectTask.id,
                estimate = projectTask.estimate
            });
        }
    }
}