using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace Dojodachi.Controllers
{
    public class DojodachiController : Controller
    {
        [HttpGet]
        [Route("")]
        public IActionResult index()
        {
            int? Fullness = HttpContext.Session.GetInt32("Fullness");
            if(Fullness == null)
            {
                Fullness = 20;
            }
            ViewBag.Full = Fullness;
            HttpContext.Session.SetInt32("Fullness", (int)Fullness);

            int? Happiness = HttpContext.Session.GetInt32("Happiness");
            if(Happiness == null)
            {
                Happiness = 20;
            }
            ViewBag.Happy = Happiness;
            HttpContext.Session.SetInt32("Happiness", (int)Happiness);

            int? Energy = HttpContext.Session.GetInt32("Energy");
            if(Energy == null)
            {
                Energy = 50;
            }
            ViewBag.Energy = Energy;
            HttpContext.Session.SetInt32("Energy", (int)Energy);

            int? Meals = HttpContext.Session.GetInt32("Meals");
            if(Meals == null)
            {
                Meals = 3;
            }
            ViewBag.Meals = Meals;
            HttpContext.Session.SetInt32("Meals", (int)Meals);

            HttpContext.Session.SetString("Status", "alive");
            ViewBag.Status = HttpContext.Session.GetString("Status");

            bool result = winValidate((int)Fullness, (int)Happiness, (int)Energy);
            if(result == true)
            {
                object winner = new
                {
                    fullness = Fullness,
                    happiness = Happiness,
                    energy = Energy,
                    message = "Your drag queen slayed it, you win!!",
                    status = "winner",
                };
                // HttpContext.Session.SetString("Status", "winner");
                HttpContext.Session.SetInt32("Fullness", (int)Fullness);
                HttpContext.Session.SetInt32("Happiness", (int)Happiness);
                HttpContext.Session.SetInt32("Energy", (int)Energy);
                HttpContext.Session.SetString("Status", "winner");
                return Json(winner);
            }

            return View();
        }

        [HttpGet]
        [Route("feed")]
        public JsonResult feed()
        {
            int? Fullness = HttpContext.Session.GetInt32("Fullness");
            int? Happiness = HttpContext.Session.GetInt32("Happiness");
            int? Energy = HttpContext.Session.GetInt32("Energy");
            int? Meals = HttpContext.Session.GetInt32("Meals");
            if(Meals == 0)
            {
                object noFood = new
                {
                    message = "You don't have any food for the queens.",
                };
                return Json(noFood);
            }
            else
            {
                Random rate = new Random();
                int success = rate.Next(0,4);
                if(success == 0)
                {
                    int newMeals = (int)Meals - 1;
                    object noGood = new
                    {
                        meals = newMeals,
                        message = "Feed attempt unsuccessful...",
                    };
                    HttpContext.Session.SetInt32("Meals", newMeals);
                    return Json(noGood);
                }
                else
                {
                    Random rand = new Random();
                    int Increase = rand.Next(5,11);
                    int newFullness = (int)Fullness + Increase;
                    int newMeals = (int)Meals - 1;
                    bool result = winValidate(newFullness, (int)Happiness, (int)Energy);
                    if(result == true)
                    {
                        object winner = new
                        {
                            fullness = newFullness,
                            happiness = Happiness,
                            energy = Energy,
                            message = "Your drag queen slayed it, you win!!",
                            status = "winner",
                        };
                        HttpContext.Session.SetInt32("Fullness", newFullness);
                        HttpContext.Session.SetInt32("Happiness", (int)Happiness);
                        HttpContext.Session.SetInt32("Energy", (int)Energy);
                        HttpContext.Session.SetString("Status", "winner");
                        return Json(winner);
                    }
                    object beenFed = new
                    {
                        fullness = newFullness,
                        meals = newMeals,
                        message = $"Your Drag queen gained {Increase} Fullness.",
                    };
                    HttpContext.Session.SetInt32("Fullness", newFullness);
                    HttpContext.Session.SetInt32("Meals", newMeals);
                    return Json(beenFed);
                }
            }
        }

        [HttpGet]
        [Route("slay")]
        public JsonResult slay()
        {
            int? Fullness = HttpContext.Session.GetInt32("Fullness");
            int? Happiness = HttpContext.Session.GetInt32("Happiness");
            int? Energy = HttpContext.Session.GetInt32("Energy");
            if(Energy == 0)
            {
                object noEnergy = new
                {
                    message = "You don't have enough to dance for your life.",
                };
                return Json(noEnergy);
            }
            else
            {
                Random rate = new Random();
                int success = rate.Next(0,4);
                if(success == 0)
                {
                    int newEnergy = (int)Energy - 5;
                    object noGood = new
                    {
                        energy = newEnergy,
                        message = "Slay attempt unsuccessful...",
                    };
                    HttpContext.Session.SetInt32("Energy", newEnergy);
                    return Json(noGood);
                }
                else
                {
                    Random rand = new Random();
                    int Increase = rand.Next(5,11);
                    int newHappiness = (int)Happiness + Increase;
                    int newEnergy = (int)Energy - 5;
                    bool result = winValidate((int)Fullness, newHappiness, newEnergy);
                    if(result == true)
                    {
                        object winner = new
                        {
                            fullness = (int)Fullness,
                            happiness = newHappiness,
                            energy = newEnergy,
                            message = "Your drag queen slayed it, you win!!",
                            status = "winner",
                        };
                        HttpContext.Session.SetInt32("Fullness", (int)Fullness);
                        HttpContext.Session.SetInt32("Happiness", newHappiness);
                        HttpContext.Session.SetInt32("Energy", newEnergy);
                        HttpContext.Session.SetString("Status", "winner");
                        return Json(winner);
                    }
                    object justPlayed = new
                    {
                        happiness = newHappiness,
                        energy = newEnergy,
                        message = $"Your Drag queen gained {Increase} Happiness.",
                    };
                    HttpContext.Session.SetInt32("Happiness", newHappiness);
                    HttpContext.Session.SetInt32("Energy", newEnergy);
                    return Json(justPlayed);
                }
            }
        }

        [HttpGet]
        [Route("werq")]
        public JsonResult werq()
        {
            int? Energy = HttpContext.Session.GetInt32("Energy");
            int? Meals = HttpContext.Session.GetInt32("Meals");
            if(Energy == 0)
            {
                object noWork = new
                {
                    message = "You don't have enough energy to dance for your life.",
                };
                return Json(noWork);
            }
            else
            {
                Random rand = new Random();
                int Increase = rand.Next(1,4);
                int newMeals = (int)Meals + Increase;
                int newEnergy = (int)Energy - 5;
                object justWorked = new
                {
                    energy = newEnergy,
                    meals = newMeals,
                    message = $"Your Drag queen gained {Increase} Meals.",
                };
                HttpContext.Session.SetInt32("Energy", newEnergy);
                HttpContext.Session.SetInt32("Meals", newMeals);
                return Json(justWorked);
            }
        }

        [HttpGet]
        [Route("sleep")]
        public JsonResult sleep()
        {
            int? Fullness = HttpContext.Session.GetInt32("Fullness");
            int? Happiness = HttpContext.Session.GetInt32("Happiness");
            int? Energy = HttpContext.Session.GetInt32("Energy");
            int newFullness = (int)Fullness - 5;
            int newHappiness = (int)Happiness - 5;
            bool result = lifeValidate(newFullness, newHappiness);
            if(result == false)
            {
                object dead = new
                {
                    fullness = newFullness,
                    happiness = newHappiness,
                    message = "Oh no, your drag queen is dead...sashay away",
                    status = "dead",
                };
                HttpContext.Session.SetInt32("Fullness", newFullness);
                HttpContext.Session.SetInt32("Happiness", newHappiness);
                HttpContext.Session.SetString("Status", "dead");
                return Json(dead);
            }
            int newEnergy = (int)Energy + 15;
            bool checkresult = winValidate(newFullness, newHappiness, newEnergy);
            if(checkresult == true)
            {
                object winner = new
                {
                    fullness = newFullness,
                    happiness = newHappiness,
                    energy = newEnergy,
                    message = "Your drag dachi slayed it, you win!!",
                    status = "winner",
                };
                HttpContext.Session.SetInt32("Fullness", newFullness);
                HttpContext.Session.SetInt32("Happiness", newHappiness);
                HttpContext.Session.SetInt32("Energy", newEnergy);
                HttpContext.Session.SetString("Status", "winner");
                return Json(winner);
            }
            object justSlept = new
            {
                fullness = newFullness,
                happiness = newHappiness,
                energy = newEnergy,
                message = $"Your Drag queen gained 15 Energy.",
            };
            HttpContext.Session.SetInt32("Fullness", newFullness);
            HttpContext.Session.SetInt32("Happiness", newHappiness);
            HttpContext.Session.SetInt32("Energy", newEnergy);
            return Json(justSlept);
        }

        [HttpGet]
        [Route("restart")]
        public IActionResult restart()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("index");
        }

        public bool lifeValidate(int fullness, int happiness)
        {
            if(fullness < 1 || happiness < 1)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public bool winValidate(int fullness, int happiness, int energy)
        {
            if(fullness > 99 && happiness > 99 && energy > 99)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}