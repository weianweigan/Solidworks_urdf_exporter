using System.Collections.Generic;
using System.Collections.Specialized;
using System.Runtime.Serialization;
using System.Windows.Forms;

namespace SW2URDF.URDF;

[DataContract(Name = "Origin", Namespace = "http://schemas.datacontract.org/2004/07/SW2URDF")]
public class Origin : URDFElement
{
    [DataMember]
    private readonly URDFAttribute XYZAttribute;

    [DataMember]
    private readonly URDFAttribute RPYAttribute;

    private double[] XYZ
    {
        get => (double[])XYZAttribute.Value;
        set => XYZAttribute.Value = value;
    }

    public double[] GetXYZ()
    {
        return (double[])XYZ.Clone();
    }

    public void SetXYZ(double[] xyz)
    {
        XYZ = xyz;
    }

    public double X
    {
        get => XYZ[0];
        set => XYZ[0] = value;
    }

    public double Y
    {
        get => XYZ[1];
        set => XYZ[1] = value;
    }

    public double Z
    {
        get => XYZ[2];
        set => XYZ[2] = value;
    }

    private double[] RPY
    {
        get => (double[])RPYAttribute.Value;
        set => RPYAttribute.Value = value;
    }

    public double[] GetRPY()
    {
        return (double[])RPY.Clone();
    }

    public void SetRPY(double[] rpy)
    {
        RPY = rpy;
    }

    public double Roll
    {
        get => RPY[0];
        set => RPY[0] = value;
    }

    public double Pitch
    {
        get => RPY[1];
        set => RPY[1] = value;
    }

    public double Yaw
    {
        get => RPY[2];
        set => RPY[2] = value;
    }

    [DataMember]
    public bool isCustomized;

    public Origin(bool isRequired)
        : base("origin", isRequired)
    {
        isCustomized = false;

        XYZAttribute = new URDFAttribute("xyz", true, new double[] { 0, 0, 0 });
        RPYAttribute = new URDFAttribute("rpy", true, new double[] { 0, 0, 0 });

        Attributes.Add(XYZAttribute);
        Attributes.Add(RPYAttribute);
    }

    public void FillBoxes(
        TextBox boxX,
        TextBox boxY,
        TextBox boxZ,
        TextBox boxRoll,
        TextBox boxPitch,
        TextBox boxYaw,
        string format
    )
    {
        string[] xyzText = XYZAttribute.GetTextArrayFromDoubleArray(format);
        if (xyzText != null)
        {
            boxX.Text = xyzText[0];
            boxY.Text = xyzText[1];
            boxZ.Text = xyzText[2];
        }

        string[] rpyText = RPYAttribute.GetTextArrayFromDoubleArray(format);
        if (rpyText != null)
        {
            boxRoll.Text = rpyText[0];
            boxPitch.Text = rpyText[1];
            boxYaw.Text = rpyText[2];
        }
    }

    public void Update(
        TextBox boxX,
        TextBox boxY,
        TextBox boxZ,
        TextBox boxRoll,
        TextBox boxPitch,
        TextBox boxYaw
    )
    {
        XYZAttribute.SetDoubleArrayFromStringArray(
            new string[] { boxX.Text, boxY.Text, boxZ.Text }
        );
        RPYAttribute.SetDoubleArrayFromStringArray(
            new string[] { boxRoll.Text, boxPitch.Text, boxYaw.Text }
        );
    }

    /// <summary>
    /// Origin is a unique case in that its attributes are stored as double arrays.
    /// </summary>
    /// <param name="context"></param>
    /// <param name="dictionary"></param>
    public override void SetElementFromData(List<string> context, StringDictionary dictionary)
    {
        string typeName = GetType().Name;
        List<string> updatedContext = new List<string>(context) { typeName };

        double[] xyz = new double[3];
        double[] rpy = new double[3];

        string contextString = string.Join(".", updatedContext) + ".xyz";
        for (int i = 0; i < 3; i++)
        {
            string lookupString = contextString + "." + "xyz"[i];
            if (!dictionary.ContainsKey(lookupString))
            {
                logger.Info("CSV file does not contain column for " + lookupString);
                continue;
            }

            object value = URDFAttribute.GetValueFromString(dictionary[lookupString]);
            if (value != null && value.GetType() == typeof(double))
            {
                xyz[i] = (double)value;
            }
        }

        contextString = string.Join(".", updatedContext) + ".rpy";
        for (int i = 0; i < 3; i++)
        {
            string lookupString = contextString + "." + "rpy"[i];
            if (!dictionary.ContainsKey(lookupString))
            {
                logger.Info("CSV file does not contain column for " + lookupString);
                continue;
            }

            object value = URDFAttribute.GetValueFromString(dictionary[lookupString]);
            if (value != null && value.GetType() == typeof(double))
            {
                rpy[i] = (double)value;
            }
        }

        XYZAttribute.Value = xyz;
        RPYAttribute.Value = rpy;
    }
}
