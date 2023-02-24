using httpmvc.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
// using Newtonsoft.Json;
using System.Text.Json;
using System.Text;
using System.Net.Http;

namespace httpmvc.Controllers
{
    public class EmployeeController : Controller
    {
        Uri baseAddress = new Uri("https://localhost:7068/api");
        HttpClient client;

        public EmployeeController()
        {
            //creating instance to call httpClient
            client = new HttpClient();
            client.BaseAddress = baseAddress;
        }

        public IActionResult Index()
        {
            //mapping to database
            List<EmployeeViewModel> modelList = new List<EmployeeViewModel>();
            //returning a message/data from your action and calling GetAsync() method api
            HttpResponseMessage response = client.GetAsync(client.BaseAddress + "/Students").Result;
            if (response.IsSuccessStatusCode)
            {

                string data = response.Content.ReadAsStringAsync().Result;

                //converting string to object
                modelList = JsonSerializer.Deserialize<List<EmployeeViewModel>>(data)!;

             //   modelList = JsonConvert.DeserializeObject<List<EmployeeViewModel>>(data);
            }
            return View(modelList);
        }

 
        public ActionResult Create(EmployeeViewModel model)
        {
            //converting object to string 
            //  string data = JsonConvert.SerializeObject(model);
            string data = JsonSerializer.Serialize(model);
            StringContent content = new StringContent(data, Encoding.UTF8, "application/json");

            HttpResponseMessage response = client.PostAsync(client.BaseAddress + "/Students", content).Result;
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            return View();
        }


        public ActionResult Edit(int Id)
        {
            EmployeeViewModel model = new EmployeeViewModel();
            HttpResponseMessage response = client.GetAsync(client.BaseAddress + "/Students/" + Id).Result;
            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;

                model = JsonSerializer.Deserialize<EmployeeViewModel>(data)!;
              //  model = JsonConvert.DeserializeObject<EmployeeViewModel>(data);
            }
            return View("Create", model);
        }

        [HttpPost]
        public ActionResult Edit(EmployeeViewModel model)
        {
            string data = JsonSerializer.Serialize(model);
          //  string data = JsonConvert.SerializeObject(model);
            StringContent content = new StringContent(data, Encoding.UTF8, "application/json");

            HttpResponseMessage response = client.PutAsync(client.BaseAddress + "/Students/" + model.Id, content).Result;
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            return View("Create", model);
        }


        public ActionResult Delete(int id)
        {
         
            HttpResponseMessage response = client.DeleteAsync(client.BaseAddress + "/Students/" + id).Result;
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }

     

        [HttpPost]
        public async Task<IActionResult> Login11(string username, string password)
        {
            EmployeeViewModel model = new EmployeeViewModel();
           
            var url = "https://localhost:7068/api/Students";

            var content = new FormUrlEncodedContent(new[]
              {
                new KeyValuePair<string, string>("Name", "Mahalaxmi"),
                 new KeyValuePair<string, string>("Password", "1234")
                 });

            // Send the request to the server
            var response = await client.PostAsync(url, content);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.ErrorMessage = "Invalid username or password";
                return View();
            }
        }

    }
} 
