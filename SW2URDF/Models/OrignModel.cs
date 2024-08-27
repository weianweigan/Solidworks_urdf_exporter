using CommunityToolkit.Mvvm.ComponentModel;

namespace SW2URDF.Models;

public sealed partial class OrignModel : ObservableObject
{
    [ObservableProperty]
    private double _x;

    [ObservableProperty]
    private double _y;

    [ObservableProperty]
    private double _z;

    [ObservableProperty]
    private double _roll;

    [ObservableProperty]
    private double _pitch;

    [ObservableProperty]
    private double _yaw;
}

public sealed partial class MomentOfInertial : ObservableObject
{
    [ObservableProperty]
    private double _ixx;

    [ObservableProperty]
    private double _ixy;

    [ObservableProperty]
    private double _iyy;

    [ObservableProperty]
    private double _ixz;

    [ObservableProperty]
    private double _iyz;

    [ObservableProperty]
    private double _izz;
}
