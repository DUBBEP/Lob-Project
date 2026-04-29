
[System.Serializable]
public class LoginFields { public string name; public string password; }

[System.Serializable]
public class AuthResponse { public string token; public UserData user; }

[System.Serializable]
public class UserData { public int id; public string name; }

[System.Serializable]
public class LoginResponse
{
    public bool success;
    public LaravelErrorResponse response;
}

[System.Serializable]
public class LaravelErrorResponse
{
    public string message; // The general message (e.g., "The given data was invalid.")
    public ErrorDetails errors;
}

[System.Serializable]
public class ErrorDetails
{
    // These names must match your form field names (name, password)
    public string[] name;
    public string[] password;
}