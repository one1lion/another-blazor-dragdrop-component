using DragonDrop.DragAndDrop;

namespace DragonDrop.Lib {
  public class DnDPayload<TListItem, TItem, TGroup> {
    public DnDContainer<TListItem, TItem, TGroup> FromContainer { get; set; }
    public int FromIndex { get; set; }
    public string Address { get; set; }
    public bool IsContainer { get; set; }
  }
}
