using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SqlServer.Server;
using System.Data.SqlTypes;
using System.IO;

[Serializable]
[SqlUserDefinedAggregate(Format.UserDefined,
    IsNullIfEmpty = true,
    Name = "MEDIAN",
    IsInvariantToDuplicates = false,
    IsInvariantToOrder = true,
    IsInvariantToNulls = true,
    MaxByteSize = 8000)]
public class Median : IBinarySerialize
{
    private List<double> values;

    public void Init()
    {
        values = new List<double>();
    }

    public void Accumulate(SqlDouble num)
    {
        if (num.IsNull)
        {
            return;
        }

        this.values.Add(num.ToSqlDecimal().ToDouble());
    }

    public void Merge(Median other)
    {
        this.values.AddRange(other.values);
    }

    public SqlDouble Terminate()
    {
        this.values.Sort();

        if (values.Count % 2 == 1)
        {
            return new SqlDouble(values[values.Count / 2]);
        }
        else
        {
            return new SqlDouble((values[(values.Count / 2) - 1] + values[values.Count / 2]) / 2);
        }
    }

    public void Read(BinaryReader r)
    {
        String[] vals = r.ReadString().Split(new char[] { ',' });

        foreach (String value in vals)
        {
            double val;

            if (Double.TryParse(value,out val))
            {
                values.Add(val);
            }
        }
    }

    public void Write(BinaryWriter w)
    {
        String[] strValues = new string[values.Count];

        for (int i = 0; i < values.Count; i++)
        {
            strValues[i] = values.ToString();
        }
        
        w.Write(String.Join(",", strValues));
    }
}

[Serializable()]
[SqlUserDefinedAggregate(Format.UserDefined,
    IsNullIfEmpty = true,
    Name = "MEDIAN",
    IsInvariantToDuplicates = false,
    IsInvariantToOrder = true,
    IsInvariantToNulls = true,
    MaxByteSize = 8000)]
public class MedianAbsoluteDeviation : IBinarySerialize
{
    private List<double> values;

    public void Init()
    {
        values = new List<double>();
    }

    public void Accumulate(SqlDouble num)
    {
        if (num.IsNull)
        {
            return;
        }

        this.values.Add(num.ToSqlDecimal().ToDouble());
    }

    public void Merge(MedianAbsoluteDeviation other)
    {
        this.values.AddRange(other.values);
    }

    public SqlDouble Terminate()
    {
        this.values.Sort();
        double median;
        List<double> deviations = new List<double>();

        if (values.Count % 2 == 1)
        {
            median = values[(int)(values.Count / 2)];
        }
        else
        {
            median = (values[(values.Count / 2) - 1] + values[values.Count / 2]) / 2;
        }

        foreach (double value in values)
        {
            deviations.Add(Math.Abs(value - median));
        }
        
        if (deviations.Count % 2 == 1)
        {
            return new SqlDouble(deviations[(int)(deviations.Count / 2)]);
        }
        else
        {
            return new SqlDouble((deviations[(deviations.Count / 2) - 1] + deviations[deviations.Count / 2]) / 2);
        }
    }

    public void Read(BinaryReader r)
    {
        String[] vals = r.ReadString().Split(new char[] { ',' });

        foreach (String value in vals)
        {
            double val;

            if (Double.TryParse(value, out val))
            {
                values.Add(val);
            }
        }
    }

    public void Write(BinaryWriter w)
    {
        String[] strValues = new string[values.Count];

        for (int i = 0; i < values.Count; i++)
        {
            strValues[i] = values.ToString();
        }

        w.Write(String.Join(",", strValues));
    }
}