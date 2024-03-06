using Blazor.Data;
using Blazor.Data.Models;
using Blazor.Data.Parsers;
using Attribute = Blazor.Data.Models.Attribute;

namespace UnitTests.Data;

public class EventScriptTests
{
    private ISqlParser parser = SqlParserFactory.GetParser("sqlite");

    [Fact]
    public void CanCalculatePlacements()
    {
        List<Entity> entities = parser.SqlScriptToEntities(SqliteScriptTestData.EventScript);
        EntityPlacementOrganizer organizer = new();
        Action exp = () => organizer.CalculateRelativePlacements(entities);
        Exception? exception = Record.Exception(exp);
        Assert.Null(exception);
    }

    [Fact]
    public void GuestEntityIsLoadedCorrect()
    {
        List<Entity> entities = parser.SqlScriptToEntities(SqliteScriptTestData.EventScript);
        Entity guest = entities.Single(x => x.Name.Equals("Guest"));
        Assert.NotNull(guest);

        Attribute? id = guest.Attributes.SingleOrDefault(x => x.Name.Equals("Id"));
        Assert.NotNull(id);
        Assert.True(id.IsPrimaryKey);
    }

    [Fact]
    public void EventEntityIsLoadedCorrect()
    {
        List<Entity> entities = parser.SqlScriptToEntities(SqliteScriptTestData.EventScript);
        Entity evt = entities.Single(x => x.Name.Equals("VeaEvent"));
        Assert.NotNull(evt);

        Attribute? id = evt.Attributes.SingleOrDefault(x => x.Name.Equals("Id"));
        Assert.NotNull(id);
        Assert.True(id.IsPrimaryKey);

        Attribute? maxGuestNumber = evt.Attributes.SingleOrDefault(x => x.Name.Equals("MaxGuestNumber"));
        Assert.NotNull(maxGuestNumber);

        Attribute? visibility = evt.Attributes.SingleOrDefault(x => x.Name.Equals("Visibility"));
        Assert.NotNull(visibility);

        Attribute? desc = evt.Attributes.SingleOrDefault(x => x.Name.Equals("Description"));
        Assert.NotNull(desc);

        Attribute? status = evt.Attributes.SingleOrDefault(x => x.Name.Equals("Status"));
        Assert.NotNull(status);

        Attribute? endTime = evt.Attributes.SingleOrDefault(x => x.Name.Equals("EndTime"));
        Assert.NotNull(endTime);

        Attribute? startTime = evt.Attributes.SingleOrDefault(x => x.Name.Equals("StartTime"));
        Assert.NotNull(startTime);

        Attribute? title = evt.Attributes.SingleOrDefault(x => x.Name.Equals("Title"));
        Assert.NotNull(title);
    }

    [Fact]
    public void GuestFkIsLoadedCorrect()
    {
        List<Entity> entities = parser.SqlScriptToEntities(SqliteScriptTestData.EventScript);
        Entity? guestFk = entities.SingleOrDefault(x => x.Name.Equals("GuestFk"));
        Assert.NotNull(guestFk);

        Attribute? guestFkAttr = guestFk.Attributes.SingleOrDefault(x => x.Name.Equals("GuestFk"));
        Assert.NotNull(guestFkAttr);
        Assert.True(guestFkAttr.IsPrimaryKey);
        Assert.NotNull(guestFkAttr.ForeignKey);
        Assert.Equal("Guest", guestFkAttr.ForeignKey!.TargetTableName);
        Assert.Equal("Id", guestFkAttr.ForeignKey.TargetAttributeName);
    }
}