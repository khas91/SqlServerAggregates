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
    MaxByteSize = -1)]
public class Median : IBinarySerialize
{
    private List<double> values;

    public void Init()
    {
        values = new List<double>();
    }

    public void Accumulate(SqlDouble num)
    {
        if (!num.IsNull)
        {
            this.values.Add(num.Value);
        }
    }

    public void Merge(Median other)
    {
        this.values.AddRange(other.values);
    }

    public SqlDouble Terminate()
    {
        
        if (values.Count == 0)
        {
            return SqlDouble.Null;
        }

        values.Sort();

        int index = values.Count / 2;

        if (values.Count % 2 == 1)
        {
            return values[index];
        }
        else
        {
            return (double)(values[index] + values[index - 1]) / 2.0d;
        }
    }

    public void Read(BinaryReader r)
    {
        Init();

        int numValues = r.ReadInt32();

        for (int i = 0; i < numValues; i++)
        {
            values.Add(r.ReadDouble());
        }
    }

    public void Write(BinaryWriter w)
    {
        w.Write(values.Count);

        foreach (double value in values)
        {
            w.Write(value);
        }
    }
}

[Serializable()]
[SqlUserDefinedAggregate(Format.UserDefined,
    IsNullIfEmpty = true,
    Name = "MAD",
    IsInvariantToDuplicates = false,
    IsInvariantToOrder = true,
    IsInvariantToNulls = true,
    MaxByteSize = -1)]
public class MedianAbsoluteDeviation : IBinarySerialize
{
    private List<double> values;

    public void Init()
    {
        values = new List<double>();
    }

    public void Accumulate(SqlDouble num)
    {
        if (!num.IsNull)
        {
            this.values.Add(num.Value);
        }
    }

    public void Merge(MedianAbsoluteDeviation other)
    {
        this.values.AddRange(other.values);
    }

    public SqlDouble Terminate()
    {
        if (values.Count == 0)
        {
            return SqlDouble.Null;
        }

        this.values.Sort();
        double median;
        int index = values.Count / 2;
        List<double> deviations = new List<double>();

        if (values.Count % 2 == 1)
        {
            median = values[index];
        }
        else
        {
            median = (values[index - 1] + values[index]) / 2.0d;
        }

        foreach (double value in values)
        {
            deviations.Add(Math.Abs(value - median));
        }
   
        index = deviations.Count / 2;

        deviations.Sort();

        if (deviations.Count % 2 == 1)
        {
            return (double)deviations[index];
        }
        else
        {
            return (double)(deviations[index - 1] + deviations[index]) / 2;
        }
    }

    public void Read(BinaryReader r)
    {
        Init();

        int numValues = r.ReadInt32();

        for (int i = 0; i < numValues; i++)
        {
            values.Add(r.ReadDouble());
        }
    }

    public void Write(BinaryWriter w)
    {
        w.Write(values.Count);

        foreach (double value in values)
        {
            w.Write(value);
        }
    }
}