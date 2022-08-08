using System.Net;
using System.Text;
using GraphQL;
using GraphQL.Client.Http;

namespace TradeArtTestProject;

public static class Extensions
{
    public static string ToStringView(this byte[] bytes)
    {
        var str = new StringBuilder();

        foreach (var t in bytes)
            str.Append($"{t:X2}");

        return str.ToString();
    }

    public static bool IsOk<T>(this GraphQLResponse<T> response)
    {
        var statusCode = (response as GraphQLHttpResponse<T>)!.StatusCode;
        return statusCode == HttpStatusCode.OK;
    }
}