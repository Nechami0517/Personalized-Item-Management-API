using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using project.Interfaces;
using project.Models;

namespace project.Services;


public abstract class ServiceItemJson<T> : GetFuncService<T>, IServiceItems<T> where T : IGeneric 
{
    public ServiceItemJson(IHostEnvironment env) : base(env)
    {

    }

    protected void saveToFile()
    {
        System.Console.WriteLine("in save to file----------------------------" + filePath);
        var options = new JsonSerializerOptions
        {
            WriteIndented = true
        };

        var jsonData = JsonSerializer.Serialize(MyList, options);
        File.WriteAllText(filePath, jsonData);
    }
    



public abstract List<T> Get();
    public abstract T Get(int id);

    public abstract int Insert(T newT);

    public abstract bool Update(int id, T book);

    public abstract bool Delete(int id);

    public abstract void deleteUsersItem(int id);
    
}

