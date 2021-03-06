﻿using System;
using Newtonsoft.Json;
using Verify;

class Scrubber<T> :
    WriteOnlyJsonConverter
    where T : struct
{
    static string name = typeof(T).Name;
    static Func<T, int> intOrNext = null!;

    public static void SetIntOrNext(Func<T, int> func)
    {
        intOrNext = func;
    }

    public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
    {
        if (value == null)
        {
            return;
        }

        WriteValue(writer, (T) value);
    }

    public void WriteValue(JsonWriter writer, T value)
    {
        var next = intOrNext(value);
        writer.WriteRawValue($"{name}_{next}");
    }

    public override bool CanRead => false;

    public override bool CanConvert(Type type)
    {
        return type == typeof(T) ||
               type == typeof(T?);
    }
}