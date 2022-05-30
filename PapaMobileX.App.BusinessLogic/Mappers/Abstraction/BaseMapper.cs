namespace PapaMobileX.App.BusinessLogic.Mappers.Abstraction;

public abstract class BaseMapper<TIn, TOut> : IMapper<TIn, TOut>
{
    public abstract TOut Map(TIn input);
    
    public IEnumerable<TOut> Map(IEnumerable<TIn> input)
    {
        return input.Select(Map).ToList();
    }
}