﻿using Xunit;

namespace SW2URDF.Test;

[Collection("Requires SW Test Collection")]
public class TestSwAttached : SW2URDFTest
{
    public TestSwAttached(SWTestFixture fixture)
        : base(fixture) { }

    [Fact]
    public void TestSWOpens()
    {
        Assert.NotNull(SwApp);
    }

    [Theory]
    [InlineData("3_DOF_ARM")]
    [InlineData("4_WHEELER")]
    [InlineData("ORIGINAL_3_DOF_ARM")]
    public void TestModelDocOpens(string modelName)
    {
        OpenSWDocument(modelName);
        Assert.True(SwApp.CloseAllDocuments(true));
    }
}
