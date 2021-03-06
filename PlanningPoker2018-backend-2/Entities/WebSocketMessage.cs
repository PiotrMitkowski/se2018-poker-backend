﻿using Newtonsoft.Json.Linq;
using System;

namespace PlanningPoker2018_backend_2.Entities
{
    public class WebSocketMessage
    {
        public String roomId { get; set; }
        public String type { get; set; }
        public JObject content { get; set; }
        public string socketId { get; set; }
    }
}