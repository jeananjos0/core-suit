using System;
using AutoMapper;
using CoreSuit.Application.DTOs.Example;
using CoreSuit.Domain.Entities;

namespace CoreSuit.Application.DTOs
{
    /// <summary>
    /// Perfil de mapeamento do AutoMapper responsável por definir as conversões entre as entidades de domínio
    /// e seus respectivos DTOs utilizados na camada de aplicação.
    /// 
    /// Essa classe é essencial para que o AutoMapper saiba como transformar objetos entre as camadas,
    /// evitando mapeamentos manuais repetitivos e garantindo consistência.
    /// </summary>
    public class DomainToDTOMappingProfile : Profile
    {
        /// <summary>
        /// Construtor que registra os mapeamentos definidos no método ConfigMappings().
        /// A base Profile pertence ao AutoMapper e permite o registro centralizado de todas as regras de conversão.
        /// </summary>
        public DomainToDTOMappingProfile()
        {
            ConfigMappings();
        }

        /// <summary>
        /// Configura os mapeamentos entre entidades e DTOs da aplicação.
        /// </summary>
        private void ConfigMappings()
        {
            // Mapeamentos relacionados à entidade Example

            // Mapeamento bidirecional entre ExampleEntity e ExampleDTO
            // Permite converter tanto de entidade para DTO quanto de DTO para entidade
            CreateMap<ExampleEntity, ExampleDTO>().ReverseMap();

            // Mapeamento da DTO de criação para a entidade
            // Usado no processo de criação de registros (POST)
            CreateMap<CreateExampleDTO, ExampleEntity>();

            // Mapeamento da DTO de atualização para a entidade
            // Usado no processo de edição de registros (PUT/PATCH)
            CreateMap<UpdateExampleDTO, ExampleEntity>();
        }
    }
}
