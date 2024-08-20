using System.Collections.Generic;
using System.Collections.Specialized;
using System.Runtime.Serialization;
using System.Windows.Forms;

namespace SW2URDF.URDF;

[DataContract(IsReference = true, Namespace = "http://schemas.datacontract.org/2004/07/SW2URDF")]
public class Axis : URDFElement
{
    [DataMember]
    private readonly URDFAttribute XYZAttribute;

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
        XYZ = (double[])xyz.Clone();
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

    public Axis()
        : base("axis", false)
    {
        XYZAttribute = new URDFAttribute("xyz", true, new double[] { 0, 0, 0 });

        Attributes.Add(XYZAttribute);
    }

    public void FillBoxes(TextBox boxX, TextBox boxY, TextBox boxZ, string format)
    {
        string[] xyzText = XYZAttribute.GetTextArrayFromDoubleArray(format);
        if (xyzText != null)
        {
            boxX.Text = xyzText[0];
            boxY.Text = xyzText[1];
            boxZ.Text = xyzText[2];
        }
    }

    public void Update(TextBox boxX, TextBox boxY, TextBox boxZ)
    {
        XYZAttribute.SetDoubleArrayFromStringArray(
            new string[] { boxX.Text, boxY.Text, boxZ.Text }
        );
    }

    /// <summary>
    /// Axis is similar to Origin in that its attribute is stored as a double array.
    /// </summary>
    /// <param name="context"></param>
    /// <param name="dictionary"></param>
    public override void SetElementFromData(List<string> context, StringDictionary dictionary)
    {
        string typeName = GetType().Name;
        List<string> updatedContext = new List<string>(context) { typeName };

        double[] xyz = new double[3];

        string contextString = string.Join(".", updatedContext) + ".xyz";
        for (int i = 0; i < xyz.Length; i++)
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

        XYZAttribute.Value = xyz;
    }
}
