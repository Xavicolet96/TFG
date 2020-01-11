// Class that represents vertices, edges and faces
using System;

[Serializable()]
public class Element 
{
    public int step;
    public int id;

    // Saves the step in which the element was created
    public Element(int s, int id)
    {
        step = s;
        this.id = id;
    }
}