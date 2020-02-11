namespace DragonDrop.Lib {
  public class ListUpdateEventArgs {
    public UpdateEventType UpdateEventType { get; set; }
    public object AffectedFromList { get; set; }
    public int AffectedIndexInFromList { get; set; }
    public object AffectedToList { get; set; }
    public int AffectedIndexInToList { get; set; }
    public object AffectedItem { get; set; }
    public string AffectedFromAddress { get; set; }
    public string AffectedToAddress { get; set; }
  }

  public enum UpdateEventType {
    NotSet,
    ItemAdded,
    ItemEdited,
    ItemCopied,
    ItemMoved,
    ItemsGrouped,
    ItemRemoved,
    Ungrouped
  }
}
