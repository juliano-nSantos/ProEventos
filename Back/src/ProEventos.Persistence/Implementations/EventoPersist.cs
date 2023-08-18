using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProEventos.Domain.Models;
using ProEventos.Persistence.Data;
using ProEventos.Persistence.Interfaces;

namespace ProEventos.Persistence.Implementations
{
    public class EventoPersist : IEventoPersist
    {
        private readonly ProEventosContext _context;
        public EventoPersist(ProEventosContext context)
        {
            _context = context;        
            //_context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;    
        }

        public async Task<Evento[]> GetAllEventosAsync(bool includePalestrantes = false)
        {
            IQueryable<Evento> query = _context.Eventos
                        .Include(e => e.Lotes)
                        .Include(e=>e.RedesSociais);

            if(includePalestrantes){
                query = query
                    .Include(e => e.PalestranteEventos)
                    .ThenInclude(pe => pe.Palestrante);
            }

            query = query.AsNoTracking().OrderBy(e => e.Id);

            return await query.ToArrayAsync();
        }

        public async Task<Evento[]> GetAllEventosByTemaAsync(string tema, bool includePalestrantes = false)
        {
            IQueryable<Evento> query = _context.Eventos
                        .Where(e => e.Tema.ToLower().Contains(tema.ToLower()))
                        .Include(e => e.Lotes)
                        .Include(e=>e.RedesSociais);

            if(includePalestrantes){
                query = query
                    .Include(e => e.PalestranteEventos)
                    .ThenInclude(pe => pe.Palestrante);
            }

            query = query.AsNoTracking().OrderBy(e => e.Id);

            return await query.ToArrayAsync();
        }

        public async Task<Evento> GetEventoByIdAsync(int eventoId, bool includePalestrantes = false)
        {
            IQueryable<Evento> query = _context.Eventos
                        .Where(e => e.Id == eventoId)
                        .Include(e => e.Lotes)
                        .Include(e=>e.RedesSociais);

            if(includePalestrantes){
                query = query
                    .Include(e => e.PalestranteEventos)
                    .ThenInclude(pe => pe.Palestrante);
            }

            query = query.AsNoTracking().OrderBy(e => e.Id);

            return await query.FirstOrDefaultAsync();
        }
    }
}