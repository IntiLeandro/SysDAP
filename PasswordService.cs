using Microsoft.AspNetCore.Identity;
public class PasswordService    
{
    private readonly PasswordHasher<object> _passwordHasher;

    public PasswordService()
    {
        _passwordHasher = new PasswordHasher<object>();
    }

    // Generar hash de contraseña
    public string HashPassword(string password)
    {
        return _passwordHasher.HashPassword(null, password);
    }

    // Verificar la contraseña proporcionada con el hash almacenado
    public bool VerifyPassword(string hashedPassword, string providedPassword)
    {
        var result = _passwordHasher.VerifyHashedPassword(null, hashedPassword, providedPassword);
        return result == PasswordVerificationResult.Success;
    }
}

