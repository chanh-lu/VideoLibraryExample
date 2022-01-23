using Microsoft.Extensions.Configuration;
using VideoLibraryExample.Commons;

namespace VideoLibraryExample.Tests.DataSources;

public class BookmarksDataSourceTests
{
    private const string TargetBookmarkName = "bookmark bar child name";
    private const string ErroMessageFailedToDeserialize = $"Failed to deserialize json string to object of type '{nameof(Bookmark)}'";
    private const string ErrorMessageTargetBookmarkNotExists = $"Cannot find bookmark 'folder' named '{TargetBookmarkName}'";

    private readonly Mock<IFileWrapper> _mockFileWrapper;
    private readonly BookmarksDataSource _subject;

    public BookmarksDataSourceTests()
    {
        _mockFileWrapper = new Mock<IFileWrapper>();

        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string>
            {
                {"TargetBookmarkName", TargetBookmarkName},
                {"TargetBookmarkFile", "C:\\User"}
            })
            .Build();

        _subject = new BookmarksDataSource(configuration, _mockFileWrapper.Object);
    }

    [Fact]
    public void Get_Should_Throw_Exception_When_Bookmark_Has_No_BookmarkRoot()
    {
        var jsonString = $@"{{}}";

        _mockFileWrapper.Setup(x => x.ReadAllText(It.IsAny<string>())).Returns(jsonString);

        Action act = () => _subject.Get();

        act.Should().Throw<Exception>().WithMessage(ErroMessageFailedToDeserialize);
    }

    [Fact]
    public void Get_Should_Throw_Exception_When_BookmarkRoot_Has_No_BookmarkBar()
    {
        var jsonString = $@"{{
            ""roots"": {{}}
        }}";

        _mockFileWrapper.Setup(x => x.ReadAllText(It.IsAny<string>())).Returns(jsonString);

        Action act = () => _subject.Get();

        act.Should().Throw<Exception>().WithMessage(ErroMessageFailedToDeserialize);
    }

    [Fact]
    public void Get_Should_Throw_Exception_When_BookmarkBar_Has_No_Children()
    {
        var jsonString = $@"{{
            ""roots"": {{
                ""bookmark_bar"": {{}}
            }}
        }}";

        _mockFileWrapper.Setup(x => x.ReadAllText(It.IsAny<string>())).Returns(jsonString);

        Action act = () => _subject.Get();

        act.Should().Throw<Exception>().WithMessage(ErrorMessageTargetBookmarkNotExists);
    }

    [Fact]
    public void Get_Should_Throw_Exception_When_BookmarkBar_Has_Empty_Children()
    {
        var jsonString = $@"{{
            ""roots"": {{
                ""bookmark_bar"": {{
                    ""children"": []
                }}
            }}
        }}";

        _mockFileWrapper.Setup(x => x.ReadAllText(It.IsAny<string>())).Returns(jsonString);

        Action act = () => _subject.Get();

        act.Should().Throw<Exception>().WithMessage(ErrorMessageTargetBookmarkNotExists);
    }

    [Fact]
    public void Get_Should_Throw_Exception_When_Cannot_Find_TargetBookmark_Name()
    {
        var jsonString = $@"{{
            ""roots"": {{
                ""bookmark_bar"": {{
                    ""children"": [
                        {{
                            ""name"": ""Some other name"",
                            ""type"": ""folder"",
                            ""children"": []
                        }}
                    ]
                }}
            }}
        }}";

        _mockFileWrapper.Setup(x => x.ReadAllText(It.IsAny<string>())).Returns(jsonString);

        Action act = () => _subject.Get();

        act.Should().Throw<Exception>().WithMessage(ErrorMessageTargetBookmarkNotExists);
    }

    [Fact]
    public void Get_Should_Throw_Exception_When_Cannot_Find_TargetBookmark_Type()
    {
        var jsonString = $@"{{
            ""roots"": {{
                ""bookmark_bar"": {{
                    ""children"": [
                        {{
                            ""name"": ""{TargetBookmarkName}"",
                            ""type"": ""some other type"",
                            ""children"": []
                        }}
                    ]
                }}
            }}
        }}";

        _mockFileWrapper.Setup(x => x.ReadAllText(It.IsAny<string>())).Returns(jsonString);

        Action act = () => _subject.Get();

        act.Should().Throw<Exception>().WithMessage(ErrorMessageTargetBookmarkNotExists);
    }

    [Fact]
    public void Get_Should_Return_Empty_Dictionary_When_TargetBookmarkName_Has_No_Children_Property()
    {
        var jsonString = $@"{{
            ""roots"": {{
                ""bookmark_bar"": {{
                    ""children"": [
                        {{
                            ""name"": ""{TargetBookmarkName}"",
                            ""type"": ""folder""
                        }}
                    ]
                }}
            }}
        }}";

        _mockFileWrapper.Setup(x => x.ReadAllText(It.IsAny<string>())).Returns(jsonString);

        var result = _subject.Get();

        result.Should().BeOfType<Dictionary<string, string>>();
        result.Should().BeEmpty();
    }

    [Fact]
    public void Get_Should_Return_Empty_Dictionary_When_TargetBookmarkName_Has_Empty_Children()
    {
        var jsonString = $@"{{
            ""roots"": {{
                ""bookmark_bar"": {{
                    ""children"": [
                        {{
                            ""name"": ""{TargetBookmarkName}"",
                            ""type"": ""folder"",
                            ""children"": []
                        }}
                    ]
                }}
            }}
        }}";

        _mockFileWrapper.Setup(x => x.ReadAllText(It.IsAny<string>())).Returns(jsonString);

        var result = _subject.Get();

        result.Should().BeOfType<Dictionary<string, string>>();
        result.Should().BeEmpty();
    }

    [Fact]
    public void Get_Should_Return_Empty_Dictionary_When_TargetBookmarkName_Child_Has_Empty_Name_Or_Url()
    {
        var jsonString = $@"{{
            ""roots"": {{
                ""bookmark_bar"": {{
                    ""children"": [
                        {{
                            ""name"": ""{TargetBookmarkName}"",
                            ""type"": ""folder"",
                            ""children"": [
                                {{
                                    ""name"": """",
                                    ""url"": ""not empty url""
                                }},
                                {{
                                    ""name"": ""not empty name"",
                                    ""url"": """"
                                }}
                            ]
                        }}
                    ]
                }}
            }}
        }}";

        _mockFileWrapper.Setup(x => x.ReadAllText(It.IsAny<string>())).Returns(jsonString);

        var result = _subject.Get();

        result.Should().BeOfType<Dictionary<string, string>>();
        result.Should().BeEmpty();
    }

    [Fact]
    public void Get_Should_Return_Non_Empty_Dictionary()
    {
        const string TargetBookmarkChildName = "bookmark bar child's child name";

        var jsonString = $@"{{
            ""roots"": {{
                ""bookmark_bar"": {{
                    ""children"": [
                        {{
                            ""name"": ""{TargetBookmarkName}"",
                            ""type"": ""folder"",
                            ""children"": [
                                {{
                                    ""name"": ""{TargetBookmarkChildName}"",
                                    ""url"": ""https://example.com""
                                }}
                            ]
                        }}
                    ]
                }}
            }},
            ""extra_prop_ignored"": -1
        }}";

        _mockFileWrapper.Setup(x => x.ReadAllText(It.IsAny<string>())).Returns(jsonString);

        var result = _subject.Get();

        result.Should().BeOfType<Dictionary<string, string>>();
        result.Should().HaveCount(1);
        result.Should().ContainKey(TargetBookmarkChildName);
        result[TargetBookmarkChildName].Should().Be("https://example.com");
    }
}