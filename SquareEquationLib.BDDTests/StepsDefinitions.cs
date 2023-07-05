namespace SquareEquationLib.BDDTests;
using SquareEquationLib;
using TechTalk.SpecFlow;


[Binding]
public class StepDefinitions
{
    private const double TOLERATE = 10e-7;
    private double[] _coefficients = new double[3];
    private Func<double[]> _roots = () => new double[0];
    [When("вычисляются корни квадратного уравнения")]
    public void EvaluateEquationRoots()
    {
        _roots = () => SquareEquation.Solve(
                _coefficients[0],
                _coefficients[1],
                _coefficients[2]
                );
    }
    [Given(@"Квадратное уравнение с коэффициентами \((.*), (.*), (.*)\)")]
    public void GiveSqaureEquationCoefficients(string a, string b, string c) 
    {
        string[] input = new string[] {a.Split(".")[^1], b.Split(".")[^1], c.Split(".")[^1]};
        
        for (int i = 0; i < 3; i++)
        {
            if (input[i] == "NegativeInfinity")
                _coefficients[i] = double.NegativeInfinity;
            else if (input[i] == "PositiveInfinity")
                _coefficients[i] = double.PositiveInfinity;
            else if (input[i] == "NaN")
                _coefficients[i] = double.NaN;
            else
                _coefficients[i] = double.Parse(input[i]);
        }
    }
    [Then("выбрасывается исключение ArgumentException")]
    public void ThrowingArgumentException()
    {
        Assert.Throws<ArgumentException>(() => _roots());
    }
    
    [Then(@"квадратное уравнение имеет один корень (.*) кратности два")]
    public void SquareEquationHasOneRoot(double x)
    {
        double[] actualRoots = _roots();
        double[] expectedRoots = new double[] {x};
        
        if (actualRoots.Length != 1)
        {
            Assert.Fail("");
        }

        for (int i = 0; i < expectedRoots.Length; i++)
        {
            Assert.Equal(actualRoots[i], expectedRoots[i]);
        }
    }

    [Then(@"квадратное уравнение имеет два корня \((.*), (.*)\) кратности один")]
    public void SqaureEquationHasTwoRoots(double x1, double x2)
    {
        double[] actualRoots = _roots();
        double[] expectedRoots = new double[] {x1, x2};
        
        Array.Sort(expectedRoots);
        Array.Sort(actualRoots);

        if (actualRoots.Length != 2)
        {
            Assert.Fail("");
        }

        for (int i = 0; i < expectedRoots.Length; i++)
        {
            Assert.Equal(actualRoots[i], expectedRoots[i]);
        }
    }

    [Then(@"множество корней квадратного уравнения пустое")]
    public void SqaureEquationHasNoRoots()
    {
        double[] actualRoots = _roots();
        Assert.Empty(actualRoots);
    }
}
