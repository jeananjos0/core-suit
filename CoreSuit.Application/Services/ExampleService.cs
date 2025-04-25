using AutoMapper;
using CoreSuit.Application.DTOs.Example;
using CoreSuit.Application.Interfaces;
using CoreSuit.Domain.Entities;
using CoreSuit.Domain.Interfaces.Repositories;

namespace CoreSuit.Application.Services;

/// <summary>
/// Serviço responsável por gerenciar as regras de negócio da entidade <see cref="ExampleEntity"/>.
/// Herda a lógica genérica de criação, edição, deleção lógica e reativação.
/// </summary>
public class ExampleService : BaseService<ExampleEntity, ExampleDTO, CreateExampleDTO, UpdateExampleDTO>, IExampleService
{
    /// <summary>
    /// Construtor da classe ExampleService, injetando o repositório específico e o mapeador.
    /// </summary>
    public ExampleService(
        IExampleRepository repository,
        IMapper mapper
    ) : base(repository, mapper)
    {
    }

    /// <summary>
    /// Validação personalizada antes de criar um novo registro.
    /// Aqui é possível garantir integridade com entidades relacionadas.
    /// </summary>
    protected override Task ValidateBeforeCreateAsync(CreateExampleDTO dto) => ValidarRelacionamentos(dto.Name);

    /// <summary>
    /// Validação personalizada antes de atualizar um registro existente.
    /// </summary>
    protected override Task ValidateBeforeUpdateAsync(UpdateExampleDTO dto, ExampleEntity entity) => ValidarRelacionamentos(dto.Name);

    /// <summary>
    /// Valida se a entidade já está inativa antes de executar a deleção lógica.
    /// </summary>
    /// <param name="entity">Entidade a ser validada.</param>
    /// <exception cref="KeyNotFoundException">Lançada se o item já estiver inativo.</exception>
    protected override Task ValidateBeforeDeleteAsync(ExampleEntity entity)
    {
        if (entity.DeletedAt != null)
            throw new KeyNotFoundException("Registro já está inativo.");
        return Task.CompletedTask;
    }

    /// <summary>
    /// Valida se a entidade já está ativa antes de ativá-la novamente.
    /// </summary>
    /// <param name="entity">Entidade a ser validada.</param>
    /// <exception cref="KeyNotFoundException">Lançada se o item já estiver ativo.</exception>
    protected override Task ValidateBeforeActivateAsync(ExampleEntity entity)
    {
        if (entity.DeletedAt == null)
            throw new KeyNotFoundException("Registro já está ativo.");
        return Task.CompletedTask;
    }

    /// <summary>
    /// Valida a existência de relacionamentos necessários antes de criar ou atualizar.
    /// Esse método é útil para garantir integridade referencial em entidades relacionadas.
    /// 
    /// Exemplos de validações possíveis:
    /// - Verificar se uma categoria informada realmente existe.
    /// - Verificar se o ID de um tipo de produto está presente no banco.
    /// - Validar se subcategorias e unidades de medida associadas estão ativas.
    /// </summary>
    /// <param name="name">Nome ou outro campo do DTO usado para auxiliar nas validações (pode ser substituído por múltiplos parâmetros).</param>
    /// <exception cref="KeyNotFoundException">Lançada quando algum relacionamento não é encontrado no banco.</exception>
    private async Task ValidarRelacionamentos(string name)
    {
        // EXEMPLO 1 — Categoria deve existir
        // if (!await _categoriaProdutoRepository.All(new()).AnyAsync(x => x.Id == categoriaId))
        //     throw new KeyNotFoundException("Categoria informada não existe.");

        // EXEMPLO 2 — Tipo de produto deve existir
        // if (!await _tipoProdutoRepository.All(new()).AnyAsync(x => x.Id == tipoProdutoId))
        //     throw new KeyNotFoundException("Tipo de Produto informado não existe.");

        // EXEMPLO 3 — Subcategoria associada deve existir e estar ativa
        // if (!await _subcategoriaRepository.All(new() { OnlyActives = true }).AnyAsync(x => x.Id == subcategoriaId))
        //     throw new KeyNotFoundException("Subcategoria informada não está ativa ou não existe.");

        // EXEMPLO 4 — Unidade de medida obrigatória
        // if (!await _unidadeDeMedidaRepository.All(new()).AnyAsync(x => x.Id == unidadeDeMedidaId))
        //     throw new KeyNotFoundException("Unidade de Medida informada não existe.");

        // EXEMPLO 5 — Validação de unicidade por nome
        // if (await _repository.All(new()).AnyAsync(x => x.Name.ToLower() == name.ToLower()))
        //     throw new ValidationException("Já existe um registro com o mesmo nome.");
    }

}
