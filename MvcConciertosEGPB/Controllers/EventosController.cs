using Microsoft.AspNetCore.Mvc;
using MvcConciertosEGPB.Models;
using MvcConciertosEGPB.Services;

namespace MvcConciertosEGPB.Controllers
{
    public class EventosController : Controller
    {
        private ServiceApiConciertos service;
        private ServiceStorageS3 serviceStorage;
        private string UrlBucket;
        public EventosController(ServiceApiConciertos service, ServiceStorageS3 serviceStorage, KeyModel model)
        {
            this.service = service;
            this.serviceStorage = serviceStorage;
            this.UrlBucket = model.BucketUrl;
        }

        public async Task<IActionResult> Categorias()
        {
            List<Categoria> categorias = await this.service.GetCategoriasAsync();
            return View(categorias);
        } 
        
        public async Task<IActionResult> Eventos()
        {
            List<Evento> eventos = await this.service.GetEventosAsync();
            foreach(Evento evento in eventos)
            {
                evento.Imagen = this.UrlBucket + evento.Imagen;
            }
            return View(eventos);
        }
        
        public async Task<IActionResult> EventosCategoria(int id)
        {
            List<Evento> eventos = await this.service.GetEventosCategoriaAsync(id);
            foreach (Evento evento in eventos)
            {
                evento.Imagen = this.UrlBucket + evento.Imagen;
            }
            return View(eventos);
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Evento evento, IFormFile file)
        {
            string fileName = file.FileName;
            using (Stream stream = file.OpenReadStream())
            {
                await this.serviceStorage.UploadFileAsync(fileName, stream);
            }
            evento.Imagen = fileName;
            await this.service.NewEventoAsync(evento);
            return RedirectToAction("Eventos");
        }
    }
}
