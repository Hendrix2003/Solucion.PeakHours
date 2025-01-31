﻿using Microsoft.JSInterop;

public class LocalStorageHelper
{
    private readonly IJSRuntime _jsRuntime;

    public LocalStorageHelper(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
    }

    public async Task<string> GetItem(string key)
    {
        return await _jsRuntime.InvokeAsync<string>("localStorage.getItem", key);
    }

    public async Task SetItem(string key, string value)
    {
        await _jsRuntime.InvokeVoidAsync("localStorage.setItem", key, value);
    }

    public async Task RemoveItem(string key)
    {
        await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", key);
    }

    public async Task Clear()
    {
        await _jsRuntime.InvokeVoidAsync("localStorage.clear");
    }
}