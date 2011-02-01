/// <summary>
/// Represents a visual cue to the user of what is being dragged.
/// Could be a semi-transparent image of the source component.
/// </summary>
public interface IBitDragProxy
{
	BitControl DraggedControl { get; set; }

    bool Visible { get; set; }

    //Vector2 Position { get; set; }

}
