using System.Text.RegularExpressions;
using Microsoft.Playwright;
using Microsoft.Playwright.MSTest;

namespace Homework_JE;

[TestClass]
public class Homework_JE : PageTest
{
    private string? username;
    private string? password;

    [TestInitialize]
    public void Pipeline()
    {
        username = Environment.GetEnvironmentVariable("HW_Selected_User") ?? "standard_user";
        password = Environment.GetEnvironmentVariable("HW_S_Password") ?? "secret_sauce";
    }

    [TestMethod]
    public async Task LoginWithMultipleUsers()
    {
        await Page
            .GotoAsync("https://www.saucedemo.com/");

        // Check if the page has exact title "Swag Labs".
        await Expect(Page)
            .ToHaveTitleAsync("Swag Labs");

        // Fill the username and password fields.
        await Page
            .FillAsync("[data-test='username']", username ?? string.Empty);
        await Page
            .FillAsync("[data-test='password']", password ?? string.Empty);

        // Click the login button.
        await Page
            .GetByRole(AriaRole.Button, new() { Name = "Login" })
            .ClickAsync();

        // Check if the expected page is correctly loaded after login.
        await Expect(Page
            .Locator(".app_logo"))
            .ToHaveTextAsync("Swag Labs");
        
        // Find and click on the item we want to purchase.
        await Page
            .Locator("[data-test='item-1-title-link']").ClickAsync();
        
        // Verify that the URL is expected after selecting the item.
        await Expect(Page)
            .ToHaveURLAsync("https://www.saucedemo.com/inventory-item.html?id=1");

        // Add the item to the cart.
        await Page
             .GetByRole(AriaRole.Button, new() { Name = "Add to cart" })
             .ClickAsync();

        // Verify that the cart badge is updated to show selected items.
        await Expect(Page
            .Locator(".shopping_cart_badge"))
            .ToHaveTextAsync("1");

        // Click on shopping cart icon to see contents.
         await Page
           .Locator(".shopping_cart_link")
           .ClickAsync();

        // Verify that the cart page is displayed.
        await Expect(Page.Locator(".title"))
            .ToHaveTextAsync("Your Cart");

        // Go to checkout page.
        await Page
            .GetByRole(AriaRole.Button, new() { Name = "Checkout" })
            .ClickAsync();       

        // Verify that the Checkout page is displayed.
        await Expect(Page.Locator(".title"))
            .ToHaveTextAsync("Checkout: Your Information");
        
        // Fill the checkout form with data.
        await Page.FillAsync("[data-test='firstName']", "Tomass");
        await Page.FillAsync("[data-test='lastName']", "O");
        await Page.FillAsync("[data-test='postalCode']", "LV-1111");

        // Go to the next step of checkout.
        await Page
            .GetByRole(AriaRole.Button, new() { Name = "Continue" })
            .ClickAsync();

        // Verify that the Checkout Overview page is displayed.
        await Expect(Page.Locator(".title"))
            .ToHaveTextAsync("Checkout: Overview");

        // Click on the button "Finish".
        await Page
            .GetByRole(AriaRole.Button, new() { Name = "Finish" })
            .ClickAsync();

        // Check that correct page is displayed.
        await Expect(Page.Locator(".title"))
            .ToHaveTextAsync("Checkout: Complete!");

        // Verify that order completion message is present.
        await Expect(Page.Locator(".complete-header"))
            .ToHaveTextAsync("Thank you for your order!");

        // Return back to home page.
        await Page
            .GetByRole(AriaRole.Button, new() { Name = "Back Home" })
            .ClickAsync();
    }
} 