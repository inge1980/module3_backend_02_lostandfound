using webapi.Model;
using webapi.Services;
using webapi.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace tests;

public class WebapiTests
{

    [Fact]
    public void New_item_should_have_found_time()
    {
        var before = DateTime.UtcNow;

        var item = new Item();

        var after = DateTime.UtcNow;

        Assert.InRange(
            item.FoundAtUtc,
            before,
            after);
    }

    [Fact]
    public void Available_item_can_be_claimed()
    {
        var item = new Item();

        item.Claim("Ola Nordmann");

        Assert.Equal(ItemStatus.Claimed, item.Status);
        Assert.Equal("Ola Nordmann", item.ClaimedBy);
        Assert.NotNull(item.ClaimedAtUtc);
    }

    [Fact]
    public void Claimed_item_cannot_be_claimed_again()
    {
        var item = new Item();

        item.Claim("Ola");

        Assert.Throws<InvalidOperationException>(
            () => item.Claim("Per"));
    }

    [Fact]
    public void Claimed_item_can_be_returned()
    {
        var item = new Item();

        item.Claim("Ola");

        item.Return();

        Assert.Equal(ItemStatus.Returned, item.Status);
        Assert.NotNull(item.ReturnedAtUtc);
    }

    [Fact]
    public void Available_item_cannot_be_returned()
    {
        var item = new Item();

        Assert.Throws<InvalidOperationException>(
            () => item.Return());
    }

    [Fact]
    public void Available_item_can_be_deleted()
    {
        var item = new Item();

        var exception = Record.Exception(
            () => item.EnsureCanDelete());

        Assert.Null(exception);
    }
    
    [Fact]
    public void Claimed_item_cannot_be_deleted()
    {
        var item = new Item();

        item.Claim("Ola");

        Assert.Throws<InvalidOperationException>(
            () => item.EnsureCanDelete());
    }
}
