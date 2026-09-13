using BabyTracker.Models;
using BabyTracker.Services;

namespace BabyTracker.Views;

public partial class HistoryPage : ContentPage
{
    private readonly DatabaseService _database;
    private RecordType? _currentFilter;

    public HistoryPage(DatabaseService database)
    {
        InitializeComponent();
        _database = database;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        LoadRecords();
    }

    private void LoadRecords()
    {
        List<BabyRecord> records;

        if (_currentFilter.HasValue)
        {
            records = _database.GetRecordsByType(_currentFilter.Value);
        }
        else
        {
            records = _database.GetAllRecords();
        }

        RecordsCollectionView.ItemsSource = records;
    }

    private void UpdateFilterButtons()
    {
        AllFilterButton.Style = _currentFilter == null
            ? (Style)Resources["PrimaryButton"]
            : (Style)Resources["ButtonBase"];

        AllFilterButton.BackgroundColor = _currentFilter == null
            ? (Color)Resources["PrimaryColor"]
            : (Color)Resources["SurfaceColor"];

        AllFilterButton.TextColor = _currentFilter == null
            ? Colors.White
            : (Color)Resources["TextColor"];

        WakeupsFilterButton.Style = _currentFilter == RecordType.Wakeup
            ? (Style)Resources["PrimaryButton"]
            : (Style)Resources["ButtonBase"];

        WakeupsFilterButton.BackgroundColor = _currentFilter == RecordType.Wakeup
            ? (Color)Resources["PrimaryColor"]
            : (Color)Resources["SurfaceColor"];

        WakeupsFilterButton.TextColor = _currentFilter == RecordType.Wakeup
            ? Colors.White
            : (Color)Resources["TextColor"];

        FeedingsFilterButton.Style = _currentFilter == RecordType.Feeding
            ? (Style)Resources["PrimaryButton"]
            : (Style)Resources["ButtonBase"];

        FeedingsFilterButton.BackgroundColor = _currentFilter == RecordType.Feeding
            ? (Color)Resources["PrimaryColor"]
            : (Color)Resources["SurfaceColor"];

        FeedingsFilterButton.TextColor = _currentFilter == RecordType.Feeding
            ? Colors.White
            : (Color)Resources["TextColor"];
    }

    private async void OnFilterClicked(object? sender, EventArgs e)
    {
        if (sender == AllFilterButton)
            _currentFilter = null;
        else if (sender == WakeupsFilterButton)
            _currentFilter = RecordType.Wakeup;
        else if (sender == FeedingsFilterButton)
            _currentFilter = RecordType.Feeding;

        UpdateFilterButtons();
        LoadRecords();
    }

    private async void OnDeleteSwiped(object? sender, EventArgs e)
    {
        if (sender is SwipeItem swipeItem && swipeItem.CommandParameter is int id)
        {
            bool answer = await DisplayAlert("Delete Record", "Are you sure you want to delete this record?", "Delete", "Cancel");
            if (answer)
            {
                _database.DeleteRecord(id);
                LoadRecords();
            }
        }
    }
}
