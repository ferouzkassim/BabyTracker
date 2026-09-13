using BabyTracker.Models;
using BabyTracker.Services;

namespace BabyTracker.Views;

public partial class MainPage : ContentPage
{
    private readonly DatabaseService _database;
    private System.Timers.Timer? _timer;

    public MainPage(DatabaseService database)
    {
        InitializeComponent();
        _database = database;
        _database.Initialize();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        UpdateCurrentTime();
        UpdateTodaySummary();
        UpdateLastActivity();

        _timer = new System.Timers.Timer(1000);
        _timer.Elapsed += (s, e) => MainThread.BeginInvokeOnMainThread(UpdateCurrentTime);
        _timer.Start();
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        _timer?.Stop();
        _timer?.Dispose();
        _timer = null;
    }

    private void UpdateCurrentTime()
    {
        CurrentTimeLabel.Text = DateTime.Now.ToString("hh:mm:ss tt");
    }

    private void UpdateTodaySummary()
    {
        var todayRecords = _database.GetTodayRecords();
        var wakeups = todayRecords.Count(r => r.Type == RecordType.Wakeup);
        var feedings = todayRecords.Count(r => r.Type == RecordType.Feeding);

        WakeupsTodayCount.Text = wakeups.ToString();
        FeedingsTodayCount.Text = feedings.ToString();
    }

    private void UpdateLastActivity()
    {
        var allRecords = _database.GetAllRecords();
        var lastRecord = allRecords.FirstOrDefault();

        if (lastRecord != null)
        {
            var icon = lastRecord.Type == RecordType.Wakeup ? "☀️" : "🍼";
            var type = lastRecord.Type.ToString();
            var time = lastRecord.Timestamp.ToString("hh:mm tt");
            var ago = GetTimeAgo(lastRecord.Timestamp);
            LastActivityLabel.Text = $"{icon} {type} at {time} ({ago})";
        }
        else
        {
            LastActivityLabel.Text = "No activities recorded yet";
        }
    }

    private string GetTimeAgo(DateTime timestamp)
    {
        var span = DateTime.Now - timestamp;

        if (span.TotalMinutes < 1)
            return "just now";
        if (span.TotalMinutes < 60)
            return $"{(int)span.TotalMinutes} min ago";
        if (span.TotalHours < 24)
            return $"{(int)span.TotalHours} hr ago";
        return $"{(int)span.TotalDays} days ago";
    }

    private async void OnRecordWakeupClicked(object? sender, EventArgs e)
    {
        var record = new BabyRecord
        {
            Type = RecordType.Wakeup,
            Timestamp = DateTime.Now
        };

        _database.SaveRecord(record);

        // Button animation feedback
        await RecordWakeupButton.ScaleTo(0.95, 100);
        await RecordWakeupButton.ScaleTo(1, 100);

        UpdateTodaySummary();
        UpdateLastActivity();

        await DisplayAlert("Recorded! ☀️", $"Wake-up time recorded at {record.Timestamp:hh:mm tt}", "OK");
    }

    private async void OnRecordFeedingClicked(object? sender, EventArgs e)
    {
        var record = new BabyRecord
        {
            Type = RecordType.Feeding,
            Timestamp = DateTime.Now
        };

        _database.SaveRecord(record);

        // Button animation feedback
        await RecordFeedingButton.ScaleTo(0.95, 100);
        await RecordFeedingButton.ScaleTo(1, 100);

        UpdateTodaySummary();
        UpdateLastActivity();

        await DisplayAlert("Recorded! 🍼", $"Feeding time recorded at {record.Timestamp:hh:mm tt}", "OK");
    }

    private async void OnViewHistoryClicked(object? sender, EventArgs e)
    {
        await Navigation.PushAsync(new HistoryPage(_database));
    }
}
