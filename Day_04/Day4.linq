<Query Kind="Program" />

void Main()
{
	var records = new List<string>();
	var line = string.Empty;
	var file = new System.IO.StreamReader(@"C:\AoC2018\Day_04\input.txt");
	while((line = file.ReadLine()) != null) records.Add(line);
	
	// this is ugly fugly but it works
	$"( 18 seconds =^/ ) Guard most likely to be asleep's (ID * MostCommonlySleptMinute): {Solutions.PartOne(records)}".Dump();
	$"( 20 seconds =^/ ) Guard most frequently asleep on the same minute's (ID * MostCommonlySlepMinute): {Solutions.PartTwo(records)}".Dump();
		
}

public static class Solutions {

	public static int PartOne(List<string> records) {
		
		var guardRecords = GetGuardRecords(records);
		
		var sleepiestGuard = guardRecords.OrderByDescending(kvp => kvp.Value.TotalMinutesAsleep).First();
				
		return sleepiestGuard.Key * sleepiestGuard.Value.MostCommonlySleptMinute();
		
	}
	
	public static int PartTwo(List<string> records) {
	
		var guardRecords = GetGuardRecords(records);
		
		var guardMostFrequentlyAsleepOnSameMinute = guardRecords.Aggregate((l, r) => l.Value.NumberOfTimesSleptOnMostCommonMinute() > r.Value.NumberOfTimesSleptOnMostCommonMinute() ? l : r).Key;
		
		return guardMostFrequentlyAsleepOnSameMinute * guardRecords[guardMostFrequentlyAsleepOnSameMinute].MostCommonlySleptMinute();
		
	}

	private static Dictionary<int, GuardData> GetGuardRecords(List<string> records)
	{
		var allRecords = records.Select(r => new Record(r)).OrderBy(r => r.TimeStamp);

		var guardRecords = new Dictionary<int, GuardData>();

		foreach (var record in allRecords.Where(r => r.RecordType == RecordType.BeginShift))
		{
			if (!guardRecords.ContainsKey(record.GuardId)) guardRecords[record.GuardId] = new GuardData();
			var shift = allRecords.Where(r =>
						r.TimeStamp.Date == (record.TimeStamp.Hour == 23 ? record.TimeStamp.AddDays(1).Date : record.TimeStamp.Date)
					 && r.RecordType != RecordType.BeginShift);
			guardRecords[record.GuardId].WorkShift(shift);
		}
		
		return guardRecords;
	}

}

public enum RecordType {
	BeginShift,
	WakesUp,
	FallsAsleep
}

public class Record { 
	
	public RecordType RecordType { get; set; }
	
	public DateTime TimeStamp { get; set; }
	
	public string Message { get; set; }
	
	public int GuardId { get; set; }
	
	public Record(string record) {
		
		this.TimeStamp = DateTime.Parse(record.Slice(record.IndexOf('[') + 1, record.IndexOf(']')));
		this.Message = record.Slice(record.IndexOf(']') + 2, record.Length);
		
		switch (this.Message) {
			case "falls asleep": 
				this.RecordType = RecordType.FallsAsleep; 
				break;
			case "wakes up": 
				this.RecordType = RecordType.WakesUp; 
				break;
			default: 
				this.RecordType = RecordType.BeginShift; 
				this.GuardId = int.Parse(this.Message.Slice(this.Message.IndexOf('#') + 1, this.Message.IndexOf("begins") - 1));
				break;
		}	
	}
	
}

public class GuardData
{
	public int TotalMinutesAsleep { get; set; }
		
	public Dictionary<int, int> SleepingMinutesTracker { get; set; }
	
	public GuardData() {
		this.TotalMinutesAsleep = 0;
		this.SleepingMinutesTracker = new Dictionary<int, int>();
		for (var i = 0; i < 60; i++) this.SleepingMinutesTracker[i] = 0;
	}
	
	public void WorkShift (IEnumerable<Record> records) {
		if (records.Count() == 0) return;
		for (var i = 0; i < records.Count() - 1; i++) {
			if (records.ElementAt(i).RecordType == RecordType.FallsAsleep) {
				for (var m = records.ElementAt(i).TimeStamp.Minute; m < records.ElementAt(i + 1).TimeStamp.Minute; m++){
					this.TotalMinutesAsleep++;
					this.SleepingMinutesTracker[m]++;
				}
			}
		}
	}
	
	public int MostCommonlySleptMinute() {
		return this.SleepingMinutesTracker.Aggregate((l, r) => l.Value > r.Value ? l : r).Key;
	}
	
	public int NumberOfTimesSleptOnMostCommonMinute() {
		return this.SleepingMinutesTracker.Max(kvp => kvp.Value);
	}


}

// Define other methods and classes here

public static class Extensions
{
    public static string Slice(this string source, int start, int end)
    {
        if (end < 0) end = source.Length + end;
        int len = end - start;               
        return source.Substring(start, len);
    }
}