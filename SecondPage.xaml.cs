using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snackbar.Snackbar;

namespace Snackbar;

public partial class SecondPage : ContentPage, ISnackbarConsumer
{
    private ISnackbarContainer _snackbar = new SnackbarContainer();
    public ISnackbarContainer? Snackbar
    {
        get => _snackbar;
        set => _snackbar = value;
    }
    
    public SecondPage()
    {
        try
        {
            InitializeComponent();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
        
    }
}