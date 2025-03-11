namespace JsmeNaLede.API.Model.Interfaces;

public interface IAuditable
{
    public DateTimeOffset CreatedDate { get; set; }
    public DateTimeOffset? UpdatedDate { get; set; }
}
