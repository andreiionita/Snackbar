namespace Snackbar.Snackbar;

public interface ISnackbarContainer: IDisposable
{
    Task ShowSnackbarAsync();
}