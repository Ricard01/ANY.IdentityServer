using System.Security.Claims;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;


namespace ANY.DuendeIDS.Infrastructure.Services;

public  class MyProfileService : IProfileService
{
    
    public MyProfileService() //TestUserStore users
    {
        //_users = users;
    }

    // GetProfileDataAsync is what controls what claims are issued in the response
    public Task GetProfileDataAsync(ProfileDataRequestContext context)
    {
        // context holds information about the request, the user, the client, the scopes, and the claims being requested
        // context.Subject is the user for whom the result is request is being made
        // context.Subject.Claims is the claims collection from the user's session cookie at login time
        // context.IssuedClaims is the collection of claims that your logic has decided to return in the response


        // if (context.Client.ClientId == "bffAngular")
        // {
            context.IssuedClaims.Add(new Claim("Permissions", "hola"));

            var liClaims = context.IssuedClaims;
            foreach (var claim in liClaims)
            {
                Console.WriteLine($"{claim.Type} - {claim.Value}");

            }
        // }

      


        return Task.CompletedTask;
    }

    // IsActiveAsync is called to ask your custom logic if the user is still "active".
    // If the user is not "active" then no new tokens will be created for them, even 
    // if the user has an active session with IdentityServer.
    public Task IsActiveAsync(IsActiveContext context)
    {
        // as above, context.Subject is the user for whom the result is request is being made
        // setting context.IsActive to false allows your logic to indicate that the token should not be created
        // context.IsActive defaults to true

        return Task.CompletedTask;
    }
}