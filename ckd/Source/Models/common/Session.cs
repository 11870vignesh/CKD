namespace Models.common;

using System;

public class Session
{
    public Session(string loginId)
    {
        Id = Guid.NewGuid();
        LoginTime = DateTime.Now;
        LoginId = loginId;
    }

    public Guid Id { get; }

    public DateTime LoginTime { get; }

    public string LoginId { get; set; }

}