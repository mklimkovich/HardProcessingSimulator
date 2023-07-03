namespace WebApi.Services;

public interface IBase64Encoder
{
    string Encode(string rawData);
}