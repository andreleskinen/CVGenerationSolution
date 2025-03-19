using System;
using System.Collections.Generic;

namespace CVGenerationSolution.Models
{
    public class ResumeData
    {
        public string Name { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public List<string> Skills { get; set; } = new List<string>();
        public List<Project> Projects { get; set; } = new List<Project>();
    }

    public class Project
    {
        public string Title { get; set; } = string.Empty;
        public string Company { get; set; } = string.Empty;
        public string Dates { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }
}
