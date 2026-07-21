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

    [Fact]
    public async Task Create_item_returns_201()
    {
        var service = new ItemService();
        var controller = new ItemsController(service);
        var request = new CreateItemRequest
        {
            Title = "Lommebok",
            Description = "Svart lommebok med bankkort og førerkort",
            Category = "Other",
            FoundLocation = "Park"
        };

        var result = await controller.Create(request);
        Assert.IsType<CreatedAtActionResult>(result);
    }

    private Item CreateAvailableItem()
    {
        return new Item
        {
            Title = "Found item 1",
            Description = "Found during cleaning up the park",
            Category = "Other",
            FoundLocation = "Park"
        };
    }

    private Item CreateReturnedItem()
    {
        var item = new Item
        {
            Title = "Found item 2",
            Description = "Found during a busy day at the mall",
            Category = "Other",
            FoundLocation = "Park"
        };

        item.Claim("Jane Doe");
        item.Return();

        return item;
    }

    private Item CreateClaimedItem()
    {
        var item = new Item
        {
            Title = "Found item 3",
            Description = "Found during a long road trip",
            Category = "Other",
            FoundLocation = "Park"
        };

        item.Claim("John Doe");

        return item;
    }
}
