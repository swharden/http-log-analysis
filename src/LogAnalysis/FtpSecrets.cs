using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogAnalysis;

internal class FtpSecrets
{
    public readonly string Hostname;
    public readonly string Username;
    public readonly string Password;

    public FtpSecrets()
    {
        var config = new ConfigurationBuilder().AddUserSecrets<FtpSecrets>().Build();
        Hostname = config["hostname"];
        Username = config["username"];
        Password = config["password"];

        if (string.IsNullOrEmpty(Hostname))
            throw new InvalidOperationException("secret 'hostname' not found");

        if (string.IsNullOrEmpty(Username))
            throw new InvalidOperationException("secret 'username' not found");

        if (string.IsNullOrEmpty(Password))
            throw new InvalidOperationException("secret 'password' not found");
    }

    public override string ToString()
    {
        return $"{Hostname} {Username}:{Password}";
    }
}
