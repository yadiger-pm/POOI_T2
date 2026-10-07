using Newtonsoft.Json;
using POOI_T2_Medrano.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace POOI_T2_Medrano.Controllers
{
    public class AlumnoController : Controller
    {
        private string rutaJson = System.Web.Hosting.HostingEnvironment.MapPath("~/App_Data/alumnos.json");

        private List<Alumno> ObtenerAlumnos()
        {
            if (!System.IO.File.Exists(rutaJson))
            {
                // Si el archivo no existe, crea una lista vacía inicial serializada como string json
                System.IO.File.WriteAllText(rutaJson, "[]");
            }

            string json = System.IO.File.ReadAllText(rutaJson);
            return JsonConvert.DeserializeObject<List<Alumno>>(json) ?? new List<Alumno>();
        }

        private void GuardarAlumnos(List<Alumno> lista)
        {
            string json = JsonConvert.SerializeObject(lista, Formatting.Indented);
            System.IO.File.WriteAllText(rutaJson, json);
        }

        // listar
        public ActionResult Index()
        {
            List<Alumno> lista = ObtenerAlumnos();
            return View(lista);
        }

        public ActionResult Agregar()
        {
            return View();
        }

        // dni duplicado
        [HttpPost]
        public ActionResult Agregar(Alumno obj)
        {
            List<Alumno> lista = ObtenerAlumnos();

            // Verifica si el DNI ya existe
            if (lista.Any(a => a.dni == obj.dni))
            {
                ViewBag.Error = "El DNI ingresado ya existe en la colección.";
                return View(obj);
            }

            lista.Add(obj);
            GuardarAlumnos(lista);
            TempData["Mensaje"] = "Alumno guardado correctamente.";

            return RedirectToAction("Index");
        }

        //ELIMINAR por DNI
        public ActionResult Eliminar(string dni)
        {
            List<Alumno> lista = ObtenerAlumnos();
            Alumno obj = lista.FirstOrDefault(a => a.dni == dni);

            if (obj != null)
            {
                lista.Remove(obj);
                GuardarAlumnos(lista);
                TempData["Mensaje"] = "Alumno eliminado correctamente.";
            }

            return RedirectToAction("Index");
        }

        public ActionResult Detalles(string dni)
        {
            List<Alumno> lista = ObtenerAlumnos();
            Alumno obj = lista.FirstOrDefault(a => a.dni == dni);
            if (obj == null)
            {
                return RedirectToAction("Index");
            }
            return View(obj);
        }

        // ACTUALIZAR 
        public ActionResult Actualizar(string dni)
        {
            List<Alumno> lista = ObtenerAlumnos();
            Alumno obj = lista.FirstOrDefault(a => a.dni == dni);
            if (obj == null)
            {
                return RedirectToAction("Index");
            }
            return View(obj);
        }

        // ACTUALIZAR (POST)
        [HttpPost]
        public ActionResult Actualizar(Alumno obj)
        {
            List<Alumno> lista = ObtenerAlumnos();
            Alumno existente = lista.FirstOrDefault(a => a.dni == obj.dni);

            if (existente != null)
            {
                existente.nombres = obj.nombres;
                existente.apellidos = obj.apellidos;
                existente.carrera = obj.carrera;
                existente.ciclo = obj.ciclo;

                GuardarAlumnos(lista);
                TempData["Mensaje"] = "Alumno actualizado correctamente.";
            }

            return RedirectToAction("Index");
        }
    }
}
