 
using Utils;

public class MousePainter : MouseClick
{
    public static MousePainter Instance;
   
    private void Awake()
    {
        Instance = this;
    } 
      

    public  void UpdatePainter()
    {
        Painter();
    }
}
