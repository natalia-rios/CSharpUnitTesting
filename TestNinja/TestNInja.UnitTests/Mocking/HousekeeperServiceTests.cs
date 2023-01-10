using Moq;
using NUnit.Framework;
using TestNinja.Mocking;

namespace TestNInja.UnitTests.Mocking;

[TestFixture]
public class HousekeeperServiceTests
{
    [SetUp]
    public void SetUp()
    {
        _houseKeeper = new Housekeeper { Email = "a", FullName = "b", Oid = 1, StatementEmailBody = "c" };

        var unitOfWork = new Mock<IUnitOfWork>();
        unitOfWork.Setup(uow => uow.Query<Housekeeper>()).Returns(new List<Housekeeper>
        {
            _houseKeeper
        }.AsQueryable());

        _statementFileName = "fileName";
        _statementGenerator = new Mock<IStatementGenerator>();
        _statementGenerator
            .Setup(sg => sg.SaveStatement(_houseKeeper.Oid, _houseKeeper.FullName, _statementDate))
            .Returns(() => _statementFileName);

        _emailSender = new Mock<IEmailSender>();
        _messageBox = new Mock<IXtraMessageBox>();

        _service = new HouseKeeperService(
            unitOfWork.Object,
            _statementGenerator.Object,
            _emailSender.Object,
            _messageBox.Object);
    }

    private string _statementFileName;
    private Mock<IStatementGenerator> _statementGenerator;
    private Mock<IXtraMessageBox> _messageBox;
    private Housekeeper _houseKeeper;
    private Mock<IEmailSender> _emailSender;
    private HouseKeeperService _service;
    private DateTime _statementDate;

    private void VerifyEmailNotSent()
    {
        _emailSender.Verify(es => es.EmailFile(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>()),
            Times.Never);
    }

    private void VerifyEmailSent()
    {
        _emailSender.Verify(es => es.EmailFile(
            _houseKeeper.Email,
            _houseKeeper.StatementEmailBody,
            _statementFileName,
            It.IsAny<string>()));
    }

    [Test]
    public void SendStatementEmails_WhenCalled_GenerateStatements()
    {
        _service.SendStatementEmails(_statementDate);
        _statementGenerator.Verify(sg =>
            sg.SaveStatement(_houseKeeper.Oid, _houseKeeper.FullName, _statementDate));
    }

    [Test]
    [TestCase(null)]
    [TestCase(" ")]
    public void SendStatementEmails_HouseKeepersEmailIsNull_ShouldNotGenerateStatements(string nullOrEmpty)
    {
        _houseKeeper.Email = nullOrEmpty;
        _service.SendStatementEmails(_statementDate);
        _statementGenerator.Verify(sg =>
            sg.SaveStatement(_houseKeeper.Oid, _houseKeeper.FullName, _statementDate), Times.Never);
    }

    [Test]
    public void SendStatementEmails_WhenCalled_EmailTheStatement()
    {
        _statementGenerator.Setup(sg =>
                sg.SaveStatement(_houseKeeper.Oid, _houseKeeper.FullName, _statementDate))
            .Returns(_statementFileName);

        _service.SendStatementEmails(_statementDate);

        _emailSender.Verify(es => es.EmailFile(
            _houseKeeper.Email,
            _houseKeeper.StatementEmailBody,
            _statementFileName,
            It.IsAny<string>()
        ));
    }

    [Test]
    public void SendStatementEmails_StatementFileNameIsNull_ShouldNotEmailTheStatement()
    {
        _statementFileName = null;

        _service.SendStatementEmails(_statementDate);

        VerifyEmailNotSent();
    }

    [Test]
    public void SendStatementEmails_StatementFileNameIsEmptyString_ShouldNotEmailTheStatement()
    {
        _statementFileName = "";

        _service.SendStatementEmails(_statementDate);

        VerifyEmailNotSent();
    }

    [Test]
    public void SendStatementEmails_StatementFileNameIsWhitespace_ShouldNotEmailTheStatement()
    {
        _statementFileName = " ";

        _service.SendStatementEmails(_statementDate);

        VerifyEmailNotSent();
    }

    [Test]
    public void SendStatementEmails_EmailSendingFails_DisplayAMessageBox()
    {
        _emailSender.Setup(es => es.EmailFile(
            It.IsAny<string>(),
            It.IsAny<string>(),
            It.IsAny<string>(),
            It.IsAny<string>()
        )).Throws<Exception>();

        _service.SendStatementEmails(_statementDate);

        _messageBox.Verify(mb => mb.Show(It.IsAny<string>(), It.IsAny<string>(), MessageBoxButtons.OK));
    }
}