using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.AspNetCore.Http;

public class TestSession : ISession
{
    private Dictionary<string, object> _sessionStorage = new();

    public string Id => throw new NotImplementedException();

    public bool IsAvailable => throw new NotImplementedException();

    public IEnumerable<string> Keys => _sessionStorage.Keys;

    public void Clear()
    {
        _sessionStorage.Clear();
    }

    public Task CommitAsync(CancellationToken cancellationToken = default) => throw new NotImplementedException();

    public Task LoadAsync(CancellationToken cancellationToken = default) => throw new NotImplementedException();

    public void Remove(string key)
    {
        _sessionStorage.Remove(key);
    }

    public void Set(string key, byte[] value)
    {
        _sessionStorage[key] = value;
    }

    public bool TryGetValue(string key, out byte[] value)
    {
        if (_sessionStorage.TryGetValue(key, out var objValue) && objValue is byte[] bytes)
        {
            value = bytes;
            return true;
        }

        value = null;
        return false;
    }
}
