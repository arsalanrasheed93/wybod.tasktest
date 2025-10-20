using Wybod.TaskTest.Data.Models;

namespace Wybod.TaskTest.Data;

public interface IDataContext
{
    IList<TaskItem> Tasks { get; }
}

public class DataContext : IDataContext
{
    public IList<TaskItem> Tasks { get; } = new List<TaskItem>()
    {        
        new() { Id = Guid.Parse("550e8400-e29b-41d4-a716-446655440000"), Title = "Research hiking trails", Description = "Find and compare local hiking trails for weekend outdoor activity", IsCompleted = true, IsActive = true },
        new() { Id = Guid.Parse("a1b2c3d4-e5f6-4789-9123-456789abcdef"), Title = "Plan team lunch", Description = "Coordinate restaurant reservations for Friday team gathering", IsCompleted = true, IsActive = true},
        new() { Id = Guid.Parse("11111111-2222-3333-4444-555555555555"), Title = "Organize home office", Description = "Declutter desk and reorganize filing system for better productivity", IsCompleted = false, IsActive = true },
        new() { Id = Guid.Parse("66666666-7777-8888-9999-000000000000"), Title = "Book dentist appointment", Description = "Schedule routine dental checkup for next month", IsCompleted = false, IsActive = true },
        new() { Id = Guid.Parse("12345678-90ab-cdef-1234-567890abcdef"), Title = "Review monthly budget", Description = "Analyze spending patterns and adjust budget categories as needed", IsCompleted = false, IsActive = true },
        new() { Id = Guid.Parse("fedcba98-7654-3210-ffff-eeeeeeeeeeee"), Title = "Learn basic photography", Description = "Complete online photography course to improve vacation photos", IsCompleted = false, IsActive = true },
        new() { Id = Guid.Parse("abcdabcd-abcd-abcd-abcd-abcdabcdabcd"), Title = "Update personal website", Description = "Refresh portfolio and add recent project examples", IsCompleted = false, IsActive = true },
        new() { Id = Guid.Parse("99999999-aaaa-bbbb-cccc-dddddddddddd"), Title = "Prepare presentation slides", Description = "Create slides for upcoming community workshop on gardening", IsCompleted = false, IsActive = true },
        new() { Id = Guid.Parse("eeeeeeee-dddd-cccc-bbbb-aaaaaaaaaaaa"), Title = "Research vacation destinations", Description = "Compare travel options for summer holiday planning", IsCompleted = false, IsActive = true },
        new() { Id = Guid.Parse("10101010-2020-3030-4040-505050505050"), Title = "Clean out garage", Description = "Sort through storage items and donate unused equipment", IsCompleted = false, IsActive = true },
        new() { Id = Guid.Parse("abababab-cdcd-dede-efef-fefefefefefe"), Title = "Read industry newsletter", Description = "Catch up on monthly newsletter from professional association", IsCompleted = false, IsActive = true },
        new() { Id = Guid.Parse("0a0b0c0d-0e0f-1011-1213-141516171819"), Title = "Plan garden layout", Description = "Design vegetable garden arrangement for spring planting", IsCompleted = false, IsActive = true },
        new() { Id = Guid.Parse("1f2e3d4c-5b6a-7988-8776-665544332211"), Title = "Schedule car maintenance", Description = "Book oil change and tire rotation at local service center", IsCompleted = false, IsActive = true },
        new() { Id = Guid.Parse("aaaa1111-bbbb-cccc-dddd-585796587548"), Title = "Organize photo library", Description = "Sort and tag digital photos from recent events", IsCompleted = false, IsActive = true },
        new() { Id = Guid.Parse("deadbeef-dead-beef-dead-beefdeadbeef"), Title = "Practice piano pieces", Description = "Rehearse new songs for upcoming recital performance", IsCompleted = false, IsActive = true },
        new() { Id = Guid.Parse("cafebabe-cafe-babe-cafe-babecafebabe"), Title = "Update emergency contacts", Description = "Review and refresh contact information for family members", IsCompleted = false, IsActive = true },
        new() { Id = Guid.Parse("12121212-3434-5656-7878-909090909090"), Title = "Research cooking classes", Description = "Find local culinary workshops for Italian cuisine", IsCompleted = false, IsActive = true },
        new() { Id = Guid.Parse("31415926-5358-9793-2384-626433832795"), Title = "Backup computer files", Description = "Create backup of important documents to external drive", IsCompleted = false, IsActive = true },
        new() { Id = Guid.Parse("22222222-3333-4444-5555-666666666666"), Title = "Plan charity event", Description = "Coordinate details for neighborhood food drive next month", IsCompleted = false, IsActive = true },
        new() { Id = Guid.Parse("99998888-7777-6666-5555-444433332222"), Title = "Review insurance policies", Description = "Compare coverage options and renewal dates for home and auto", IsCompleted = false, IsActive = true },

    };
}