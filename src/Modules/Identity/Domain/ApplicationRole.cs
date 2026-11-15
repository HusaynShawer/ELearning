using Microsoft.AspNetCore.Identity;

namespace ELearning.Identity.Domain;

public class ApplicationRole : IdentityRole<Guid>
{
    public ApplicationRole() { }
    public ApplicationRole(string name) : base(name) { }
}
