namespace LegadoCultural.Services;

public static class CsvParser
{
    public static string[] ParseLine(string line)
    {
        var fields = new List<string>();
        int i = 0;
        while (i < line.Length)
        {
            if (line[i] == '"')
            {
                i++;
                var sb = new System.Text.StringBuilder();
                while (i < line.Length)
                {
                    if (line[i] == '"' && i + 1 < line.Length && line[i + 1] == '"')
                    { sb.Append('"'); i += 2; }
                    else if (line[i] == '"')
                    { i++; break; }
                    else
                    { sb.Append(line[i++]); }
                }
                fields.Add(sb.ToString());
                if (i < line.Length && line[i] == ',') i++;
            }
            else
            {
                int end = line.IndexOf(',', i);
                if (end == -1) { fields.Add(line[i..]); break; }
                fields.Add(line[i..end]);
                i = end + 1;
            }
        }
        return [.. fields];
    }

    public static string Escape(string value)
    {
        if (value.Contains(',') || value.Contains('"') || value.Contains('\n'))
            return '"' + value.Replace("\"", "\"\"") + '"';
        return value;
    }
}
