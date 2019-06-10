using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace KonMongWausau.Controllers
{
    public class WausauController : Controller
    {
        /// <summary>
        /// The index view.
        /// </summary>
        /// <returns>The view to be returned.</returns>
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// The Things to do view.
        /// </summary>
        /// <returns>The view to be returned.</returns>
        public IActionResult ThingsToDo()
        {
            return View();
        }

        /// <summary>
        /// The hotel view.
        /// </summary>
        /// <returns>The view to be returned.</returns>
        public IActionResult AreaHotels()
        {
            return View();
        }

        /// <summary>
        /// The Education view.
        /// </summary>
        /// <returns>The view to be returned.</returns>
        public IActionResult Entertainment()
        {
            return View();
        }

        /// <summary>
        /// The food view.
        /// </summary>
        /// <returns>The view to be returned.</returns>
        public IActionResult Eatery()
        {
            return View();
        }
    }
}