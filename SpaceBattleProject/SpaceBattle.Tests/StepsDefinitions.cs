namespace SpaceBattle.Tests;
using SpaceBattleLib;
using TechTalk.SpecFlow;

[Binding]
public class SpaceBattle_Tests
{
    private SpaceBattle _spaceShip = new SpaceBattle();
    private Exception _actualException = new Exception();
    [Given(@"космический корабль, положение в пространстве которого невозможно определить")]
    public void CantDefinePosition()
    {
        _spaceShip.SetCurrentPosition(new double[] {double.NaN, double.NaN});
    }
    [Given(@"имеет мгновенную скорость \((.*), (.*)\)")]
    public void HasInstantSpeed(double x, double y)
    {
        _spaceShip.SetInstantSpeed(new double[] {x, y});
    }
    
    [When(@"происходит прямолинейное равномерное движение без деформации")]
    public void ExecuteUniformMovement()
    {
        try
        {
            _spaceShip.UniformMovementStep();
        }
        catch(Exception e)
        {
            _actualException = e;
        }
    }
    [Then(@"возникает ошибка Exception")]
    public void TrowsExceptionExpception()
    {
        Assert.ThrowsAsync<Exception>(() => throw _actualException);
    }
    [Given(@"космический корабль находится в точке пространства с координатами \((.*), (.*)\)")]
    public void SpaceshipInPosition(double x, double y)
    {
        _spaceShip.SetCurrentPosition(new double[] {x,y});
    }
    [Given(@"скорость корабля определить невозможно")]
    public void CantDefineInstantSpeed()
    {
        _spaceShip.SetInstantSpeed(new double[] {double.NaN, double.NaN});
    }
     [Then(@"космический корабль перемещается в точку пространства с координатами \((.*), (.*)\)")]
    public void SpaceShipMoveToPoint(double x, double y)
    {
        double[] expectedCoord = new double[] {x, y};
        for (int i = 0; i < 2; i++)
        {
            Assert.Equal(expectedCoord[i], _spaceShip.CurrentPosition[i], 6);
        }
    }
    [Given(@"изменить положение в пространстве космического корабля невозможно")]
    public void CantChangePosition()
    {
        _spaceShip.InstantSpeed = new double[] {double.NaN, double.NaN};
    }
}