# FluentFuzzer Documentation

If you need fuzzing for your integration tests or unit tests, you can use FluentFuzzer and any unit tests framework, which you have used already.  

## How to use

1. Simple example, which used NUnit framework. Parameter timeInSec set duration method execution:  

```
public class ConstructClass
{
	public string Name { get; set; }

	public List<string> NameChildren { get; set; }

	public int Age { get; set; }
}

[TestFixture]
public class FuzzerRunnerConstructTests
{
	[Test]
	public async Task FuzzerTestConstructObject()
	{
		await Fuzzer.Instance.RunAsync<ConstructClass>(async construction =>
		{
			await Task.Delay(100);
			if (construction is not null)
				Console.WriteLine(construction.Name + " " + construction.NameChildren?.Count + " " + construction.Age);
		},
		timeInSec: 300);
	}
}
```

2. There is parallel execution. Set 2 threads:  

```
await Fuzzer.Instance
	.MakeParallelExecution(2)
	.RunAsync<string>(async text =>
	{
		await Task.Delay(100);
		if (DateTime.TryParse(text, out var dt1))
		{
			var s = dt1.ToString("O");
			var dt2 = DateTime.Parse(s, null, DateTimeStyles.RoundtripKind);

			if (dt1 != dt2)
				throw new Exception();
		}
	});
```

3. You can write result with exeption to folder.  

```
await Fuzzer.Instance
	.MakeParallelExecution(2)
	.WriteResultToFolder("C:/Result/MyFuzzing")
	.RunAsync<string>(async text =>
	{
		await Task.Delay(100);
		if (text.Contains("1"))
			throw new Exception("Tests exception");
	});
```
