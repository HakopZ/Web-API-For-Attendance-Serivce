using System.Security.Claims;

public class DomainMiddleWare
{
    private RequestDelegate requestDelegate;
    private List<string> domains;

    public DomainMiddleWare(RequestDelegate req, List<string> allowedDomains)
    {
        requestDelegate = req;
        domains = allowedDomains;
    }

    public async Task Invoke(HttpContext context)
    {
        string domain = context.Request.Host.Host;
        if (!domains.Contains(domain))
        {
            context.Response.StatusCode = 401;
            await context.Response.WriteAsync("Unauthorized");
            return;

        }
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, "domainUser"),
            new Claim(ClaimTypes.Role, "domainRole"),
        };

        var identity = new ClaimsIdentity(claims, "DomainAuth");
        var principal = new ClaimsPrincipal(identity);
        context.User = principal;

        await requestDelegate(context);
    }
}