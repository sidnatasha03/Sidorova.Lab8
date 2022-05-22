using System.Text;
using System.Text.Encodings;
using System.Text.Encodings.Web;
using System.Text.Json;

{

    var path = "Table.csv";

    Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

    Encoding encoding = Encoding.GetEncoding(1251);

    var lines = File.ReadAllLines(path, encoding);
    Console.WriteLine(lines);
    var bakery = new Bakery[lines.Length - 1];

    for (var i = 1; i < lines.Length; i++)
    {
        var splits = lines[i].Split(';');
        var bake = new Bakery();
        bake.Название_пекарни = splits[0];
        bake.Общая_производительность_продукции = Convert.ToDouble(splits[1]);
        bake.Средняя_стоимость_продукции = Convert.ToDouble(splits[2]);
        bake.Средние_издержки_в_мес = Convert.ToDouble(splits[3]);
        bake.Среднее_количество_покупателей_в_день = Convert.ToDouble(splits[4]);
        bakery[i - 1] = bake;
    }

    var result = "result.csv";
    using (StreamWriter streamWriter = new StreamWriter(result, false, encoding))
    {
        streamWriter.WriteLine($"Название;Производительность;Стоимость;Издержки;Покупатели;Доход;Прибыль");
        for (int i = 0; i < bakery.Length; i++)
        {
            streamWriter.WriteLine(bakery[i].ToExcel());
        }
    }

    var jsonOptions = new JsonSerializerOptions()
    {
        Encoder = JavaScriptEncoder.Default
    };

    var json = JsonSerializer.Serialize(bakery, jsonOptions);
    File.WriteAllText("result.json", json);

    var stringJson = File.ReadAllText("result.json");
    var array = JsonSerializer.Deserialize<Bakery[]>(stringJson);
    foreach (var item in array)
    {
        Console.WriteLine(item.ToString());
    }

    string jsonNewtonsoft = Newtonsoft.Json.JsonConvert.SerializeObject(bakery, Newtonsoft.Json.Formatting.Indented);


    File.WriteAllText("NewtonsoftResult.json", jsonNewtonsoft);
}




public class Bakery
{
    public string Название_пекарни { get; set; }
    public double Общая_производительность_продукции { get; set; }
    public double Средняя_стоимость_продукции { get; set; }

    public double Средние_издержки_в_мес { get; set; }

    public double Среднее_количество_покупателей_в_день { get; set; }

    public double Средний_доход_в_месяц { get => Среднее_количество_покупателей_в_день * 12 * Средняя_стоимость_продукции; }
    public double Средняя_прибыль_в_месяц { get => Средний_доход_в_месяц - Средние_издержки_в_мес; }
    public override string ToString()
    {
        return $" {Название_пекарни}\n Общая производительность: {Общая_производительность_продукции} изделий в день\n Средняя стоимость продукции: {Средняя_стоимость_продукции}\n Средние издержки: {Средние_издержки_в_мес} в мес\n Среднее количество покупателей:{Среднее_количество_покупателей_в_день} в день\n Средний доход в месяц:{Средний_доход_в_месяц} рублей в месяц\n Средняя прибыль в месяц: {Средняя_прибыль_в_месяц} рублей в месяц";
    }

    public string ToExcel()
    {
        return $" {Название_пекарни};{Общая_производительность_продукции};{Средняя_стоимость_продукции};{Средние_издержки_в_мес};{Среднее_количество_покупателей_в_день};{Средний_доход_в_месяц}; {Средняя_прибыль_в_месяц}";

    }
}

