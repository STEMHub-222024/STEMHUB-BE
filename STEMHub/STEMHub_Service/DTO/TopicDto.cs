﻿using System.ComponentModel.DataAnnotations.Schema;

namespace STEMHub.STEMHub_Service.DTO
{
    public class TopicDto
    {
        public Guid TopicId { get; set; }
        public string? TopicName { get; set; }
        public string? TopicImage { get; set; }
        public Guid STEMId { get; set; }
    }
}