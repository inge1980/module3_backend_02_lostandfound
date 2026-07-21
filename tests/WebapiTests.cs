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
}
