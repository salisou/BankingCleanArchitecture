using AutoMapper;
using Banking.Application.DTOs;
using Banking.Domain.Entities;

namespace Banking.Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Carta Mappings
        CreateMap<Carta, CartaResponseDto>().ReverseMap();

        // Cliente Mappings
        CreateMap<Cliente, ClienteResponseDto>().ReverseMap();
        CreateMap<ClienteCreateDto, Cliente>().ReverseMap();

        // ContoCorrente Mappings
        CreateMap<ContoCorrente, ContoCorrenteResponseDto>().ReverseMap();
        CreateMap<ContoCorrenteCreateDto, ContoCorrente>().ReverseMap();

        // Dipendente Mappings
        CreateMap<Dipendente, DipendenteResponseDto>().ReverseMap();
        CreateMap<DipendenteCreateDto, Dipendente>().ReverseMap();

        // DossierTitoli Mappings
        CreateMap<DossierTitoli, DossierTitoliResponseDto>().ReverseMap();

        // Filiale Mappings
        CreateMap<Filiale, FilialeResponseDto>().ReverseMap();
        CreateMap<FilialeCreateDto, Filiale>().ReverseMap();

        // PortafoglioTitoli Mappings
        CreateMap<PortafoglioTitoli, PortafoglioTitoliResponseDto>().ReverseMap();

        // Prestito Mappings
        CreateMap<Prestito, PrestitoResponseDto>().ReverseMap();
        CreateMap<PrestitoCreateDto, Prestito>().ReverseMap();

        // RataPrestito Mappings
        CreateMap<RataPrestito, RataPrestitoResponseDto>().ReverseMap();

        // Transazione Mappings
        CreateMap<Transazione, TransazioneResponseDto>().ReverseMap();
        CreateMap<TransazioneCreateDto, Transazione>().ReverseMap();
    }
}
