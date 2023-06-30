namespace SpaceBattleLib;
public class SpaceBattle
{
    public double[] CurrentPosition = new double[] {double.NaN, double.NaN};
    public double[] InstantSpeed = new double[] {double.NaN, double.NaN};
    private bool IsValidCoordinates(double[] coord)
    {
        foreach(double a in coord)
        {
            if (double.IsNaN(a) || double.IsInfinity(a))
            {
                return false;
            }
        }
        return true;
    }
    public SpaceBattle() {}
    public void SetCurrentPosition(double[] position)
    {
        this.CurrentPosition = position;
    }
    public void SetInstantSpeed(double[] instantSpeed)
    {
        this.InstantSpeed = instantSpeed;
    }
    public double[] UniformMovementStep()
    {
        if (IsValidCoordinates(this.CurrentPosition) && IsValidCoordinates(this.InstantSpeed))
        {
            this.CurrentPosition[0] += this.InstantSpeed[0];
            this.CurrentPosition[1] += this.InstantSpeed[1];
        }
        else 
        {
             throw new Exception();
        }
        return this.CurrentPosition;
    }
}
