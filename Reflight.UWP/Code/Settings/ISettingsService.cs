namespace ParrotDiscoReflight.Code.Settings
{
    public interface ISettingsService
    {
        string VideoFolder { get; set; }
        string Username { get; set; }
        string Password { get; set; }
    }
}