using httpmvc.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
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
            client = new HttpClient();
            client.BaseAddress = baseAddress;
        }

        public IActionResult Index()
        {
            List<EmployeeViewModel> modelList = new List<EmployeeViewModel>();
            HttpResponseMessage response = client.GetAsync(client.BaseAddress + "/Students").Result;
            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;

                modelList = JsonConvert.DeserializeObject<List<EmployeeViewModel>>(data);
            }
            return View(modelList);
        }

 
        public ActionResult Create(EmployeeViewModel model)
        {
            string data = JsonConvert.SerializeObject(model);
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

                model = JsonConvert.DeserializeObject<EmployeeViewModel>(data);
            }
          /*  string data1 = JsonConvert.SerializeObject(model);
            StringContent content = new StringContent(data1, Encoding.UTF8, "application/json");

            HttpResponseMessage response12 = client.PutAsync(client.BaseAddress + "/Students/" + model.Id, content).Result;
            if (response12.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }*/
            return View("Create", model);
        }

        [HttpPost]
        public ActionResult Edit(EmployeeViewModel model)
        {

            string data = JsonConvert.SerializeObject(model);
            StringContent content = new StringContent(data, Encoding.UTF8, "application/json");

            HttpResponseMessage response = client.PutAsync(client.BaseAddress + "/Students/" + model.Id, content).Result;
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            return View("Create", model);
        }


        public ActionResult Delete(int Id)
        {
            EmployeeViewModel model = new EmployeeViewModel();
            var deleteTask = client.DeleteAsync("/Students/" + model.Id);

            var result = deleteTask.Result;
            if (result.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }



    }
} 
