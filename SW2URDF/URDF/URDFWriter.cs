﻿using System.Text;
using System.Xml;

namespace SW2URDF.URDF;

//Initiates the XMLWriter and its necessary settings
public class URDFWriter
{
    public XmlWriter writer;

    public URDFWriter(string savePath)
    {
        XmlWriterSettings settings = new XmlWriterSettings
        {
            Encoding = new UTF8Encoding(false),
            Indent = true,
            NewLineOnAttributes = true,
        };
        writer = XmlWriter.Create(savePath, settings);
    }
}
