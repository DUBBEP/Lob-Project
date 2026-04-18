
[System.Serializable]
public class LoginFields { public string name; public string password; }

[System.Serializable]
public class AuthResponse { public string token; public UserData user; }

[System.Serializable]
public class UserData { public int id; public string name; }
