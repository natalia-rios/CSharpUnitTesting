using Moq;
using NUnit.Framework;
using TestNinja.Mocking;
using Assert = NUnit.Framework.Assert;

namespace TestNInja.UnitTests.Mocking;

[TestFixture]
public class BookingHelperOverlappingBookExistsTests
{
    [SetUp]
    public void SetUp()
    {
        _existingBooking = new Booking
        {
            Id = 2,
            ArrivalDate = ArriveOn(2017, 1, 15),
            DepartureDate = DepartOn(2017, 1, 20),
            Reference = "a"
        };

        _repository = new Mock<IBookingRepository>();
        _repository.Setup(r => r.GetActiveBookings(1)).Returns(new List<Booking>
        {
            _existingBooking
        }.AsQueryable());
    }

    private Booking _existingBooking;
    private Mock<IBookingRepository> _repository;

    private DateTime Before(DateTime dateTime, int days = 1)
    {
        return dateTime.AddDays(-days);
    }

    private DateTime After(DateTime dateTime, int days = 1)
    {
        return dateTime.AddDays(days);
    }

    private DateTime ArriveOn(int year, int month, int day)
    {
        return new DateTime(year, month, day, 14, 0, 0);
    }

    private DateTime DepartOn(int year, int month, int day)
    {
        return new DateTime(year, month, day, 10, 0, 0);
    }

    [Test]
    public void BookingStartsAndFinishesBeforeExistingBooking_ReturnEmptyString()
    {
        var result = BookingHelper.OverlappingBookingsExist(new Booking
        {
            Id = 1,
            ArrivalDate = Before(_existingBooking.ArrivalDate, 2),
            DepartureDate = Before(_existingBooking.ArrivalDate, 1)
        }, _repository.Object);

        Assert.IsEmpty(result);
    }

    [Test]
    public void BookingStartsBeforeAndFinishesInTheMiddleOfExistingBook_ReturnExistingBooksReference()
    {
        var result = BookingHelper.OverlappingBookingsExist(new Booking
        {
            Id = 1,
            ArrivalDate = Before(_existingBooking.ArrivalDate),
            DepartureDate = After(_existingBooking.ArrivalDate)
        }, _repository.Object);

        Assert.That(result, Is.EqualTo(_existingBooking.Reference));
    }

    [Test]
    public void BookingStartsBeforeAndFinishesAfterExistingBook_ReturnExistingBooksReference()
    {
        var result = BookingHelper.OverlappingBookingsExist(new Booking
        {
            Id = 1,
            ArrivalDate = Before(_existingBooking.ArrivalDate),
            DepartureDate = After(_existingBooking.DepartureDate)
        }, _repository.Object);

        Assert.That(result, Is.EqualTo(_existingBooking.Reference));
    }

    [Test]
    public void BookingStartsAndFinishesInTheMiddleOfExistingBook_ReturnExistingBooksReference()
    {
        var result = BookingHelper.OverlappingBookingsExist(new Booking
        {
            Id = 1,
            ArrivalDate = After(_existingBooking.ArrivalDate),
            DepartureDate = Before(_existingBooking.DepartureDate)
        }, _repository.Object);

        Assert.That(result, Is.EqualTo(_existingBooking.Reference));
    }

    [Test]
    public void BookingStartsInTheMiddleOfAnExistingBookingButFinishesAfter_ReturnExistingBooksReference()
    {
        var result = BookingHelper.OverlappingBookingsExist(new Booking
        {
            Id = 1,
            ArrivalDate = After(_existingBooking.ArrivalDate),
            DepartureDate = After(_existingBooking.DepartureDate)
        }, _repository.Object);

        Assert.That(result, Is.EqualTo(_existingBooking.Reference));
    }

    [Test]
    public void BookingStartsAndFinishesAfterExistingBooking_ReturnEmptyString()
    {
        var result = BookingHelper.OverlappingBookingsExist(new Booking
        {
            Id = 1,
            ArrivalDate = After(_existingBooking.DepartureDate),
            DepartureDate = After(_existingBooking.DepartureDate, 2)
        }, _repository.Object);

        Assert.IsEmpty(result);
    }

    [Test]
    public void BookingsOverlapButNewBookingIsCancelled_ReturnEmptyString()
    {
        var result = BookingHelper.OverlappingBookingsExist(new Booking
        {
            Id = 1,
            ArrivalDate = Before(_existingBooking.ArrivalDate),
            DepartureDate = After(_existingBooking.ArrivalDate),
            Status = "Cancelled"
        }, _repository.Object);

        Assert.IsEmpty(result);
    }
}