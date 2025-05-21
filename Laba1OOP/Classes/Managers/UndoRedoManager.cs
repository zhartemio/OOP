using System.Collections.Generic;

public class UndoRedoManager<T>
{
    private List<T> shapes;
    private List<T> removedShapes;

    public UndoRedoManager(List<T> shapes, List<T> removedShapes)
    {
        this.shapes = shapes;
        this.removedShapes = removedShapes;
    }

    public bool CanUndo => shapes.Count > 0;
    public bool CanRedo => removedShapes.Count > 0;

    public void Add(T shape)
    {
        shapes.Add(shape);
        removedShapes.Clear(); 
    }

    public void Undo()
    {
        if (!CanUndo) return;
        T last = shapes[^1];
        shapes.RemoveAt(shapes.Count - 1);
        removedShapes.Add(last);
    }

    public void Redo()
    {
        if (!CanRedo) return;
        T last = removedShapes[^1];
        removedShapes.RemoveAt(removedShapes.Count - 1);
        shapes.Add(last);
    }

    public void Clear()
    {
        shapes.Clear();
        removedShapes.Clear();
    }
}
