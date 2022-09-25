 
using Microsoft.AspNetCore.Identity;

namespace ChatDomain.Models;
 
public class User : BaseEntity 
{
    private readonly IdentityUser<Guid> _identityUser;
    public string FullName { get; set; }

    public string UserName
    {
        get { return _identityUser.UserName; }
        set { _identityUser.UserName = value; }
    }
    public string Email
    {
        get { return _identityUser.Email; }
        set { _identityUser.Email = value; }
    }
    public Guid UserGuid
    {
        get { return _identityUser.Id; }
        set { _identityUser.Id = value; }
    }
    public User(IdentityUser<Guid> identityUser)
    {
        _identityUser = identityUser;
    }
    
}