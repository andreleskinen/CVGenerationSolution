using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.IO;
using System.Threading.Tasks;
using CVGenerationSolution.Models;

namespace CKEditorIntegration.Controllers
{
    public class EditorController : Controller
    {
        private readonly string _templatePath = "wwwroot/template.html";
        private readonly string _dataPath = "wwwroot/data.json";

        public async Task<IActionResult> Index()
        {
            if (!System.IO.File.Exists(_templatePath) || !System.IO.File.Exists(_dataPath))
            {
                return Content("Missing template.html or data.json in wwwroot folder.");
            }

            string jsonData = await System.IO.File.ReadAllTextAsync(_dataPath);
            var resumeData = JsonConvert.DeserializeObject<ResumeData>(jsonData);

            if (resumeData == null)
            {
                return Content("Error: Could not parse data.json.");
            }

            string htmlTemplate = await System.IO.File.ReadAllTextAsync(_templatePath);
            string populatedHtml = ReplacePlaceholders(htmlTemplate, resumeData);

            ViewData["HtmlContent"] = populatedHtml;
            return View();
        }

        private string ReplacePlaceholders(string template, ResumeData data)
        {
            template = template.Replace("{{name}}", data.Name ?? "")
                               .Replace("{{address}}", data.Address ?? "")
                               .Replace("{{phone}}", data.Phone ?? "")
                               .Replace("{{email}}", data.Email ?? "");

            string skillsHtml = data.Skills != null && data.Skills.Count > 0
                ? string.Join("", data.Skills.Select(skill => $"<li>{skill}</li>"))
                : "<li>No skills listed</li>";

            template = template.Replace("{{skills}}", skillsHtml);

            string projectsHtml = data.Projects != null && data.Projects.Count > 0
                ? string.Join("", data.Projects.Select(proj => $"<div class='project'><b>{proj.Title}</b> ({proj.Company}, {proj.Dates})<p>{proj.Description}</p></div>"))
                : "<div>No projects listed</div>";

            template = template.Replace("{{projects}}", projectsHtml);

            return template;
        }

        [HttpPost]
        public async Task<IActionResult> Save(string editorContent)
        {
            if (!string.IsNullOrEmpty(editorContent))
            {
                await System.IO.File.WriteAllTextAsync(_templatePath, editorContent);
            }
            return RedirectToAction("Index");
        }
    }
}
