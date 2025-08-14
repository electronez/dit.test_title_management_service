namespace DIT.Test_Title_Management_Service.Tests.Infrastructure;

[CollectionDefinition(CollectionName)]
public class WebApplicationCollection : ICollectionFixture<TestWebApplication>
{
    public const string CollectionName = "DIT.Test_Title_Management_Service.Tests";
}