namespace AwesomeLib.Services.Interfaces
{
    public interface IConverter
    {
        string ConvertToJson(object data);
        string ConvertToJson(object data, bool camelCase, bool indented);
    }
}