using Microsoft.AspNetCore.Mvc;
using TaskManagementWeb.Models.DTO;

namespace TaskManagementWeb.Controllers
{
    public class ProjectController : Controller
    {
        private readonly IHttpClientFactory httpClient;

        public ProjectController(IHttpClientFactory httpClient)
        {
            this.httpClient = httpClient;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            List<ProjectDTO> returnResponse = new List<ProjectDTO>();
            try
            {
                //Get all projects from the database and pass them to the view
                var client = httpClient.CreateClient();

                var response = await client.GetAsync("https://localhost:7198/api/Project");

                response.EnsureSuccessStatusCode();

                returnResponse.AddRange(await response.Content.ReadFromJsonAsync<IEnumerable<ProjectDTO>>());

            }
            catch (Exception ex)
            {
                //Log exception
            }
            return View(returnResponse);
        }
        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(ProjectDTO project)
        {
            try
            {
                project.CreatedBy = "Admin";
                project.UpdatedBy = "Admin";
                var client = httpClient.CreateClient();
                var response = await client.PostAsJsonAsync("https://localhost:7198/api/Project", project);
                response.EnsureSuccessStatusCode();

                var postRes = await response.Content.ReadFromJsonAsync<ProjectDTO>();

                if (postRes != null)
                {
                    return RedirectToAction("Index", "Project");
                }
            }
            catch (Exception ex)
            {
                //Log exception
            }
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            ProjectDTO returnResponse = new ProjectDTO();
            try
            {
                var client = httpClient.CreateClient();
                var response = await client.GetFromJsonAsync<ProjectDTO>($"https://localhost:7198/api/Project/{id}");

                if (response != null)
                {
                    return View(response);
                }
            }
            catch (Exception ex)
            {
                //Log exception
            }
            return View(null);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(ProjectDTO project)
        {
            try
            {
                project.UpdatedBy = "Admin";
                project.CreatedBy = "Admin";
                var client = httpClient.CreateClient();
                var response = await client.PutAsJsonAsync($"https://localhost:7198/api/Project/{project.Id}", project);
                response.EnsureSuccessStatusCode();
                var postRes = await response.Content.ReadFromJsonAsync<ProjectDTO>();
                if (postRes != null)
                {
                    return RedirectToAction("Edit", "Project");
                }
            }
            catch (Exception ex)
            {
                //Log exception
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Delete(ProjectDTO project)
        {
            try
            {
                var client = httpClient.CreateClient();
                var httpResponse = await client.DeleteAsync($"https://localhost:7198/api/Project/{project.Id}");

                httpResponse.EnsureSuccessStatusCode();

                return RedirectToAction("Index", "Project");

            }
            catch (Exception ex)
            {

            }
            return View("Edit");
        }
    }
}
