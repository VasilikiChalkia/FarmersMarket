namespace FarmersMarket.Models;

public static class AppRoles
{
    public const string SystemAdmin = "SystemAdmin";
    public const string MunicipalUser = "MunicipalUser";
    public const string Seller = "Seller";

    public static readonly string[] All = [SystemAdmin, MunicipalUser, Seller];
}