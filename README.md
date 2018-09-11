[![Codacy Badge](https://api.codacy.com/project/badge/Grade/c1d0f1f96bdd4bf8b798367b9241b962)](https://www.codacy.com/app/ThiagoBarradas/newtonsoft-extensions-jsonmasking?utm_source=github.com&amp;utm_medium=referral&amp;utm_content=ThiagoBarradas/newtonsoft-extensions-jsonmasking&amp;utm_campaign=Badge_Grade)
[![Build status](https://ci.appveyor.com/api/projects/status/tyep2lwnuk9k1oxx/branch/master?svg=true)](https://ci.appveyor.com/project/ThiagoBarradas/newtonsoft-extensions-jsonmasking/branch/master)
[![codecov](https://codecov.io/gh/ThiagoBarradas/newtonsoft-extensions-jsonmasking/branch/master/graph/badge.svg)](https://codecov.io/gh/ThiagoBarradas/newtonsoft-extensions-jsonmasking)
[![NuGet Downloads](https://img.shields.io/nuget/dt/Newtonsoft.Extensions.JsonMasking.svg)](https://www.nuget.org/packages/Newtonsoft.Extensions.JsonMasking/)
[![NuGet Version](https://img.shields.io/nuget/v/Newtonsoft.Extensions.JsonMasking.svg)](https://www.nuget.org/packages/Newtonsoft.Extensions.JsonMasking/)

# Json Masking Extension for Newntonsoft

Replace fields in json, replacing by something, don't care if property is in depth objects. Very useful to replace passwords credit card number, etc.

# Sample

```c#

var example = new 
{
	SomeValue = "Demo",
	Password = "SomePasswordHere",
	DepthObject = new 
	{
		Password = "SomePasswordHere2"
	},
	"CreditCardNumber" : "5555000011112222"
};

var exampleAsString = JsonConvert.Serialize(example); // value must be a json string to masked
var blacklist = new string[] { "password", "creditcardnumber" }; // insensitive
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
		"Password" : "******"
	},
	"CreditCardNumber" : "******"
}
```

## Install via NuGet

```
PM> Install-Package Newtonsoft.Extensions.JsonMasking
```

## How can I contribute?
Please, refer to [CONTRIBUTING](.github/CONTRIBUTING.md)

## Found something strange or need a new feature?
Open a new Issue following our issue template [ISSUE_TEMPLATE](.github/ISSUE_TEMPLATE.md)

## Changelog
See in [nuget version history](https://www.nuget.org/packages/Newtonsoft.Extensions.JsonMasking)

## Did you like it? Please, make a donate :)

if you liked this project, please make a contribution and help to keep this and other initiatives, send me some Satochis.

BTC Wallet: `1G535x1rYdMo9CNdTGK3eG6XJddBHdaqfX`

![1G535x1rYdMo9CNdTGK3eG6XJddBHdaqfX](https://i.imgur.com/mN7ueoE.png)
