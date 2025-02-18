﻿using Xunit;
using Verify = CSharpVerifier<Xunit.Analyzers.CollectionDefinitionClassesMustBePublic>;

public class CollectionDefinitionClassesMustBePublicFixerTests
{
	[Theory]
	[InlineData("")]
	[InlineData("internal ")]
	public async void MakesClassPublic(string modifier)
	{
		var before = $@"
[Xunit.CollectionDefinition(""MyCollection"")]
{modifier}class [|CollectionDefinitionClass|] {{ }}";

		var after = @"
[Xunit.CollectionDefinition(""MyCollection"")]
public class CollectionDefinitionClass { }";

		await Verify.VerifyCodeFixAsyncV2(before, after);
	}

	[Theory]
	[InlineData("")]
	[InlineData("internal ")]
	public async void ForPartialClassDeclarations_MakesSingleDeclarationPublic(string modifier)
	{
		var before = $@"
[Xunit.CollectionDefinition(""MyCollection"")]
{modifier}partial class [|CollectionDefinitionClass|] {{ }}

partial class CollectionDefinitionClass {{ }}";

		var after = @"
[Xunit.CollectionDefinition(""MyCollection"")]
public partial class CollectionDefinitionClass { }

partial class CollectionDefinitionClass { }";

		await Verify.VerifyCodeFixAsyncV2(before, after);
	}
}
