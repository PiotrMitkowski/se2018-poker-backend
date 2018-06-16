﻿using Microsoft.EntityFrameworkCore;
using PlanningPoker2018_backend_2.Controllers;
using PlanningPoker2018_backend_2.Entities;
using PlanningPoker2018_backend_2.Models;
using Xbehave;
using Xunit;

namespace PlanningPoker2018_backend.Tests
{
    public class ManageTasksFeature
    {
        private DatabaseContext _context;

        public ManageTasksFeature()
        {
            SetUpTest();
        }

        [Scenario]
        public void SelectingTaskToEstimate(ProjectTask task)
        {
            "Given a new task"
                .x(() => task = new ProjectTask()
                {
                    id = 1,
                    status = TaskStatus.UNESTIMATED.name,
                    title = "New task in BDD",
                    RoomId = 999,
                    estimate = 0
                });
            "And having the task in database"
                .x(() =>
                {
                    _context.ProjectTask.Add(task);
                    _context.SaveChanges();
                });

            "When I select task"
                .x(async () =>
                {
                    await new ProjectTasksController(_context).UpdateTaskStatus(task.id, TaskStatus.VOTING.name);
                });

            "Then task status is 'Voting'"
                .x(() => Assert.Equal(task.status, TaskStatus.VOTING.name));
        }

        private void SetUpTest()
        {
            var builder = new DbContextOptionsBuilder<DatabaseContext>().UseInMemoryDatabase("Planning-Poker-DB");
            _context = new DatabaseContext(builder.Options);
        }
    }
}