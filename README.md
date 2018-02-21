# PwnedPasswords.net
This is a very quick and dirty .net client for Troy Hunt's [excellent PwnedPasswords service](https://www.troyhunt.com/ive-just-launched-pwned-passwords-version-2/).

The library will call the PwnedPasswords service using a partial of a SHA1 hash of any password you give it. At no point does it send the password itself, or indeed anything except the first 5 characters of the SHA1 representation of that password.

This means you can safely check if a password is on a known password list *without giving away the password itself*.

# How to use

Standard use:

```C#
// Create an instance of the client, then call CheckPassword:

var pwdClient = new PwnedPasswordsClient(); // Client instance, you can also pass in your own instance of HttpClient

if(pwdClient.CheckPassword("password")) // "password" can be any clear text password
{
    // If the method returns true, the password was known
    Console.WriteLine("pwned!");
}
else
{
    // If it returned false, the password is not currently known.
    Console.WriteLine("Not pwned!");
}
```

You can also check a SHA1 directly, if you don't feel comfortable passing a raw password around:

```C#
if(pwdClient.CheckSha1HashedPassword("21BD12DC183F740EE76F27B78EB39C8AD972A757"))
{
    Console.WriteLine("pwned!");
}
else
{
    Console.WriteLine("Not pwned!");
}
```

# Disclaimer
This library is in no way affiliated with, or endorsed by Troy Hunt, PwnedPasswords, HaveIBeenPwned or any of the services they provide.

This library has received MINIMAL testing at best - use at your own risk! Or just steal the code and make it better.


