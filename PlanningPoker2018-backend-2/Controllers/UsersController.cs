﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PlanningPoker2018_backend_2.Entities;
using PlanningPoker2018_backend_2.Models;

namespace PlanningPoker2018_backend_2.Controllers
{
    [Produces("application/json")]
    [Route("api/Users")]
    public class UsersController : Controller
    {
        private readonly DatabaseContext _context;

        public UsersController(DatabaseContext context)
        {
            _context = context;
        }

        // GET: api/Users/{userId}
        [HttpGet("{userId}")]
        public User GetUser(int userId)
        {
            return _context.User.First(u => u.id == userId);
        }

        // POST: api/Users
        [HttpPost]
        public IActionResult AuthenticateUser([FromBody] AuthBody authBody)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!_context.User.Any(u => authBody.mailAddress.Equals(u.mailAddress)))
            {
                return NotFound();
            }

            var user = _context.User.First(u => u.mailAddress.Equals(authBody.mailAddress));
            if (BCrypt.Net.BCrypt.Verify(authBody.password, user.password))
            {
                var response = new AuthResponseBody()
                {
                    id = user.id,
                    mailAddress = user.mailAddress,
                    team = user.team,
                    username = user.username
                };
                return Ok(response);
            }
            else
            {
                return BadRequest(new BasicResponse {message = "Wrong password"});
            }
        }


        // PUT: api/Users
        [HttpPut]
        public async Task<IActionResult> PutNewUser([FromBody] User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            user.password = BCrypt.Net.BCrypt.HashPassword(user.password, 9);
            if (IsUserExists(user))
            {
                return BadRequest(new BasicResponse {message = "User already exists"});
            }

            _context.User.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUser", new {user.id},
                new BasicResponse {message = "User created successfully"});
        }

        private bool IsUserExists(User user)
        {
            return _context.User.Any(u => u.mailAddress == user.mailAddress);
        }

        [HttpGet("{mailAddress}/summaries")]
        public IActionResult GetGameSummaries([FromRoute] string mailAddress)
        {
            var summariesList = new List<GameSummary>();
            if (!_context.User.Any(u => u.mailAddress == mailAddress))
            {
                return NotFound(new BasicResponse {message = "User not found"});
            }

            var user = _context.User.First(u => u.mailAddress == mailAddress);
            var participantRefs = _context.RoomParticipant.Where(rp => rp.mailAddress.Equals(user.mailAddress));
            var rooms = _context.Room.Where(r =>
                r.hostMailAddress.Equals(user.mailAddress) || participantRefs.Any(pr => pr.roomId == r.id)).ToList();

            rooms.ForEach(r =>
            {
                var roomName = r.name;
                var roomDate = r.roomDate;
                var roomTasks = _context.ProjectTask.Where(task => task.RoomId == r.id).ToArray();
                var roomHost = r.hostMailAddress ?? r.hostUsername;
                summariesList.Add(new GameSummary()
                {
                    host = roomHost,
                    roomName = roomName,
                    date = roomDate,
                    participants = _context.RoomParticipant.Where(rp => rp.roomId.Equals(r.id)).ToArray(),
                    tasks = roomTasks
                });
            });
            return Ok(summariesList);
        }

        [HttpGet("{mailAddress}/teams")]
        public IActionResult GetUserTeams([FromRoute] string mailAddress)
        {
            if (!_context.User.Any(u => u.mailAddress == mailAddress))
            {
                return NotFound(new BasicResponse() {message = "User with specified mail address not found"});
            }

            var memberRefs = _context.TeamMember.Where(m => m.mailAddress == mailAddress);
            var userTeams =
                _context.EstimationTeam.Where(t => t.creator == mailAddress || memberRefs.Any(r => r.teamId == t.id))
                    .ToList();
            userTeams.ForEach(t =>
            {
                t.members = _context.TeamMember.Where(m => m.teamId == t.id)
                    .Select(s => s.mailAddress)
                    .ToList();
            });
            return Ok(userTeams);
        }
    }
}