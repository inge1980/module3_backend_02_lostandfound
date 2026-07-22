using webapi.Model;
using webapi.Services;
using webapi.Controllers;
using webapi.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace tests;

public class WebapiTests
{

    [Fact]
    public void New_item_should_be_available()
    {
        var item = new Item();
        Assert.Equal(ItemStatus.Available, item.Status);
    }
    
    [Fact]
    public void New_item_should_have_found_time()
    {
        var before = DateTime.UtcNow;
        var item = new Item();
        var after = DateTime.UtcNow;
        Assert.InRange(
            item.FoundAtUtc,
            before,
            after
        );
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
            () => item.Claim("Per")
        );
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
            () => item.Return()
        );
    }

    [Fact]
    public void Available_item_can_be_deleted()
    {
        var item = new Item();
        var exception = Record.Exception(
            () => item.EnsureCanDelete()
        );
        Assert.Null(exception);
    }
    
    [Fact]
    public void Claimed_item_cannot_be_deleted()
    {
        var item = new Item();
        item.Claim("Ola");
        Assert.Throws<InvalidOperationException>(
            () => item.EnsureCanDelete()
        );
    }

    [Fact]
    public async Task Can_filter_items_by_status()
    {
        var repository = new InMemoryItemRepository();
        var available = new Item();
        var claimed = new Item();
        claimed.Claim("Ola");
        await repository.AddAsync(available);
        await repository.AddAsync(claimed);
        var result = await repository.GetAllAsync(
            ItemStatus.Claimed
        );
        Assert.Single(result);
        Assert.Equal(
            ItemStatus.Claimed,
            result.First().Status
        );
    }

    [Fact]
    public async Task Return_claimed_item_returns_200()
    {
        var repository = new InMemoryItemRepository();
        var service = new ItemService(repository);
        var controller = new ItemsController(service);

        var item = CreateClaimedItem();

        await repository.AddAsync(item);

        var result = await controller.Return(item.Id);

        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public async Task Claim_available_item_returns_200()
    {
        var repository = new InMemoryItemRepository();
        var service = new ItemService(repository);
        var controller = new ItemsController(service);

        var item = CreateAvailableItem();

        await repository.AddAsync(item);

        var request = new ClaimItemRequest
        {
            ClaimedBy = "Ola"
        };

        var result = await controller.Claim(item.Id, request);

        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public async Task Create_item_returns_201()
    {
        var repository = new InMemoryItemRepository();
        var service = new ItemService(repository);
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

    [Fact]
    public async Task Delete_available_item_returns_204()
    {
        var repository = new InMemoryItemRepository();
        var service = new ItemService(repository);
        var controller = new ItemsController(service);
        var item = CreateAvailableItem();
        await repository.AddAsync(item);
        var result = await controller.Delete(item.Id);
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task Create_item_without_title_returns_400()
    {
        var repository = new InMemoryItemRepository();
        var service = new ItemService(repository);
        var controller = new ItemsController(service);

        var request = new CreateItemRequest
        {
            Title = "",
            Description = "No title",
            Category = "Other",
            FoundLocation = "Park"
        };

        var result = await controller.Create(request);

        Assert.IsType<BadRequestResult>(result);
    }

    [Fact]
    public async Task Create_item_with_title_longer_than_80_characters_returns_400()
    {
        var repository = new InMemoryItemRepository();
        var service = new ItemService(repository);
        var controller = new ItemsController(service);

        var request = new CreateItemRequest
        {
            Title = new string('A', 81),
            Description = "Too long title",
            Category = "Other",
            FoundLocation = "Park"
        };

        var result = await controller.Create(request);

        Assert.IsType<BadRequestResult>(result);
    }

    [Fact]
    public async Task Get_missing_item_returns_404()
    {
        var repository = new InMemoryItemRepository();
        var service = new ItemService(repository);
        var controller = new ItemsController(service);

        var result = await controller.GetById(Guid.NewGuid());

        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task Delete_claimed_item_returns_409()
    {
        var repository = new InMemoryItemRepository();
        var service = new ItemService(repository);
        var controller = new ItemsController(service);
        var item = CreateClaimedItem();
        await repository.AddAsync(item);
        var result = await controller.Delete(item.Id);
        Assert.IsType<ConflictResult>(result);
    }

    [Fact]
    public async Task Claim_claimed_item_returns_409()
    {
        var repository = new InMemoryItemRepository();
        var service = new ItemService(repository);
        var controller = new ItemsController(service);

        var item = CreateClaimedItem();

        await repository.AddAsync(item);

        var request = new ClaimItemRequest
        {
            ClaimedBy = "Per"
        };

        var result = await controller.Claim(item.Id, request);

        Assert.IsType<ConflictResult>(result);
    }

    [Fact]
    public async Task Return_available_item_returns_409()
    {
        var repository = new InMemoryItemRepository();
        var service = new ItemService(repository);
        var controller = new ItemsController(service);

        var item = CreateAvailableItem();

        await repository.AddAsync(item);

        var result = await controller.Return(item.Id);

        Assert.IsType<ConflictResult>(result);
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
