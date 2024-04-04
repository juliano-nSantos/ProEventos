using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ProEventos.Application.Dtos;
using ProEventos.Application.Interfaces;
using ProEventos.Domain.Models;
using ProEventos.Persistence.Interfaces;

namespace ProEventos.Application.Implementations
{
    public class EventosService : IEventosService
    {
        private readonly IGeralPersist _geralPersist;
        private readonly IEventoPersist _eventoPersist;
        private readonly IMapper _mapper;

        public EventosService(IGeralPersist geralPersist, 
                              IEventoPersist eventoPersist,
                              IMapper mapper)
        {            
            _geralPersist = geralPersist;
            _eventoPersist = eventoPersist;
            _mapper = mapper;
        }

        public async Task<EventoDto> AddEvento(EventoDto model)
        {
            try
            {
                var evento = _mapper.Map<Evento>(model);

                _geralPersist.Add<Evento>(evento);

                if(await _geralPersist.SaveChangesAsync()){

                    var eventoRetorno = await _eventoPersist.GetEventoByIdAsync(evento.Id, false);
                    
                    return _mapper.Map<EventoDto>(eventoRetorno);
                }

                return null;
            }
            catch (Exception ex)
            {                
                throw new Exception(ex.Message); 
            }
        }

        public async Task<bool> DeleteEvento(int eventoId)
        {
            try
            {
                var evento = await _eventoPersist.GetEventoByIdAsync(eventoId, false);
                if(evento == null) throw new Exception("Evento para delete não encontrado");

                

                _geralPersist.Delete<Evento>(evento);
                 
                return await _geralPersist.SaveChangesAsync();
                    
            }
            catch (Exception ex)
            {                
                throw new Exception(ex.Message);
            }
        }

        public async Task<EventoDto> UpdateEvento(int eventoId, EventoDto model)
        {
            try
            {
                var evento = await _eventoPersist.GetEventoByIdAsync(eventoId, false);
                if(evento == null) return null;

                model.Id = evento.Id;
                
                _mapper.Map(model, evento);

                _geralPersist.Update<Evento>(evento);

                 if(await _geralPersist.SaveChangesAsync()){
                    
                    var eventoRetorno = await _eventoPersist.GetEventoByIdAsync(evento.Id, false);
                    
                    return _mapper.Map<EventoDto>(eventoRetorno);
                }

                return null;
            }
            catch (Exception ex)
            {                
                throw new Exception(ex.Message);
            }
        }

        public async Task<EventoDto[]> GetAllEventosAsync(bool includePalestrantes = false)
        {
            try
            {
                var eventos = await _eventoPersist.GetAllEventosAsync(includePalestrantes);

                if(eventos == null) return null;

                var eventosDto = _mapper.Map<EventoDto[]>(eventos);

                return eventosDto;
            }
            catch (Exception ex)
            {                
                throw new Exception(ex.Message);
            }
        }

        public async Task<EventoDto[]> GetAllEventosByTemaAsync(string tema, bool includePalestrantes = false)
        {
            try
            {
                var eventos = await _eventoPersist.GetAllEventosByTemaAsync(tema, includePalestrantes);

                if(eventos == null) return null;

                var eventosDto = _mapper.Map<EventoDto[]>(eventos);

                return eventosDto;
            }
            catch (Exception ex)
            {                
                throw new Exception(ex.Message);
            }
        }

        public async Task<EventoDto> GetEventoByIdAsync(int eventoId, bool includePalestrantes = false)
        {
            try
            {
                var evento = await _eventoPersist.GetEventoByIdAsync(eventoId, includePalestrantes);

                if(evento == null) return null;

                var eventoDto = _mapper.Map<EventoDto>(evento);

                return eventoDto;
            }
            catch (Exception ex)
            {                
                throw new Exception(ex.Message);
            }
        }       
    }
}