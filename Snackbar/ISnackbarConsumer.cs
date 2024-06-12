namespace Snackbar.Snackbar;

public interface ISnackbarConsumer
{
    ISnackbarContainer? Snackbar { get; set; }
}