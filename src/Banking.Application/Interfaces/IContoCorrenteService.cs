using Banking.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banking.Application.Interfaces
{
    public interface IContoCorrenteService
    {
        // 1. Metodi di Lettura (Query)
        Task<ContoCorrenteResponseDto?> GetContoByIdAsync(int id);
        Task<ContoCorrenteResponseDto?> GetContoByIBANAsync(string iban);
        Task<IEnumerable<ContoCorrenteResponseDto>> GetContiByClienteIdAsync(int clienteId);
        Task<IEnumerable<TransazioneResponseDto>> GetEstrattoContoAsync(string iban, int limit = 20);

        // 2. Metodi di Scrittura / Creazione (Command)
        Task<ContoCorrenteResponseDto> CreateContoAsync(ContoCorrenteCreateDto dto);

        // 3. Operazioni Dispositive / Logica Core di Business
        Task<bool> EseguiVersamentoAsync(string iban, decimal importo, string descrizione);
        Task<bool> EseguiPrelievoAsync(string iban, decimal importo, string descrizione);
        Task<bool> EseguiBonificoAsync(string ibanSorgente, string ibanDestinatario, decimal importo, string descrizione);
    }
}
