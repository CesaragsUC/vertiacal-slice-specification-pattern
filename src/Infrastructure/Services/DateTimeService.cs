using Application.Abstractions;

namespace Application.Infrastructure.Services;



public class DateTimeService : IDateTime
{
    public DateTime Now => DateTime.Now;
}