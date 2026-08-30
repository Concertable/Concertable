using Concertable.B2B.Concert.Api.Responses;
using Concertable.B2B.Concert.Application.DTOs;

namespace Concertable.B2B.Concert.Api.Mappers;

internal sealed class ArtistApplicationResponseMapper : IApplicationResponseMapper
{
    private readonly IApplicationMapper mapper;

    public ArtistApplicationResponseMapper(IApplicationMapper mapper)
    {
        this.mapper = mapper;
    }

    public ApplicationResponse ToResponse(ApplicationDto dto) => mapper.ToArtistResponse(dto);
}
