[![Build Status](https://barradas.visualstudio.com/Contributions/_apis/build/status/NugetPackage/JsonMasking?branchName=develop)](https://barradas.visualstudio.com/Contributions/_build/latest?definitionId=1&branchName=develop)
[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=ThiagoBarradas_jsonmasking&metric=alert_status)](https://sonarcloud.io/dashboard?id=ThiagoBarradas_jsonmasking)
[![Coverage](https://sonarcloud.io/api/project_badges/measure?project=ThiagoBarradas_jsonmasking&metric=coverage)](https://sonarcloud.io/dashboard?id=ThiagoBarradas_jsonmasking)
[![NuGet Downloads](https://img.shields.io/nuget/dt/JsonMasking.svg)](https://www.nuget.org/packages/JsonMasking/)
[![NuGet Version](https://img.shields.io/nuget/v/JsonMasking.svg)](https://www.nuget.org/packages/JsonMasking/)

# Json Masking 

Replace fields in json, replacing by something, don't care if property is in depth objects. Very useful to replace passwords, credit card number, etc.

This library matching insensitive values with field namespaces. You can use wildcard * to allow any char in pattern;

# Sample

```c#

- Completely masking some of the properties

var example = new 
{
	SomeValue = "Demo",
	Password = "SomePasswordHere",
	DepthObject = new 
	{
		Password = "SomePasswordHere2",
		Card = new 
		{
			Number = "555500022223333"
		}
	},
	CreditCardNumber = "5555000011112222",
	Card = new 
	{
		Number = "555500022223333"
	}
};

var exampleAsString = JsonConvert.Serialize(example); // value must be a json string to masked

// note that password is only replaced when is in root path
var blacklist = new string[] { "password", "card.number", "*.card.number" "creditcardnumber" };
var mask = "******";

var maskedExampleAsString = exampleAsString.MaskFields(blacklist, mask);

Console.WriteLine(maskedExampleAsString);

```

Output
```json
{
	"SomeValue" : "Demo",
	"Password" : "******",
	"DepthObject" : {
		"Password" : "SomePasswordHere2",
		"Card" : {
			"Number" : "******"
		}
	},
	"CreditCardNumber" : "******",
	"Card" : {
		"Number" : "******"
	}
}
```

```c#

- Partially masking some of the properties

var example = new 
{
	SomeValue = "Demo",
	Password = "SomePasswordHere",
	DepthObject = new 
	{
		Password = "SomePasswordHere2",
		Card = new 
		{
			Number = "555500022223333"
		}
	},
	CreditCardNumber = "5555000011112222",
	Card = new 
	{
		Number = "555500022223333"
	}
};

var blacklistPartial = new Dictionary<string, Func<string, string>>(StringComparer.OrdinalIgnoreCase) // The key is the property to be partially masked and the value is the function to be applied.
{
    { "*card.number", text =>  // Note that the property "*card.number" is also in the blacklist. If the property only exists in the blacklistPartial, it will not be masked.
        Regex.Replace(
            text,
            @"(\d{4,5})[ -|]?(\d{3,6})[ -|]?(\d{3,5})[ -|]?(\d{3,4})",
            match => $"{match.Value.Substring(0, 6)}*****{match.Value.Substring(match.Value.Length - 4, 4)}")
    }
};

var exampleAsString = JsonConvert.Serialize(example); // The value must be a JSON string to be masked.

// Note that the password is only replaced when it is in the root path.
var blacklist = new string[] { "password", "*card.number", "creditcardnumber" };
var mask = "******";

var maskedExampleAsString = exampleAsString.MaskFields(blacklist, mask, blacklistPartial); // The blacklistPartial is optional. If provided, it will apply the mask only if the property is also in the blacklist.

Console.WriteLine(maskedExampleAsString);

```

Output
```json
{
	"SomeValue" : "Demo",
	"Password" : "******",
	"DepthObject" : {
		"Password" : "SomePasswordHere2",
		"Card" : {
			"Number" : "555500*****3333"
		}
	},
	"CreditCardNumber" : "******",
	"Card" : {
		"Number" : "555500*****3333"
	}
}
```

## Install via NuGet

```
PM> Install-Package JsonMasking
```

## How can I contribute?
Please, refer to [CONTRIBUTING](.github/CONTRIBUTING.md)

## Found something strange or need a new feature?
Open a new Issue following our issue template [ISSUE_TEMPLATE](.github/ISSUE_TEMPLATE.md)

## Changelog
See in [nuget version history](https://www.nuget.org/packages/JsonMasking)

## Did you like it? Please, make a donate :)

if you liked this project, please make a contribution and help to keep this and other initiatives, send me some Satochis.

BTC Wallet: `1G535x1rYdMo9CNdTGK3eG6XJddBHdaqfX`

![1G535x1rYdMo9CNdTGK3eG6XJddBHdaqfX](https://i.imgur.com/mN7ueoE.png)
