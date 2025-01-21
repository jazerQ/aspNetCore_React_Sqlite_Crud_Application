namespace MagicVilla_VillaAPI.Contracts
{
    public record BookResponse(
        Guid Id,
        string Title,
        string Description,
        decimal Price
        );
}
