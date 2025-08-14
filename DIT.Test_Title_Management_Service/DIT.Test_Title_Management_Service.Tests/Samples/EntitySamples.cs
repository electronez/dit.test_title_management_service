namespace DIT.Test_Title_Management_Service.Tests.Samples;

using DIT.Test_Title_Management_Service.Domain.Enums;
using DIT.Test_Title_Management_Service.Domain.Titles;
using DIT.Test_Title_Management_Service.Domain.Workers;

public static class EntitySamples
{
    public static Title CreateTitle()
    {
        var titleName = new TitleName(
            $"OriginalName{Guid.NewGuid()}",
            $"EnglishName{Guid.NewGuid()}",
            $"LocalizedName{Guid.NewGuid()}");

        var title = Title.Create(titleName, $"Description{Guid.NewGuid()}");
        title.AddChapter(1, $"Name{Guid.NewGuid()}");
        title.AddChapter(2, $"Name{Guid.NewGuid()}");
        title.AddChapter(3, $"Name{Guid.NewGuid()}");

        return title;
    }

    public static Worker CreateWorker()
    {
        var profile = new Profile($"FirstName{Guid.NewGuid()}", $"LastName{Guid.NewGuid()}");
        var worker = Worker.Create($"username{Guid.NewGuid()}", profile, [WorkerRole.Translator]);
        return worker;
    }
}