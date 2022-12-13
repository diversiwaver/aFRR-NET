using AutoMapper;

namespace WebAPI.DTOs.DTOConverters;

public static class DTOConverter<T, U>
{
    private readonly static MapperConfiguration config = new(cfg =>
    {
        cfg.CreateMap<T, U>();

    });
    private readonly static Mapper mapper = new(config);

    //      OUTPUT                     <FROM>      <TO>               <SOURCE>
    //var bookingList = DtoConverter<BookingDto, Booking>.FromList(bookingDtoList);
    //var bookingDto = DtoConverter<Booking, BookingDto>.From(booking);
    //var booking = DtoConverter<BookingDto, Booking>.From(bookingDto);
    public static U From(T sourceObject) => mapper.Map<T, U>(sourceObject);
    public static IEnumerable<U> FromList(IEnumerable<T> sourceList) => sourceList.ToList().Select(obj => From(obj));
}
