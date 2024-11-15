using Microsoft.AspNetCore.SignalR;

namespace StudentAttendace.Hubs;

public class AttendanceHub() : Hub
{
    public async Task JoinLectureGroup(int lectureId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, lectureId.ToString());
    }
}