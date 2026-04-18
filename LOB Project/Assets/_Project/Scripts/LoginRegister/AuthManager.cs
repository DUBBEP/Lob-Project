using UnityEngine;

public static class AuthManager
{
    public static string Token { get; set; }
    public static bool IsLoggedIn => !string.IsNullOrEmpty(Token);

    public static string GetUsername()
        => PlayerPrefs.GetString("CurrentUsername", "UnknownPlayer");
}