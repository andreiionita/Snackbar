using Microsoft.Maui.Controls.Compatibility;
namespace Snackbar.Snackbar;

[ContentProperty(nameof(Content))]
public class SnackbarContainer : Layout<View>, ISnackbarContainer
{
    public SnackbarContainer()
    {
        _snackbarPlaceholder = new ContentView();
        Children.Add(_snackbarPlaceholder);
    }
    
    private static void OnContentChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var wrapper = (SnackbarContainer)bindable;
        if (oldValue is not null)
        {
            wrapper.Children.Remove((View)oldValue);
        }

        if (newValue is not null)
        {
            wrapper.Children.Insert(0, (View)newValue);
        }
    }

    protected override void OnBindingContextChanged()
    {
        base.OnBindingContextChanged();

        if (BindingContext is ISnackbarConsumer consumer)
        {
            consumer.Snackbar = this;
        }
    }

    public static readonly BindableProperty SnackbarMarginProperty = BindableProperty.Create(nameof(SnackbarMargin), typeof(Thickness), typeof(SnackbarContainer), new Thickness(16.0));
    public static readonly BindableProperty BottomOffsetProperty = BindableProperty.Create(nameof(BottomOffset), typeof(double), typeof(SnackbarContainer), 0.0);
    public static readonly BindableProperty ContentProperty = BindableProperty.Create(nameof(Content), typeof(View), typeof(SnackbarContainer), propertyChanged: OnContentChanged);

    private readonly CancellationTokenSource _cancellationTokenSource = new();
    private readonly ContentView _snackbarPlaceholder;

    public View Content
    {
        get => (View)GetValue(ContentProperty);
        set => SetValue(ContentProperty, value);
    }

    public Thickness SnackbarMargin
    {
        get => (Thickness)GetValue(SnackbarMarginProperty);
        set => SetValue(SnackbarMarginProperty, value);
    }

    public double BottomOffset
    {
        get => (double)GetValue(BottomOffsetProperty);
        set => SetValue(BottomOffsetProperty, value);
    }
    
    public Task ShowSnackbarAsync() => Task.CompletedTask;

    public void Dispose()
    {
        _cancellationTokenSource.Cancel();
        _cancellationTokenSource.Dispose();
    }

    protected override void LayoutChildren(double x, double y, double width, double height)
    {
        if (Content is not null)
        {
            LayoutChildIntoBoundingRegion(Content, new RectF(Convert.ToSingle(x),
                                                             Convert.ToSingle(y),
                                                             Convert.ToSingle(width),
                                                             Convert.ToSingle(height)));
        }

        LayoutSnackbar(x, y, width, height);
    }

    private void LayoutSnackbar(double x, double y, double width, double height)
    {
        var snackbarSize = _snackbarPlaceholder.Measure(width, height).Request;

        var snackbarPosition = new Point(x + (width - snackbarSize.Width) / 2,
                                         y + height - snackbarSize.Height - BottomOffset);
        LayoutChildIntoBoundingRegion(_snackbarPlaceholder, new RectF(snackbarPosition, snackbarSize));
    }
}