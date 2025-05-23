namespace Questao2.Interfaces;

public interface IGolsCalculator
{
    Task<int> getTotalScoredGoals(string time, int ano);
}
