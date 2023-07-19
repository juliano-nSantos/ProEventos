using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ProEventos.API.Models;

namespace ProEventos.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EventoController : ControllerBase
    {     
        public IEnumerable<Evento> _evento = new Evento[]{
            new Evento {
                EventoId = 1,
                Tema = "Angular 11 e .Net 5",
                Local = "Belo Horizonte",
                Lote = "1° Lote",
                Quantidade = 250,
                DataEvento = DateTime.Now.AddDays(2).ToString("dd/MM/yyyy"),
                ImagemUrl = "foto.png"
            },
             new Evento {
                EventoId = 2,
                Tema = "Angular 11 e suas novidades",
                Local = "São Paulo",
                Lote = "2° Lote",
                Quantidade = 350,
                DataEvento = DateTime.Now.AddDays(3).ToString("dd/MM/yyyy"),
                ImagemUrl = "foto1.png"
            }
        };

        public EventoController()
        {
            
        }

        [HttpGet]
        public IEnumerable<Evento> Get()
        {
           return _evento;
        }

        [HttpGet("{id}")]
        public IEnumerable<Evento> GetById(int id)
        {
            return _evento.Where(even => even.EventoId == id);
        }
    }
}
