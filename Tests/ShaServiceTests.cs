using TradeArtTestProject.Services.Interfaces;
using Xunit;

namespace TradeArtTestProject.Tests;

public class ShaServiceTests
{
    private readonly IShaService _shaService;

    public ShaServiceTests(IShaService shaService)
        => _shaService = shaService;

    [Theory]
    // Hashes obtained via Get-FileHash powershell command
    [InlineData(@"TestFiles\file1.dat", "AE56BDDB2A3E7F970ABE7168CABA79C9D96166AB523ADB6C8C0FD007493B4A1D")]
    [InlineData(@"TestFiles\file2.dat", "DFCF285693701E5C888BE06768A5A85422C9ADB142B7298CD6F77C561BF861BB")]
    public async Task CalculateHash_CorrectResults(string filePath, string expected)
    {
        Assert.True(File.Exists(filePath));

        var shaResult = await _shaService.CalculateShaAsync(filePath);

        Assert.True(shaResult.Success);
        Assert.Equal(expected, shaResult.Data);
    }

    [Fact]
    public async Task CalculateHash_FileNotFound()
    {
        var shaResult = await _shaService.CalculateShaAsync("fileNotExist");

        Assert.False(shaResult.Success);
        Assert.Equal("File not found", shaResult.ErrorMessage);
    }
}