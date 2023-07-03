using System.Buffers;
using System.Text;

namespace WebApi.Services.Implementation;

public class Base64Encoder : IBase64Encoder
{
    public string Encode(string rawData)
    {
        ArrayBufferWriter<byte> buffer = new();

        Encoding.UTF32.GetBytes(rawData.AsSpan(), buffer);

        string result = Convert.ToBase64String(buffer.WrittenSpan);

        return result;
    }
}