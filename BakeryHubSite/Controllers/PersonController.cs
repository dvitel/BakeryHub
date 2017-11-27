using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BakeryHubSite.Models;

namespace BakeryHubSite.Controllers
{
    public class PersonController : Controller
    {
        static List<Person> persons = new List<Person> {
            new Person{ Name="John", StudentId = 1 },
            new Person{ Name="Kate", StudentId = 2 }
        };
        [HttpGet]
        public IActionResult List()
        {
            //fetching data from db
            var data = persons;
            return View(data);
        }

        [HttpPost]
        public IActionResult List(Person p)
        {
            //save changes to db - update or insert
            persons.Add(p);
            return RedirectToAction("List");
        }
    }
}