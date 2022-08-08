using System.Security.Cryptography;
using TradeArtTestProject.Communication;
using TradeArtTestProject.Services.Interfaces;

namespace TradeArtTestProject.Services;

public class ShaService : ServiceBase, IShaService
{
    private const int ChunkSize = 4096;

    public async Task<ServiceResult<string>> CalculateShaAsync(string filePath)
    {
        if (!File.Exists(filePath))
        {
            return ErrorResult<string>("File not found");
        }

        await using var streamToReadFrom = File.OpenRead(filePath);
        var length = streamToReadFrom.Length;
        using var sha = SHA256.Create();

        var buffer = new byte[ChunkSize];

        // It is ok if on the first call length < ChunkSize, TransformFinalBlock will do all the work
        while (length > ChunkSize)
        {
            length -= await streamToReadFrom.ReadAsync(buffer, 0, ChunkSize);
            sha.TransformBlock(buffer, 0, ChunkSize, buffer, 0);
        }

        _ = await streamToReadFrom.ReadAsync(buffer, 0, (int)length);
        sha.TransformFinalBlock(buffer, 0, (int)length);

        return SuccessResult(sha.Hash?.ToStringView() ?? string.Empty);
    }
}

