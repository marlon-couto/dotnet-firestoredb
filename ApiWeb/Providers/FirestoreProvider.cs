﻿using ApiWeb.Dtos;
using ApiWeb.Exceptions;
using ApiWeb.Helpers;
using ApiWeb.Models;
using Google.Cloud.Firestore;

namespace ApiWeb.Providers;

public class FirestoreProvider(FirestoreDb db) : IFirestoreProvider
{
    public async Task CriarOuAtualizar<T>(T entidade) where T : IEntidadeDoFirebase
    {
        var doc = db.Collection(typeof(T).Name).Document(entidade.Id);
        await doc.SetAsync(entidade);
    }

    public async Task CriarOuAtualizar<T>(T entidade, DocumentReference doc) where T : IEntidadeDoFirebase
    {
        await doc.SetAsync(entidade);
    }

    public async Task<DocumentReference> BuscarDocumento<T>(string id) where T : IEntidadeDoFirebase
    {
        var doc = db.Collection(typeof(T).Name).Document(id);
        var snapshot = await doc.GetSnapshotAsync();
        if (!snapshot.Exists)
        {
            throw new NaoEncontradoException($"Entidade \"{typeof(T).Name}\" não encontrada.");
        }

        return doc;
    }

    public async Task<DocumentReference> BuscarDocumento<T>(Guid id) where T : IEntidadeDoFirebase
    {
        var doc = db.Collection(typeof(T).Name).Document(id.ToString("N"));
        var snapshot = await doc.GetSnapshotAsync();
        if (!snapshot.Exists)
        {
            throw new NaoEncontradoException($"Entidade \"{typeof(T).Name}\" não encontrada.");
        }

        return doc;
    }

    public async Task<T> BuscarPorId<T>(string id) where T : IEntidadeDoFirebase
    {
        var doc = db.Collection(typeof(T).Name).Document(id);
        var snapshot = await doc.GetSnapshotAsync();
        if (!snapshot.Exists)
        {
            throw new NaoEncontradoException($"Entidade \"{typeof(T).Name}\" não encontrada.");
        }

        return snapshot.ConvertTo<T>();
    }

    public async Task<T> BuscarPorId<T>(Guid id) where T : IEntidadeDoFirebase
    {
        var doc = db.Collection(typeof(T).Name).Document(id.ToString("N"));
        var snapshot = await doc.GetSnapshotAsync();
        if (!snapshot.Exists)
        {
            throw new NaoEncontradoException($"Entidade \"{typeof(T).Name}\" não encontrada.");
        }

        return snapshot.ConvertTo<T>();
    }

    public async Task<IReadOnlyCollection<T>> ListarTodos<T>(Paginacao paginacao) where T : IEntidadeDoFirebase
    {
        var colecao = db.Collection(typeof(T).Name);
        var pontoInicial = (paginacao.Pagina - 1) * paginacao.QuantidadePorPagina;
        var query = colecao.OrderBy(FieldPath.DocumentId)
            .StartAt(pontoInicial.ToString())
            .Limit(paginacao.QuantidadePorPagina);
        var snapshot = await query.GetSnapshotAsync();
        if (snapshot.Count == 0)
        {
            throw new NaoEncontradoException($"Nenhuma entidade \"{typeof(T).Name}\" encontrada.");
        }

        return snapshot.Documents.Select(x => x.ConvertTo<T>()).ToList();
    }

    public async Task<IReadOnlyCollection<T>> ListarIguais<T>(BuscaDeEntidadesPorCampoDto buscaDto)
        where T : IEntidadeDoFirebase
    {
        var colecao = db.Collection(typeof(T).Name);
        var pontoInicial = (buscaDto.Paginacao.Pagina - 1) * buscaDto.Paginacao.QuantidadePorPagina;
        var query = colecao.WhereEqualTo(buscaDto.Campo, buscaDto.Valor)
            .OrderBy(buscaDto.Campo)
            .StartAt(pontoInicial.ToString())
            .Limit(buscaDto.Paginacao.QuantidadePorPagina);
        var snapshot = await query.GetSnapshotAsync();
        if (snapshot.Count == 0)
        {
            throw new NaoEncontradoException(
                $"Nenhuma entidade \"{typeof(T).Name}\" encontrada com esses critérios de busca.");
        }

        return snapshot.Documents.Select(x => x.ConvertTo<T>()).ToList();
    }

    public async Task<IReadOnlyCollection<T>> ListarSemelhantes<T>(BuscaDeEntidadesPorCampoDto buscaDto)
        where T : IEntidadeDoFirebase
    {
        var colecao = db.Collection(typeof(T).Name);
        var pontoInicial = (buscaDto.Paginacao.Pagina - 1) * buscaDto.Paginacao.QuantidadePorPagina;
        var query = colecao.WhereGreaterThanOrEqualTo(buscaDto.Campo, buscaDto.Valor)
            .WhereLessThanOrEqualTo(buscaDto.Campo, buscaDto.Valor + "\uf8ff")
            .OrderBy(buscaDto.Campo)
            .StartAt(pontoInicial.ToString())
            .Limit(buscaDto.Paginacao.QuantidadePorPagina);
        var snapshot = await query.GetSnapshotAsync();
        if (snapshot.Count == 0)
        {
            throw new NaoEncontradoException(
                $"Nenhuma entidade \"{typeof(T).Name}\" encontrada com esses critérios de busca.");
        }

        return snapshot.Documents.Select(x => x.ConvertTo<T>()).ToList();
    }

    public async Task Excluir<T>(string id) where T : IEntidadeDoFirebase
    {
        var doc = db.Collection(typeof(T).Name).Document(id);
        await doc.DeleteAsync();
    }

    public async Task Excluir<T>(Guid id) where T : IEntidadeDoFirebase
    {
        var doc = db.Collection(typeof(T).Name).Document(id.ToString("N"));
        await doc.DeleteAsync();
    }

    public async Task Excluir(DocumentReference doc)
    {
        await doc.DeleteAsync();
    }
}