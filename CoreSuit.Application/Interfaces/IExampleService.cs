using System;
using CoreSuit.Application.DTOs.Example;
using CoreSuit.Domain.Entities;

namespace CoreSuit.Application.Interfaces;

public interface IExampleService : IBaseService<ExampleEntity, ExampleDTO, CreateExampleDTO, UpdateExampleDTO>
{

}
