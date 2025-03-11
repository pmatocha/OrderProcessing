namespace JsmeNaLede.API.Model.Interfaces;

/// <summary>
/// Object with a unique identifier of type T
/// </summary>
public interface IIdentifiable<T>
    where T : notnull
{
    T Id { get; set; }
}
