using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JewelryAuctionApplication.ViewModels;

public class ErrorsViewModel : INotifyDataErrorInfo
{
    private readonly Dictionary<string, List<string>> _propertyErrors = new Dictionary<string, List<string>>();
    public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;
    public bool HasErrors => _propertyErrors.Any();
    public IEnumerable GetErrors(string propertyName)
    {
        return _propertyErrors.GetValueOrDefault(propertyName, new List<string>());
    }
    public void AddError(string errorMessage, string propertyName)
    {
        if (!_propertyErrors.ContainsKey(propertyName))
        {
            _propertyErrors.Add(propertyName, new List<string>());
        }
        _propertyErrors[propertyName].Add(errorMessage);
        OnErrorsChanged(nameof(propertyName));
    }

    private void OnErrorsChanged(string propertyName)
    {
        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(propertyName)));
    }

    public void ClearErrors(string propertyName)
    {
        if (_propertyErrors.Remove(nameof(propertyName)))
        {
            OnErrorsChanged(nameof(propertyName)); //make sure that the change should be notified accordingly
        }
    }
}
