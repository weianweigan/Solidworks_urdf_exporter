using CommunityToolkit.Mvvm.ComponentModel;
using SolidWorks.Interop.sldworks;

namespace SW2URDF.ViewModels;

public sealed partial class PartExporterViewModel : ObservableObject
{
    public PartExporterViewModel(ISldWorks app) { }
}
