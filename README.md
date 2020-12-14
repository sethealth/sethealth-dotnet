# Sethealth dotnet-client

Sethealth dotnet client allows to access the backend sethealth API from a server. The unique use case of this library today is to provide a authentication schema to delegate the "frontend" javascript library to communicate safely with the sethealth backend.

This is accomplish by the generation of a service account in sethealth. A service account is a long-living account for non-human users, like servers. Once a service account is created, a api key and a api secret are generated, this credentials **MUST be kept private, never exposed in a client side application**.

This "long-living" credentials can be used instead to create short-living credentials in the shape of access tokens in order to call the upload/download medical data from the client.

## Install

### .NET cli
```
dotnet add package Sethealth.net
```

### Package Manager
```
PM> Install-Package Sethealth.net -Version 0.0.4
```

### Package Reference
```
<PackageReference Include="Sethealth.net" Version="0.0.4" />
```

### Package CLI
```
paket add Sethealth.net --version 0.0.4
```

## Usage

Get your service account credentials from the [Sethealth Dashboard](https://dashboard.set.health).

**.bashrc/.zshrc:***

```bash
export SETHEALTH_KEY="sa_0000000000000"
export SETHEALTH_SECRET="xxxxxxxxxxxxxxxxxxxxxxxxxxxxx="
```

**Main.cs:***

```c#
using Sethealth;

# Create sethealth client
var client = new Client();

# Ask for a short-living access token
GetTokenResponse response = await client.GetToken();
Console.WriteLine("ACCESS TOKEN {0}", response.token);
```

Alternatively, the credentials can be provided programatically by passing the `api key` and the `api secret` as arguments to `Client`.

```c#
using Sethealth;


# Credentials
var apiKey = "HERE THE API KEY";
var apiKey = "HERE THE API SECRET";
var client = new Client();

# Ask for a short-living access token
GetTokenResponse response = await client.GetToken();
Console.WriteLine("ACCESS TOKEN {0}", response.token);
```

>Note: Credentials should be kept secret, it's not a good practice to hard code them in the source code.


## Release

1. Update version in Sethealth.net/Sethealth.net.csproj
2. Run `make prep.release`
